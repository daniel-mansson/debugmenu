<script lang="ts">
	import type { PageData } from './$types';

	export let data: PageData;
</script>

{#each data.instances as instance}
	<div>
		<p>{JSON.stringify(instance)}</p>
		<a href="/app/controller/{instance.id}">Controller</a>
	</div>
{:else}
	No running instances
{/each}
