<script lang="ts">
	import { createEventDispatcher } from 'svelte';
	import Button from '../ui/button/button.svelte';
	import { mode } from 'mode-watcher';
	import * as Popover from '../ui/popover';
	import * as Form from '$lib/components/ui/form';
	import { superValidateSync } from 'sveltekit-superforms/server';
	import { z } from 'zod';
	import { CaretDown } from 'radix-icons-svelte';

	export let label: string;
	export let description: string | undefine = undefined;
	export let settings = {
		color: 'white'
	};
	export let properties: any = {};

	let props: any = {};
	let propertiesList: any[] = [];
	for (let key in properties) {
		propertiesList.push({
			key,
			prop: properties[key]
		});
		let p = properties[key];
		if (p.type === 'string') {
			props[key] = z.string().default('');
		} else if (p.type === 'number' && p.format === 'integer') {
			props[key] = z.number().int().default(0);
		} else if (p.type === 'number') {
			props[key] = z.number().default(0);
		} else if (p.type === 'boolean') {
			props[key] = z.boolean().default(false);
		}
	}

	export const schema = z.object(props);

	const form = superValidateSync(schema);

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
</script>

<Popover.Root portal={null}>
	<Popover.Trigger asChild let:builder>
		<Button
			builders={[builder]}
			variant="empty"
			style="background-color:{bgColor}; outline-color:{outlineColor}"
			class="mb-2 mr-2 overflow-hidden rounded-sm font-mono shadow-md shadow-gray-300 outline outline-1 hover:shadow-lg
hover:shadow-gray-300 hover:outline-offset-1 active:outline-double active:outline-4 
active:outline-offset-1 dark:bg-neutral-900  dark:shadow-gray-800
 dark:active:bg-neutral-800 dark:active:outline-4">{label} <CaretDown /></Button
		>
	</Popover.Trigger>
	<Popover.Content class="w-80">
		<div class="grid gap-4">
			<div class="">
				<h4 class="font-medium leading-none">{label}</h4>
				{#if description}
					<p class="text-sm text-muted-foreground">{description}</p>
				{/if}
			</div>
			<Form.Root
				class="space-y-4"
				let:config
				options={{
					onUpdate: async (f) => {
						console.log('sending');
						console.log(f);
						if (f.form.valid) {
							console.log('submitting');
							let data = f.form.data;
							for (let p of propertiesList) {
								if (p.prop.type === 'number') {
									data[p.key] = +data[p.key];
								}
							}
							console.log(data);

							dispatch('submit', data);
						}
					},
					dataType: 'json',
					SPA: true
				}}
			>
				{#each propertiesList as i}
					<Form.Field {config} name={i.key}>
						<Form.Item class="">
							{#if i.prop.type === 'boolean'}
								<Form.Checkbox></Form.Checkbox>
								<Form.Label class="">{i.key}</Form.Label>
							{:else}
								<Form.Label class="">{i.key}</Form.Label>
								<Form.Input
									value={i.prop.type === 'number' ? 0 : ''}
									type={i.prop.type}
									step={i.prop.format === 'integer' ? '1' : 'any'}
								></Form.Input>
							{/if}
						</Form.Item>
						<Form.Validation></Form.Validation>
					</Form.Field>
				{/each}
				<Button
					variant="empty"
					type="submit"
					style="background-color:{bgColor}; outline-color:{outlineColor}"
					class="my-2 ml-auto mr-2 w-24 overflow-hidden rounded-sm font-mono shadow-md shadow-gray-300 outline outline-1
hover:shadow-lg hover:shadow-gray-300 hover:outline-offset-1 active:outline-double 
active:outline-4 active:outline-offset-1  dark:bg-neutral-900
 dark:shadow-gray-800 dark:active:bg-neutral-800 dark:active:outline-4">Send</Button
				>
			</Form.Root>
		</div>
	</Popover.Content>
</Popover.Root>
