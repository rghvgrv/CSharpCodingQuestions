import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// `npm run dev` = hot-reload UI on :5173, data requests proxied to the C# app on :5080.
// base './' makes every URL relative, so the built site works at a domain root or in a subfolder.
export default defineConfig({
  plugins: [react()],
  base: './',
  build: { outDir: '../wwwroot', emptyOutDir: true },
  server: { host: true, proxy: { '/data': 'http://localhost:5080' } },
})
