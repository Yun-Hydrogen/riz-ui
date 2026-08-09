<template>
  <div class="ct-card">
    <!-- 头部：图标 + 标题/描述（图标仅跨这两行） -->
    <div class="ct-card-head">
      <div v-if="icon" class="ct-card-icon" :style="iconInstyle">
        <i :class="icon"></i>
      </div>
      <div class="ct-card-head-text">
        <div v-if="title" class="ct-card-title" :style="titleInstyle">{{ title }}</div>
        <div v-if="desc" class="ct-card-desc" :style="descInstyle">{{ desc }}</div>
      </div>
    </div>

    <!-- 内容区 -->
    <div v-if="$slots.default" class="ct-card-body">
      <slot />
    </div>
  </div>
</template>

<script setup>
defineProps({
  title:        { type: String, default: '' },
  titleInstyle: { type: Object, default: () => ({}) },
  desc:         { type: String, default: '' },
  descInstyle:  { type: Object, default: () => ({}) },
  icon:         { type: String, default: '' },
  iconInstyle:  { type: Object, default: () => ({}) }
})
</script>

<style scoped>
.ct-card {
  padding: 14px 16px;
  margin-bottom: 12px;
  border-radius: 14px;
  background: rgb(248, 250, 252, 0.6);
  border: 1px solid #e8ecf2;
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
  color: #99a;
}

.ct-card-head-text {
  flex: 1;
  min-width: 0;
}

.ct-card-title {
  font-size: 14px;
  font-weight: 700;
  color: #334;
  margin-bottom: 4px;
}

.ct-card-desc {
  font-size: 12px;
  color: #99a;
  line-height: 1.5;
}

/* ── 内容插槽区 ── */
.ct-card-body {
  margin-top: 12px;
}
</style>
