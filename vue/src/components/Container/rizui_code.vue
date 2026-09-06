<template>
  <div
    class="rzc-root"
    :class="block ? 'rzc-block' : 'rzc-inline'"
    :data-rizui-theme="themeAttr"
    :style="rootStyle"
  >
    <!-- ═══ 单行代码块（默认） ═══ -->
    <code v-if="!block" class="rzc-inner" v-html="highlighted"></code>

    <!-- ═══ 大代码块（block=true） ═══ -->
    <template v-else>
      <div class="rzc-head">
        <span v-if="langName" class="rzc-lang">{{ langName }}</span>
        <button
          class="rzc-copy"
          type="button"
          :title="copied ? '已复制' : '复制代码'"
          @click="copyCode"
        >
          <span class="rzc-copy-icon">
            <svg class="rzc-ico-copy" :class="{ 'rzc-ico-hidden': copied }" viewBox="0 0 24 24" width="13" height="13" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="9" y="9" width="13" height="13" rx="2"/><path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"/></svg>
            <svg class="rzc-ico-check" :class="{ 'rzc-ico-show': copied }" viewBox="0 0 24 24" width="13" height="13" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><path d="M20 6L9 17l-5-5"/></svg>
          </span>
        </button>
        <span class="rzc-copy-bar" :class="{ 'rzc-copy-bar-on': copied }"></span>
      </div>
      <pre class="rzc-pre"><code v-html="highlighted"></code></pre>
    </template>
  </div>
</template>

<script setup>
import { computed, ref, useSlots } from 'vue'

const props = defineProps({
  /** 代码文本（也可用默认插槽传入） Code text (or use default slot) */
  code:    { type: String, default: '' },
  /** 大代码块（多行） Large code block */
  block:   { type: Boolean, default: false },
  /** 语言（md/html/js/css/json/bash/python 等数十种） Language */
  lang:    { type: String, default: '' },
  /** 边框与复制按钮颜色，空则跟随主题色 Border & copy button color */
  color:   { type: String, default: '' },
  /** 最大高度（如 320px），默认不限制、内容全高展开 Max height */
  height:  { type: String, default: '' },
  /** 主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局 Theme mode */
  theme:   { type: String, default: 'auto' }
})

const slots = useSlots()

/* 组件级主题覆盖：Dark / Light 输出到根节点 data-rizui-theme */
const themeAttr = computed(() => {
  if (props.theme === 'Dark') return 'dark'
  if (props.theme === 'Light') return 'light'
  return undefined
})

const rootStyle = computed(() => ({
  '--rzc-border': props.color || 'var(--rz-border)',
  '--rzc-accent': props.color || 'var(--rizui_themecolor)',
  '--rzc-maxh': props.height || 'none'
}))

/* ═══════════════════════════════════════════════
   代码内容：code prop 优先，否则取默认插槽文本
   ═══════════════════════════════════════════════ */
const codeText = computed(() => {
  if (props.code) return props.code
  const nodes = slots.default ? slots.default() : []
  return nodes.map(n => (typeof n.children === 'string' ? n.children : '')).join('')
})

/* ═══════════════════════════════════════════════
   复制按钮
   ═══════════════════════════════════════════════ */
const copied = ref(false)
async function copyCode() {
  try {
    await navigator.clipboard.writeText(codeText.value)
  } catch (e) {
    const ta = document.createElement('textarea')
    ta.value = codeText.value
    document.body.appendChild(ta)
    ta.select()
    document.execCommand('copy')
    ta.remove()
  }
  copied.value = true
  setTimeout(() => { copied.value = false }, 1500)
}

/* ═══════════════════════════════════════════════
   语法高亮（轻量 tokenizer，与 MD 编辑器一致的轻量定位）
   ═══════════════════════════════════════════════ */

/* 语言别名 */
const LANG_ALIAS = {
  javascript: 'js', jsx: 'js', typescript: 'ts', tsx: 'ts', py: 'python',
  sh: 'bash', shell: 'bash', zsh: 'bash', yml: 'yaml', markdown: 'md',
  'c++': 'cpp', csharp: 'cs', 'c#': 'cs', vue: 'html', xml: 'html', htm: 'html',
  rs: 'rust', rb: 'ruby', kt: 'kotlin', go: 'go', sql: 'sql', php: 'php',
  java: 'java', kotlin: 'kotlin', swift: 'swift', json: 'json', yaml: 'yaml',
  toml: 'ini', ini: 'ini', txt: 'text', text: 'text', plain: 'text'
}

