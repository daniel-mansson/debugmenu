<script lang="ts">
	import { createEventDispatcher } from 'svelte';
	import Button from '../ui/button/button.svelte';
	import { mode } from 'mode-watcher';

	export let label: string;
	export let color: string = 'gray';

	const lookupHSL: any = {
		red: { h: 15, s: 100 },
		green: { h: 90, s: 100 },
		grass: { h: 100, s: 50 },
		blue: { h: 220, s: 100 },
		yellow: { h: 55, s: 100 },
		purple: { h: 280, s: 100 },
		teal: { h: 180, s: 100 },
		orange: { h: 37, s: 100 },
		magenta: { h: 300, s: 100 },
		aqua: { h: 180, s: 60 },
		gray: { h: 200, s: 20 }
	};

	function toHsla(h: number, s: number, l: number, a: number) {
		return `hsla(${h}, ${s}%, ${l}%, ${a})`;
	}

	function getColorValue(c: string) {
		let value = lookupHSL[c];
		if (value !== undefined) {
			return value;
		}

		return { h: 200, s: 20 };
	}
	$: dark = $mode === 'dark';
	$: colorValue = getColorValue(color);
	$: bgColor = toHsla(colorValue.h, colorValue.s, dark ? 5 : 92, 0.9);
	$: outlineColor = toHsla(colorValue.h - 3, colorValue.s, dark ? 70 : 50, 1.0);

	let dispatch = createEventDispatcher();
</script>

<Button
	on:click={() => dispatch('click')}
	variant="empty"
	style="background-color:{bgColor}; outline-color:{outlineColor}"
	class="overflow-hidden rounded-sm font-mono shadow-md shadow-gray-300 outline outline-1 hover:shadow-lg
	hover:shadow-gray-300 hover:outline-offset-1 active:outline-double active:outline-4 
	active:outline-offset-1 dark:bg-neutral-900  dark:shadow-gray-700
	 dark:active:bg-neutral-800 dark:active:outline-4">{label}</Button
>
