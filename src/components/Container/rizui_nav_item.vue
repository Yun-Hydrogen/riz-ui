<template>
  <div
    class="rnav-item-wrap"
    :class="{
      'rnav-has-children': hasChildren,
      'rnav-is-active': isActive,
      'rnav-is-expanded': isExpanded,
      'rnav-is-disabled': item.disabled
    }"
  >
    <!-- 菜单项本体（按钮 / 链接 / router-link） -->
    <component
      :is="tagComponent"
      :to="item.route"
      :href="item.href"
      :target="item.target || (item.href ? '_blank' : undefined)"
      :type="tagComponent === 'button' ? 'button' : undefined"
      class="rnav-item-btn"
      :class="{ 'rnav-btn-collapsed': collapsed }"
      :style="indentStyle"
      :title="collapsed ? (item.label || item.name || item.zhName) : undefined"
      @click="handleClick"
    >
      <!-- WinUI 3 风格左侧指示条（胶囊 Pill Indicator） -->
      <span v-if="isActive && !hasChildren" class="rnav-indicator"></span>

      <!-- 图标 -->
      <span v-if="item.icon" class="rnav-icon" :style="item.iconStyle">
        <slot name="icon" :item="item" :active="isActive">
          <i :class="item.icon"></i>
        </slot>
      </span>

      <!-- 若无图标但在折叠模式下，显示首字首字母胶囊，避免收起后空白 -->
      <span v-else-if="collapsed" class="rnav-icon rnav-icon-avatar">
        {{ (item.label || item.name || item.zhName || '').slice(0, 1) }}
      </span>

      <!-- 文本与右侧小标（折叠为 Rail 时隐藏） -->
      <span v-if="!collapsed" class="rnav-label-box">
        <span class="rnav-label" :style="item.labelStyle">
          <slot name="label" :item="item" :active="isActive">
            {{ item.label || item.name || item.zhName }}
          </slot>
        </span>

        <!-- 徽章 / 标签 -->
        <span
          v-if="item.badge !== undefined && item.badge !== null && item.badge !== ''"
          class="rnav-badge"
          :style="{ backgroundColor: item.badgeColor }"
        >
          {{ item.badge }}
        </span>
        <span
          v-else-if="item.tag || item.deprecated"
          class="rnav-tag"
          :class="{ 'rnav-tag-depr': item.deprecated }"
        >
          {{ item.tag || (item.deprecated ? '旧' : '') }}
        </span>
      </span>

      <!-- 展开/收起折角箭头（带流畅旋转动效） -->
      <span v-if="hasChildren && !collapsed" class="rnav-caret" :class="{ open: isExpanded }">
        <svg viewBox="0 0 24 24" width="12" height="12" stroke="currentColor" stroke-width="2.5" fill="none">
          <polyline points="9 18 15 12 9 6"></polyline>
        </svg>
      </span>
    </component>

    <!-- 子目录展开动画区域（现代 CSS Grid 0fr ↔ 1fr，支持无限级嵌套） -->
    <div
      v-if="hasChildren && !collapsed"
      class="rnav-submenu"
      :class="{ open: isExpanded }"
    >
      <div class="rnav-submenu-inner">
        <rizui_nav_item
          v-for="(child, idx) in children"
          :key="child.id || child.route || child.label || idx"
          :item="child"
          :depth="depth + 1"
          :collapsed="collapsed"
          :active-id="activeId"
          :expanded-ids="expandedIds"
          @select="$emit('select', $event)"
          @toggle="$emit('toggle', $event)"
        >
          <template #icon="slotProps"><slot name="icon" v-bind="slotProps" /></template>
          <template #label="slotProps"><slot name="label" v-bind="slotProps" /></template>
        </rizui_nav_item>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, resolveComponent, getCurrentInstance } from 'vue'

defineOptions({
  name: 'rizui_nav_item'
})

const props = defineProps({
  item:        { type: Object,  required: true },
  depth:       { type: Number,  default: 0 },
  collapsed:   { type: Boolean, default: false },
  activeId:    { type: [String, Number], default: '' },
  expandedIds: { type: Set,     default: () => new Set() }
})

const emit = defineEmits(['select', 'toggle'])

const instance = getCurrentInstance()
const hasRouterLink = Boolean(
  instance?.appContext?.components?.RouterLink ||
  instance?.appContext?.components?.['router-link']
)

/* 子项目列表 */
const children = computed(() => {
  return props.item.children || props.item.comps || []
})

const hasChildren = computed(() => children.value.length > 0)

/* 当前项唯一标识 */
const currentId = computed(() => {
  return props.item.id ?? props.item.route ?? props.item.label ?? props.item.name ?? ''
})

/* 是否激活 */
const isActive = computed(() => {
  if (!props.activeId) return false
  if (currentId.value === props.activeId) return true
  if (props.item.route && props.item.route === props.activeId) return true
  return false
})

/* 是否展开 */
const isExpanded = computed(() => {
  return props.expandedIds.has(currentId.value)
})

/* 标签组件类型判断：优先支持 RouterLink，兼容 a 与 button */
const tagComponent = computed(() => {
  if (props.item.href) return 'a'
  if (props.item.route && hasRouterLink) {
    return instance?.appContext?.components?.RouterLink || instance?.appContext?.components?.['router-link'] || 'router-link'
  }
  return 'button'
})

/* 缩进层级样式 */
const indentStyle = computed(() => {
  if (props.collapsed) return {}
  const padLeft = 10 + props.depth * 14
  return {
    paddingLeft: `${padLeft}px`
  }
})