/* 语言配置：名称 / 行注释 / 块注释 / 关键字 */
const JS_KW = ['const','let','var','function','return','if','else','for','while','do','switch','case','break','continue','new','class','extends','super','this','typeof','instanceof','in','of','try','catch','finally','throw','async','await','yield','import','export','from','default','delete','void','static','get','set']
const TS_KW = JS_KW.concat(['interface','type','enum','implements','declare','namespace','readonly','abstract','as','satisfies','keyof','infer','never','unknown','any','string','number','boolean','symbol','object'])
const C_KW = ['int','char','float','double','long','short','unsigned','signed','void','struct','union','enum','typedef','static','const','volatile','register','extern','return','if','else','for','while','do','switch','case','break','continue','goto','sizeof']
const CPP_KW = C_KW.concat(['class','public','private','protected','template','typename','namespace','using','new','delete','try','catch','throw','virtual','override','final','constexpr','auto','nullptr','this','operator'])
const JAVA_KW = ['public','private','protected','class','interface','enum','extends','implements','static','final','abstract','void','int','long','short','byte','char','boolean','float','double','return','if','else','for','while','do','switch','case','break','continue','new','try','catch','finally','throw','throws','this','super','import','package','null','true','false']
const PY_KW = ['def','class','return','if','elif','else','for','while','in','not','and','or','is','pass','break','continue','import','from','as','with','try','except','finally','raise','lambda','yield','global','nonlocal','del','assert','async','await','True','False','None','self','print']
const BASH_KW = ['if','then','else','elif','fi','for','while','do','done','case','esac','function','return','local','export','echo','read','exit','shift','set','source','alias','sudo','cd','ls','cat','grep','sed','awk']
const SQL_KW = ['SELECT','FROM','WHERE','INSERT','INTO','VALUES','UPDATE','SET','DELETE','CREATE','TABLE','ALTER','DROP','JOIN','LEFT','RIGHT','INNER','OUTER','ON','GROUP','BY','ORDER','HAVING','LIMIT','AND','OR','NOT','NULL','AS','DISTINCT','UNION','ALL','PRIMARY','KEY','FOREIGN','REFERENCES','INDEX','VIEW','BEGIN','COMMIT','ROLLBACK','TRANSACTION','INT','VARCHAR','TEXT','DATE','DATETIME','BOOLEAN']

