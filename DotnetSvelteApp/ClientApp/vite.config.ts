import tailwindcss from '@tailwindcss/vite';
import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vite';

export default defineConfig({
	plugins: [tailwindcss(), sveltekit()],
	server:{
		port: 44447,
		strictPort:true,
		proxy: {
			'/api': {
				target: 'http://localhost:5279',
				changeOrigin: true,
				rewrite: (path) => path.replace(/^\/api/, '')	
			},
		}
	}
});
