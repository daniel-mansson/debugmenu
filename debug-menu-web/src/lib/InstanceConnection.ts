import { derived, get, writable, type Writable } from "svelte/store";
import { ArrayQueue, ConstantBackoff, Websocket, WebsocketBuilder, WebsocketEvent } from "websocket-ts";
import { parseAsyncApi } from "./asyncApiHelpers";

export class InstanceConnection {
  readonly url: string;
  readonly token: string;

  public status: Writable<'open' | 'closed'> = writable('closed');
  public messages: Writable<any[]> = writable([]);
  public channelMessages: Writable<any> = writable({});
  public messages2: any[] = [];

  public schema: Writable<string> = writable("");
  public api: Writable<any> = writable({ buttons: [], logs: [] });
  public metadata: Writable<any> = writable({});

  public latestMessage = derived(this.messages, (m) => m[m.length - 1]);
  socket: Websocket;

  constructor(url: string, token: string) {
    this.url = url;
    this.token = token;

    let builder = new WebsocketBuilder(url)
      .withBuffer(new ArrayQueue())
      .withBackoff(new ConstantBackoff(5000));

    console.log("ctr")

    this.socket = builder.build();
    this.socket.addEventListener(WebsocketEvent.open, async () => {
      this.socket.send(token);
      this.status.set('open');
      console.log('open')

    });

    this.socket.addEventListener(WebsocketEvent.close, () => {
      this.status.set('closed');
      console.log('closed')
    });

    this.socket.addEventListener(WebsocketEvent.message, (ws, ev) => this.handleMessage(ws, ev));
  }

  async handleMessage(_: Websocket, ev: MessageEvent) {
    if (ev.data instanceof Blob) {
      console.log('handle blob ' + ev.data.size);
      let stream = ev.data.stream();
      let reader = stream.getReader();
      let r = await reader.read();
      let bytes = r.value!;
      let channelLength = bytes.at(0)!;
      let channel = new TextDecoder().decode(bytes.slice(1, channelLength + 1))

      console.log(channel)
      console.log(r.value)
      if (channel === '__internal/api') {
        let apiText = new TextDecoder().decode(bytes.slice(channelLength + 2))
        this.updateApi(apiText);
      }

      return;
    }

    let message = JSON.parse(ev.data);
    let messageWithTimestamp = {
      timestamp: new Date(),
      ...message
    };
    console.log('r: ' + message.channel)

    if (message.channel === "__internal/api") {
      this.updateApi(message.payload.text);
    }
    else if (message.channel === "__internal/metadata") {
      this.updateMetadata(message.payload);
    }

    this.messages.update(m => {
      m.push(messageWithTimestamp);
      return m;
    });

    let channelMessagesStore: Writable<any[]> = get(this.channelMessages)[message.channel];
    if (!channelMessagesStore) {
      channelMessagesStore = writable([]);
      this.channelMessages.update(m => {
        m[message.channel] = channelMessagesStore;
        return m;
      })
    }
    this.messages2.push(messageWithTimestamp);
    channelMessagesStore.update(m => {
      m.push(messageWithTimestamp);
      return m;
    })
  }

  async updateApi(apiText: string) {
    let api = await parseAsyncApi(apiText);
    console.log(api)
    this.api.set(api);
  }

  updateMetadata(payload: any) {
    this.metadata.set(payload);
  }

  getStoreForChannel(channel: string) {
    let messagesForChannel = get(this.channelMessages)[channel];
    if (!messagesForChannel) {
      messagesForChannel = writable([]);
      this.channelMessages.update(m => {
        m[channel] = messagesForChannel;
        return m;
      })
    }
    return messagesForChannel;
  }

  send(channel: string, payload: any) {
    this.socket.send(
      JSON.stringify({ channel, payload })
    );
  }

  stop() {
    this.socket.close();
  }
}