const LANGS = {
  text:    { name: 'Plain',        kw: [],                          line: [],                 block: [] },
  md:      { name: 'Markdown',     kw: [],                          line: [],                 block: [] },
  json:    { name: 'JSON',         kw: ['true','false','null'],     line: [],                 block: [] },
  js:      { name: 'JavaScript',   kw: JS_KW,                       line: ['//'],             block: [['/*', '*/']] },
  ts:      { name: 'TypeScript',   kw: TS_KW,                       line: ['//'],             block: [['/*', '*/']] },
  html:    { name: 'HTML / Vue',   kw: [],                          line: [],                 block: [['<!--', '-->']] },
  css:     { name: 'CSS',          kw: [],                          line: [],                 block: [['/*', '*/']] },
  bash:    { name: 'Bash',         kw: BASH_KW,                     line: ['#'],              block: [] },
  python:  { name: 'Python',       kw: PY_KW,                       line: ['#'],              block: [['"""', '"""'], ["'''", "'''"]] },
  java:    { name: 'Java',         kw: JAVA_KW,                     line: ['//'],             block: [['/*', '*/']] },
  c:       { name: 'C',            kw: C_KW,                        line: ['//'],             block: [['/*', '*/']] },
  cpp:     { name: 'C++',          kw: CPP_KW,                      line: ['//'],             block: [['/*', '*/']] },
  cs:      { name: 'C#',           kw: TS_KW,                       line: ['//'],             block: [['/*', '*/']] },
  sql:     { name: 'SQL',          kw: SQL_KW,                      line: ['--'],             block: [['/*', '*/']] },
  yaml:    { name: 'YAML',         kw: ['true','false','null','yes','no','on','off'], line: ['#'], block: [] },
  ini:     { name: 'INI / TOML',   kw: ['true','false'],            line: ['#', ';'],         block: [] },
  go:      { name: 'Go',           kw: ['package','import','func','return','if','else','for','range','switch','case','break','continue','go','defer','chan','map','struct','interface','type','var','const','select','default','nil','true','false','make','new','len','cap','append'], line: ['//'], block: [['/*', '*/']] },
  rust:    { name: 'Rust',         kw: ['fn','let','mut','const','static','struct','enum','impl','trait','pub','use','mod','match','if','else','for','while','loop','return','break','continue','in','as','where','dyn','async','await','move','ref','self','Self','true','false','Some','None','Ok','Err'], line: ['//'], block: [['/*', '*/']] },
  ruby:    { name: 'Ruby',         kw: ['def','end','class','module','if','elsif','else','unless','while','until','for','do','begin','rescue','ensure','return','yield','require','include','extend','new','puts','print','true','false','nil','self'], line: ['#'], block: [] },
  php:     { name: 'PHP',          kw: ['echo','print','if','else','elseif','for','foreach','while','do','switch','case','break','continue','function','class','interface','trait','extends','implements','public','private','protected','static','return','new','try','catch','finally','throw','namespace','use','require','include','true','false','null'], line: ['//', '#'], block: [['/*', '*/']] },
  kotlin:  { name: 'Kotlin',       kw: ['fun','val','var','class','object','interface','when','if','else','for','while','do','return','break','continue','try','catch','finally','throw','import','package','is','in','as','null','true','false','this','super','private','public','protected','internal','data','sealed','companion','init'], line: ['//'], block: [['/*', '*/']] },
  swift:   { name: 'Swift',        kw: ['func','let','var','class','struct','enum','protocol','extension','import','return','if','else','guard','for','while','repeat','switch','case','break','continue','in','as','is','try','catch','throw','defer','nil','true','false','self','super','init','deinit','private','public','internal','open','fileprivate','static','final','override'], line: ['//'], block: [['/*', '*/']] }
}

/* 通用常量（大写 True/False/None 等已在各自语言表内） */
const CONSTS = new Set(['true','false','null','undefined','NaN','Infinity','nil','None','True','False'])

/* 内置类型（独立配色 type） */
const TYPES = new Set([
  'string','number','boolean','object','array','symbol','bigint','void','any','unknown','never',
  'int','long','short','float','double','char','bool','byte','unsigned','signed',
  'String','Number','Boolean','Object','Array','List','Dict','Set','Tuple','None','NoneType'
])

function escapeRe(s) {
  return s.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')
}

function canonical(l) {
  return LANG_ALIAS[(l || '').toLowerCase().trim()] || (l || '').toLowerCase().trim()
}

