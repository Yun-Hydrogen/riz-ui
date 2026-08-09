<template>
  <div class="nro-overlay">
    <span class="nro-badge">{{ text }}</span>
  </div>
</template>

<script setup>
defineProps({
  text: { type: String, default: '🚧 等待实现' }
})
</script>

<style scoped>
.nro-overlay {
  position: absolute;
  inset: 0;
  z-index: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(255, 255, 255, 0.55);
  backdrop-filter: blur(1.5px);
}

.nro-badge {
  padding: 6px 16px;
  border-radius: 999px;
  background: rgba(0, 0, 0, 0.55);
  color: #fff;
  font-size: 12px;
  font-weight: 600;
  letter-spacing: 0.5px;
}
</style>
