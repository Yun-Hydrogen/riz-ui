<template>
  <div class="ct-card" :data-rizui-theme="themeAttr">
    <!-- 头部：图标 + 标题/描述（图标仅跨这两行） -->
    <div class="ct-card-head">
      <div v-if="icon" class="ct-card-icon" :style="iconStyle">
        <i :class="icon"></i>
      </div>
      <div class="ct-card-head-text">
        <div v-if="title" class="ct-card-title" :style="titleStyle">{{ title }}</div>
        <div v-if="desc" class="ct-card-desc" :style="descStyle">{{ desc }}</div>
      </div>
    </div>

    <!-- 内容区 -->
    <div v-if="$slots.default" class="ct-card-body">
      <slot />
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  /** 卡片标题 Card title */
  title:        { type: String, default: '' },
  /** 标题样式覆盖 Title style override */
  titleStyle: { type: Object, default: () => ({}) },
  /** 卡片描述 Card description */
  desc:         { type: String, default: '' },
  /** 描述样式覆盖 Desc style override */
  descStyle:  { type: Object, default: () => ({}) },
  /** 卡片图标（完整 FA class）Card icon (full FA class) */
  icon:         { type: String, default: '' },
  /** 图标样式覆盖 Icon style override */
  iconStyle:  { type: Object, default: () => ({}) },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:        { type: String, default: 'auto' }
})

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})
</script>

<style scoped>
.ct-card {
  padding: 14px 16px;
  margin-bottom: 12px;
  border-radius: 14px;
  background: var(--rz-bg-soft);
  border: 1px solid var(--rz-border);
  box-shadow: 0 2px 10px var(--rz-shadow);
}

/* 组件级强制暗色（theme="Dark"） */
.ct-card[data-rizui-theme="dark"] {
  --rz-bg-soft: var(--rz-dark-bg-soft);
  --rz-border: var(--rz-dark-border);
  --rz-text: var(--rz-dark-text);
  --rz-subtext: var(--rz-dark-subtext);
  --rz-icon: var(--rz-dark-icon);
}

/* 组件级强制亮色（theme="Light"） */
.ct-card[data-rizui-theme="light"] {
  --rz-bg-soft: rgb(248, 250, 252, 0.6);
  --rz-border: #e8ecf2;
  --rz-text: #334;
  --rz-subtext: #99a;
  --rz-icon: #99a;
}

/* ── 头部行：图标 + 标题/描述 ── */
.ct-card-head {
  display: flex;
  align-items: center;
  gap: 14px;
}

.ct-card-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  width: 40px;
  font-size: 20px;
  color: var(--rz-icon);
}

.ct-card-head-text {
  flex: 1;
  min-width: 0;
}

.ct-card-title {
  font-size: 14px;
  font-weight: 700;
  color: var(--rz-text);
  margin-bottom: 4px;
}

.ct-card-desc {
  font-size: 12px;
  color: var(--rz-subtext);
  line-height: 1.5;
}

/* ── 内容插槽区 ── */
.ct-card-body {
  margin-top: 12px;
}
</style>
