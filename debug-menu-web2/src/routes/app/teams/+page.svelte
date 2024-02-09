<script lang="ts">
	import Button from '$lib/components/ui/button/button.svelte';
	import * as Select from '$lib/components/ui/select';
	import * as Dialog from '$lib/components/ui/dialog';
	import Input from '$lib/components/ui/input/input.svelte';
	import Label from '$lib/components/ui/label/label.svelte';
	import * as Tabs from '$lib/components/ui/tabs';
	import * as Form from '$lib/components/ui/form';
	import { z } from 'zod';
	import type { SuperValidated } from 'sveltekit-superforms';
	import { superValidate, superValidateSync } from 'sveltekit-superforms/client';

	const schema = z.object({
		name: z.string().min(3).max(16)
	});
	let showTeamDialog = false;
	export let data;

	async function submitForm() {}
</script>

<div class="my-1 flex items-center justify-between">
	<h2 class="text-3xl font-bold tracking-tight">Teams</h2>
</div>

<Button on:click={() => (showTeamDialog = true)}>Create team</Button>
{JSON.stringify(data.form)}
<Dialog.Root bind:open={showTeamDialog}>
	<Dialog.Content>
		<Dialog.Header>
			<Dialog.Title>Create team</Dialog.Title>
			<Dialog.Description>Add a new team to manage applications and users.</Dialog.Description>
		</Dialog.Header>
		<Form.Root
			{schema}
			form={data.form}
			let:config
			options={{
				onSubmit: (f) => {
					console.log(JSON.stringify(f));
				},
				dataType: 'json',
				applyAction: false
			}}
		>
			<Form.Field {config} name="name">
				<Form.Item>
					<Form.Label>Team name</Form.Label>
					<Form.Input />
					<Form.Description />
					<Form.Validation />
				</Form.Item>
			</Form.Field>
			<Dialog.Footer>
				<Button variant="outline" on:click={() => (showTeamDialog = false)}>Cancel</Button>
				<Button type="submit">Continue</Button>
			</Dialog.Footer>
		</Form.Root>
	</Dialog.Content>
</Dialog.Root>
