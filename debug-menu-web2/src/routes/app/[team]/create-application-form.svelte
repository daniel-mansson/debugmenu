<script lang="ts">
	import Button from '$lib/components/ui/button/button.svelte';
	import * as Dialog from '$lib/components/ui/dialog';
	import * as Form from '$lib/components/ui/form';
	import { schema } from './schema';
	import { currentTeam } from '$lib/appstate';
	import { superValidateSync } from 'sveltekit-superforms/server';
	import { createEventDispatcher } from 'svelte';

	const form = superValidateSync(schema);
	let showDialog = false;
	export let loading = false;

	const dispatch = createEventDispatcher();

	export function toggle() {
		console.log('toggle');
		showDialog = !showDialog;
	}
	form.data.teamId = $currentTeam!;
</script>

<Dialog.Root bind:open={showDialog}>
	<Dialog.Content>
		<Dialog.Header>
			<Dialog.Title>Create application</Dialog.Title>
			<Dialog.Description
				>Add a new application to manage tokens and running instances.</Dialog.Description
			>
		</Dialog.Header>
		<Form.Root
			{schema}
			{form}
			let:config
			options={{
				onUpdate: async (f) => {
					if (f.form.valid) {
						loading = true;
						await dispatch('submit', f.form.data);
						loading = false;
						showDialog = false;
					}
				},
				dataType: 'json',
				SPA: true
			}}
		>
			<Form.Field {config} name="name">
				<Form.Item>
					<Form.Label>Application name</Form.Label>
					<Form.Input />
					<Form.Description />
					<Form.Validation />
				</Form.Item>
			</Form.Field>
			<Dialog.Footer>
				<Button
					disabled={loading}
					variant="outline"
					on:click={() => {
						showDialog = false;
					}}>Cancel</Button
				>
				<Button disabled={loading} type="submit">Continue</Button>
			</Dialog.Footer>
		</Form.Root>
	</Dialog.Content>
</Dialog.Root>
