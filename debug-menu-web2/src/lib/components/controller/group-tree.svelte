<script lang="ts">
	import { createEventDispatcher } from 'svelte';
	import Group from './group.svelte';
	import SimpleButton from './simple-button.svelte';
	import Toggle from './toggle.svelte';

	export let group: any;
	export let level = 0;
	export let commandSender: any;
</script>

<Group title={group.id ?? ''}>
	<div slot="content" class="space-b-2 space-x-2">
		{#each group.channels as channel}
			{#if channel.type === 'button'}
				<SimpleButton
					settings={channel.settings}
					label={channel.name}
					on:click={() => commandSender?.send(channel, {})}
				></SimpleButton>
			{:else if channel.type === 'toggle'}
				<Toggle
					state={channel.state}
					settings={channel.settings}
					label={channel.name}
					on:change={(evt) => {
						commandSender?.send(channel, { value: evt.detail });
					}}
				></Toggle>
			{/if}
		{/each}
		{#each group.groups as g}
			<div class="mt-8">
				<svelte:self group={g} level={level + 1} {commandSender} />
			</div>
		{/each}
	</div>
</Group>
