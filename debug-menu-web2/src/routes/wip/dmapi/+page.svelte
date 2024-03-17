<script lang="ts">
	import * as Tabs from '$lib/components/ui/tabs';
	import { onMount } from 'svelte';
	import { asyncapiMockData } from './mockdata';
	import GroupTree from '$lib/components/controller/group-tree.svelte';
	import { buildDebugmenuElements } from '$lib/debugmenuApi';
	import Button from '$lib/components/ui/button/button.svelte';

	onMount(() => {
		update();
	});

	let commandSender = {
		send: function (source: any, data: any) {
			console.log('Command! ' + source.channel);
			console.log(source);
			console.log(data);
		}
	};

	async function update() {
		api = buildDebugmenuElements(asyncapiMockData);
		console.log(api.categories);
	}
	let api: any = undefined;

	function getRandomInt(max:number) {
  		return Math.floor(Math.random() * max);
	}

	let types = ["log", "warning", "error"]
	let messages = ["The hawk didn’t understand why the ground squirrels didn’t want to be his friend.",
	"Toddlers feeding raccoons surprised even the seasoned park ranger.",
	"Harrold felt confident that nobody would ever suspect his spy pigeon.",
	"The Guinea fowl flies through the air with all the grace of a turtle."]
	function addFakeLog() {
		api.categories[0].groups[0].channels[0].history.update(h => {
		h ??= [];
		h.push({
			message: messages[getRandomInt(messages.length)],
			type: types[getRandomInt(types.length)],
			details: "details "+ messages[getRandomInt(messages.length)] + "\n"+ messages[getRandomInt(messages.length)] + "\n"+ messages[getRandomInt(messages.length)] + "\n"+ messages[getRandomInt(messages.length)] + "\n"+ messages[getRandomInt(messages.length)] + "\n"+ messages[getRandomInt(messages.length)] + "\n"+ messages[getRandomInt(messages.length)] + "\n"+ messages[getRandomInt(messages.length)] + "\n"+ messages[getRandomInt(messages.length)] + "\n"+ messages[getRandomInt(messages.length)] + "\n"+ messages[getRandomInt(messages.length)] + "\n"+ messages[getRandomInt(messages.length)] + "\n"+ messages[getRandomInt(messages.length)] + "\n",
			timestamp: Date.now()
		});
		return h;
	});
	}
</script>

<Button on:click={addFakeLog}>Add Log</Button>

<div class="my-1 flex items-center justify-between">
	<h2 class="text-3xl font-bold tracking-tight">Instance Dashboard (debugmenuapi)</h2>
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
						<GroupTree {group} {commandSender}></GroupTree>
					{/each}
				</div>
			</Tabs.Content>
		{/each}
	</Tabs.Root>
{/if}
