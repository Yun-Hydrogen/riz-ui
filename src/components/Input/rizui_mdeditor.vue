<template>
  <div class="imd-root" :data-rizui-theme="themeAttr" :style="rootStyle" :class="{ 'imd-readonly': readonly }">
    <!-- ═══ 内容画布（滚动容器；玻璃顶栏悬浮于画布上方，覆盖滚动的正文） ═══ -->
    <div class="imd-body" :style="{ height }">
    <!-- ═══ 顶栏工具栏（readonly 隐藏，液态玻璃胶囊） ═══ -->
    <div v-if="!readonly" class="imd-toolbar">
      <div class="imd-toolbar-btns">
        <button
          v-for="b in toolbarBtns"
          :key="b.id"
          class="imd-tb"
          :data-tip="b.title"
          :aria-label="b.title"
          type="button"
          :disabled="disabled"
          @mousedown.prevent
          @click="onToolbar(b.id)"
        ><i :class="b.icon"></i></button>
      </div>

      <!-- 编辑 / 预览 选项卡（右侧，滑块滑动切换） -->
      <div class="imd-tabs">
        <span class="imd-tab-slider" :class="{ 'imd-tab-slider-right': activeTab === 'preview' }"></span>
        <button
          class="imd-tab"
          :class="{ 'imd-tab-active': activeTab === 'edit' }"
          type="button"
          @mousedown.prevent
          @click="switchTab('edit')"
        ><i class="fa-solid fa-pen"></i> 编辑</button>
        <button
          class="imd-tab"
          :class="{ 'imd-tab-active': activeTab === 'preview' }"
          type="button"
          @mousedown.prevent
          @click="switchTab('preview')"
        ><i class="fa-solid fa-eye"></i> 预览</button>
      </div>
    </div>

    <Transition :name="tabAnim" mode="out-in">
      <textarea
        v-if="!readonly && activeTab === 'edit'"
        ref="taRef"
        class="imd-editor"
        :value="modelValue"
        :placeholder="placeholder"
        :disabled="disabled"
        @input="$emit('update:modelValue', $event.target.value)"
        @keydown.tab.prevent="insertIndent"
      ></textarea>

      <div v-else-if="readonly || activeTab === 'preview'" class="imd-preview">
        <div class="imd-preview-inner" v-html="renderedHtml"></div>
      </div>
    </Transition>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, nextTick, onMounted, watch } from 'vue'

const props = defineProps({
  /** v-model 绑定 Markdown 文本 Markdown content via v-model */
  modelValue:  { type: String,  default: '' },
  /** 占位提示文字 Placeholder */
  placeholder: { type: String,  default: '在此输入 Markdown 内容…' },
  /** 自定义颜色，空则跟随主题色 Custom color (defaults to theme) */
  color:       { type: String,  default: '' },
  /** 禁用 Disabled */
  disabled:    { type: Boolean, default: false },
  /** 只读（仅预览）Readonly (preview only) */
  readonly:    { type: Boolean, default: false },
  /** 编辑 / 预览区高度 Editor height */
  height:      { type: String,  default: '320px' },
  /** 初始板块 edit / preview Default tab */
  default:     { type: String,  default: 'edit' },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:       { type: String,  default: 'auto' }
})

const emit = defineEmits(['update:modelValue'])

const taRef = ref(null)
/* 编辑 / 预览 选项卡：default prop 控制初始板块（'edit' | 'preview'） */
const activeTab = ref(props.default === 'preview' ? 'preview' : 'edit')
/* tab 切换时内容滑入方向：edit→preview 左滑，preview→edit 右滑 */
const tabAnim = ref('anim-left')

function switchTab(tab) {
  if (tab === activeTab.value) return
  tabAnim.value = tab === 'preview' ? 'anim-left' : 'anim-right'
  activeTab.value = tab
}

/* 编辑框高度随内容增长（画布 .imd-body 统一滚动，玻璃顶栏才能悬浮覆盖正文） */
function autosizeTextarea() {
  const ta = taRef.value
  if (!ta) return
  ta.style.height = 'auto'
  ta.style.height = ta.scrollHeight + 'px'
}
watch(() => props.modelValue, () => nextTick(autosizeTextarea))
watch(() => props.height, () => nextTick(autosizeTextarea))
onMounted(() => nextTick(autosizeTextarea))

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const rootStyle = computed(() => ({
  '--imd-color': props.color || 'var(--rizui_themecolor)'
}))

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})

