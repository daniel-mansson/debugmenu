<script lang="ts">
	import { writable } from 'svelte/store';

	import { ScrollArea } from '$lib/components/ui/scroll-area/index';
	import * as Table from '$lib/components/ui/table/index.js';
	import { Input } from '$lib/components/ui/input/index.js';
	import { Label } from '$lib/components/ui/label/index.js';
	import { Button } from '$lib/components/ui/button/index.js';
	import Separator from '../ui/separator/separator.svelte';
	import { ClipboardCopyIcon } from 'lucide-svelte';
	import { toast } from 'svelte-sonner';
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
			entry === selectedEntry ||
			((!pauseIndex || index < pauseIndex) &&
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

	function getTypeBorderClass(type: string) {
		if (type === 'warning' || type === 'warn') return 'border-l border-l-yellow-500 ';
		if (type === 'error' || type === 'exception' || type === 'assert')
			return 'border-l border-l-red-500';
		return 'border-l border-l-gray-500';
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
		download('logs.txt', file);
	}

	let selectedEntry: LogEntry | undefined = undefined;
	function selectRow(entry: LogEntry, index: number) {
		if (selectedEntry === entry) {
			selectedEntry = undefined;
		} else {
			selectedEntry = entry;
		}
		filteredLogs = filteredLogs;
	}

	function copyToClipboard(entry: LogEntry | undefined) {
		if (!entry) return;

		let content = [];

		content.push(getTimeWithMilliseconds(new Date(entry.timestamp)) + ' ' + entry.type);
		content.push(entry.message);
		content.push(entry.details);
		let text = content.join('\n');

		const clipboardItem = new ClipboardItem({
			'text/plain': new Blob([text.trim()], { type: 'text/plain' })
		});
		navigator.clipboard.write([clipboardItem]);
		toast('Copied to Clipboard!');
	}

	let timestampVisible = true;
	let typeVisible = true;
</script>

<div class="mb-2 text-lg font-medium">{label}</div>
<div class="mb-2 grid grid-cols-2 gap-2 sm:grid-cols-4">
	<div class="flex w-full max-w-sm flex-col gap-1.5">
		<Label for="filter">Quick Filter</Label>
		<Input
			bind:value={filterString}
			type="text"
			id="filter"
			placeholder=""
			class="bg-white bg-opacity-50 dark:bg-black dark:bg-opacity-30"
		/>
	</div>
	<div class="flex w-full max-w-sm flex-col gap-1.5">
		<Label for="visible">Limit ({logs.length})</Label>
		<Input
			bind:value={maxVisible}
			type="number"
			id="visible"
			min="0"
			step="10"
			placeholder=""
			class="bg-white bg-opacity-50 dark:bg-black dark:bg-opacity-30"
		/>
	</div>

	<div class="flex w-full max-w-24 flex-col gap-1.5 sm:mt-5">
		<Button on:click={() => (pauseIndex = pauseIndex ? undefined : logs.length)} variant="outline"
			>{pauseIndex ? `Resume (${logs.length - pauseIndex})` : 'Pause'}</Button
		>
	</div>

	<div class="flex w-full max-w-24 flex-col gap-1.5 sm:ml-auto sm:mt-5">
		<Button on:click={downloadFullLog} variant="outline">Download</Button>
	</div>
</div>
<div class="h-[80vh]">
	<ScrollArea class={selectedEntry ? 'h-[60vh]' : 'h-[80vh]'}>
		<Table.Root>
			<Table.Header>
				<Table.Row>
					{#if timestampVisible}
						<Table.Head class="w-0"
							><button on:click={() => (timestampVisible = !timestampVisible)}>Timestamp</button
							></Table.Head
						>
					{/if}
					{#if typeVisible}
						<Table.Head class="w-0"
							><button on:click={() => (typeVisible = !typeVisible)}>Type</button></Table.Head
						>
					{/if}

					<Table.Head class="flex gap-2"
						><div class="my-auto mr-auto">Message</div>
						{#if !timestampVisible}
							<button class="line-through" on:click={() => (timestampVisible = !timestampVisible)}
								>Timestamp</button
							>
						{/if}
						{#if !typeVisible}
							<button class="line-through" on:click={() => (typeVisible = !typeVisible)}
								>Type</button
							>
						{/if}</Table.Head
					>
				</Table.Row>
			</Table.Header>
			<Table.Body class="bg-white bg-opacity-50 dark:bg-transparent">
				{#each filteredLogs.filter((e, index) => e === selectedEntry || index >= filteredLogs.length - maxVisible) as entry, i (i)}
					<Table.Row
						on:click={() => selectRow(entry, i)}
						class="{entry === selectedEntry
							? 'bg-gray-200 hover:bg-gray-300 dark:bg-gray-800 dark:hover:bg-gray-700'
							: ''} "
					>
						{#if timestampVisible}
							<Table.Cell class="py-1 font-mono font-medium opacity-50 "
								>{getTimeWithMilliseconds(new Date(entry.timestamp))}</Table.Cell
							>
						{/if}
						{#if typeVisible}
							<Table.Cell class="py-1 font-mono {getTypeClass(entry.type)}">
								{entry.type}
							</Table.Cell>
						{/if}
						<Table.Cell class="w-full py-1 {!typeVisible ? getTypeBorderClass(entry.type) : ''}">
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
				<div class="text-md my-1 flex gap-4">
					<div class="my-auto font-mono opacity-50">
						{getTimeWithMilliseconds(new Date(selectedEntry.timestamp))}
					</div>
					<div class="{getTypeClass(selectedEntry.type)} my-auto font-mono">
						{selectedEntry.type}
					</div>
					<div class="my-auto mr-8">{selectedEntry.message}</div>
					<Button
						on:click={() => copyToClipboard(selectedEntry)}
						variant="ghost"
						class="absolute right-1 ml-auto h-8 w-8 text-xs"
						><ClipboardCopyIcon class="absolute h-4 w-4 stroke-gray-500" /></Button
					>
				</div>

				<div class="whitespace-pre-wrap font-mono text-sm">{selectedEntry.details}</div>
			</ScrollArea>
		</div>
	{/if}
</div>
