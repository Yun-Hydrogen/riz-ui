<template>
  <div class="ucr-row" :class="{ 'ucr-not-ready': notReady, 'ucr-stack': stack }">
    <div class="ucr-label-col">
      <label>{{ label }}</label>
      <span v-if="hint" class="ucr-hint">{{ hint }}</span>
      <div v-if="stack" class="ucr-slot-wrap">
        <slot />
      </div>
    </div>
    <slot v-if="!stack" />
    <NotReadyOverlay v-if="notReady" :text="notReadyText" />
  </div>
</template>

<script setup>
import NotReadyOverlay from './NotReadyOverlay.vue'

defineProps({
  label:        { type: String, required: true },
  hint:         { type: String, default: '' },
  stack:        { type: Boolean, default: false },
  notReady:     { type: Boolean, default: false },
  notReadyText: { type: String, default: '等待实现' }
})
</script>

<style scoped>
.ucr-row {
  display: flex;
  align-items: flex-start;
  gap: 16px;
  padding: 12px 0;
  border-bottom: 1px solid #f5f5f5;
}

.ucr-stack {
  flex-direction: column;
  gap: 0;
}

.ucr-not-ready {
  position: relative;
  overflow: hidden;
}

.ucr-label-col {
  display: flex;
  flex-direction: column;
  gap: 2px;
  flex: 1;
  min-width: 120px;
  padding-top: 2px;
}

.ucr-stack .ucr-label-col {
  flex: none;
  min-width: 0;
  width: 100%;
}

.ucr-label-col > label {
  font-size: 14px;
  color: #444;
  line-height: 1.4;
}

.ucr-hint {
  font-size: 11px;
  color: #99a;
  overflow-wrap: break-word;
  line-height: 1.4;
}

.ucr-slot-wrap {
  margin-top: 8px;
}
</style>
