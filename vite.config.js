import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { resolve } from 'path'
import cssInjectedByJsPlugin from 'vite-plugin-css-injected-by-js'

export default defineConfig(({ mode }) => {
  // ── Library 构建模式：`vite build --mode lib` ──
  if (mode === 'lib') {
    return {
      plugins: [vue(), cssInjectedByJsPlugin()],
      build: {
        lib: {
          entry: resolve(__dirname, 'src/index.js'),
          name: 'RizUI',
          fileName: 'riz-ui'
        },
        rollupOptions: {
          external: ['vue'],
          output: {
            globals: { vue: 'Vue' }
          }
        },
        outDir: 'dist'
      }
    }
  }

  // ── Demo 预览模式：`vite` / `vite build` ──
  return {
    plugins: [vue()],
    root: 'demo',
    server: {
      port: 3000,
      open: true
    },
    resolve: {
      alias: {
        'riz-ui': resolve(__dirname, 'src/index.js')
      }
    },
    build: {
      outDir: '../demo-dist'
    }
  }
})
