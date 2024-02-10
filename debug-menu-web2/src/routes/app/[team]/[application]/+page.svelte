<script lang="ts">
	import { goto } from '$app/navigation';
	import { createApplication, createToken, currentTeam, tokens } from '$lib/appstate';
	import Button from '$lib/components/ui/button/button.svelte';
	import CreateTokenForm from './create-token-form.svelte';
	import { applications } from '$lib/appstate';

	export let data;

	let form: any;
	async function onSubmit(result: { name: string; applicationId: number; description: string }) {
		let created = await createToken(
			data.fetch,
			result.applicationId,
			result.name,
			result.description
		);
	}
	$: application = $applications.find((a) => a.id == data.applicationId);
</script>

<div class="my-1 flex items-center justify-between">
	<h2 class="text-3xl font-bold tracking-tight">
		{application?.name}
	</h2>
</div>

<Button
	on:click={() => {
		form.toggle();
	}}>Create Token</Button
>
<CreateTokenForm bind:this={form} on:submit={(e) => onSubmit(e.detail)} />

<div>
	<h3 class="text-1xl font-bold tracking-tight">Tokens</h3>
	{#each $tokens as token}
		<div class="my-3">
			<div>{token.name}</div>
			<div>{token.description}</div>
			<div>{token.token}</div>
		</div>
	{/each}
</div>
