<script lang="ts">
	import { signIn, signOut } from '@auth/sveltekit/client';
	import { page } from '$app/stores';
	import { onMount } from 'svelte';
	import { DebugMenuBackend } from '$lib/backend/backend';
	import { getModalStore } from '@skeletonlabs/skeleton';
	import type { ModalComponent, ModalSettings } from '@skeletonlabs/skeleton';
	import CreateTokenModal from '$lib/components/CreateTokenModal.svelte';
	export let data;
	let application = [];

	const createTokenModalComponent: ModalComponent = { ref: CreateTokenModal };

	const modalStore = getModalStore();

	let tokens = data.tokens;

	function onCreateClicked() {
		const modal: ModalSettings = {
			type: 'component',
			component: createTokenModalComponent,
			title: 'Create Token',
			body: '',
			response: async (response: any) => {
				if (response) {
					let result = await DebugMenuBackend(fetch, data.session.jwt).createToken(
						data.application.id,
						response.name,
						response.description
					);
					if (result.ok) {
						console.log('created');
						tokens.push(await result.json());
						tokens = tokens;
					} else {
						console.log('error');
					}
				}
			}
		};
		modalStore.trigger(modal);
	}

	function onDeleteClicked(token: any) {
		const modal: ModalSettings = {
			type: 'confirm',
			title: 'Delete token?',
			body: `Are you sure you wish to delete token '${token.name}'?`,
			response: async (response: boolean) => {
				if (response) {
					let result = await DebugMenuBackend(fetch, data.session.jwt).deleteToken(token.id);
					if (result.ok) {
						tokens = tokens.filter((t) => t !== token);
						console.log('created');
					} else {
						console.log('error');
					}
				}
			}
		};
		modalStore.trigger(modal);
	}
</script>

<div class="">
	<div class="mx-auto text-center text-2xl">{data.application.name}</div>

	{#each tokens as token}
		<div class="bg-surface-100-800-token my-4">
			<a href="/app/token/{token.id}/instances">
				<div class="mx-auto text-center text-xl">{token.name}</div>
				<div class="mx-auto text-center text-lg">{token.description}</div>
			</a>
			<div class="mx-auto text-center text-sm font-mono">{token.token}</div>
			<div class="mx-auto text-center">
				<button
					class="btn border border-primary-900 h-8 ml-4 mb-2"
					on:click={() => onDeleteClicked(token)}>Delete</button
				>
			</div>
		</div>
	{/each}

	<div class="text-xl text-center my-2">
		<button class="btn border border-secondary-900 h-8 ml-4" on:click={() => onCreateClicked()}
			>Create token</button
		>
	</div>
</div>
