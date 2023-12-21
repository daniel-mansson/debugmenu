<script lang="ts">
	import type { OperationInterface } from '@asyncapi/parser';

	// Props
	/** Exposes parent props to this component. */
	export let parent: any;

	// Stores
	import { getModalStore } from '@skeletonlabs/skeleton';
	const modalStore = getModalStore();

	// Form Data
	let formData: any = {};

	// We've created a custom submit function to pass the response and close the modal.
	function onFormSubmit(): void {
		if ($modalStore[0].response) $modalStore[0].response(formData);
		modalStore.close();
	}

	let operation = $modalStore[0].meta.context.operation;
	let channel = operation.channels()[0];
	console.log('');
	console.log(operation.channels()[0].id());
	console.log(operation.id());
	let fields: any[] = [];
	if (operation.id() === 'publish') {
		for (let message of operation.messages()) {
			console.log(message.payload()?.properties());
			console.log(Object.keys(message.payload()?.properties()));
			for (let propName of Object.keys(message.payload()?.properties())) {
				console.log(message.payload()?.properties()[propName].type());
				console.log(propName);
				let prop = message.payload()?.properties()[propName];
				fields.push({
					type: prop.type(),
					name: propName,
					description: prop.description(),
					format: prop.format()
				});
			}
			//	for (let prop of message.payload().properties) {
			//	}
		}
	}

	// Base Classes
	const cBase = 'card p-4 w-modal shadow-xl space-y-4';
	const cHeader = 'text-2xl font-bold';
	const cForm = 'border border-surface-500 p-4 space-y-4 rounded-container-token';
</script>

<!-- @component This example creates a simple form modal. -->

{#if $modalStore[0]}
	<div class="modal-example-form {cBase}">
		<header class="{cHeader} font-mono">{channel.id()}</header>
		<!-- Enable for debugging: -->
		<form class="modal-form {cForm}">
			{#each fields as field}
				{#if field.type === 'string'}
					<label class="label">
						<span class="font-mono mr-2">{field.name}</span>
						<span>{field.description ?? ''}</span>
						<span class="float-right">{field.format ?? ''}</span>
						<input
							class="input"
							type="text"
							bind:value={formData[field.name]}
							placeholder="Enter text..."
						/>
					</label>
				{:else if field.type === 'number'}
					<label class="label">
						<span class="font-mono mr-2">{field.name}</span>
						<span>{field.description ?? ''}</span>
						<input
							class="input"
							type="number"
							bind:value={formData[field.name]}
							placeholder="Enter number..."
						/>
					</label>
				{/if}
			{/each}
		</form>
		<!-- prettier-ignore -->
		<footer class="modal-footer {parent.regionFooter}">
        <button class="btn {parent.buttonNeutral}" on:click={parent.onClose}>{parent.buttonTextCancel}</button>
        <button class="btn {parent.buttonPositive}" on:click={onFormSubmit}>Send Command</button>
    </footer>
	</div>
{/if}
