<script lang="ts">
	import * as api from '$lib/api';
	let notificationMessage: string;
	let registerClickedOnce = false;
	let response = {
		email: '',
		userName: '',
		password: '',
		confirmPassword: ''
	};
	async function submitForm() {
		const errors = await api.login(response.email, response.password);
		notificationMessage = '';
		if (errors) {
			errors.forEach((error) => {
				notificationMessage += `${error.description}\n`;
			});
		} else {
			window.location.href = '/';
		}
	}
	async function register(ev: Event) {
		if (!registerClickedOnce) {
			registerClickedOnce = true;
			return;
		}
		if (response.password !== response.confirmPassword) {
			notificationMessage = 'Passwords do not match';
			return;
		}
		const errors = await api.register(response.userName, response.email, response.password);
		notificationMessage = '';
		if (errors) {
			Object.entries(errors).forEach(([key, val]) => {
				if (Array.isArray(val)) {
					notificationMessage += `${key}: ${val.join(', ')}\n`;
				} else {
					notificationMessage += `${key}: ${val}\n`;
				}
			});
			return;
		}
		notificationMessage = 'Email sent. Check your inbox.';
	}
</script>

<form class="login-form" on:submit|preventDefault={submitForm}>
	{#if notificationMessage}
		<p class="error">{notificationMessage}</p>
	{/if}
	{#if registerClickedOnce}
		<div class="row">
			<label for="userName">UserName</label>
			<input type="text" name="userName" bind:value={response.userName} />
		</div>
	{/if}
	<div class="row">
		<label for="email">Email</label>
		<input type="text" name="email" bind:value={response.email} />
	</div>
	<div class="row">
		<label for="password">Password</label>
		<input type="password" name="password" bind:value={response.password} />
	</div>
	{#if registerClickedOnce}
		<div class="row">
			<label for="confirmPassword">Confirm Password</label>
			<input type="password" name="confirmPassword" bind:value={response.confirmPassword} />
		</div>
	{/if}
	<div class="button-group">
		{#if !registerClickedOnce}
			<button type="submit">Login</button>
		{/if}
		<button type="button" on:click|preventDefault={register}>Register</button>
	</div>
</form>

<style>
	.login-form {
		width: 100%;
		max-width: 330px;
		padding: 15px;
		margin: auto;
	}
	.login-form .row {
		margin-bottom: 15px;
	}
	.login-form .row:last-child {
		margin-bottom: 0;
	}
	.login-form label {
		display: block;
	}
	.login-form input[type='text'],
	.login-form input[type='password'] {
		width: 100%;
		padding: 10px 0;
		border: 1px solid #ccc;
		margin-bottom: 10px;
		font-size: 16px;
		font-family: 'Open Sans', sans-serif;
	}
	.button-group {
		display: flex;
		width: 100%;
	}
	.login-form button {
		font-family: 'Open Sans', sans-serif;
		text-transform: uppercase;
		font-size: 14px;
		font-weight: bold;
		padding: 15px;
		width: 100%;
		border: 0;
		color: #fff;
	}
	.login-form button[type='submit'] {
		background: #ff3e00;
	}
	.login-form button[type='button'] {
		background: #3d3938;
	}
	.login-form button[type='button']:hover {
		background: #42322fe5;
	}
	.login-form button[type='submit']:hover {
		background: #ff4000e8;
	}
	.error {
		color: #ff3e00;
		font-weight: bold;
		font-size: 14px;
		margin-bottom: 10px;
	}
</style>
