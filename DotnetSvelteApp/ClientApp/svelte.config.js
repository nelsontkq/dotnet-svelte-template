import adapter from '@sveltejs/adapter-auto';
import preprocess from 'svelte-preprocess';
import { loadEnv } from 'vite';
import fs from 'fs';
import path from 'path';
import { spawn } from 'child_process';
// This script sets up HTTPS for the application using the ASP.NET Core HTTPS certificate
let env = { ...process.env, ...loadEnv("dev", process.cwd()) };
const isDev = env.ASPNETCORE_ENVIRONMENT === 'Development';
if (!isDev) {
	env = process.env;
}
const getAspNetPemAndKey = () => {
	if (!isDev) {
		return {};
	}
	const baseFolder =
		env.APPDATA !== undefined && env.APPDATA !== ''
			? `${env.APPDATA}/ASP.NET/https`
			: `${env.HOME}/.aspnet/https`;

	const certificateArg = process.argv.map(arg => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
	const certificateName = certificateArg ? certificateArg.groups.value : env.npm_package_name;

	if (!certificateName) {
		console.error('Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.')
		process.exit(-1);
	}

	const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
	const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

	if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
		spawn('dotnet', [
			'dev-certs',
			'https',
			'--export-path',
			certFilePath,
			'--format',
			'Pem',
			'--no-password',
		], { stdio: 'inherit', })
			.on('exit', (code) => process.exit(code));
	}
	return { certFilePath, keyFilePath };
}

let { certFilePath, keyFilePath } = getAspNetPemAndKey();

/** @type {import('@sveltejs/kit').Config} */
const config = {
	// Consult https://github.com/sveltejs/svelte-preprocess
	// for more information about preprocessors
	preprocess: preprocess(),
	kit: {
		adapter: adapter(),
		vite: {
			server: {
				port: env.ASPNETCORE_HTTPS_PORT ? env.ASPNETCORE_HTTPS_PORT : '44447',
				https: {
					ca: certFilePath,
					key: keyFilePath,
				},
				headers: {
					Connection: 'Keep-Alive'
				},
				proxy: {
					"/api": {
						target: env.ASPNETCORE_URLS.split(";")[0],
						changeOrigin: true,
						rewrite: path => path.replace(/^\/api/, ''),
						secure: false,
					}
				}
			}
		}
	},
};

export default config;
