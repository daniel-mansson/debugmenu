<script lang="ts">
	import { createEventDispatcher } from 'svelte';
	import { mode } from 'mode-watcher';
	import { writable } from 'svelte/store';
	import Label from '../ui/label/label.svelte';
	import { Input } from '../ui/input';
	import Button from '../ui/button/button.svelte';

	export let label: string;
	export let settings = {
		color: 'white'
	};
	export let state = writable<any>();
	export let maxLength: number | undefined = undefined;

	const lookupHSL: any = {
		red: { h: 15, s: 100, l: 0 },
		green: { h: 90, s: 100, l: 0 },
		grass: { h: 100, s: 50, l: 0 },
		blue: { h: 220, s: 100, l: 0 },
		yellow: { h: 55, s: 100, l: 0 },
		purple: { h: 280, s: 100, l: 0 },
		teal: { h: 180, s: 100, l: 0 },
		orange: { h: 37, s: 100, l: 0 },
		magenta: { h: 300, s: 100, l: 0 },
		aqua: { h: 180, s: 60, l: 0 },
		gray: { h: 200, s: 20, l: 0 },
		white: { h: 200, s: 20, l: 6 }
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
	$: colorValue = getColorValue(settings.color ?? 'white');
	$: bgColor = toHsla(colorValue.h, colorValue.s, dark ? 5 : 92 + colorValue.l, 0.9);
	$: outlineColor = toHsla(colorValue.h - 3, colorValue.s, dark ? 70 : 50, 1.0);

	let dispatch = createEventDispatcher();

	let value = $state?.value ?? '';
	state.subscribe((s) => {
		if (s) {
			value = s?.value;
		}
	});
</script>

<form
	on:submit={() => {
		dispatch('submit', value);
	}}
	class="mb-2 mr-2 flex w-full items-center space-x-1.5"
>
	<Input
		bind:value
		maxlength={maxLength === 0 ? undefined : maxLength}
		style="border-color:{outlineColor}"
		class="max-w-sm bg-white bg-opacity-60 dark:bg-black dark:bg-opacity-30"
		type="text"
		placeholder={label}
	/>
	<Button
		type="submit"
		variant="empty"
		style="background-color:{bgColor}; outline-color:{outlineColor}"
		class="h-9 overflow-hidden rounded-sm font-mono shadow-md shadow-gray-300 outline outline-1 hover:shadow-lg
hover:shadow-gray-300 hover:outline-offset-1 active:outline-double active:outline-4 
active:outline-offset-1 dark:bg-neutral-900  dark:shadow-gray-800
 dark:active:bg-neutral-800 dark:active:outline-4">{label}</Button
	>
</form>
