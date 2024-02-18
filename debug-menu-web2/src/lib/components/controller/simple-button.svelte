<script lang="ts">
	import { createEventDispatcher } from 'svelte';
	import Button from '../ui/button/button.svelte';

	export let label: string;
	export let color: string = 'gray';

	function getValidColor(c: string) {
		return c;
	}
	$: buttonColor = getValidColor(color);

	let dispatch = createEventDispatcher();
</script>

<Button
	on:click={() => dispatch('click')}
	variant="empty"
	class="bg-{buttonColor}-50 overflow-hidden font-mono outline outline-1 outline-{buttonColor}-400 hover:bg-{buttonColor}-100 active:border-2 active:bg-{buttonColor}-200 dark:bg-neutral-900 dark:active:bg-neutral-800 dark:active:outline-2"
	>{label}</Button
>
