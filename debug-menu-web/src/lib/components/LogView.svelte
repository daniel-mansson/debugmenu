<script lang="ts">
	import { tick } from 'svelte';
	import type { Readable } from 'svelte/store';
	import Page from '../../routes/+page.svelte';

	export let messages: Readable<any[]>;
	export let title: string;
	export let textKey = 'text';
	export let detailsKey = 'details';
	export let timestampKey = 'timestamp';
	export let typeKey = 'type';

	let scrollElement: any;
	let filterText = '';
	let maxVisible = 100;

	messages.subscribe((m) => {
		scrollToBottom();
	});

	function getClassFromType(type: string) {
		switch (type) {
			case 'log':
				return 'variant-soft';
			case 'warning':
				return 'variant-soft-warning';
			default:
				return 'variant-soft-error';
		}
	}

	function isScrollCloseToBottom() {
		return (
			scrollElement &&
			scrollElement.scrollHeight - scrollElement.clientHeight - scrollElement.scrollTop < 50
		);
	}

	async function scrollToBottom() {
		if (isScrollCloseToBottom()) {
			await tick();
			scrollElement.scroll({ top: scrollElement.scrollHeight, behavior: 'smooth' });
		}
	}
</script>

<div class="m-2 border-surface-500 border p-2 bg-surface-50-900-token">
	<div class="flex">
		<div class="font-mono">{title}</div>
		<span class="ml-auto text-surface-500-400-token">({$messages?.length})</span>
	</div>
	<div class="flex">
		<span class="text-sm text-surface-500-400-token my-auto">Filter: </span>
		<input class="ml-1 bg-surface-100-800-token h-6 my-1" type="text" bind:value={filterText} />
		<div class="ml-auto">
			<span class="text-sm text-surface-500-400-token my-auto">Limit:</span>
			<input
				class="w-20 p-1 bg-surface-100-800-token h-6 my-1"
				type="number"
				min="0"
				step={maxVisible > 100 ? 100 : 10}
				bind:value={maxVisible}
			/>
		</div>
	</div>
	<div bind:this={scrollElement} class="overflow-y-scroll h-[600px]">
		{#if $messages}
			{#each $messages.filter((m, index) => index >= $messages.length - maxVisible && m.payload.text
						.toLowerCase()
						.includes(filterText.toLowerCase())) as message, index}
				<div
					class="flex {index % 2 == 0 ? 'bg-surface-100-800-token' : 'bg-surface-50-900-token'} "
				>
					<div class="text-surface-500-400-token font-mono text-xs my-auto">
						{message.timestamp.toLocaleTimeString(navigator.language, {
							hour12: false,
							hour: '2-digit',
							minute: '2-digit',
							second: '2-digit'
						})}
					</div>
					<span class="badge-icon {getClassFromType(message.payload[typeKey])} mr-2 my-1">
						{message.payload[typeKey][0].toUpperCase()}</span
					>
					<div class="">{message.payload[textKey]}</div>
				</div>
			{/each}
		{/if}
	</div>
</div>
