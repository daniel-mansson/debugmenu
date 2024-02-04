<script lang="ts">
	import { instanceConnection } from '$lib/WebsocketManager';
	import { AppShell, AppBar, Modal, LightSwitch } from '@skeletonlabs/skeleton';
	import type { LayoutData } from './$types';
	import { signIn, signOut } from '@auth/sveltekit/client';
	import type { LayoutServerData } from '../$types';
	import { onMount } from 'svelte';

	export let data: LayoutServerData;

	onMount(() => {
		console.log('layout');
		console.log(JSON.stringify(data));
	});

	$: msg = $instanceConnection?.latestMessage;
</script>

<!-- App Shell -->
<AppShell>
	<svelte:fragment slot="header">
		<!-- App Bar -->
		<AppBar padding="px-4 py-2" border="border-b border-surface-400-500-token">
			<svelte:fragment slot="lead">
				<a href="/"><strong class="text-lg uppercase font-mono">debugmenu.io</strong></a>
			</svelte:fragment>
			<svelte:fragment slot="trail">
				<LightSwitch />

				{$msg}

				{#if data.session}
					<div>{data.session.user?.name}</div>
					<button class="btn btn-sm border" on:click={() => signOut()}>Sign out</button>
				{:else}
					<button class="btn btn-sm border" on:click={() => signIn('google')}>Sign in</button>
				{/if}
			</svelte:fragment>
		</AppBar>
	</svelte:fragment>
	<!-- Page Route Content -->
	<slot />
</AppShell>
