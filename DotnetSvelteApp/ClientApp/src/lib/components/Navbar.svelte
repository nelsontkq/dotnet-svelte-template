<script lang="ts">
	import { User, user } from '$lib/variables';
	function logout() {
		user.set(new User());
	}
</script>

<div class="navbar">
	<div class="brand-logo"><a href="/">Logo Here</a></div>
	<div class="spacer" />
	<ul class="nav-items">
		{#if $user.isAuthenticated}
			<li on:click={logout}>Logout</li>
			<li><a href="/profile">Profile</a></li>
		{:else}
			<li><a href="/login">Login</a></li>
		{/if}
	</ul>
</div>

<style>
	.brand-logo {
		width: 50px;
		border-radius: 50%;
		margin-left: 15px;
	}
	.spacer {
		flex: 1;
	}
	.navbar a {
		color: inherit;
		text-decoration: inherit;
	}
	.navbar {
		background-color: #fff;
		padding-bottom: 5px;
		border-bottom: 1px solid #ccc;
		display: inline-flex;
		width: 100%;
		align-items: center;
	}
	.nav-items {
		margin-right: 15px;
	}

	.nav-items li {
		display: block;
		margin-left: 15px;
		font-size: 14px;
		color: #333;
		cursor: pointer;
	}

	.nav-items li:hover {
		text-decoration: underline;
	}
</style>
