<script lang="ts">
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
	import { createEventDispatcher } from 'svelte';

	export let sidebarVisible: boolean;
	const dispatch = createEventDispatcher();
</script>

<aside
	id="default-sidebar"
	class="fixed left-0 top-0 z-40 h-screen w-64 {sidebarVisible
		? ''
		: '-translate-x-full'} border-r-2 transition-transform"
	aria-label="Sidebar"
>
	<div class=" h-[calc(100vh-3rem)] overflow-y-auto bg-primary-foreground px-3 py-4">
		<div class="flex">
			<a href="/app" class="mb-2 flex items-center space-x-2 rtl:space-x-reverse">
				<img src={icon} class="h-6 dark:invert" alt="Flowbite Logo" />
				<span class="self-center whitespace-nowrap text-lg font-semibold dark:text-white"
					>DEBUGMENU.IO</span
				>
			</a>
			<Button class="ml-auto h-8 w-8" variant="ghost" on:click={() => dispatch('hideClicked')}>
				<ChevronsLeftIcon class="absolute h-5 w-5 text-gray-400" />
			</Button>
		</div>
		<TeamSwitcher />
		<Separator class="my-2" />

		<ul class="space-y-2 font-medium">
			<li>
				<Button class="w-full p-2 " variant="ghost">
					<div class="flex w-full">
						<AppWindowIcon />
						<span class="ms-3">Applications</span>
					</div>
				</Button>

				<ul class="space-y-0.5 font-mono">
					<li>
						<div class="group flex w-full justify-between">
							<Button
								class="float-left h-6 w-full justify-start"
								variant="ghost"
								on:click={() => console.log('hej2')}
							>
								<span class="ms-1 text-xs">Game Client 1</span>
							</Button>
							<DropdownMenu.Root let:ids>
								<DropdownMenu.Trigger asChild let:builder>
									<Button
										class=" invisible h-6 w-8 group-hover:visible "
										builders={[builder]}
										variant="ghost"
										size="sm"
										aria-label="Open menu"
										on:click={() => console.log('hej')}
									>
										<MoreHorizontal class="h-6 w-6" />
									</Button>
								</DropdownMenu.Trigger>
								<DropdownMenu.Content class="w-[200px]" align="end">
									<DropdownMenu.Group>
										<DropdownMenu.Label>Actions</DropdownMenu.Label>
										<DropdownMenu.Item>
											<User class="mr-2 h-4 w-4" />
											Manage users...
										</DropdownMenu.Item>
										<DropdownMenu.Item>
											<Calendar class="mr-2 h-4 w-4" />
											Set due date...
										</DropdownMenu.Item>
										<DropdownMenu.Separator />
										<DropdownMenu.Sub>
											<DropdownMenu.SubTrigger>
												<Tags class="mr-2 h-4 w-4" />
												Apply label
											</DropdownMenu.SubTrigger>
											<DropdownMenu.SubContent class="p-0">asdfasdf</DropdownMenu.SubContent>
										</DropdownMenu.Sub>
										<DropdownMenu.Separator />
										<DropdownMenu.Item class="text-red-600">
											<Trash class="mr-2 h-4 w-4" />
											Delete
											<DropdownMenu.Shortcut>⌘⌫</DropdownMenu.Shortcut>
										</DropdownMenu.Item>
									</DropdownMenu.Group>
								</DropdownMenu.Content>
							</DropdownMenu.Root>
						</div>
					</li>
					<li>
						<Button class="h-6 w-full " variant="outline">
							<div class="flex w-full">
								<span class="ms-1 text-xs font-semibold">Game Client 2</span>
							</div>
						</Button>
					</li>
					<li>
						<Button class=" h-6 w-full " variant="ghost">
							<div class="flex w-full">
								<span class="ms-1 text-xs">Server 1</span>
							</div>
						</Button>
					</li>
				</ul>
			</li>
			<li>
				<Button class="w-full p-2 " variant="ghost">
					<div class="flex w-full">
						<TicketIcon />
						<span class="ms-3">Tokens</span>
					</div>
				</Button>

				<ul class="space-y-0.5 font-mono">
					<li>
						<div class="group flex w-full justify-between">
							<Button
								class="float-left h-6 w-full justify-start"
								variant="ghost"
								on:click={() => console.log('hej2')}
							>
								<span class="ms-1 text-xs">Local</span>
							</Button>
							<DropdownMenu.Root let:ids>
								<DropdownMenu.Trigger asChild let:builder>
									<Button
										class=" invisible h-6 w-8 group-hover:visible "
										builders={[builder]}
										variant="ghost"
										size="sm"
										aria-label="Open menu"
										on:click={() => console.log('hej')}
									>
										<MoreHorizontal class="h-6 w-6" />
									</Button>
								</DropdownMenu.Trigger>
								<DropdownMenu.Content class="w-[200px]" align="end">
									<DropdownMenu.Group>
										<DropdownMenu.Label>Actions</DropdownMenu.Label>
										<DropdownMenu.Item>
											<User class="mr-2 h-4 w-4" />
											Manage users...
										</DropdownMenu.Item>
										<DropdownMenu.Item>
											<Calendar class="mr-2 h-4 w-4" />
											Set due date...
										</DropdownMenu.Item>
										<DropdownMenu.Separator />
										<DropdownMenu.Sub>
											<DropdownMenu.SubTrigger>
												<Tags class="mr-2 h-4 w-4" />
												Apply label
											</DropdownMenu.SubTrigger>
											<DropdownMenu.SubContent class="p-0">asdfasdf</DropdownMenu.SubContent>
										</DropdownMenu.Sub>
										<DropdownMenu.Separator />
										<DropdownMenu.Item class="text-red-600">
											<Trash class="mr-2 h-4 w-4" />
											Delete
											<DropdownMenu.Shortcut>⌘⌫</DropdownMenu.Shortcut>
										</DropdownMenu.Item>
									</DropdownMenu.Group>
								</DropdownMenu.Content>
							</DropdownMenu.Root>
						</div>
					</li>
					<li>
						<Button class="h-6 w-full " variant="outline">
							<div class="flex w-full">
								<span class="ms-1 text-xs font-semibold">Dev Build</span>
							</div>
						</Button>
					</li>
					<li>
						<Button class=" h-6 w-full " variant="ghost">
							<div class="flex w-full">
								<span class="ms-1 text-xs">Playtest</span>
							</div>
						</Button>
					</li>
				</ul>
			</li>
			<li>
				<Button class="w-full p-2 " variant="ghost">
					<div class="flex w-full">
						<Cog />
						<span class="ms-3">Instances</span>
					</div>
				</Button>
				<ul class="space-y-0.5 font-mono">
					<li>
						<Button class=" h-6 w-full " variant="ghost">
							<div class="flex w-full">
								<span class="ms-1 text-xs">b5b976f5</span>
							</div>
						</Button>
					</li>
					<li>
						<Button class=" h-6 w-full " variant="ghost">
							<div class="flex w-full">
								<span class="ms-1 text-xs">3e9d6451</span>
							</div>
						</Button>
					</li>
					<li>
						<Button class=" h-6 w-full " variant="ghost">
							<div class="flex w-full">
								<span class="ms-1 text-xs">a51c458d</span>
							</div>
						</Button>
					</li>
					<li>
						<Button class=" h-6 w-full " variant="ghost">
							<div class="flex w-full">
								<span class="ms-1 text-xs">f90678f5</span>
							</div>
						</Button>
					</li>
					<li>
						<Button class=" h-6 w-full " variant="ghost">
							<div class="flex w-full">
								<span class="ms-1 text-xs">c54ec6d2</span>
							</div>
						</Button>
					</li>
					<li>
						<Button class=" h-6 w-full " variant="ghost">
							<div class="flex w-full">
								<span class="ms-1 text-xs">5131d63e</span>
							</div>
						</Button>
					</li>
				</ul>
			</li>
		</ul>

		<div class="absolute inset-x-0 bottom-0 h-12 w-full bg-primary-foreground">
			<Separator class="my-2 mt-auto " />
			<div class="flex">
				<Avatar.Root class="mx-2 h-8 w-8">
					<Avatar.Fallback>AK</Avatar.Fallback>
				</Avatar.Root>
				<span class="my-auto text-sm font-semibold"> Alicia Koch </span>
				<Button on:click={toggleMode} variant="outline" class="ml-4 h-8 w-8">
					<Sun
						class="absolute h-5 w-5 rotate-0 scale-100 transition-all dark:-rotate-90 dark:scale-0"
					/>
					<Moon
						class="absolute h-5 w-5 rotate-90 scale-0 transition-all dark:rotate-0 dark:scale-100"
					/>
					<span class="sr-only">Toggle theme</span>
				</Button>
			</div>
		</div>
	</div>
</aside>
