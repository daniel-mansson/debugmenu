<script lang="ts">
	import ComapctCard from '$lib/components/test/ComapctCard.svelte';
	import TestCard from '$lib/components/test/TestCard.svelte';
	import { Separator } from '$lib/components/ui/separator';
	import icon from '$lib/assets/up_arrow.svg';
	import { Button } from '$lib/components/ui/button';
	import * as Card from '$lib/components/ui/card';
</script>

<div class="mx-auto my-2 w-10/12 self-center">
	<nav class="border-gray-200 bg-white dark:bg-gray-900">
		<div class="mx-auto flex max-w-screen-xl flex-wrap items-center justify-between p-4">
			<a href="https://flowbite.com/" class="flex items-center space-x-3 rtl:space-x-reverse">
				<img src={icon} class="h-8" alt="Flowbite Logo" />
				<span class="self-center whitespace-nowrap text-xl font-semibold dark:text-white"
					>DEBUGMENU.IO</span
				>
			</a>

			<div class=" flex flex-col space-x-reverse md:w-auto" id="navbar-default">
				<Button variant="outline" class="active:outline active:outline-1">Launch app</Button>
			</div>
		</div>
	</nav>
	<Separator />

	<div class=" my-48 text-center">
		<h1 class="mx-auto scroll-m-20 pb-2 text-4xl font-semibold">
			Hasdf ASdfasdas Dasd fasdfasdas d
		</h1>
	</div>
	<Separator />
	<!-- <div
		class="h-8 w-full rounded-bl-full
  rounded-br-full bg-gradient-to-b from-blue-50 to-transparent"
	></div> -->

	<Card.Root>asdasd</Card.Root>
</div>
