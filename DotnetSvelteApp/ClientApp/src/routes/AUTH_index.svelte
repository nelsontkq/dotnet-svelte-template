<script lang="ts">
	import { user } from '$lib/variables';
	let defaultName: string = 'Svelte';
</script>

<main>
	<h1>Hello {$user.userName ? $user.userName : defaultName}!</h1>
	<p>
		Visit the <a href="https://svelte.dev/tutorial">Svelte tutorial</a> to learn how to build Svelte
		apps.
	</p>
	<a href="/forecasts">View Weather Forecast</a>
</main>

<style>
	main {
		text-align: center;
		padding: 1em;
		max-width: 240px;
		margin: 0 auto;
	}
	h1 {
		color: #ff3e00;
		font-weight: 400;
	}
	h1 {
		font-size: 4em;
		text-transform: uppercase;
		font-weight: 100;
	}

	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>
