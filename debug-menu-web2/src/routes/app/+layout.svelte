<script lang="ts">
	import '../../app.pcss';

	import Navbar from '$lib/components/skeleton/navbar/navbar.svelte';
	import Sidebar from '$lib/components/skeleton/sidebar/sidebar.svelte';
	import {
		currentApplication,
		currentInstance,
		currentUser,
		currentTeam,
		currentBackendToken
	} from '$lib/appstate';
	import Breadcrumbs from '$lib/components/skeleton/navbar/breadcrumbs.svelte';
	import ConnectionStatus from '$lib/components/skeleton/navbar/connection-status.svelte';
	import { onMount } from 'svelte';
	import { DebugMenuBackend } from '$lib/backend/backend';
	import { get } from 'svelte/store';
	import { invalidateAll } from '$app/navigation';

	let screenWidth: number;
	onMount(async () => {
		sidebarVisible = screenWidth >= 640;

		let response = await DebugMenuBackend(
			fetch,
			get(currentBackendToken)!
		).processStartupEventsByUser(data.user.id);
		let startupEvents = await response.json();
		if (startupEvents.events.length > 0) {
			invalidateAll();
		}
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
