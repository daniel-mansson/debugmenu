<script lang="ts">
	import '../app.postcss';
	import { AppShell, AppBar, Modal } from '@skeletonlabs/skeleton';

	// Floating UI for Popups
	import { computePosition, autoUpdate, flip, shift, offset, arrow } from '@floating-ui/dom';
	import { storePopup } from '@skeletonlabs/skeleton';
	storePopup.set({ computePosition, autoUpdate, flip, shift, offset, arrow });

	import { initializeStores } from '@skeletonlabs/skeleton';
	initializeStores();
</script>

<Modal />

<!-- App Shell -->
<AppShell>
	<slot />
</AppShell>