function handleClick(e) {
  if (props.item.disabled) {
    e.preventDefault()
    return
  }

  if (hasChildren.value) {
    emit('toggle', props.item)
  } else {
    emit('select', props.item)
    if (props.item.route && !hasRouterLink && instance?.proxy?.$router) {
      instance.proxy.$router.push(props.item.route)
    }
  }
}
</script>

<style scoped>
.rnav-item-wrap {
  position: relative;
  width: 100%;
  margin-bottom: 2px;
}

/* 菜单项主体按钮 */
.rnav-item-btn {
  position: relative;
  display: flex;
  align-items: center;
  width: 100%;
  padding: 8px 10px;
  border: none;
  background: transparent;
  border-radius: 8px;
  font-size: 13px;
  font-family: inherit;
  color: var(--rz-text);
  text-decoration: none;
  cursor: pointer;
  user-select: none;
  box-sizing: border-box;
  text-align: left;
  line-height: 1.4;
  outline: none;
  transition:
    background-color 0.16s ease,
    color 0.16s ease,
    transform 0.1s ease;
}

/* 悬浮微高亮 */
.rnav-item-btn:hover {
  background: color-mix(in srgb, var(--rz-text) 6%, var(--rz-bg));
  color: var(--rz-text);
}

/* WinUI 3 风格按压缩放回弹反馈 */
.rnav-item-btn:active {
  transform: scale(0.985);
}

/* 激活状态 */
.rnav-is-active > .rnav-item-btn {
  background: color-mix(in srgb, var(--rnav-color) 12%, var(--rz-bg));
  color: var(--rnav-color);
  font-weight: 600;
}

/* WinUI 3 经典左侧指示胶囊 (Pill Indicator) */
.rnav-indicator {
  position: absolute;
  left: 3px;
  top: 50%;
  transform: translateY(-50%);
  width: 3.5px;
  height: 16px;
  border-radius: 999px;
  background: var(--rnav-color);
  box-shadow: 0 0 6px color-mix(in srgb, var(--rnav-color) 40%, transparent);
  animation: rnav-pill-enter 0.22s cubic-bezier(0.2, 0.8, 0.2, 1);
}

@keyframes rnav-pill-enter {
  0% {
    transform: translateY(-50%) scaleY(0.3);
    opacity: 0;
  }
  100% {
    transform: translateY(-50%) scaleY(1);
    opacity: 1;
  }
}

/* 文字旁图标 */
.rnav-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 20px;
  height: 20px;
  margin-right: 8px;
  font-size: 14px;
  flex-shrink: 0;
  color: inherit;
  transition: transform 0.2s ease;
}

.rnav-item-btn:hover .rnav-icon {
  transform: scale(1.08);
}

/* ── 折叠 Rail 模式下按钮与图标绝对水平垂直居中 ── */
.rnav-btn-collapsed {
  justify-content: center;
  padding: 8px 0;
  width: 100%;
}

.rnav-btn-collapsed .rnav-icon {
  margin-right: 0 !important;
  margin-left: 0 !important;
  font-size: 16px;
}

.rnav-btn-collapsed .rnav-indicator {
  left: 2px;
  height: 18px;
}

.rnav-icon-avatar {
  font-size: 13px;
  font-weight: 700;
  border-radius: 6px;
  background: color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  color: var(--rz-text);
}

/* 文字与标签包装 */
.rnav-label-box {
  display: flex;
  align-items: center;
  flex: 1;
  min-width: 0;
}

.rnav-label {
  flex: 1;
  min-width: 0;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

/* 折角箭头指示器 */
.rnav-caret {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 16px;
  height: 16px;
  margin-left: 6px;
  color: var(--rz-subtext);
  flex-shrink: 0;
  transition: transform 0.24s cubic-bezier(0.4, 0, 0.2, 1), color 0.16s ease;
}

.rnav-caret.open {
  transform: rotate(90deg);
  color: var(--rz-text);
}

/* 徽章 Badge */
.rnav-badge {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 0 6px;
  min-width: 16px;
  height: 16px;
  border-radius: 999px;
  font-size: 10px;
  font-weight: 700;
  color: #fff;
  background: var(--rnav-color);
  margin-left: 6px;
}

/* 标签 Tag */
.rnav-tag {
  display: inline-block;
  margin-left: 6px;
  padding: 1px 5px;
  border-radius: 4px;
  background: color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  color: var(--rz-subtext);
  font-size: 10px;
  font-weight: 600;
  line-height: 1.3;
}

.rnav-tag-depr {
  background: rgba(230, 162, 60, 0.18);
  color: #e6a23c;
}

/* 禁用态 */
.rnav-is-disabled > .rnav-item-btn {
  opacity: 0.45;
  cursor: not-allowed;
  pointer-events: none;
}

/* ── 子目录展开动画（Grid 0fr ↔ 1fr，现代流畅无卡顿） ── */
.rnav-submenu {
  display: grid;
  grid-template-rows: 0fr;
  opacity: 0;
  padding: 0;
  transition:
    grid-template-rows 0.28s cubic-bezier(0.4, 0, 0.2, 1),
    opacity 0.2s ease,
    padding 0.2s ease;
}

.rnav-submenu.open {
  grid-template-rows: 1fr;
  opacity: 1;
  padding: 2px 0 4px;
}

.rnav-submenu-inner {
  overflow: hidden;
  min-height: 0;
}
</style>
