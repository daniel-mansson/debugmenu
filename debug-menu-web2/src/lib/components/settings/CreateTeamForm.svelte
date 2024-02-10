<script lang="ts">
	import Button from '$lib/components/ui/button/button.svelte';
	import * as Dialog from '$lib/components/ui/dialog';
	import * as Form from '$lib/components/ui/form';

	let showTeamDialog = false;
</script>

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
