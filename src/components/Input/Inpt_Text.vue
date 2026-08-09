<template>
  <textarea
    v-if="plaintext"
    class="inpt-root inpt-plain"
    :value="modelValue"
    :placeholder="placeholder"
    :disabled="disabled"
    :readonly="readonly"
    :style="rootStyle"
    rows="4"
    @input="$emit('update:modelValue', $event.target.value)"
  ></textarea>
  <input
    v-else
    class="inpt-root"
    :type="type"
    :value="modelValue"
    :placeholder="placeholder"
    :disabled="disabled"
    :readonly="readonly"
    :style="rootStyle"
    @input="$emit('update:modelValue', type === 'number' ? ($event.target.value === '' ? null : Number($event.target.value)) : $event.target.value)"
  />
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  modelValue:   { type: [String, Number], default: '' },
  type:         { type: String,  default: 'text' },
  placeholder:  { type: String,  default: '' },
  color:        { type: String,  default: '#39c5bb' },
  plaintext:    { type: Boolean, default: false },
  disabled:     { type: Boolean, default: false },
  readonly:     { type: Boolean, default: false }
})

defineEmits(['update:modelValue'])

const rootStyle = computed(() => ({
  '--inpt-color': props.color
}))
</script>

<style scoped>
.inpt-root {
  padding: 5px 12px;
  border: 1.5px solid #d0d8e0;
  border-radius: 999px;
  font-size: 12px;
  text-align: center;
  outline: none;
  font-family: inherit;
  color: #445;
  background: #fff;
  transition: border-color 0.2s;
  box-sizing: border-box;
}

.inpt-root:focus {
  border-color: var(--inpt-color, #39c5bb);
}

.inpt-root::placeholder {
  color: #b0b8c4;
}

.inpt-root:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.inpt-root:read-only {
  background: #f5f6f8;
  color: #99a;
}

/* 隐藏 number 输入框的上下箭头 */
.inpt-root::-webkit-inner-spin-button,
.inpt-root::-webkit-outer-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

.inpt-root {
  -moz-appearance: textfield;
  appearance: textfield;
}

/* ── plaintext 多行模式 ── */
.inpt-plain {
  display: block;
  width: 100%;
  border-radius: 10px;
  text-align: left;
  line-height: 1.6;
  resize: vertical;
  min-height: 80px;
  padding: 8px 12px;
}
</style>