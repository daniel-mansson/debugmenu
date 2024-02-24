<script lang="ts">
	import Group from './group.svelte';
	import SimpleButton from './simple-button.svelte';

	export let group: any;
	export let level = 0;
</script>

<Group title={group.id}>
	<div slot="content" class="space-b-2 space-x-2">
		{#each group.elements as e}
			{#if e.type === 'button'}
				<SimpleButton label={e.id}></SimpleButton>
			{/if}
		{/each}
		{#each group.groups as g}
			<div class="mt-8">
				<svelte:self group={g} level={level + 1} />
			</div>
		{/each}
	</div>
</Group>
