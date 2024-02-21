<script lang="ts">
	import { mode } from 'mode-watcher';

	export let title: string;
	export let collapsed: boolean = false;
	export let color: string = 'transparent';

	function getValidBorderColor(c: string) {
		let bgColor = getValidBackgroundColor(c);
		return bgColor === 'transparent' ? 'gray' : bgColor;
	}
	function getValidBackgroundColor(c: string) {
		return c;
	}

	const lookupHSL: any = {
		red: { h: 15, s: 70 },
		green: { h: 90, s: 70 },
		grass: { h: 100, s: 50 },
		blue: { h: 220, s: 70 },
		yellow: { h: 55, s: 70 },
		purple: { h: 280, s: 70 },
		teal: { h: 180, s: 70 },
		orange: { h: 37, s: 70 },
		magenta: { h: 300, s: 70 },
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
	$: bgColor2 = toHsla(colorValue.h, colorValue.s, dark ? 5 : 95, 0.9);
	$: outlineColor = toHsla(colorValue.h, colorValue.s, dark ? 25 : 60, 1.0);
</script>

<div
	class="min-h-10 rounded-md border-2 border-dashed p-4"
	style="border-color: {outlineColor}; background-color: {bgColor2}; "
>
	<div class="relative -left-4 -top-10 h-0 w-0 font-mono text-gray-700 dark:text-gray-200">
		{title}
	</div>
	{#if !collapsed}
		<slot name="content" />
	{/if}
</div>
