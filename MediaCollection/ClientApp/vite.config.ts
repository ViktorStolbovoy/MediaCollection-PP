import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  base: '/app/',
  server: {
    proxy: {
      '/api': { target: 'http://localhost:46098', changeOrigin: true },
      '/ws': { target: 'ws://localhost:46098', ws: true },
    },
  },
  build: {
    outDir: '../wwwroot/app',
    emptyOutDir: true,
    rollupOptions: {
      output: {
        entryFileNames: 'assets/main.js',
        chunkFileNames: 'assets/[name].js',
        assetFileNames: 'assets/[name][extname]',
      },
    },
  },
});