/* ════════════════════════════════════════════════════════
   工具栏定义
   ════════════════════════════════════════════════════════ */
const toolbarBtns = [
  { id: 'bold',      title: '加粗',     icon: 'fa-solid fa-bold' },
  { id: 'italic',    title: '斜体',     icon: 'fa-solid fa-italic' },
  { id: 'strike',    title: '删除线',   icon: 'fa-solid fa-strikethrough' },
  { id: 'h2',        title: '二级标题', icon: 'fa-solid fa-heading' },
  { id: 'quote',     title: '引用',     icon: 'fa-solid fa-quote-left' },
  { id: 'ul',        title: '无序列表', icon: 'fa-solid fa-list-ul' },
  { id: 'ol',        title: '有序列表', icon: 'fa-solid fa-list-ol' },
  { id: 'task',      title: '任务列表', icon: 'fa-solid fa-list-check' },
  { id: 'code',      title: '行内代码', icon: 'fa-solid fa-code' },
  { id: 'codeblock', title: '代码块',   icon: 'fa-solid fa-code-branch' },
  { id: 'link',      title: '链接',     icon: 'fa-solid fa-link' },
  { id: 'image',     title: '图片',     icon: 'fa-solid fa-image' },
  { id: 'table',     title: '表格',     icon: 'fa-solid fa-table' },
  { id: 'hr',        title: '分割线',   icon: 'fa-solid fa-minus' }
]

/* ════════════════════════════════════════════════════════
   光标插入逻辑
   ════════════════════════════════════════════════════════ */
function insert(before, after = '', select = '') {
  const ta = taRef.value
  if (!ta) return
  const start = ta.selectionStart
  const end = ta.selectionEnd
  const sel = props.modelValue.slice(start, end) || select
  const next = props.modelValue.slice(0, start) + before + sel + after + props.modelValue.slice(end)
  emit('update:modelValue', next)
  nextTick(() => {
    ta.focus()
    const pos = start + before.length + sel.length
    ta.setSelectionRange(pos, pos)
  })
}

/* 在当前行首添加前缀 */
function linePrefix(prefix) {
  const ta = taRef.value
  if (!ta) return
  const start = ta.selectionStart
  const lineStart = props.modelValue.lastIndexOf('\n', start - 1) + 1
  const lineEnd = props.modelValue.indexOf('\n', start)
  const lineEndPos = lineEnd === -1 ? props.modelValue.length : lineEnd
  const lineText = props.modelValue.slice(lineStart, lineEndPos)
  const next = props.modelValue.slice(0, lineStart) + prefix + lineText + props.modelValue.slice(lineEndPos)
  emit('update:modelValue', next)
  nextTick(() => {
    ta.focus()
    const pos = lineStart + prefix.length + (start - lineStart)
    ta.setSelectionRange(pos, pos)
  })
}

function insertIndent() {
  const ta = taRef.value
  if (!ta) return
  const start = ta.selectionStart
  const end = ta.selectionEnd
  const next = props.modelValue.slice(0, start) + '  ' + props.modelValue.slice(end)
  emit('update:modelValue', next)
  nextTick(() => {
    ta.focus()
    ta.setSelectionRange(start + 2, start + 2)
  })
}

function onToolbar(id) {
  /* 若当前在预览选项卡，先切回编辑再执行插入 */
  if (activeTab.value !== 'edit') {
    switchTab('edit')
    nextTick(() => doToolbar(id))
  } else {
    doToolbar(id)
  }
}

function doToolbar(id) {
  if (props.disabled) return
  switch (id) {
    case 'bold':      insert('**', '**', '加粗文本'); break
    case 'italic':    insert('*', '*', '斜体文本'); break
    case 'strike':    insert('~~', '~~', '删除文本'); break
    case 'h2':        linePrefix('## '); break
    case 'quote':     linePrefix('> '); break
    case 'ul':        linePrefix('- '); break
    case 'ol':        linePrefix('1. '); break
    case 'task':      linePrefix('- [ ] '); break
    case 'code':      insert('`', '`', 'code'); break
    case 'codeblock': insert('\n```\n', '\n```\n', 'const a = 1'); break
    case 'link':      insert('[', '](https://)', '链接文字'); break
    case 'image':     insert('![', '](https://example.com/image.png)', '图片描述'); break
    case 'table':     insert('\n| 列1 | 列2 |\n| --- | --- |\n| 内容 | 内容 |\n', '', ''); break
    case 'hr':        insert('\n---\n', '', ''); break
  }
}

