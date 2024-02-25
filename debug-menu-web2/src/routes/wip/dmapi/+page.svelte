<script lang="ts">
	import * as Tabs from '$lib/components/ui/tabs';
	import * as Card from '$lib/components/ui/card';
	import Button from '$lib/components/ui/button/button.svelte';
	import Group from '$lib/components/controller/group.svelte';
	import SimpleButton from '$lib/components/controller/simple-button.svelte';
	import { onMount } from 'svelte';
	import { asyncapiMockData } from './mockdata';
	import { parseAsyncApi } from '$lib/asyncApiHelpers';
	import GroupTree from '$lib/components/controller/group-tree.svelte';
	import { buildDebugmenuElements, parseDebugmenuApi } from '$lib/debugmenuApi';

	onMount(() => {
		update();
	});

	async function update() {
		api = buildDebugmenuElements(asyncapiMockData);

		console.log(api.categories);
	}

	let api: any = undefined;

	let stuff: any;
</script>

<div class="my-1 flex items-center justify-between">
	<h2 class="text-3xl font-bold tracking-tight">Instance Dashboard (asyncapi)</h2>
	<div class="flex items-center space-x-2">Status and Settings</div>
</div>

{#if api?.categories}
	<Tabs.Root value="default" class="my-2 space-y-1">
		{#if api.categories.length > 1}
			<Tabs.List class=" ">
				{#each api.categories as category}
					<Tabs.Trigger value={category.id ?? 'default'}>{category.id ?? 'Default'}</Tabs.Trigger>
				{/each}
			</Tabs.List>
		{/if}

		{#each api.categories as category}
			<Tabs.Content value={category.id ?? 'default'}>
				<div
					class="space-y-8 rounded-lg border-2 border-dashed border-gray-200 p-4 pt-6 dark:border-gray-700"
				>
					{#each category.groups as group}
						<GroupTree {group}></GroupTree>
					{/each}
				</div></Tabs.Content
			>
		{/each}
	</Tabs.Root>
{/if}
