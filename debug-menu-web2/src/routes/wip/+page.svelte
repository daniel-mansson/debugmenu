<script lang="ts">
	import * as Tabs from '$lib/components/ui/tabs';
	import * as Card from '$lib/components/ui/card';
	import Button from '$lib/components/ui/button/button.svelte';
	import Group from '$lib/components/controller/group.svelte';
	import SimpleButton from '$lib/components/controller/simple-button.svelte';
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
<div
	class="space-y-8 rounded-lg border-2 border-dashed border-gray-200 p-4 pt-6 dark:border-gray-700"
>
	<Group title="gameplay">
		<div slot="content">
			<div class="grid grid-cols-3 gap-4 md:grid-cols-4 lg:grid-cols-6">
				<Button
					variant="empty"
					class="bg-yellow-50 outline outline-1 outline-yellow-400 hover:bg-yellow-100 active:border-2 active:bg-yellow-200 dark:bg-neutral-900 dark:active:bg-neutral-800 dark:active:outline-2"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-red-50 outline outline-1 outline-red-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
			</div>
		</div>
	</Group>
	<Group title="settings" color="red">
		<div slot="content">
			<div class="grid grid-cols-3 gap-4 md:grid-cols-4 lg:grid-cols-6">
				<Button
					variant="empty"
					class="bg-yellow-50 outline outline-1 outline-yellow-400 hover:bg-yellow-100 active:border-2 active:bg-yellow-200 dark:bg-neutral-900 dark:active:bg-neutral-800 dark:active:outline-2"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-red-50 outline outline-1 outline-red-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
			</div>
		</div>
	</Group>
	<Group title="settings" color="gray">
		<div slot="content">
			<div class="grid grid-cols-3 gap-4 md:grid-cols-4 lg:grid-cols-6">
				<Button
					variant="empty"
					class="bg-yellow-50 outline outline-1 outline-yellow-400 hover:bg-yellow-100 active:border-2 active:bg-yellow-200 dark:bg-neutral-900 dark:active:bg-neutral-800 dark:active:outline-2"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-red-50 outline outline-1 outline-red-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
			</div>
		</div>
	</Group>
	<Group title="cqwcqc" color="gray">
		<div slot="content" class="mt-2 space-y-6">
			<Group title="erger" color="blue">
				<div slot="content">
					<div class="grid grid-cols-3 gap-4 md:grid-cols-4 lg:grid-cols-6">
						<SimpleButton label="Restart game 1" color="yellow" />
						<SimpleButton label="Restart game 2" color="red" />
						<SimpleButton label="Restart game 3" color="blue" />
					</div>
				</div>
			</Group>
			<Group title="asdf" color="green">
				<div slot="content">
					<div class="grid grid-cols-3 gap-4 md:grid-cols-4 lg:grid-cols-6">
						<Button
							variant="empty"
							class="bg-yellow-50 outline outline-1 outline-yellow-400 hover:bg-yellow-100 active:border-2 active:bg-yellow-200 dark:bg-neutral-900 dark:active:bg-neutral-800 dark:active:outline-2"
							>Restart game</Button
						>
						<Button
							variant="ghost"
							class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
							>Restart game</Button
						>
						<Button
							variant="ghost"
							class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
							>Restart game</Button
						>
						<Button
							variant="ghost"
							class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
							>Restart game</Button
						>
						<Button
							variant="ghost"
							class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
							>Restart game</Button
						>
						<Button
							variant="ghost"
							class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
							>Restart game</Button
						>
						<Button
							variant="ghost"
							class="bg-red-50 outline outline-1 outline-red-400 dark:bg-neutral-900"
							>Restart game</Button
						>
						<Button
							variant="ghost"
							class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
							>Restart game</Button
						>
					</div>
				</div>
			</Group>
		</div>
	</Group>
	<div
		class="rounded-md border-2 border-dashed border-blue-200 bg-gradient-to-tl from-transparent to-blue-100 p-4 dark:border-green-800 dark:to-green-950"
	>
		<div class="bg-blue relative -top-7 h-0 w-0 font-mono text-gray-700 dark:text-gray-200">
			gameplay/asdf/asdf
		</div>
		<div class="grid grid-cols-3 gap-4 md:grid-cols-4 lg:grid-cols-6">
			<Button
				variant="empty"
				class="bg-yellow-50 outline outline-1 outline-yellow-400 hover:bg-yellow-100 active:border-2 active:bg-yellow-200 dark:bg-neutral-900 dark:active:bg-neutral-800 dark:active:outline-2"
				>Restart game</Button
			>
			<Button
				variant="ghost"
				class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
				>Restart game</Button
			>
			<Button
				variant="ghost"
				class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
				>Restart game</Button
			>
			<Button
				variant="ghost"
				class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
				>Restart game</Button
			>
			<Button
				variant="ghost"
				class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
				>Restart game</Button
			>
			<Button
				variant="ghost"
				class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
				>Restart game</Button
			>
			<Button
				variant="ghost"
				class="bg-red-50 outline outline-1 outline-red-400 dark:bg-neutral-900">Restart game</Button
			>
			<Button
				variant="ghost"
				class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
				>Restart game</Button
			>
		</div>
	</div>
	<div
		class="space-y-4 rounded-md border-2 border-dashed border-gray-200 bg-gradient-to-tl from-transparent to-transparent p-4 dark:border-green-800 dark:to-green-950"
	>
		<div class="bg-blue relative -top-7 h-0 w-0 font-mono text-gray-700 dark:text-gray-200">
			gameplay
		</div>

		<div
			class="rounded-md border-2 border-dashed border-blue-200 bg-gradient-to-tl from-transparent to-blue-100 p-4 dark:border-green-800 dark:to-green-950"
		>
			<div class="bg-blue relative -top-7 h-0 w-0 font-mono text-gray-700 dark:text-gray-200">
				asdf/asdf
			</div>
			<div class="grid grid-cols-3 gap-4 md:grid-cols-4 lg:grid-cols-6">
				<Button
					variant="empty"
					class="bg-yellow-50 outline outline-1 outline-yellow-400 hover:bg-yellow-100 active:border-2 active:bg-yellow-200 dark:bg-neutral-900 dark:active:bg-neutral-800 dark:active:outline-2"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-red-50 outline outline-1 outline-red-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
			</div>
		</div>

		<div
			class="rounded-md border-2 border-dashed border-blue-200 bg-gradient-to-tl from-transparent to-blue-100 p-4 dark:border-green-800 dark:to-green-950"
		>
			<div class="bg-blue relative -top-7 h-0 w-0 font-mono text-gray-700 dark:text-gray-200">
				eragr/awdd
			</div>
			<div class="grid grid-cols-3 gap-4 md:grid-cols-4 lg:grid-cols-6">
				<Button
					variant="empty"
					class="bg-yellow-50 outline outline-1 outline-yellow-400 hover:bg-yellow-100 active:border-2 active:bg-yellow-200 dark:bg-neutral-900 dark:active:bg-neutral-800 dark:active:outline-2"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-red-50 outline outline-1 outline-red-400 dark:bg-neutral-900"
					>Restart game</Button
				>
				<Button
					variant="ghost"
					class="bg-blue-50 outline outline-1 outline-blue-400 dark:bg-neutral-900"
					>Restart game</Button
				>
			</div>
		</div>
	</div>
</div>
