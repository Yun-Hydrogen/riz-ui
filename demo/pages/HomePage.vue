<script setup>
import { inject } from 'vue'
const themeColor = inject('themeColor')
</script>

<template>
  <main class="main">
    <section class="home-hero">
      <h1 class="home-logo">RizUI</h1>
      <p class="home-desc">一套轻量、精致的 Vue 3 通用 UI 组件库</p>
      <div class="home-actions">
        <router-link to="/demo/cntr-card" class="home-btn home-btn-primary" :style="{ background: themeColor }">浏览组件</router-link>
        <a class="home-btn home-btn-ghost" href="https://github.com/YunHydrogen/riz-ui" target="_blank">GitHub</a>
      </div>
    </section>
    <section class="home-features">
      <div class="home-feat"><div class="home-feat-icon"><i class="fa-solid fa-palette"></i></div><h3>主题定制</h3><p>所有控件支持 color prop 一键换色</p></div>
      <div class="home-feat"><div class="home-feat-icon"><i class="fa-solid fa-box"></i></div><h3>即装即用</h3><p><code>npm install github:YunHydrogen/riz-ui</code></p></div>
      <div class="home-feat"><div class="home-feat-icon"><i class="fa-solid fa-bolt"></i></div><h3>Vue 3 原生</h3><p>Composition API · Teleport · Transition</p></div>
      <div class="home-feat"><div class="home-feat-icon"><i class="fa-solid fa-code"></i></div><h3>AGPLv3 开源</h3><p>自由使用 / 修改 / 分发</p></div>
    </section>
    <section class="home-quick">
      <h2>快速开始</h2>
      <pre class="code-block"><code>npm install github:YunHydrogen/riz-ui

import { Cntr_Card, UiSwitch } from 'riz-ui'</code></pre>
    </section>
  </main>
</template>
