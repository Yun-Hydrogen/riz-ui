<template>
  <div class="imd-root" :style="rootStyle" :class="{ 'imd-readonly': readonly }">
    <!-- ═══ 顶栏工具栏（readonly 隐藏） ═══ -->
    <div v-if="!readonly" class="imd-toolbar">
      <div class="imd-toolbar-btns">
        <button
          v-for="b in toolbarBtns"
          :key="b.id"
          class="imd-tb"
          :title="b.title"
          type="button"
          :disabled="disabled"
          @mousedown.prevent
          @click="onToolbar(b.id)"
        ><i :class="b.icon"></i></button>
      </div>

      <!-- 编辑 / 预览 选项卡（右侧） -->
      <div class="imd-tabs">
        <button
          class="imd-tab"
          :class="{ 'imd-tab-active': activeTab === 'edit' }"
          type="button"
          @mousedown.prevent
          @click="activeTab = 'edit'"
        ><i class="fa-solid fa-pen"></i> 编辑</button>
        <button
          class="imd-tab"
          :class="{ 'imd-tab-active': activeTab === 'preview' }"
          type="button"
          @mousedown.prevent
          @click="activeTab = 'preview'"
        ><i class="fa-solid fa-eye"></i> 预览</button>
      </div>
    </div>

    <!-- ═══ 内容区 ═══ -->
    <div class="imd-body">
      <textarea
        v-if="!readonly && activeTab === 'edit'"
        ref="taRef"
        class="imd-editor"
        :value="modelValue"
        :placeholder="placeholder"
        :disabled="disabled"
        :style="{ height }"
        @input="$emit('update:modelValue', $event.target.value)"
        @keydown.tab.prevent="insertIndent"
      ></textarea>

      <div v-if="readonly || activeTab === 'preview'" class="imd-preview" :style="{ height }">
        <div class="imd-preview-inner" v-html="renderedHtml"></div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, nextTick } from 'vue'

const props = defineProps({
  modelValue:  { type: String,  default: '' },
  placeholder: { type: String,  default: '在此输入 Markdown 内容…' },
  color:       { type: String,  default: '' },
  disabled:    { type: Boolean, default: false },
  readonly:    { type: Boolean, default: false },
  height:      { type: String,  default: '320px' },
  default:     { type: String,  default: 'edit' }
})

const emit = defineEmits(['update:modelValue'])

const taRef = ref(null)
/* 编辑 / 预览 选项卡：default prop 控制初始板块（'edit' | 'preview'） */
const activeTab = ref(props.default === 'preview' ? 'preview' : 'edit')

/* 空 color 时跟随全局主题色变量 --rizui_themecolor */
const rootStyle = computed(() => ({
  '--imd-color': props.color || 'var(--rizui_themecolor)'
}))

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
    activeTab.value = 'edit'
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
  display: flex;
  flex-direction: column;
  width: 100%;
  border: 1.5px solid #d0d8e0;
  border-radius: 12px;
  overflow: hidden;
  background: #fff;
  transition: border-color 0.2s;
}

.imd-root:focus-within {
  border-color: var(--imd-color);
}

/* ═══ 工具栏 ═══ */
.imd-toolbar {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 6px 8px;
  border-bottom: 1px solid #e8ecf2;
  background: linear-gradient(to bottom, #fdfdfe, #f6f8fa);
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
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 28px;
  height: 28px;
  border: none;
  border-radius: 6px;
  background: transparent;
  color: #667;
  font-size: 12px;
  cursor: pointer;
  transition: background 0.15s, color 0.15s;
}

.imd-tb:hover:not(:disabled) {
  background: #eef1f5;
  color: var(--imd-color);
}

.imd-tb:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

/* ── 编辑 / 预览 选项卡（右侧胶囊） ── */
.imd-tabs {
  display: inline-flex;
  gap: 4px;
  padding: 3px;
  border-radius: 999px;
  background: #eceff4;
  flex-shrink: 0;
}

.imd-tab {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 5px 14px;
  border: none;
  border-radius: 999px;
  background: transparent;
  font-size: 12px;
  font-weight: 500;
  font-family: inherit;
  color: #889;
  cursor: pointer;
  transition: all 0.2s ease;
}

.imd-tab:hover:not(:disabled) {
  color: var(--imd-color);
}

.imd-tab:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.imd-tab-active {
  background: #fff;
  color: var(--imd-color);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.12);
}