/* ════════════════════════════════════════════════════════
   Markdown 渲染（轻量解析器）
   ════════════════════════════════════════════════════════ */
const renderedHtml = computed(() => renderMarkdown(props.modelValue || ''))

function escapeHtml(s) {
  return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;')
}

function safeUrl(u) {
  return /^(https?:|mailto:|#|\/|\.\/|\.\.\/|data:image\/)/i.test(u) ? u : '#'
}

function renderInline(text) {
  let t = escapeHtml(text)
  // 行内代码
  t = t.replace(/`([^`]+)`/g, '<code>$1</code>')
  // 图片
  t = t.replace(/!\[([^\]]*)\]\(([^)\s]+)\)/g, (m, alt, url) => `<img src="${safeUrl(url)}" alt="${alt}" loading="lazy">`)
  // 链接
  t = t.replace(/\[([^\]]+)\]\(([^)\s]+)\)/g, (m, label, url) => `<a href="${safeUrl(url)}" target="_blank" rel="noopener">${label}</a>`)
  // 粗体
  t = t.replace(/\*\*([^*]+)\*\*/g, '<strong>$1</strong>')
  // 斜体
  t = t.replace(/(^|[^*])\*([^*\s][^*]*)\*/g, '$1<em>$2</em>')
  // 删除线
  t = t.replace(/~~([^~]+)~~/g, '<del>$1</del>')
  return t
}

function renderMarkdown(md) {
  const src = md.replace(/\r\n/g, '\n')
  const lines = src.split('\n')
  const html = []
  let i = 0
  let paragraph = []

  const flushParagraph = () => {
    if (paragraph.length) {
      html.push('<p>' + paragraph.map(renderInline).join('<br>') + '</p>')
      paragraph = []
    }
  }

  while (i < lines.length) {
    const line = lines[i]
    const trimmed = line.trim()

    if (!trimmed) { flushParagraph(); i++; continue }

    /* 代码块 */
    if (trimmed.startsWith('```')) {
      flushParagraph()
      const buf = []
      i++
      while (i < lines.length && !lines[i].trim().startsWith('```')) {
        buf.push(lines[i]); i++
      }
      i++
      html.push('<pre><code>' + escapeHtml(buf.join('\n')) + '</code></pre>')
      continue
    }

    /* 标题 */
    const h = trimmed.match(/^(#{1,6})\s+(.*)$/)
    if (h) {
      flushParagraph()
      const lvl = h[1].length
      html.push(`<h${lvl}>${renderInline(h[2])}</h${lvl}>`)
      i++; continue
    }

    /* 分割线 */
    if (/^(-{3,}|\*{3,}|_{3,})$/.test(trimmed)) {
      flushParagraph()
      html.push('<hr>')
      i++; continue
    }

    /* 表格：当前行含 | 且下一行是分隔行 */
    if (trimmed.includes('|') && lines[i + 1] && /^\s*\|?[\s:\-|]+\|?\s*$/.test(lines[i + 1].trim())) {
      flushParagraph()
      const splitRow = (row) => row.replace(/^\|/, '').replace(/\|$/, '').split('|').map(s => s.trim())
      const head = splitRow(trimmed)
      i += 2
      const rows = []
      while (i < lines.length && lines[i].trim() && lines[i].includes('|')) {
        rows.push(splitRow(lines[i]))
        i++
      }
      let tbl = '<table><thead><tr>' + head.map(c => '<th>' + renderInline(c) + '</th>').join('') + '</tr></thead><tbody>'
      tbl += rows.map(r => '<tr>' + r.map(c => '<td>' + renderInline(c) + '</td>').join('') + '</tr>').join('')
      tbl += '</tbody></table>'
      html.push(tbl)
      continue
    }

    /* 引用 */
    if (trimmed.startsWith('>')) {
      flushParagraph()
      const quote = []
      while (i < lines.length && lines[i].trim().startsWith('>')) {
        quote.push(lines[i].trim().replace(/^>\s?/, ''))
        i++
      }
      html.push('<blockquote><p>' + quote.map(renderInline).join('<br>') + '</p></blockquote>')
      continue
    }

    /* 无序列表（含任务列表） */
    const ul = trimmed.match(/^[-*+]\s+(.*)$/)
    if (ul) {
      flushParagraph()
      html.push('<ul>')
      while (i < lines.length) {
        const m = lines[i].trim().match(/^[-*+]\s+(.*)$/)
        if (!m) break
        const task = m[1].match(/^\[([ xX])\]\s+(.*)$/)
        if (task) {
          const checked = task[1].toLowerCase() === 'x' ? ' checked' : ''
          html.push(`<li class="imd-task${checked}"><input type="checkbox"${checked} disabled> ${renderInline(task[2])}</li>`)
        } else {
          html.push('<li>' + renderInline(m[1]) + '</li>')
        }
        i++
      }
      html.push('</ul>')
      continue
    }

    /* 有序列表 */
    const ol = trimmed.match(/^\d+\.\s+(.*)$/)
    if (ol) {
      flushParagraph()
      html.push('<ol>')
      while (i < lines.length) {
        const m = lines[i].trim().match(/^\d+\.\s+(.*)$/)
        if (!m) break
        html.push('<li>' + renderInline(m[1]) + '</li>')
        i++
      }
      html.push('</ol>')
      continue
    }

    /* 普通段落 */
    paragraph.push(trimmed)
    i++
  }
  flushParagraph()
  return html.join('\n')
}
</script>

