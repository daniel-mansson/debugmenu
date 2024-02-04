<script lang="ts">
	import { Separator } from '$lib/components/ui/separator';
	import icon from '$lib/assets/up_arrow.svg';
	import { Button } from '$lib/components/ui/button';
	import type { LayoutData } from './$types';
	import { signIn, signOut } from '@auth/sveltekit/client';

	export let data: LayoutData;
</script>

<div class="mx-auto my-2 w-10/12 self-center">
	<nav class="border-gray-200">
		<div class="mx-auto flex max-w-screen-xl flex-wrap items-center justify-between p-4">
			<a href="/" class="flex items-center space-x-3 rtl:space-x-reverse">
				<img src={icon} class="h-8 dark:invert" alt="Flowbite Logo" />
				<span class="self-center whitespace-nowrap text-xl font-semibold dark:text-white"
					>DEBUGMENU.IO</span
				>
			</a>

			<div class=" flex flex-col space-x-reverse md:w-auto" id="navbar-default">
				<Button href="/app" variant="outline" class="">Launch app</Button>
			</div>
		</div>
	</nav>
	<Separator />

	<div class=" my-48 text-center">
		<h1 class="mx-auto scroll-m-20 pb-2 text-4xl font-semibold">
			Control your application from anywhere
		</h1>
		<Button href="/app">Get started</Button>
	</div>
	<Separator />

	<div class="">
		{#if data.session}
			<div>{data.session.user?.name}</div>
			<button class="btn btn-sm border" on:click={() => signOut()}>Sign out</button>
		{:else}
			<button class="btn btn-sm border" on:click={() => signIn()}>Sign in</button>
		{/if}
	</div>
</div>
