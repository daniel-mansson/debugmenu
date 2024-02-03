<script lang="ts">
	import '../../app.pcss';

	import Navbar from '$lib/components/skeleton/navbar/navbar.svelte';
	import Sidebar from '$lib/components/skeleton/sidebar/sidebar.svelte';
	import { currentApplication, currentInstance } from '$lib/appstate';
	import Breadcrumbs from '$lib/components/skeleton/navbar/breadcrumbs.svelte';
	import { signIn, signOut } from '@auth/sveltekit/client';

	let sidebarVisible = false;
	function toggleSidebar() {
		sidebarVisible = !sidebarVisible;
	}

	export let data;
</script>

<Navbar on:menuClicked={toggleSidebar}>
	<Breadcrumbs slot="route" {...data}></Breadcrumbs>
</Navbar>

<Sidebar {sidebarVisible} on:hideClicked={toggleSidebar} />

<div class="p-4 {sidebarVisible ? 'sm:ml-64' : ''}">
	<slot />
</div>

<div class="absolute bottom-0 right-0">
	{JSON.stringify(data)}
	{$currentApplication}

	{#if data.session}
		<div>{data.session.user?.name}</div>
		<button class="btn btn-sm border" on:click={() => signOut()}>Sign out</button>
	{:else}
		<button class="btn btn-sm border" on:click={() => signIn('google')}>Sign in</button>
	{/if}
</div>
