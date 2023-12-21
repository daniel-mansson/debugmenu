<script lang="ts">
	import type { OperationInterface } from '@asyncapi/parser';
	import { getModalStore, type ModalComponent, type ModalSettings } from '@skeletonlabs/skeleton';
	import FormButtonModal from './FormButtonModal.svelte';
	import { createEventDispatcher } from 'svelte';
	import FormButton from './FormButton.svelte';
	import SimpleButton from './SimpleButton.svelte';

	export let channel: string;
	export let buttons: any[];

	let dispatch = createEventDispatcher();
</script>

<div class="m-2 border-surface-500 border p-2 bg-surface-50-900-token">
	<div class="font-mono">{channel}</div>
	<div class="">
		{#each buttons as button}
			{#if button.fields.length > 0}
				<FormButton {button} on:submit={(e) => dispatch('submit', e.detail)} />
			{:else}
				<SimpleButton {button} on:submit={(e) => dispatch('submit', e.detail)} />
			{/if}
		{/each}
	</div>
</div>
