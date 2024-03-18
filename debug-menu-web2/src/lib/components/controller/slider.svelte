<script lang="ts">
	import { createEventDispatcher } from 'svelte';
	import { mode } from 'mode-watcher';
	import { writable } from 'svelte/store';
	import Label from '../ui/label/label.svelte';
	import Slider from '../ui/slider/slider.svelte';
	import Input from '../ui/input/input.svelte';

	export let label: string;
	export let settings = {
		color: 'white'
	};
	export let state = writable<any>();
	export let range = {
		min: 0,
		max: 100,
		step: 0.1
	};

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

	let lock = false;

	function onValueChanged(v: number) {
		if (inputValue !== +v) {
			dispatch('change', v);
			inputValue = +v;
			sliderValue = [+v];
		}
	}

	state.subscribe((s) => {
		if (s?.value) {
			inputValue = s?.value;
			sliderValue = [s?.value];
		}
	});

	let value = $state?.value ?? 0;
	let sliderValue = [value];
	let inputValue = value;
</script>

<div
	style="border-color:{outlineColor};"
	class="mb-2 mr-2 flex h-12 shrink items-center justify-start gap-4 space-x-2 rounded-md border py-5 pl-2"
>
	<Label>{label}</Label>
	<Slider
		onValueChange={(v) => onValueChanged(v[0])}
		step={range.step}
		min={range.min}
		max={range.max}
		bind:value={sliderValue}
		on:change={() => console.log('asdfgas')}
	></Slider>
	<Input
		type="number"
		class="w-20 border-none bg-white bg-opacity-20 dark:bg-black dark:bg-opacity-20"
		step={range.step}
		min={range.min}
		max={range.max}
		bind:value={inputValue}
		on:change={(evt) => onValueChanged(inputValue)}
	></Input>
</div>
