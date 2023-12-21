<script lang="ts">
	import { getModalStore, type ModalComponent, type ModalSettings } from '@skeletonlabs/skeleton';
	import { createEventDispatcher } from 'svelte';
	import FormButtonModal from './FormButtonModal.svelte';

	export let button: any;

	let dispatch = createEventDispatcher();

	const modalComponent: ModalComponent = { ref: FormButtonModal };
	const modalStore = getModalStore();

	function onClick(context: any) {
		const modal: ModalSettings = {
			type: 'component',
			component: modalComponent,
			meta: { context: context },
			response: async (response: any) => {
				dispatch('submit', { button, payload: response });
			}
		};
		modalStore.trigger(modal);
	}
</script>

<button on:click={() => onClick(button)} class="m-1 btn btn-sm variant-outline-primary">
	<div class="flex">
		{button.channelParts[button.channelParts.length - 1]}
		<div class="text-surface-500-400-token ml-1">⇪</div>
	</div>
</button>
