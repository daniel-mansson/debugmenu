<script lang="ts">
	import { createEventDispatcher } from 'svelte';
	import { mode } from 'mode-watcher';
	import Switch from '../ui/switch/switch.svelte';
	import Label from '../ui/label/label.svelte';
	import { writable } from 'svelte/store';

	export let label: string;
	export let settings = {
		color: 'white'
	};
	export let state = writable<any>();

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

	function onValueChanged(value: boolean) {
		dispatch('change', value);
	}

	let checked = !!$state;
	state.subscribe((s) => {
		checked = !!s;
	});
</script>

<div class=" my-2 flex h-10 items-center justify-start space-x-2">
	<Switch onCheckedChange={onValueChanged} bind:checked />
	<Label>{label}</Label>
</div>