function highlight(code, l) {
  const lang = canonical(l)
  const cfg = LANGS[lang] || LANGS.text
  const kw = new Set(cfg.kw)

  /* ── 按语言组装 注释 / 字符串 / 操作符 / 数字 / 标识符 主正则（固定 5 个捕获组） ── */
  const parts = []
  if (cfg.line.length) parts.push('(?:' + cfg.line.map(c => escapeRe(c) + '[^\\n]*').join('|') + ')')
  if (cfg.block.length) parts.push('(?:' + cfg.block.map(([a, b]) => escapeRe(a) + '[\\s\\S]*?' + escapeRe(b)).join('|') + ')')
  const commentAlt = parts.length ? parts.join('|') : '(?!)'
  const starter = '(' + commentAlt + ')|("(?:[^"\\\\]|\\\\.)*"|\'(?:[^\'\\\\]|\\\\.)*\'|`(?:[^`\\\\]|\\\\.)*`)|(=>|===|!==|==|!=|<=|>=|&&|\\|\\||[-+*/%!&|^~=]+)|(\\b\\d[\\d_]*(?:\\.\\d+)?\\b)|(\\b[A-Za-z_$][\\w$]*\\b)'
  const re = new RegExp(starter, 'g')

  let out = ''
  let last = 0
  let m
  while ((m = re.exec(code)) !== null) {
    out += code.slice(last, m.index).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;')
    const [, comment, str, op, num, ident] = m
    if (comment) {
      out += '<span class="rzc-tok-com">' + comment.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;') + '</span>'
    } else if (str) {
      out += '<span class="rzc-tok-str">' + str.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;') + '</span>'
    } else if (op) {
      out += '<span class="rzc-tok-op">' + op.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;') + '</span>'
    } else if (num) {
      out += '<span class="rzc-tok-num">' + num.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;') + '</span>'
    } else if (ident) {
      const esc = ident.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;')
      /* HTML/XML：标签名（前面是 < 或 /） */
      const prev = code.slice(Math.max(0, m.index - 2), m.index)
      const isTag = (lang === 'html') && /<\/?$/.test(prev)
      const rest = code.slice(re.lastIndex)
      if (isTag) {
        out += '<span class="rzc-tok-tag">' + esc + '</span>'
      } else if (TYPES.has(ident)) {
        out += '<span class="rzc-tok-type">' + esc + '</span>'
      } else if (kw.has(ident) || CONSTS.has(ident)) {
        out += '<span class="rzc-tok-kw">' + esc + '</span>'
      } else if (/^\s*\(/.test(rest)) {
        /* 函数调用：标识符后紧跟 ( */
        out += '<span class="rzc-tok-fn">' + esc + '</span>'
      } else if (/^\s*=/.test(rest)) {
        /* 属性 / 参数：标识符后紧跟 = （如 HTML class="x"、JS 具名参数） */
        out += '<span class="rzc-tok-attr">' + esc + '</span>'
      } else {
        out += esc
      }
    }
    last = re.lastIndex
  }
  out += code.slice(last).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;')
  return out
}

const highlighted = computed(() => highlight(codeText.value, props.lang))
const langName = computed(() => {
  const cfg = LANGS[canonical(props.lang)]
  return props.lang && cfg ? cfg.name : ''
})
</script>

<style scoped>
/* ═══ 根：语义变量重映射（组件级强制主题） ═══ */
/* 代码 token 色使用全局 --rz-code-*（浅色默认，随 :root 明暗切换） */

.rzc-root[data-rizui-theme="dark"] {
  --rz-code-kw: var(--rz-code-dark-kw);
  --rz-code-str: var(--rz-code-dark-str);
  --rz-code-num: var(--rz-code-dark-num);
  --rz-code-com: var(--rz-code-dark-com);
  --rz-code-fn: var(--rz-code-dark-fn);
  --rz-code-tag: var(--rz-code-dark-tag);
  --rz-code-attr: var(--rz-code-dark-attr);
  --rz-code-op: var(--rz-code-dark-op);
  --rz-code-type: var(--rz-code-dark-type);
  --rz-bg: var(--rz-dark-bg);
  --rz-border: var(--rz-dark-border);
  --rz-text: var(--rz-dark-text);
  --rz-subtext: var(--rz-dark-subtext);
}

.rzc-root[data-rizui-theme="light"] {
  --rz-code-kw: #a626a4;
  --rz-code-str: #50a14f;
  --rz-code-num: #986801;
  --rz-code-com: #9aa0a6;
  --rz-code-fn: #4078f2;
  --rz-code-tag: #e45649;
  --rz-code-attr: #b76b01;
  --rz-code-op: #9b50e0;
  --rz-code-type: #1d7fb6;
  --rz-bg: #fff;
  --rz-border: #e8ecf2;
  --rz-text: #334;
  --rz-subtext: #99a;
}

/* ── 高亮 token 色（v-html 内容无 data-v，需 :deep 穿透） ── */
.rzc-root :deep(.rzc-tok-kw)  { color: var(--rz-code-kw); font-weight: 600; }
.rzc-root :deep(.rzc-tok-str) { color: var(--rz-code-str); }
.rzc-root :deep(.rzc-tok-num) { color: var(--rz-code-num); }
.rzc-root :deep(.rzc-tok-com) { color: var(--rz-code-com); font-style: italic; }
.rzc-root :deep(.rzc-tok-fn)  { color: var(--rz-code-fn); }
.rzc-root :deep(.rzc-tok-tag) { color: var(--rz-code-tag); }
.rzc-root :deep(.rzc-tok-attr) { color: var(--rz-code-attr); }
.rzc-root :deep(.rzc-tok-op)  { color: var(--rz-code-op); }
.rzc-root :deep(.rzc-tok-type) { color: var(--rz-code-type); }

/* ═══ 单行代码块（字体跟随父控件设置） ═══ */
.rzc-inline {
  display: inline-block;
  max-width: 100%;
  padding: 2px 9px;
  border-radius: 6px;
  border: 1px solid var(--rzc-border);
  background: color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  font-family: ui-monospace, 'Cascadia Code', Consolas, 'Courier New', monospace;
  font-size: inherit;
  line-height: inherit;
  color: var(--rz-text);
  white-space: pre-wrap;
  word-break: break-all;
  vertical-align: middle;
}

.rzc-inline .rzc-inner {
  font-family: inherit;
  font-size: inherit;
  line-height: inherit;
  color: inherit;
}

/* ═══ 大代码块 ═══ */
.rzc-block {
  position: relative;
  width: 100%;
  border: 1px solid var(--rzc-border);
  border-radius: 10px;
  background: color-mix(in srgb, var(--rz-text) 5%, var(--rz-bg));
  overflow: hidden;
  box-sizing: border-box;
}

.rzc-head {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 8px;
  padding: 5px 10px 5px 12px;
  border-bottom: 1px solid color-mix(in srgb, var(--rz-text) 8%, var(--rz-bg));
  overflow: hidden;
}

.rzc-lang {
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 0.04em;
  color: var(--rz-subtext);
  user-select: none;
}

/* 右上角复制悬浮按钮 */
.rzc-copy {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 4px;
  padding: 3px 8px;
  border: none;
  border-radius: 6px;
  background: color-mix(in srgb, var(--rzc-accent) 12%, transparent);
  color: var(--rzc-accent);
  font-size: 11px;
  font-family: inherit;
  cursor: pointer;
  opacity: 0.45;
  transition: opacity 0.15s;
}

.rzc-copy:hover {
  opacity: 1;
}

/* ── 复制按钮图标淡入淡出（勾 ↔ 复制图标） ── */
.rzc-copy-icon {
  position: relative;
  display: inline-flex;
  width: 13px;
  height: 13px;
  flex-shrink: 0;
}

.rzc-copy-icon svg {
  position: absolute;
  inset: 0;
  transition: opacity 0.18s ease, transform 0.18s ease;
}

.rzc-copy-icon .rzc-ico-check {
  opacity: 0;
  transform: scale(0.5);
}

.rzc-copy-icon .rzc-ico-check.rzc-ico-show {
  opacity: 1;
  transform: scale(1);
}

.rzc-copy-icon .rzc-ico-copy.rzc-ico-hidden {
  opacity: 0;
  transform: scale(0.5);
}

/* ── 复制后顶栏正进度条（与按钮图标恢复 1.5s 同步，从左向右 0→100%） ── */
.rzc-copy-bar {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  height: 2px;
  background: var(--rzc-accent);
  transform: scaleX(0);
  transform-origin: left;
  pointer-events: none;
}

.rzc-copy-bar-on {
  animation: rzc-copy-progress 1.5s linear forwards;
}

/* 1.5s：0→1.2s 从左向右填满，1.2→1.5s 淡出（与按钮图标恢复同步） */
@keyframes rzc-copy-progress {
  0%   { transform: scaleX(0); opacity: 1; }
  80%  { transform: scaleX(1); opacity: 1; }
  100% { transform: scaleX(1); opacity: 0; }
}

.rzc-pre {
  margin: 0;
  padding: 10px 12px;
  overflow: auto;
  max-height: var(--rzc-maxh, none);
  font-family: ui-monospace, 'Cascadia Code', Consolas, 'Courier New', monospace;
  font-size: 12.5px;
  line-height: 1.7;
  color: var(--rz-text);
  tab-size: 2;
}

/* ── 滚动条：半透明主题色（垂直 / 水平同款） ── */
.rzc-pre::-webkit-scrollbar {
  width: 8px;
  height: 8px;
}

.rzc-pre::-webkit-scrollbar-track {
  background: transparent;
}

.rzc-pre::-webkit-scrollbar-thumb {
  background: color-mix(in srgb, var(--rzc-accent) 28%, transparent);
  border-radius: 999px;
}

.rzc-pre::-webkit-scrollbar-thumb:hover {
  background: color-mix(in srgb, var(--rzc-accent) 48%, transparent);
}

.rzc-pre {
  scrollbar-width: thin;
  scrollbar-color: color-mix(in srgb, var(--rzc-accent) 28%, transparent) transparent;
}

.rzc-root :deep(.rzc-pre code) {
  font-family: inherit;
  font-size: inherit;
  color: inherit;
  white-space: pre;
}
</style>
