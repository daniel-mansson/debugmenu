<script lang="ts">
	import ComapctCard from '$lib/components/test/ComapctCard.svelte';
	import TestCard from '$lib/components/test/TestCard.svelte';
	import { Separator } from '$lib/components/ui/separator';
	import icon from '$lib/assets/up_arrow.svg';
	import { Button } from '$lib/components/ui/button';
	import { UploadCloudIcon } from 'lucide-svelte';

	export let data;
</script>

<div class="mx-auto my-2 w-10/12 self-center">
	<nav class="border-gray-200">
		<div class="mx-auto flex max-w-screen-xl flex-wrap items-center justify-between p-4">
			<a href="/" class="flex items-center space-x-3 rtl:space-x-reverse">
				<UploadCloudIcon class="ml-2 h-6 dark:stroke-white"></UploadCloudIcon>
				<span class="self-center whitespace-nowrap text-xl font-semibold dark:text-white"
					>DEBUGMENU.IO</span
				>
			</a>

			<div class=" flex flex-row space-x-reverse md:w-auto" id="navbar-default">
				{#if data.user}
					<Button href="/app" variant="outline" class="">Launch app</Button>
					<Button href="/auth/signout" variant="outline" class="ml-2">Sign out</Button>
				{:else}
					<Button href="/auth/signin" variant="outline" class="">Sign in</Button>
				{/if}
			</div>
		</div>
	</nav>
	<Separator />

	<div class=" my-48 text-center">
		<h1 class="mx-auto scroll-m-20 pb-2 text-4xl font-semibold">Remote in-browser debug menu</h1>
		{#if data.user}
			<Button href="/app">Get started</Button>
		{:else}
			<Button href="/auth/signin">Get started</Button>
		{/if}
	</div>
	<Separator />
</div>
