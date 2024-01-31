<script lang="ts">
	import '../../app.pcss';
	import { Button } from '$lib/components/ui/button';
	import {
		Sun,
		Moon,
		AppWindowIcon,
		Cog,
		TicketIcon,
		MoreHorizontal,
		Trash,
		Tags,
		User,
		Calendar,
		CloudOff,
		ChevronsLeftIcon
	} from 'lucide-svelte';
	import icon from '$lib/assets/up_arrow.svg';

	import { toggleMode } from 'mode-watcher';
	import { Separator } from '$lib/components/ui/separator';
	import { TeamSwitcher } from '$lib/components/dashboard';
	import * as DropdownMenu from '$lib/components/ui/dropdown-menu';
	import * as Avatar from '$lib/components/ui/avatar';
	import * as Tabs from '$lib/components/ui/tabs';
	import Navbar from '$lib/components/skeleton/navbar/navbar.svelte';
	import Sidebar from '$lib/components/skeleton/sidebar/sidebar.svelte';
	import { currentInstance } from '$lib/appstate';
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

{JSON.stringify(data)}

<Sidebar {sidebarVisible} on:hideClicked={toggleSidebar} />

<div class="p-4 {sidebarVisible ? 'sm:ml-64' : ''}">
	<slot />
</div>
