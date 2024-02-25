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

	onMount(() => {
		update();
	});

	async function update() {
		console.log('parsing');
		doc = await parseAsyncApi(asyncapiMockData);

		console.log(doc.categories);
		stuff = '';

		stuff = JSON.stringify(doc.categories);
	}

	let doc: any = undefined;

	let stuff: any;
</script>

<div class="my-1 flex items-center justify-between">
	<h2 class="text-3xl font-bold tracking-tight">Instance Dashboard (asyncapi)</h2>
	<div class="flex items-center space-x-2">Status and Settings</div>
</div>

{#if doc?.categories}
	{#each doc.categories as category}
		<div>{category.id}</div>

		<div
			class="space-y-8 rounded-lg border-2 border-dashed border-gray-200 p-4 pt-6 dark:border-gray-700"
		>
			{#each category.groups as group}
				<GroupTree {group}></GroupTree>
			{/each}
		</div>
	{/each}
{/if}
<Tabs.Root value="overview" class="my-2 space-y-1">
	<Tabs.List class=" ">
		<Tabs.Trigger value="commands">Commands</Tabs.Trigger>
		<Tabs.Trigger value="logs">Logs</Tabs.Trigger>
		<Tabs.Trigger value="stats">Statistics</Tabs.Trigger>
		<Tabs.Trigger value="errors">Errors</Tabs.Trigger>
	</Tabs.List>
</Tabs.Root>
<div
	class="space-y-8 rounded-lg border-2 border-dashed border-gray-200 p-4 pt-6 dark:border-gray-700"
>
	<Group title="cqwcqc" color="red">
		<div slot="content" class="mt-2 space-y-6">
			<Group title="erger" color="blue">
				<div slot="content">
					<div class="grid grid-cols-3 gap-4 md:grid-cols-4 lg:grid-cols-6">
						<SimpleButton label="Yellow" color="yellow" />
						<SimpleButton label="Red" color="red" />
						<SimpleButton label="Blue" color="blue" />
						<SimpleButton label="No def" />
						<SimpleButton label="Invalid" color="asdfasdf" />
						<SimpleButton label="grass" color="grass" />
					</div>
				</div>
			</Group>
			<Group title="asdf" color="green">
				<div slot="content">
					<div class="grid grid-cols-3 gap-4 md:grid-cols-4 lg:grid-cols-6">
						<SimpleButton label="teal" color="teal" />
						<SimpleButton label="yellow" color="yellow" />
						<SimpleButton label="purple" color="purple" />
						<SimpleButton label="green" color="green" />
						<SimpleButton label="red" color="red" />
						<SimpleButton label="orange" color="orange" />
						<SimpleButton label="grass" color="grass" />
						<SimpleButton label="aqua" color="aqua" />
						<SimpleButton label="magenta" color="magenta" />
					</div>
				</div>
			</Group>
		</div>
	</Group>
</div>
