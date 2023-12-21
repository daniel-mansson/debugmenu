import { get, writable, type Writable } from "svelte/store";
import { InstanceConnection } from "./InstanceConnection";

export const instanceConnection: Writable<InstanceConnection | null> = writable(null)

export function changeInstance(url: string, token: string) {
    let previous = get(instanceConnection)
    if (previous?.url !== url) {
        previous?.stop();
        instanceConnection.set(new InstanceConnection(url, token));
    }
}