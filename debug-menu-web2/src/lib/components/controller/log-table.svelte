<script lang="ts">
	import { writable } from 'svelte/store';

	import * as Table from '$lib/components/ui/table/index.js';
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

	function add() {
		$history.push({
			timestamp: Date.now(),
			message:
				'There was a time when he would have embraced the change that was coming. In his youth, he sought adventure and the unknown, but that had been years ago. He wished he could go back and learn to find the excitement that came with change but it was useless. That curiosity had long left him to where he had come to loathe anything that put him out of his comfort zone.',
			type: 'info',
			details: 'details'
		});
		$history = $history;
	}

	$: logs = $history ?? [];

	function getTimeWithMilliseconds(date: Date) {
		const t = date.toTimeString();
		return `${t.substring(0, 8)}${(date.getMilliseconds() / 1000).toFixed(3).substring(1)}`;
	}
</script>

<button on:click={add}>Add</button>
<Table.Root>
	<Table.Header>
		<Table.Row>
			<Table.Head class="w-0">Timestamp</Table.Head>
			<Table.Head class="w-0">Type</Table.Head>
			<Table.Head>Message</Table.Head>
		</Table.Row>
	</Table.Header>
	<Table.Body>
		{#each logs as entry, i (i)}
			<Table.Row class={i % 2 ? '' : ''}>
				<Table.Cell class="py-1 font-mono font-medium opacity-50"
					>{getTimeWithMilliseconds(new Date(entry.timestamp))}</Table.Cell
				>
				<Table.Cell class="py-1 ">
					{entry.type}
				</Table.Cell>
				<Table.Cell class="py-1 ">
					{entry.message}
				</Table.Cell>
			</Table.Row>
		{/each}
	</Table.Body>
</Table.Root>
