<script lang="ts">
	import { currentConnection } from '$lib/appstate';

	$: connection = $currentConnection?.status;
</script>

<div class="">{$connection}</div>
