<script lang="ts">
	import type { Forecast } from '$lib/forecast';
	export let name: string = 'Svelte';
	let error: string;
	const response: Promise<Forecast[]> = fetch('api/weatherForecast').then((response) => {
		if (response.status === 404) {
			error = 'Server unreachable';
		} else if (response.status === 401) {
			error = 'Unauthorized - please login';
		} else {
			return response.json();
		}
	});
</script>

<main>
	<h1>Hello {name}!</h1>
	<p>
		Visit the <a href="https://svelte.dev/tutorial">Svelte tutorial</a> to learn how to build Svelte
		apps.
	</p>
	<h2>Today's Forecast</h2>
	<table>
		<th>Date</th>
		<th>Temperature (C)</th>
		<th>Summary</th>
		{#await response then forecasts}
			{#if error}
				<tr>
					<td colspan="3">{error}</td>
				</tr>
			{:else}
				{#each forecasts as forecast}
					<tr>
						<td>{new Date(forecast.date).toLocaleDateString()}</td>
						<td>{forecast.temperatureC}</td>
						<td>{forecast.summary}</td>
					</tr>
				{/each}
			{/if}
		{/await}
	</table>
</main>

<style>
	main {
		text-align: center;
		padding: 1em;
		max-width: 240px;
		margin: 0 auto;
	}
	table {
		margin: 0 auto;
	}
	h1,
	h2,
	th {
		color: #ff3e00;
		font-weight: 400;
	}
	h2 {
		font-size: 3em;
		text-transform: uppercase;
		font-weight: 100;
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
