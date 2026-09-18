import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// `npm run dev` = hot-reload UI on :5173, API calls proxied to the C# app on :5080.
export default defineConfig({
  plugins: [react()],
  build: { outDir: '../wwwroot', emptyOutDir: true },
  server: { host: true, proxy: { '/api': 'http://localhost:5080' } },
})
