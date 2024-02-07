<script lang="ts">
	import Button from '$lib/components/ui/button/button.svelte';
	import UserAuthForm from '$lib/components/auth/user-auth-form.svelte';
	import Separator from '$lib/components/ui/separator/separator.svelte';
	import icon from '$lib/assets/up_arrow.svg';
	import { goto } from '$app/navigation';

	function signInClicked(provider: string) {
		console.log(provider);
		goto(`/auth/${provider}/signin`);
	}
</script>

<div class="h-screen w-full">
	<div class="mx-auto mb-2 w-10/12 self-center">
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
		<div class="mx-auto mt-4 sm:w-[350px]">
			<div class="lg:p-8">
				<div class="to mx-auto flex w-full flex-col justify-center space-y-6">
					<div class="flex flex-col space-y-2 text-center">
						<h1 class="text-2xl font-semibold tracking-tight">Sign in</h1>
						<p class="text-sm text-muted-foreground">Continue with</p>
					</div>
					<UserAuthForm on:click={(evt) => signInClicked(evt.detail)} />
					<p class="px-8 text-center text-sm text-muted-foreground">
						By clicking continue, you agree to our{' '}
						<a href="/terms" class="underline underline-offset-4 hover:text-primary">
							Terms of Service
						</a>{' '}
						and{' '}
						<a href="/privacy" class="underline underline-offset-4 hover:text-primary">
							Privacy Policy
						</a>
						.
					</p>
				</div>
			</div>
		</div>
	</div>
</div>
