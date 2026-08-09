<template>
  <Teleport :to="teleport">
    <Transition name="uid-pop">
      <div
        v-if="show"
        class="uid-overlay"
        :class="{ 'uid-anchored': isAnchored }"
        @click.self="$emit('update:show', false)"
      >
        <div class="uid-box" :style="boxStyle">
          <div class="uid-header">
            <span class="uid-title">{{ title }}</span>
            <button class="uid-close" @click="$emit('update:show', false)">✕</button>
          </div>

          <div class="uid-body">
            <slot />
          </div>

          <div v-if="$slots.footer" class="uid-footer">
            <slot name="footer" />
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  show:     { type: Boolean, default: false },
  title:    { type: String, default: '' },
  color:    { type: String, default: '#39c5bb' },
  teleport: { type: String, default: 'body' },
  width:    { type: String, default: 'auto' }
})

defineEmits(['update:show'])

const isAnchored = computed(() => props.teleport !== 'body')

const boxStyle = computed(() => ({
  width: props.width,
  '--uid-color': props.color
}))
</script>

<style scoped>
.uid-overlay {
  position: fixed;
  inset: 0;
  z-index: 9990;
  background: rgba(255, 255, 255, 0.25);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40px;
}

.uid-anchored {
  position: absolute;
}

.uid-pop-enter-active,
.uid-pop-leave-active {
  transition: opacity 0.2s ease, transform 0.2s ease;
}
.uid-pop-enter-from,
.uid-pop-leave-to {
  opacity: 0;
  transform: scale(0.92);
}

.uid-box {
  background: #fff;
  border-radius: 16px;
  box-shadow: 0 12px 40px rgba(0, 0, 0, 0.18);
  min-width: 280px;
  max-width: 90vw;
  max-height: 70vh;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.uid-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px 20px 12px;
}

.uid-title {
  font-size: 15px;
  font-weight: 700;
  color: #334;
}

.uid-close {
  width: 26px;
  height: 26px;
  border: none;
  border-radius: 50%;
  background: #f0f2f5;
  color: #888;
  font-size: 12px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: all 0.2s;
}

.uid-close:hover {
  background: #e55;
  color: #fff;
}

.uid-body {
  padding: 0 20px 8px;
  font-size: 13px;
  color: #667;
  line-height: 1.6;
  overflow-y: auto;
}

.uid-footer {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  padding: 12px 20px 16px;
}
</style>
