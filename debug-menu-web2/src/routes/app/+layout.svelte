<script lang="ts">
	import '../../app.pcss';

	import Navbar from '$lib/components/skeleton/navbar/navbar.svelte';
	import Sidebar from '$lib/components/skeleton/sidebar/sidebar.svelte';
	import { currentApplication, currentInstance, currentUser, currentTeam } from '$lib/appstate';
	import Breadcrumbs from '$lib/components/skeleton/navbar/breadcrumbs.svelte';
	import ConnectionStatus from '$lib/components/skeleton/navbar/connection-status.svelte';
	import { onMount } from 'svelte';

	let screenWidth: number;
	onMount(() => {
		sidebarVisible = screenWidth >= 640;
	});
	let sidebarVisible = true;
	function toggleSidebar() {
		sidebarVisible = !sidebarVisible;
	}

	export let data;
</script>

<svelte:window bind:innerWidth={screenWidth} />

<Navbar on:menuClicked={toggleSidebar}>
	<Breadcrumbs slot="route" {...data}></Breadcrumbs>
	<ConnectionStatus slot="right"></ConnectionStatus>
</Navbar>

<Sidebar {sidebarVisible} on:hideClicked={toggleSidebar} />

<div class="p-4 {sidebarVisible ? 'sm:ml-64' : ''}">
	<slot />
</div>
