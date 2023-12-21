<script lang="ts">
	import type { PageData } from './$types';
	import { changeInstance, instanceConnection } from '$lib/WebsocketManager';
	import LogView from '$lib/components/LogView.svelte';
	import ButtonContainer from '$lib/components/ButtonContainer.svelte';

	export let data: PageData;

	let lastEvent: MessageEvent | undefined;
	changeInstance(`wss://localhost:8082/ws/room/${data.roomId}/controller`, data.session!.jwt);

	$: logMessages = $instanceConnection!.messages;
	let element: any;
	$: api = $instanceConnection!.api;
	$: buttons = Map.groupBy($api.buttons, (b) => b.category);
	$: logs = $api.logs;
	console.log('api');
	console.log($api);

	function onClick(evt: any) {
		console.log(JSON.stringify({ channel: evt.button.channel, payload: evt.payload }));

		$instanceConnection?.send(evt.button.channel, evt.payload);
	}
</script>

<!-- 
<ButtonContainer channel="/gameplay" {buttons} on:submit={(r) => onClick(r.detail)} /> -->

{#each buttons as buttonGroup}
	<ButtonContainer
		channel={buttonGroup[0]}
		buttons={buttonGroup[1]}
		on:submit={(r) => onClick(r.detail)}
	/>
{/each}
{#each logs as log}
	<LogView
		title={'/' + log.channel}
		messages={$instanceConnection?.getStoreForChannel(log.channel)}
	/>
{/each}
