<script lang="ts">
	import { goto } from '$app/navigation';
	import { createApplication, currentTeam } from '$lib/appstate';
	import Button from '$lib/components/ui/button/button.svelte';
	import CreateApplicationForm from './create-application-form.svelte';

	export let data;

	let form: any;
	async function onSubmit(result: { name: string; teamId: number }) {
		let created = await createApplication(data.fetch, result.name, result.teamId);
		goto(`/app/${$currentTeam}/${created.id}`);
	}
</script>

<div class="my-1 flex items-center justify-between">
	<h2 class="text-3xl font-bold tracking-tight">Applications</h2>
</div>

<Button
	on:click={() => {
		form.toggle();
	}}>Create Application</Button
>
<CreateApplicationForm bind:this={form} on:submit={(e) => onSubmit(e.detail)}
></CreateApplicationForm>
