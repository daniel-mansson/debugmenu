<script lang="ts">
	import { teams, applications, instances, tokens } from '$lib/appstate';

	export let team: string | undefined = undefined;
	export let application: string | undefined = undefined;
	export let token: string | undefined = undefined;
	export let instance: string | undefined = undefined;
</script>

<div
	class="ml-4 flex overflow-auto whitespace-nowrap py-1 font-mono text-sm text-gray-500 sm:my-auto sm:ml-12"
>
	{#if team}
		<a
			href="/app/{team}"
			class="mx-1 my-auto rounded-sm p-1 hover:bg-accent hover:text-accent-foreground active:outline active:outline-1"
			>{$teams.find((a) => a.id == team)?.name ?? team}</a
		>
		{#if application}
			<div class="my-auto">/</div>
			<a
				href="/app/{team}/{application}"
				class="mx-1 my-auto rounded-sm p-1 hover:bg-accent hover:text-accent-foreground active:outline active:outline-1"
				>{$applications.find((a) => a.id == application)?.name ?? application}</a
			>
			{#if token}
				<div class="my-auto">/</div>
				<a
					href="/app/{team}/{application}/{token}"
					class="mx-1 my-auto rounded-sm p-1 hover:bg-accent hover:text-accent-foreground active:outline active:outline-1"
					>{$tokens.find((t) => t.id == token)?.name ?? token}
				</a>
				{#if instance}
					<div class="my-auto">/</div>
					<a
						href="/app/{team}/{application}/{token}/{instance}"
						class="mx-1 my-auto rounded-sm p-1 hover:bg-accent hover:text-accent-foreground active:outline active:outline-1"
						>{$instances.find((i) => i.id == instance)?.name ?? instance}
					</a>
				{/if}
			{/if}
		{/if}
	{/if}
</div>