<style scoped>
.imd-root {
  position: relative;
  display: flex;
  flex-direction: column;
  width: 100%;
  border: 1.5px solid var(--rz-border);
  border-radius: 12px;
  overflow: clip;
  background: var(--rz-bg);
}

/* 组件级强制暗色（theme="Dark"） */
.imd-root[data-rizui-theme="dark"] {
  --rz-bg: var(--rz-dark-bg);
  --rz-border: var(--rz-dark-border);
  --rz-text: var(--rz-dark-text);
  --rz-subtext: var(--rz-dark-subtext);
  --rz-shadow: var(--rz-dark-shadow);
}

/* 组件级强制亮色（theme="Light"） */
.imd-root[data-rizui-theme="light"] {
  --rz-bg: #fff;
  --rz-border: #e8ecf2;
  --rz-text: #334;
  --rz-subtext: #99a;
  --rz-shadow: rgba(30, 41, 59, 0.08);
}

/* ── 聚焦动画：完整圆角主题色边框（2.5px）从中轴往两侧展开并蔓延发光；失焦反向收回 ── */
.imd-root::before {
  content: '';
  position: absolute;
  inset: 0;
  border-radius: inherit;
  border: 2px solid var(--imd-color);
  pointer-events: none;
  opacity: 0;
  filter: drop-shadow(0 0 6px color-mix(in srgb, var(--imd-color) 55%, transparent));
  mask-image: linear-gradient(#000, #000);
  -webkit-mask-image: linear-gradient(#000, #000);
  mask-repeat: no-repeat;
  -webkit-mask-repeat: no-repeat;
  mask-size: 0% 100%;
  -webkit-mask-size: 0% 100%;
  mask-position: center;
  -webkit-mask-position: center;
  transition: mask-size 0.35s ease, -webkit-mask-size 0.35s ease, opacity 0.2s ease;
  z-index: 6;
}

.imd-root:focus-within::before {
  mask-size: 100% 100%;
  -webkit-mask-size: 100% 100%;
  opacity: 1;
}

/* ═══ 工具栏（高斯模糊玻璃：高透明底 + 强模糊，正文透过后被虚化，滚动吸顶遮挡内容） ═══ */
.imd-toolbar {
  position: sticky;
  top: 10px;
  z-index: 5;
  margin: 10px 10px 6px;
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 5px 12px;
  border: 1px solid color-mix(in srgb, var(--rz-text) 12%, transparent);
  border-radius: 999px;
  background: color-mix(in srgb, var(--rz-bg) 38%, transparent);
  backdrop-filter: blur(24px) saturate(160%);
  -webkit-backdrop-filter: blur(24px) saturate(160%);
  box-shadow: 0 4px 20px color-mix(in srgb, var(--rz-shadow) 70%, transparent);
}

.imd-toolbar-btns {
  display: flex;
  align-items: center;
  gap: 2px;
  flex: 1;
  min-width: 0;
  flex-wrap: wrap;
}

.imd-tb {
  position: relative;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 28px;
  height: 28px;
  border: none;
  border-radius: 6px;
  background: transparent;
  color: var(--rz-subtext);
  font-size: 12px;
  cursor: pointer;
}

/* ── 自定义工具提示（替代原生 title） ── */
.imd-tb::after {
  content: attr(data-tip);
  position: absolute;
  top: calc(100% + 6px);
  left: 50%;
  transform: translateX(-50%) translateY(3px);
  background: color-mix(in srgb, var(--rz-text) 88%, var(--rz-bg));
  color: var(--rz-bg);
  font-size: 11px;
  line-height: 1.4;
  font-weight: 500;
  padding: 3px 9px;
  border-radius: 6px;
  white-space: nowrap;
  opacity: 0;
  pointer-events: none;
  transition: opacity 0.15s ease, transform 0.15s ease;
  z-index: 8;
}

.imd-tb:hover::after {
  opacity: 1;
  transform: translateX(-50%) translateY(0);
}

.imd-tb:hover:not(:disabled) {
  background: color-mix(in srgb, var(--rz-text) 6%, var(--rz-bg));
  color: var(--imd-color);
}

.imd-tb:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

/* ── 编辑 / 预览 选项卡（右侧胶囊，滑块滑动切换） ── */
.imd-tabs {
  position: relative;
  display: inline-flex;
  gap: 4px;
  padding: 3px;
  border-radius: 999px;
  background: color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  flex-shrink: 0;
}

/* 滑块：从当前 tab 滑向目标 tab */
.imd-tab-slider {
  position: absolute;
  top: 3px;
  bottom: 3px;
  left: 3px;
  width: calc(50% - 3px);
  border-radius: 999px;
  background: var(--rz-bg);
  box-shadow: 0 2px 8px var(--rz-shadow);
  transition: transform 0.28s cubic-bezier(0.4, 0, 0.2, 1);
  pointer-events: none;
}

.imd-tab-slider-right {
  transform: translateX(100%);
}

.imd-tab {
  position: relative;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 6px;
  flex: 1;
  padding: 5px 14px;
  border: none;
  border-radius: 999px;
  background: transparent;
  font-size: 12px;
  font-weight: 500;
  font-family: inherit;
  color: var(--rz-subtext);
  cursor: pointer;
  transition: color 0.2s ease;
}

.imd-tab:hover:not(:disabled) {
  color: var(--imd-color);
}

.imd-tab:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.imd-tab-active {
  background: transparent;
  color: var(--imd-color);
}

/* ═══ 内容画布（滚动容器：正文在此滚动，玻璃顶栏悬浮覆盖其上） ═══ */
.imd-body {
  position: relative;
  overflow-y: auto;
  overscroll-behavior: contain;
  background: var(--rz-bg);
}

.imd-editor {
  display: block;
  width: 100%;
  min-height: 100%;
  height: auto;
  padding: 12px 14px;
  border: none;
  outline: none;
  resize: none;
  overflow: hidden;
  font-family: ui-monospace, 'Cascadia Code', Consolas, 'Courier New', monospace;
  font-size: 13px;
  line-height: 1.7;
  color: var(--rz-text);
  background: transparent;
}

.imd-editor:disabled {
  opacity: 0.55;
  cursor: not-allowed;
}

.imd-preview {
  display: block;
  width: 100%;
  min-height: 100%;
  overflow: visible;
  background: transparent;
}

/* ── 滚动条：主题色半透明（画布滚动） ── */
.imd-body::-webkit-scrollbar {
  width: 8px;
  height: 8px;
}

.imd-body::-webkit-scrollbar-track {
  background: transparent;
}

.imd-body::-webkit-scrollbar-thumb {
  background: color-mix(in srgb, var(--imd-color) 28%, transparent);
  border-radius: 999px;
}

.imd-body::-webkit-scrollbar-thumb:hover {
  background: color-mix(in srgb, var(--imd-color) 48%, transparent);
}

.imd-body {
  scrollbar-width: thin;
  scrollbar-color: color-mix(in srgb, var(--imd-color) 28%, transparent) transparent;
}

/* ── readonly：仅显示预览，画布内滚动 ── */
.imd-readonly .imd-preview {
  min-height: 100%;
}

.imd-preview-inner {
  padding: 12px 14px;
  font-size: 13px;
  line-height: 1.7;
  color: var(--rz-text);
  word-break: break-word;
}

/* ── 预览区 Markdown 样式（作用于 v-html 内容） ── */
.imd-preview-inner :deep(h1),
.imd-preview-inner :deep(h2),
.imd-preview-inner :deep(h3),
.imd-preview-inner :deep(h4),
.imd-preview-inner :deep(h5),
.imd-preview-inner :deep(h6) {
  margin: 1em 0 0.5em;
  font-weight: 700;
  color: var(--rz-text);
  line-height: 1.4;
}

.imd-preview-inner :deep(h1) { font-size: 22px; border-bottom: 1px solid var(--rz-border); padding-bottom: 6px; }
.imd-preview-inner :deep(h2) { font-size: 18px; border-bottom: 1px solid var(--rz-border); padding-bottom: 5px; }
.imd-preview-inner :deep(h3) { font-size: 16px; }
.imd-preview-inner :deep(h4) { font-size: 14px; }
.imd-preview-inner :deep(p) { margin: 0.5em 0; }
.imd-preview-inner :deep(a) { color: var(--imd-color); text-decoration: none; }
.imd-preview-inner :deep(a:hover) { text-decoration: underline; }
.imd-preview-inner :deep(code) {
  padding: 1px 6px;
  border-radius: 4px;
  background: color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  color: #c7254e;
  font-family: ui-monospace, Consolas, monospace;
  font-size: 12px;
}
.imd-preview-inner :deep(pre) {
  margin: 0.75em 0;
  padding: 12px 14px;
  border-radius: 8px;
  background: color-mix(in srgb, var(--rz-text) 5%, var(--rz-bg));
  overflow-x: auto;
}
.imd-preview-inner :deep(pre code) {
  padding: 0;
  background: transparent;
  color: var(--rz-text);
}
.imd-preview-inner :deep(blockquote) {
  margin: 0.75em 0;
  padding: 6px 14px;
  border-left: 3px solid var(--imd-color);
  background: color-mix(in srgb, var(--imd-color) 6%, transparent);
  border-radius: 0 8px 8px 0;
  color: var(--rz-subtext);
}
.imd-preview-inner :deep(ul),
.imd-preview-inner :deep(ol) {
  margin: 0.5em 0;
  padding-left: 1.6em;
}
.imd-preview-inner :deep(li) { margin: 0.2em 0; }
.imd-preview-inner :deep(li.imd-task) { list-style: none; margin-left: -1.2em; }
.imd-preview-inner :deep(li.imd-task input) { margin-right: 6px; accent-color: var(--imd-color); }
.imd-preview-inner :deep(table) {
  margin: 0.75em 0;
  border-collapse: collapse;
  width: 100%;
  font-size: 12px;
}
.imd-preview-inner :deep(th),
.imd-preview-inner :deep(td) {
  padding: 6px 10px;
  border: 1px solid var(--rz-border);
  text-align: left;
}
.imd-preview-inner :deep(th) {
  background: color-mix(in srgb, var(--rz-text) 5%, var(--rz-bg));
  font-weight: 600;
  color: var(--rz-text);
}
.imd-preview-inner :deep(tr:nth-child(2n) td) { background: color-mix(in srgb, var(--rz-text) 3%, var(--rz-bg)); }
.imd-preview-inner :deep(img) {
  max-width: 100%;
  border-radius: 8px;
  margin: 0.5em 0;
}
.imd-preview-inner :deep(hr) {
  margin: 1em 0;
  border: none;
  border-top: 1px solid var(--rz-border);
}
.imd-preview-inner :deep(del) { color: var(--rz-subtext); }

/* ═══ tab 切换：内容画布左滑 / 右滑 ═══ */
.anim-left-enter-active,
.anim-left-leave-active,
.anim-right-enter-active,
.anim-right-leave-active {
  transition: transform 0.28s ease, opacity 0.28s ease;
}

/* 编辑 → 预览：向左滑（新内容从右进入） */
.anim-left-enter-from {
  transform: translateX(24px);
  opacity: 0;
}
.anim-left-leave-to {
  transform: translateX(-12px);
  opacity: 0;
}

/* 预览 → 编辑：向右滑（新内容从左进入） */
.anim-right-enter-from {
  transform: translateX(-24px);
  opacity: 0;
}
.anim-right-leave-to {
  transform: translateX(12px);
  opacity: 0;
}
</style>
