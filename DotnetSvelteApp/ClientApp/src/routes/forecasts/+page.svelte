<script lang="ts">
	import type { Forecast } from '$lib/forecast';

	let { name = 'Svelte' } = $props();

	let error = $state<string>('');
	let forecasts = $state<Forecast[]>([]);
	let loading = $state(true);

	// Use $effect to handle the async fetch
	$effect(() => {
		fetch('api/weatherForecast')
			.then((response) => {
				if (response.status === 404) {
					error = 'Server unreachable';
					loading = false;
				} else {
					return response.json();
				}
			})
			.then((data: Forecast[]) => {
				if (data) {
					forecasts = data;
				}
				loading = false;
			})
			.catch(() => {
				error = 'Failed to fetch forecast';
				loading = false;
			});
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
		<thead>
			<tr>
				<th>Date</th>
				<th>Temperature (C)</th>
				<th>Summary</th>
			</tr>
		</thead>
		<tbody>
			{#if loading}
				<tr>
					<td colspan="3">Loading...</td>
				</tr>
			{:else if error}
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
		</tbody>
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
