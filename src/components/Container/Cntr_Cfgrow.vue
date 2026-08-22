<template>
  <div class="cfgr-row" :class="{ 'cfgr-stack': stack, 'cfgr-disabled': disabled }">
    <!-- 头部：图标 + label/hint（图标仅跨 label/hint 两行） -->
    <div class="cfgr-head">
      <div v-if="icon" class="cfgr-icon" :style="iconInstyle">
        <i :class="icon"></i>
      </div>
      <div class="cfgr-head-text">
        <div v-if="label" class="cfgr-label" :style="labelInstyle">{{ label }}</div>
        <div v-if="hint" class="cfgr-hint" :style="hintInstyle">{{ hint }}</div>
      </div>
      <!-- 非 stack 模式控件在右侧 -->
      <div v-if="!stack" class="cfgr-slot-inner" :class="{ 'cfgr-slot-disabled': disabled }">
        <slot/>
      </div>
    </div>
    <!-- stack 模式控件在头部下方独立一行 -->
    <div v-if="stack" class="cfgr-slot-wrap">
      <div class="cfgr-slot-inner" :class="{ 'cfgr-slot-disabled': disabled }">
        <slot/>
      </div>
    </div>
  </div>
</template>

<script setup>
defineProps({
  label: {type: String, default: ''},
  labelInstyle: {type: Object, default: () => ({})},
  hint: {type: String, default: ''},
  hintInstyle: {type: Object, default: () => ({})},
  icon: {type: String, default: ''},
  iconInstyle: {type: Object, default: () => ({})},
  stack: {type: Boolean, default: false},
  disabled: {type: Boolean, default: false}
})
</script>

<style scoped>
.cfgr-row {
  padding: 12px 0;
  border-bottom: 1px solid #f5f5f5;
}

/* ── 头部行：图标 + label/hint（横向排列，图标跨 label/hint 两行） ── */
.cfgr-head {
  display: flex;
  align-items: center;
  gap: 14px;
}

.cfgr-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  width: 36px;
  font-size: 18px;
  color: #99a;
}

.cfgr-head-text {
  flex: 1;
  min-width: 0;
}

.cfgr-label {
  font-size: 14px;
  color: #444;
  line-height: 1.4;
  margin-bottom: 2px;
}

.cfgr-hint {
  font-size: 11px;
  color: #99a;
  overflow-wrap: break-word;
  line-height: 1.4;
}

/* ── stack 模式：控件在头部下方独立一行，不影响头部图标高度 ── */
.cfgr-slot-wrap {
  margin-top: 8px;
}

.cfgr-slot-inner {
  display: flex;
  align-items: center;
  flex: 1;
  min-width: 0;
  justify-content: flex-end;
}

.cfgr-stack .cfgr-slot-inner {
  display: block;
}

.cfgr-slot-disabled {
  opacity: 0.45;
  pointer-events: none;
  user-select: none;
}
</style>