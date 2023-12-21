<script lang="ts">
	import { InstanceConnection } from '$lib/InstanceConnection';
	import { changeInstance, instanceConnection } from '$lib/WebsocketManager.js';
	import { parseAsyncApi } from '$lib/asyncApiHelpers';
	import FormButtonModal from '$lib/components/FormButtonModal.svelte';
	import Parser, {
		type AsyncAPIDocumentInterface,
		type OperationInterface
	} from '@asyncapi/parser';
	import { getModalStore, type ModalComponent, type ModalSettings } from '@skeletonlabs/skeleton';
	import { onMount } from 'svelte';
	import { derived, get } from 'svelte/store';

	const createTokenModalComponent: ModalComponent = { ref: FormButtonModal };

	export let data;
	const modalStore = getModalStore();

	function onClicked(context: OperationInterface) {
		const modal: ModalSettings = {
			type: 'component',
			component: createTokenModalComponent,
			title: 'Create Token',
			body: '',
			meta: { hello: 'this is hello', context: context },
			response: async (response: any) => {
				console.log(response);
			}
		};
		modalStore.trigger(modal);
	}

	let publishOps: any = [];
	let document: AsyncAPIDocumentInterface;

	let stuff: any[] = [];
	let latest: any = null;
	changeInstance(
		`wss://localhost:8082/ws/room/${'8ee16b26-3945-4079-8dd8-941ca57f23f0'}/controller`,
		data.session!.jwt
	);

	onMount(async () => {
		let asyncApiYaml = `
  asyncapi: '2.6.0'
  info:
    title: Example AsyncAPI specification
    version: '0.1.0'
  channels:
    example-channel:
      subscribe:
        tags:
          - name: log
        message:
          payload:
            type: object
            properties:
              exampleField:
                type: string
              exampleNumber:
                type: number
              exampleDate:
                type: string
                format: date-time
    gameplay/spawn:
      publish:
        tags:
          - name: button
        message:
          payload:
            type: object
            properties:
              exampleField:
                type: string
                description: This is an example text field
              exampleNumber:
                type: number
              exampleDate:
                type: string
                format: date-time
`;
		const parser = new Parser();
		let result = await parser.parse(asyncApiYaml);

		let api = await parseAsyncApi(asyncApiYaml);

		document = result.document!;
		console.log(api);

		if (document) {
			for (let operation of document.allOperations()) {
				if (operation.id() === 'publish') {
					let hasButtonTag = operation.tags().filterBy((t) => t.name() === 'button').length == 1;
					if (hasButtonTag) {
						publishOps.push(operation);
					}
				}
			}
		}
		latest = derived($instanceConnection!.messages, (m) => m![m.length - 1]);
	});
	$: l = $instanceConnection!.latestMessage;
	$: state = $instanceConnection?.status;
	$: messages = $instanceConnection?.getStoreForChannel('log');
</script>

{#if document}
	{#each publishOps as op}
		<button
			on:click={() => onClicked(op)}
			class="btn btn-sm font-mono border border-primary-100-800-token"
		>
			{op.channels()[0].id()}</button
		>
	{/each}
{/if}
d: {$l?.payload?.text}
s: {$state}
{#if $messages}
	{#if $messages.length > 0}
		<div>
			{$messages[$messages.length - 1].timestamp.toLocaleTimeString(navigator.language, {
				hour: '2-digit',
				minute: '2-digit'
			})}
			{$messages[$messages.length - 1].payload.text}
		</div>
	{/if}
	<div>------</div>

	{#each $messages as s}
		<div>{s.payload.text}</div>
	{/each}
{/if}
