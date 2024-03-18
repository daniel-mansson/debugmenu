<script lang="ts">
	import * as Tabs from '$lib/components/ui/tabs';
	import * as Card from '$lib/components/ui/card';
	import Button from '$lib/components/ui/button/button.svelte';
	import Group from '$lib/components/controller/group.svelte';
	import SimpleButton from '$lib/components/controller/simple-button.svelte';
	import Toggle from '$lib/components/controller/toggle.svelte';
	import { writable } from 'svelte/store';
	import ModalButton from '$lib/components/controller/modal-button.svelte';
	import Slider from '$lib/components/controller/slider.svelte';
	import SingleText from '$lib/components/controller/single-text.svelte';

	let toggleState = writable<boolean>(false);
	let sliderState = writable<number>(0);

	function toggleChange(evt: CustomEvent) {
		toggleState.set(evt.detail);
	}

	let props = {
		someString: {
			type: 'string',
			description: 'Some string'
		},
		someBoolean: {
			type: 'boolean',
			description: 'Some boolean'
		},
		someNumber: {
			type: 'number',
			description: 'some number'
		},
		someInteger: {
			type: 'number',
			format: 'integer',
			description: 'some integer'
		}
	};

	function randSlider() {
		sliderState.set({
			value: Math.random() * 100
		});
	}
</script>

<Button on:click={randSlider}>random slider</Button>
<div class="my-1 flex items-center justify-between">
	<h2 class="text-3xl font-bold tracking-tight">Instance Dashboard</h2>
	<div class="flex items-center space-x-2">Status and Settings</div>
</div>
<Tabs.Root value="overview" class="my-2 space-y-1">
	<Tabs.List class=" ">
		<Tabs.Trigger value="commands">Commands</Tabs.Trigger>
		<Tabs.Trigger value="logs">Logs</Tabs.Trigger>
		<Tabs.Trigger value="stats">Statistics</Tabs.Trigger>
		<Tabs.Trigger value="errors">Errors</Tabs.Trigger>
	</Tabs.List>
</Tabs.Root>

<div
	class="space-y-8 rounded-lg border-2 border-dashed border-gray-200 p-4 pt-6 dark:border-gray-700"
>
	<Group title="cqwcqc" color="red">
		<div slot="content" class="mt-2 space-y-6">
			<Group title="erger" color="blue">
				<div slot="content">
					<SimpleButton label="Yellow" settings={{ color: 'yellow' }} />
					<ModalButton label="Modal" properties={props} settings={{ color: 'red' }} />
					<SingleText label="SetText" settings={{ color: 'red' }} />
					<Toggle label="Red" settings={{ color: 'blue' }} state={toggleState} />
					<Toggle label="Red" state={toggleState} on:change={toggleChange} />
					<SimpleButton label="Red" settings={{ color: 'red' }} />
					<SimpleButton label="Red" settings={{ color: 'red' }} />
					<Slider
						label="Red"
						range={{ min: 10, max: 500, step: 5 }}
						settings={{ color: 'red' }}
						state={sliderState}
					/>
					<Slider label="Time of something  " settings={{ color: 'green' }} state={sliderState} />
				</div>
			</Group>
			<Group title="asdf" color="green">
				<div slot="content">
					<div class="grid grid-cols-3 gap-4 md:grid-cols-4 lg:grid-cols-6">
						<SimpleButton label="teal" settings={{ color: 'teal' }} />
					</div>
				</div>
			</Group>
		</div>
	</Group>
</div>

{$toggleState}
