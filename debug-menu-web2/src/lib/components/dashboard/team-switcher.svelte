<script lang="ts">
	import { CaretSort, Check, Enter, PlusCircled } from 'radix-icons-svelte';
	import { cn } from '$lib/utils';
	import * as Avatar from '$lib/components/ui/avatar';
	import { Button } from '$lib/components/ui/button';
	import * as Command from '$lib/components/ui/command';
	import * as Dialog from '$lib/components/ui/dialog';
	import { Input } from '$lib/components/ui/input';
	import { Label } from '$lib/components/ui/label';
	import * as Popover from '$lib/components/ui/popover';
	import * as Select from '$lib/components/ui/select';
	import { tick } from 'svelte';
	import { teams, currentTeam, updateTeam } from '$lib/appstate';
	import { goto } from '$app/navigation';

	let className: string | undefined | null = undefined;
	export { className as class };

	let open = false;
	let showTeamDialog = false;

	function closeAndRefocusTrigger(triggerId: string) {
		open = false;

		console.log('close ' + triggerId);
		tick().then(() => document.getElementById(triggerId)?.focus());
	}

	$: selectedTeam = $teams.find((t) => t.id == $currentTeam);
</script>

<Dialog.Root bind:open={showTeamDialog}>
	<Popover.Root bind:open let:ids>
		<Popover.Trigger asChild let:builder>
			<Button
				builders={[builder]}
				variant="outline"
				role="combobox"
				aria-expanded={open}
				aria-label="Select a team"
				class={cn('w-[200px] justify-between', className)}
			>
				<Avatar.Root class="mr-2 h-5 w-5">
					<Avatar.Image src={selectedTeam?.icon} alt={selectedTeam?.name} />
					<Avatar.Fallback>{(selectedTeam?.name ?? '?')[0]}</Avatar.Fallback>
				</Avatar.Root>
				{selectedTeam ? selectedTeam.name : 'No Team'}
				<CaretSort class="ml-auto h-4 w-4 shrink-0 opacity-50" />
			</Button>
		</Popover.Trigger>
		<Popover.Content class="w-[200px] p-0">
			<Command.Root>
				<Command.List>
					<Command.Empty>No team found.</Command.Empty>
					{#each $teams as team}
						<Command.Item
							onSelect={() => {
								selectedTeam = team;
								closeAndRefocusTrigger(ids.trigger);
								goto(`/app/${team.id}`);
							}}
							value={team.name}
							class="text-sm"
						>
							<Avatar.Root class="mr-2 h-5 w-5">
								<Avatar.Image
									src="https://avatar.vercel.sh/${team.icon}.png"
									alt={team.name}
									class="grayscale"
								/>
								<Avatar.Fallback>SC</Avatar.Fallback>
							</Avatar.Root>
							{team.name}
							<Check
								class={cn('ml-auto h-4 w-4', selectedTeam?.id !== team.id && 'text-transparent')}
							/>
						</Command.Item>
					{/each}
				</Command.List>
				<Command.Separator />
				<Command.List>
					<Command.Group>
						<Command.Item
							onSelect={() => {
								closeAndRefocusTrigger(ids.trigger);
							}}
						>
							<a href="/app" class="flex">
								<Enter class="my-auto mr-1 h-4 w-4" />
								Manage Teams
							</a>
						</Command.Item>
					</Command.Group>
				</Command.List>
			</Command.Root>
		</Popover.Content>
	</Popover.Root>
	<Dialog.Content>
		<Dialog.Header>
			<Dialog.Title>Create team</Dialog.Title>
			<Dialog.Description>Add a new team to manage products and customers.</Dialog.Description>
		</Dialog.Header>
		<div>
			<div class="space-y-4 py-2 pb-4">
				<div class="space-y-2">
					<Label for="name">Team name</Label>
					<Input id="name" placeholder="Acme Inc." />
				</div>
				<div class="space-y-2">
					<Label for="plan">Subscription plan</Label>
					<Select.Root>
						<Select.Trigger>
							<Select.Value placeholder="Select a plan" />
						</Select.Trigger>
						<Select.Content>
							<Select.Item value="free">
								<span class="font-medium">Free </span>-<span class="text-muted-foreground">
									Trial for two weeks
								</span>
							</Select.Item>
							<Select.Item value="pro">
								<span class="font-medium">Pro</span> -
								<span class="text-muted-foreground"> $9/month per user </span>
							</Select.Item>
						</Select.Content>
					</Select.Root>
				</div>
			</div>
		</div>
		<Dialog.Footer>
			<Button variant="outline" on:click={() => (showTeamDialog = false)}>Cancel</Button>
			<Button type="submit">Continue</Button>
		</Dialog.Footer>
	</Dialog.Content>
</Dialog.Root>
