<script lang="ts">
	import Button from '../ui/button/button.svelte';
	import Input from '../ui/input/input.svelte';
	import Label from '../ui/label/label.svelte';
	import { cn } from '$lib/utils';
	import { SplineIcon } from 'lucide-svelte';

	let className: string | undefined | null = undefined;
	export { className as class };

	let target = '';
	let isLoading = false;
	async function onSubmit(provider: string) {
		isLoading = true;
		target = provider;
		setTimeout(() => {
			isLoading = false;
		}, 3000);
	}
</script>

<div class={cn('grid gap-6', className)} {...$$restProps}>
	<Button
		variant="secondary"
		type="button"
		disabled={isLoading}
		on:click={() => onSubmit('github')}
	>
		{#if isLoading && target === 'github'}
			<SplineIcon class="mr-2 h-4 w-4 animate-spin" />
		{:else}
			<SplineIcon class="mr-2 h-4 w-4" />
		{/if}
		{' '}
		GitHub
	</Button>
	<Button
		variant="secondary"
		type="button"
		disabled={isLoading}
		on:click={() => onSubmit('google')}
	>
		{#if isLoading && target === 'google'}
			<SplineIcon class="mr-2 h-4 w-4 animate-spin" />
		{:else}
			<SplineIcon class="mr-2 h-4 w-4" />
		{/if}
		{' '}
		Google
	</Button>
</div>
