<script lang="ts">
  import type { Forecast } from "../models/forecast";
  export let name: string;
  let forecasts: Forecast[] = [];
  let weatherForecastPromise = fetch("/weatherForecast").then(
    async (response) => {
      forecasts = await response.json();
    }
  );
</script>

<main>
  <h1>Hello {name}!</h1>
  <p>
    Visit the <a href="https://svelte.dev/tutorial">Svelte tutorial</a> to learn
    how to build Svelte apps.
  </p>
  <h2>Today's Forecast</h2>
  <table>
    <th>Date</th>
    <th>Temperature (C)</th>
    <th>Summary</th>
    {#each forecasts as forecast}
      <tr>
        <td>{new Date(forecast.date).toLocaleDateString()}</td>
        <td>{forecast.temperatureC}</td>
        <td>{forecast.summary}</td>
      </tr>
    {/each}
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
