<script lang="ts">
	export let title: string;
	export let collapsed: boolean = false;
	export let color: string = 'transparent';

	function getValidBorderColor(color: string) {
		let bgColor = getValidBackgroundColor(color);
		return bgColor === 'transparent' ? 'gray' : bgColor;
	}
	function getValidBackgroundColor(color: string) {
		return 'gray';
	}

	$: borderColor = getValidBorderColor(color);
	$: bgColor = getValidBorderColor(color);

	$: borderClass = `border-${borderColor}-200 dark:border-${borderColor}-800`;
	$: bgClass = `bg-gradient-to-tl from-transparent to-${bgColor}-100 dark:to-${bgColor}-950`;
</script>

<div class="mt-2 min-h-10 rounded-md border-2 border-dashed p-4 {borderClass} {bgClass}">
	<div class="relative -left-4 -top-10 h-0 w-0 font-mono text-gray-700 dark:text-gray-200">
		{title}
	</div>
	{#if !collapsed}
		<slot name="content" />
	{/if}
</div>
