<script lang="ts">
	import { createEventDispatcher } from 'svelte';

	export let button: any;

	let dispatch = createEventDispatcher();
</script>

<button
	on:click={() => dispatch('submit', { button, payload: '' })}
	class="m-1 btn btn-sm variant-outline-primary"
>
	<div class="">
		<div class="flex">
			{button.channelParts[button.channelParts.length - 1]}
			<div class="text-surface-500-400-token ml-1">→</div>
		</div>
	</div>
</button>
