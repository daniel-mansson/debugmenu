<script lang="ts">
	import { applications, instances, tokens } from '$lib/appstate';

	export let application: string | undefined;
	export let token: string | undefined;
	export let instance: string | undefined;
</script>

<div
	class="ml-4 flex overflow-auto whitespace-nowrap py-1 font-mono text-sm text-gray-500 sm:my-auto sm:ml-12"
>
	{#if application}
		<a
			href="/app/{application}"
			class="mx-1 my-auto rounded-sm p-1 hover:bg-accent hover:text-accent-foreground active:outline active:outline-1"
			>{$applications.find((a) => a.id == application)?.name ?? application}</a
		>
	{/if}
	{#if token}
		<div class="my-auto">/</div>
		<a
			href="/app/{application}/{token}"
			class="mx-1 my-auto rounded-sm p-1 hover:bg-accent hover:text-accent-foreground active:outline active:outline-1"
			>{$tokens.find((t) => t.id == token)?.name ?? token}
		</a>
	{/if}
	{#if instance}
		<div class="my-auto">/</div>
		<a
			href="/app/{application}/{token}/{instance}"
			class="mx-1 my-auto rounded-sm p-1 hover:bg-accent hover:text-accent-foreground active:outline active:outline-1"
			>{$instances.find((i) => i.id == instance)?.name ?? instance}
		</a>
	{/if}
</div>
