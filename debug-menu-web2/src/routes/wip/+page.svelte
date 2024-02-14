<script lang="ts">
	import * as Tabs from '$lib/components/ui/tabs';
	import * as Card from '$lib/components/ui/card';
	import Button from '$lib/components/ui/button/button.svelte';
	import { ArrowRightCircleIcon } from 'lucide-svelte';
</script>

<div class="my-1 flex items-center justify-between">
	<h2 class="text-3xl font-bold tracking-tight">Instance Dashboard</h2>
	<div class="flex items-center space-x-2">Status and Settings</div>
</div>
<Tabs.Root value="overview" class="my-2 space-y-1">
	<Tabs.List class=" ">
		<Tabs.Trigger value="overview">Overview</Tabs.Trigger>
		<Tabs.Trigger value="overv7iew">Overview</Tabs.Trigger>
		<Tabs.Trigger value="reports">Reports</Tabs.Trigger>
		<Tabs.Trigger value="notifications">Notifications</Tabs.Trigger>
	</Tabs.List>
</Tabs.Root>
<div class="rounded-lg border-2 border-dashed border-gray-200 p-4 dark:border-gray-700">
	<div class="text-secondary-foreground">Group name</div>
	<div
		class="mb-4 grid grid-cols-3 gap-4 border-2 border-dashed border-gray-100 p-2 dark:border-gray-800 md:grid-cols-4 lg:grid-cols-6"
	>
		<Button
			variant="empty"
			class="bg-yellow-50 outline outline-1 outline-yellow-400 hover:bg-yellow-100 active:border-2 active:bg-yellow-200 dark:bg-neutral-900 dark:active:bg-neutral-800 dark:active:outline-2"
			>Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button variant="ghost" class="bg-red-50 outline outline-1 outline-red-400 dark:bg-neutral-900"
			>Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
	</div>
	<div class="text-secondary-foreground">Group name</div>
	<div
		class="mb-4 grid grid-cols-3 gap-4 border-2 border-dashed border-gray-100 p-2 dark:border-gray-800 md:grid-cols-4 lg:grid-cols-6"
	>
		<Button
			variant="empty"
			class="bg-yellow-50 outline outline-1 outline-yellow-400 hover:bg-yellow-100 active:border-2 active:bg-yellow-200 dark:bg-neutral-900 dark:active:bg-neutral-800 dark:active:outline-2"
			>Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button variant="ghost" class="bg-red-50 outline outline-1 outline-red-400 dark:bg-neutral-900"
			>Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
	</div>
	<div class="text-secondary-foreground">Group name</div>
	<div
		class="mb-4 grid grid-cols-3 gap-4 border-2 border-dashed border-gray-100 p-2 dark:border-gray-800 md:grid-cols-4 lg:grid-cols-6"
	>
		<Button
			variant="empty"
			class="bg-yellow-50 outline outline-1 outline-yellow-400 hover:bg-yellow-100 active:border-2 active:bg-yellow-200 dark:bg-neutral-900 dark:active:bg-neutral-800 dark:active:outline-2"
			>Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
		<Button variant="ghost" class="bg-red-50 outline outline-1 outline-red-400 dark:bg-neutral-900"
			>Restart game</Button
		>
		<Button
			variant="ghost"
			class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900">Restart game</Button
		>
	</div>
</div>
