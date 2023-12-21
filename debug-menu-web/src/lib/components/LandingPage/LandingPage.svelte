<script>
	import BottomCallToAction from './BottomCallToAction.svelte';
	import Feature1 from './Feature1.svelte';
	import Feature2 from './Feature2.svelte';
	import Feature3 from './Feature3.svelte';
	import FeaturesHeader from './FeaturesHeader.svelte';
	import NavBar from './NavBar.svelte';
	import Pricing from './Pricing.svelte';
	import SupportedPlatforms from './SupportedPlatforms.svelte';
	import Top from './Top.svelte';
	import UseCaseSummary from './UseCaseSummary.svelte';
</script>

<div
	class="scroll-smooth leading-normal tracking-normal text-white gradient bg-gradient-to-tr"
	style="font-family: 'Source Sans Pro', sans-serif;"
/>

<NavBar />

<Top />

<UseCaseSummary />
<SupportedPlatforms />
<FeaturesHeader />
<Feature1 />
<Feature2 />
<Feature3 />
<Pricing />
<BottomCallToAction />
