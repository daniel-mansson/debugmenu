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
		red: { h: 15, s: 70, l: 0 },
		green: { h: 90, s: 70, l: 0 },
		grass: { h: 100, s: 50, l: 0 },
		blue: { h: 220, s: 70, l: 0 },
		yellow: { h: 55, s: 70, l: 0 },
		purple: { h: 280, s: 70, l: 0 },
		teal: { h: 180, s: 70, l: 0 },
		orange: { h: 37, s: 70, l: 0 },
		magenta: { h: 300, s: 70, l: 0 },
		aqua: { h: 180, s: 60, l: 0 },
		gray: { h: 200, s: 20, l: 0 }
	};

	function toHsla(h: number, s: number, l: number, a: number) {
		return `hsla(${h}, ${s}%, ${l}%, ${a})`;
	}

	function getColorValue(c: string) {
		let value = lookupHSL[c];
		if (value !== undefined) {
			return value;
		}

		return { h: hashCode(title) % 360, s: 15, l: 0 };
	}
	function randomInt(max: number) {
		return Math.floor(Math.random() * max);
	}
	function hashCode(s: string): number {
		let h: number = 0;
		for (let i = 0; i < s.length; i++) h = (Math.imul(31, h) + s.charCodeAt(i)) | 0;

		return h;
	}

	$: dark = $mode === 'dark';
	$: colorValue = getColorValue(color);
	$: bgColor2 = toHsla(
		colorValue.h,
		colorValue.s,
		dark ? 5 - colorValue.l : 97 + colorValue.l,
		0.5
	);
	$: outlineColor = toHsla(
		colorValue.h,
		colorValue.s,
		dark ? 25 - colorValue.l : 60 + colorValue.l,
		1.0
	);
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
