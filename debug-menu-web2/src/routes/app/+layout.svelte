<script lang="ts">
	import '../../app.pcss';

	import Navbar from '$lib/components/skeleton/navbar/navbar.svelte';
	import Sidebar from '$lib/components/skeleton/sidebar/sidebar.svelte';
	import { currentApplication, currentInstance, currentUser, currentTeam } from '$lib/appstate';
	import Breadcrumbs from '$lib/components/skeleton/navbar/breadcrumbs.svelte';

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

<div class="absolute bottom-0 right-0 font-mono">
	{data.user.name}
	u:{$currentUser?.id}
	t:{$currentTeam}
</div>
