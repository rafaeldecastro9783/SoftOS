import { defineConfig, loadEnv } from 'vite';
import { fileURLToPath } from 'url';
import vue from '@vitejs/plugin-vue';

export default ({ mode }: { mode: string }) => {
  const env = loadEnv(mode, process.cwd());

  return defineConfig({
    build: {
      rollupOptions: {
        output: {
          manualChunks(id) {
            if (id.includes('node_modules')) {
              return id.toString().split('node_modules/')[1].split('/')[0].toString();
            }
          },
        },
      },
    },
    plugins: [vue()],
    resolve: {
      alias: {
        assets: fileURLToPath(new URL('./src/assets', import.meta.url)),
        components: fileURLToPath(new URL('./src/components', import.meta.url)),
        enums: fileURLToPath(new URL('./src/enums', import.meta.url)),
        helpers: fileURLToPath(new URL('./src/helpers', import.meta.url)),
        plugins: fileURLToPath(new URL('./src/plugins', import.meta.url)),
        stores: fileURLToPath(new URL('./src/stores', import.meta.url)),
        typings: fileURLToPath(new URL('./src/typings', import.meta.url)),
        utils: fileURLToPath(new URL('./src/utils', import.meta.url)),
        views: fileURLToPath(new URL('./src/views', import.meta.url)),
      },
    },
    server: {
      strictPort: true,
      port: 5173,
      proxy: {
        '/Ajax': {
          secure: false,
          changeOrigin: true,
          target: env.VITE_BACKEND_URL,
        },
      },
    },
  });
};
