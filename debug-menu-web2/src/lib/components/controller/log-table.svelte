<script lang="ts">
	import { writable } from 'svelte/store';

	import { ScrollArea } from '$lib/components/ui/scroll-area/index';
	import * as Table from '$lib/components/ui/table/index.js';
	import { Input } from '$lib/components/ui/input/index.js';
	import { Label } from '$lib/components/ui/label/index.js';
	import { Button } from '$lib/components/ui/button/index.js';
	import Separator from '../ui/separator/separator.svelte';
	export let label: string;
	export let settings = {
		color: 'white'
	};
	type LogEntry = {
		timestamp: number;
		message: string;
		type: string;
		details: string;
	};

	export let state = writable<LogEntry>();
	export let history = writable<LogEntry[]>();

	$: logs = $history ?? [];
	$: filteredLogs = logs.filter(
		(entry, index) =>
			entry === selectedEntry || (
			(!pauseIndex || index < pauseIndex) &&
			entry.message.toLowerCase().includes(filterString.toLowerCase()))
	);

	function getTimeWithMilliseconds(date: Date) {
		const t = date.toTimeString();
		return `${t.substring(0, 8)}${(date.getMilliseconds() / 1000).toFixed(3).substring(1)}`;
	}

	function getTypeClass(type: string) {
		if (type === 'warning' || type === 'warn') return 'text-yellow-500';
		if (type === 'error' || type === 'exception' || type === 'assert') return 'text-red-500';
		return 'text-gray-500';
	}

	let pauseIndex: number | undefined = undefined;
	let maxVisible = 50;
	let filterString = '';

	function download(filename: string, text: string) {
		var element = document.createElement('a');
		element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
		element.setAttribute('download', filename);

		element.style.display = 'none';
		document.body.appendChild(element);

		element.click();

		document.body.removeChild(element);
	}

	function downloadFullLog() {
		let content = [];

		for (const entry of logs) {
			content.push(getTimeWithMilliseconds(new Date(entry.timestamp)) + ' ' + entry.type);
			content.push(entry.message);
			content.push(entry.details);
		}

		let file = content.join('\n');
		download('test.txt', file);
	}

	let selectedEntry: LogEntry | undefined = undefined;
	function selectRow(entry: LogEntry, index: number) {
		if(selectedEntry === entry) {
			selectedEntry = undefined;
		} else {
			selectedEntry = entry;
		}
		filteredLogs = filteredLogs;
	}
</script>

<div class="mb-2 flex gap-2">
	<div class="mr-12 text-lg font-medium">{label}</div>
	<div class="flex w-full max-w-sm flex-col gap-1.5">
		<Label for="filter">Quick Filter</Label>
		<Input bind:value={filterString} type="text" id="filter" placeholder="" class="" />
	</div>
	<div class="flex w-full max-w-32 flex-col gap-1.5">
		<Label for="visible">Limit ({logs.length})</Label>
		<Input
			bind:value={maxVisible}
			type="number"
			id="visible"
			min="0"
			step="10"
			placeholder=""
			class=""
		/>
	</div>

	<div class="mt-5 flex w-full max-w-24 flex-col gap-1.5">
		<Button on:click={() => (pauseIndex = pauseIndex ? undefined : logs.length)} variant="outline"
			>{pauseIndex ? `Resume (${logs.length - pauseIndex})` : 'Pause'}</Button
		>
	</div>

	<div class="ml-auto mt-5 flex w-full max-w-24 flex-col gap-1.5">
		<Button on:click={downloadFullLog} variant="outline">Download</Button>
	</div>
</div>
<div class="h-[80vh]">
	<ScrollArea class={selectedEntry ? 'h-[60vh]' : 'h-[80vh]'}>
		<Table.Root>
			<Table.Header>
				<Table.Row>
					<Table.Head class="w-0">Timestamp</Table.Head>
					<Table.Head class="w-0">Type</Table.Head>
					<Table.Head>Message</Table.Head>
				</Table.Row>
			</Table.Header>
			<Table.Body>
				{#each filteredLogs.filter((m, index) => index >= filteredLogs.length - maxVisible) as entry, i (i)}
					<Table.Row
						on:click={() => selectRow(entry, i)}
						class={entry === selectedEntry ? 'bg-gray-200 hover:bg-gray-300 dark:bg-gray-800 dark:hover:bg-gray-700' : ''}
					>
						<Table.Cell class="py-1 font-mono font-medium opacity-50"
							>{getTimeWithMilliseconds(new Date(entry.timestamp))}</Table.Cell
						>
						<Table.Cell class="py-1 {getTypeClass(entry.type)}">
							{entry.type}
						</Table.Cell>
						<Table.Cell class="w-full py-1">
							{entry.message}
						</Table.Cell>
					</Table.Row>
				{/each}
			</Table.Body>
		</Table.Root>
	</ScrollArea>
	{#if selectedEntry}
		<div class="h-[20vh]">
			<Separator></Separator>
			<ScrollArea class="h-full">
				<div class="whitespace-pre-wrap font-mono">{selectedEntry.details}</div>
			</ScrollArea>
		</div>
	{/if}
</div>
