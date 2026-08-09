<template>
  <div class="ui-card">
    <div class="ui-card-title">{{ title }}</div>
    <div v-if="desc" class="ui-card-desc">{{ desc }}</div>
    <slot />
  </div>
</template>

<script setup>
defineProps({
  title: { type: String, required: true },
  desc:  { type: String, default: '' }
})
</script>

<style scoped>
.ui-card {
  padding: 14px 16px;
  margin-bottom: 12px;
  border-radius: 14px;
  background: rgb(248, 250, 252, 0.6);
  border: 1px solid #e8ecf2;
}

.ui-card-title {
  font-size: 14px;
  font-weight: 700;
  color: #334;
  margin-bottom: 4px;
}

.ui-card-desc {
  font-size: 12px;
  color: #99a;
  margin-bottom: 12px;
  line-height: 1.5;
}
</style>