/* ═══ 内容区 ═══ */
.imd-body {
  display: flex;
}

.imd-editor {
  flex: 1;
  min-width: 0;
  padding: 12px 14px;
  border: none;
  outline: none;
  resize: none;
  font-family: ui-monospace, 'Cascadia Code', Consolas, 'Courier New', monospace;
  font-size: 13px;
  line-height: 1.7;
  color: #334;
  background: transparent;
}

.imd-editor:disabled {
  opacity: 0.55;
  cursor: not-allowed;
}

.imd-preview {
  flex: 1;
  min-width: 0;
  overflow-y: auto;
  background: #fff;
}

/* ── 滚动条：主题色半透明 ── */
.imd-editor::-webkit-scrollbar,
.imd-preview::-webkit-scrollbar {
  width: 8px;
  height: 8px;
}

.imd-editor::-webkit-scrollbar-track,
.imd-preview::-webkit-scrollbar-track {
  background: transparent;
}

.imd-editor::-webkit-scrollbar-thumb,
.imd-preview::-webkit-scrollbar-thumb {
  background: color-mix(in srgb, var(--imd-color) 28%, transparent);
  border-radius: 999px;
}

.imd-editor::-webkit-scrollbar-thumb:hover,
.imd-preview::-webkit-scrollbar-thumb:hover {
  background: color-mix(in srgb, var(--imd-color) 48%, transparent);
}

.imd-editor,
.imd-preview {
  scrollbar-width: thin;
  scrollbar-color: color-mix(in srgb, var(--imd-color) 28%, transparent) transparent;
}

/* ── readonly：仅显示预览，height 约束高度，超出滚动 ── */
.imd-readonly .imd-preview {
  flex: 1;
  min-height: 120px;
  overflow-y: auto;
}

.imd-preview-inner {
  padding: 12px 14px;
  font-size: 13px;
  line-height: 1.7;
  color: #445;
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
  color: #222;
  line-height: 1.4;
}

.imd-preview-inner :deep(h1) { font-size: 22px; border-bottom: 1px solid #eef0f4; padding-bottom: 6px; }
.imd-preview-inner :deep(h2) { font-size: 18px; border-bottom: 1px solid #eef0f4; padding-bottom: 5px; }
.imd-preview-inner :deep(h3) { font-size: 16px; }
.imd-preview-inner :deep(h4) { font-size: 14px; }
.imd-preview-inner :deep(p) { margin: 0.5em 0; }
.imd-preview-inner :deep(a) { color: var(--imd-color); text-decoration: none; }
.imd-preview-inner :deep(a:hover) { text-decoration: underline; }
.imd-preview-inner :deep(code) {
  padding: 1px 6px;
  border-radius: 4px;
  background: #f0f2f5;
  color: #c7254e;
  font-family: ui-monospace, Consolas, monospace;
  font-size: 12px;
}
.imd-preview-inner :deep(pre) {
  margin: 0.75em 0;
  padding: 12px 14px;
  border-radius: 8px;
  background: #f6f8fa;
  overflow-x: auto;
}
.imd-preview-inner :deep(pre code) {
  padding: 0;
  background: transparent;
  color: #334;
}
.imd-preview-inner :deep(blockquote) {
  margin: 0.75em 0;
  padding: 6px 14px;
  border-left: 3px solid var(--imd-color);
  background: color-mix(in srgb, var(--imd-color) 6%, transparent);
  border-radius: 0 8px 8px 0;
  color: #667;
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
  border: 1px solid #e4e8ee;
  text-align: left;
}
.imd-preview-inner :deep(th) {
  background: #f6f8fa;
  font-weight: 600;
  color: #334;
}
.imd-preview-inner :deep(tr:nth-child(2n) td) { background: #fafbfc; }
.imd-preview-inner :deep(img) {
  max-width: 100%;
  border-radius: 8px;
  margin: 0.5em 0;
}
.imd-preview-inner :deep(hr) {
  margin: 1em 0;
  border: none;
  border-top: 1px solid #e4e8ee;
}
.imd-preview-inner :deep(del) { color: #99a; }
</style>
