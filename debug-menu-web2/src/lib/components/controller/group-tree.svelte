<script lang="ts">
	import { createEventDispatcher } from 'svelte';
	import Group from './group.svelte';
	import SimpleButton from './simple-button.svelte';
	import Toggle from './toggle.svelte';
	import ModalButton from './modal-button.svelte';
	import SingleText from './single-text.svelte';

	export let group: any;
	export let level = 0;
	export let commandSender: any;
</script>

<Group title={group.id ?? ''}>
	<div slot="content" class="">
		{#each group.channels as channel}
			{#if channel.type === 'button'}
				{#if channel.publish.type === 'object' && Object.keys(channel.publish.properties).length > 0}
					<ModalButton
						properties={channel.publish.properties}
						settings={channel.settings}
						label={channel.name}
						on:submit={(evt) => commandSender?.send(channel, evt.detail)}
					></ModalButton>
				{:else}
					<SimpleButton
						settings={channel.settings}
						label={channel.name}
						on:click={() => commandSender?.send(channel, {})}
					></SimpleButton>
				{/if}
			{:else if channel.type === 'toggle'}
				<Toggle
					state={channel.state}
					settings={channel.settings}
					label={channel.name}
					on:change={(evt) => {
						commandSender?.send(channel, { value: evt.detail });
					}}
				></Toggle>
			{:else if channel.type === 'text-field' && Object.keys(channel.publish.properties).length > 0}
				<SingleText
					maxLength={channel?.publish?.properties?.value?.maxLength}
					state={channel.state}
					settings={channel.settings}
					label={channel.name}
					on:submit={(evt) => {
						commandSender?.send(channel, { value: evt.detail });
					}}
				></SingleText>
			{/if}
		{/each}
		{#each group.groups as g}
			<div class="mt-8">
				<svelte:self group={g} level={level + 1} {commandSender} />
			</div>
		{/each}
	</div>
</Group>
