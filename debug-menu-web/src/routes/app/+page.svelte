<script lang="ts">
	import { signIn, signOut } from '@auth/sveltekit/client';
	import { page } from '$app/stores';
	import { onMount } from 'svelte';
	import { getToken } from '@auth/core/jwt';
	import { DebugMenuBackend } from '$lib/backend/backend';
	import { clickOutside } from '$lib/clickOutside';
	import { Modal, getModalStore } from '@skeletonlabs/skeleton';
	import type { ModalSettings, ModalComponent, ModalStore } from '@skeletonlabs/skeleton';
	import type { PageData } from './$types';

	export let data: PageData;

	let applications = data.applications;

	let deleteTarget: any;
	async function onDeleteClicked(application: any) {
		if (deleteTarget == application) {
			console.log('delete clicked for ' + application.name);
			let result = await DebugMenuBackend(fetch, data.session.jwt).deleteApplication(
				application.id
			);

			if (result.ok) {
				applications = applications.filter((a) => a !== application);
			}

			deleteTarget = null;
		} else {
			deleteTarget = application;
		}
	}
	const modalStore = getModalStore();

	function onOutsideClick(application: any) {
		if (deleteTarget === application) {
			deleteTarget = null;
		}
	}

	async function onCreateClicked(application: any) {
		const modal: ModalSettings = {
			type: 'prompt',
			// Data
			title: 'Create Application',
			body: 'Provide the application name in the field below.',
			// Populates the input value and attributes
			value: '',
			valueAttr: { type: 'text', minlength: 3, maxlength: 10, required: true },
			// Returns the updated response value
			response: async (response: string) => {
				if (response) {
					console.log('creating ' + response);
					let result = await DebugMenuBackend(fetch, $page.data.session.jwt).createApplication(
						response,
						$page?.data?.session?.userId
					);
					if (result.ok) {
						applications.push(await result.json());
						applications = applications;
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

{#if data.session}
	<div>
		{#each applications as application}
			<div class="text-xl text-center my-2">
				<a href="/app/application/{application.id}"
					>Application {application.id}: {application.name}</a
				>
				<button
					class="btn border border-warning-900 h-8 ml-4"
					use:clickOutside
					on:outsideclick={() => onOutsideClick(application)}
					on:click={() => onDeleteClicked(application)}
					>Delete{deleteTarget === application ? '(really?)' : ''}</button
				>
			</div>
		{/each}
		<div class="text-xl text-center my-2">
			<button class="btn border border-secondary-900 h-8 ml-4" on:click={() => onCreateClicked()}
				>Create application</button
			>
		</div>
	</div>
{:else}
	<p>Not signed in.</p>
{/if}
