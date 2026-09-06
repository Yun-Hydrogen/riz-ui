<script setup>
import { inject } from 'vue'
import { rizui_code, rizui_card, rizui_cfgrow, rizui_infobox } from 'riz-ui'

const themeColor = inject('themeColor')

const props = [
  { name: 'code',   type: 'String',  default: '""',     desc: '代码文本（也可用默认插槽传入）' },
  { name: 'block',  type: 'Boolean', default: 'false',  desc: '大代码块模式（多行展示 + 语法高亮 + 悬浮复制按钮）' },
  { name: 'lang',   type: 'String',  default: '""',     desc: '代码语言，支持 js / ts / html / css / md / py / sql / bash 等数十种简短名与别名' },
  { name: 'color',  type: 'String',  default: '""',     desc: '边框与复制按钮主题色，空时跟随 --rizui_themecolor' },
  { name: 'height', type: 'String',  default: '""',     desc: '代码块最大高度（如 240px），超高滚动；默认全高展开' },
  { name: 'theme',  type: 'String',  default: "'auto'", desc: '主题模式：Dark 强制暗色 / Light 强制亮色 / auto 跟随全局' }
]

/* ── 全部支持语言（简短名 + 显示名 + 别名） ── */
const langList = [
  { name: 'js',     label: 'JavaScript',  aliases: 'javascript / jsx' },
  { name: 'ts',     label: 'TypeScript',  aliases: 'typescript / tsx' },
  { name: 'html',   label: 'HTML / Vue',  aliases: 'xml / vue / htm' },
  { name: 'css',    label: 'CSS' },
  { name: 'md',     label: 'Markdown',    aliases: 'markdown' },
  { name: 'json',   label: 'JSON' },
  { name: 'bash',   label: 'Bash',        aliases: 'sh / shell / zsh' },
  { name: 'py',     label: 'Python',      aliases: 'python' },
  { name: 'sql',    label: 'SQL' },
  { name: 'yaml',   label: 'YAML',        aliases: 'yml' },
  { name: 'ini',    label: 'INI / TOML',  aliases: 'toml' },
  { name: 'java',   label: 'Java' },
  { name: 'c',      label: 'C' },
  { name: 'cpp',    label: 'C++',         aliases: 'c++' },
  { name: 'cs',     label: 'C#',          aliases: 'csharp / c#' },
  { name: 'go',     label: 'Go' },
  { name: 'rust',   label: 'Rust',        aliases: 'rs' },
  { name: 'ruby',   label: 'Ruby',        aliases: 'rb' },
  { name: 'php',    label: 'PHP' },
  { name: 'kotlin', label: 'Kotlin',      aliases: 'kt' },
  { name: 'swift',  label: 'Swift' },
  { name: 'text',   label: 'Plain Text',  aliases: 'txt / plain' }
]

const aronaJs = `function aronaGreet(sensei) {
  // 什亭之箱主控程序
  const greeting = \`早安，\${sensei}！今天也要元气满满地工作哦～\`
  console.log(greeting)
  return greeting
}

aronaGreet('老师')`

const schaleHtml = `<!-- 什亭之箱 -->
<div class="schale-window">
  <h1>什亭之箱</h1>
  <p class="status">阿洛娜：睡眠中 普拉娜：工作中</p>
</div>`

const gameDevPy = `def deploy_cleaner_robot():
    # 游戏开发部
    print("爱丽丝：光啊——！")
    return "已清除废墟区域障碍"`

const kivotosSql = `SELECT s.name, s.school, s.club, s.weapon_type
FROM students AS s
JOIN clubs AS c ON s.club_id = c.id
WHERE s.status = 'active'
ORDER BY s.power_level DESC;`

const longLine = `const schaleNotice = "关于基沃托斯各学园（格赫娜、崔尼蒂、阿拜多斯、千禧年、百鬼夜行、山海经、红冬、瓦尔基里）联合作战指令已经下达…";`

const card1Usage = `<!-- 单行行内代码胶囊（默认） -->
<rizui_code>const answer = 42</rizui_code>
<span>欢迎回到沙勒，请执行 <rizui_code>schale-cli login</rizui_code> 登入什亭之箱。</span>`

const card2Usage = `<!-- block 大代码块 + lang 语法高亮与复制按钮 -->
<rizui_code block lang="js" :code="aronaJs" />
<rizui_code block lang="html" :code="schaleHtml" />`

const card3Usage = `<!-- 多语言支持：py / sql 等数十种 -->
<rizui_code block lang="py" :code="gameDevPy" />
<rizui_code block lang="sql" :code="kivotosSql" />`

const card4Usage = `<!-- height 最大高度限制，超高滚动，水平长行滚动 -->
<rizui_code block lang="js" :code="aronaJs" height="140px" />
<rizui_code block lang="js" :code="longLine" height="100px" />`

const card5Usage = `<!-- color 定制高亮与复制按钮主题色 -->
<rizui_code block lang="py" :code="gameDevPy" color="#8b5cf6" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row">
        <h2>代码块</h2>
        <p class="sec-tag">rizui_code</p>
      </div>
      <p class="sec-desc">代码渲染组件。默认呈现行内代码胶囊，设置 <rizui_code style="font-size: 10px;">block</rizui_code> 开启大代码块，支持数十种常见语言语法高亮、右上角悬浮复制、高度限制与主题定制。</p>
    </section>

    <section class="sec-demo">
      <h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <!-- 单行胶囊 -->
      <rizui_card title="行内代码胶囊 | 默认形态" desc="不设置 block 属性即为单行代码胶囊，适合嵌入文本段落中展示指令或键名">
        <rizui_cfgrow label="插槽传入" hint="默认插槽直接放置纯文本">
          <rizui_code>const answer = 42</rizui_code>
        </rizui_cfgrow>
        <rizui_cfgrow label="段落混排" hint="与正文行内基线对齐">
          <span style="font-size: 12px;">欢迎回到沙勒，请执行 <rizui_code>schale-cli login</rizui_code> 登入什亭之箱。</span>
        </rizui_cfgrow>
        <rizui_code block lang="html" :code="card1Usage" style="margin-top: 12px" />
      </rizui_card>

      <!-- 大代码块 -->
      <rizui_card title="大代码块高亮 | block + lang" desc="设置 block 开启多行代码块，通过 lang 指定语法高亮与右上角语言标签">
        <rizui_code block lang="js" :code="aronaJs" />
        <rizui_code block lang="html" :code="schaleHtml" style="margin-top: 12px" />
        <rizui_code block lang="html" :code="card2Usage" style="margin-top: 12px" />
        <rizui_code block lang="py" :code="gameDevPy" style="margin-top: 12px" />
        <rizui_code block lang="sql" :code="kivotosSql" style="margin-top: 12px" />
        <rizui_code block lang="html" :code="card3Usage" style="margin-top: 12px" />
      </rizui_card>

      <!-- 全部支持语言一览 -->
      <rizui_card title="支持语言清单 | 22 种常用格式" desc="lang 属性直接使用简短名或常见别名（悬停可查看别名）">
        <div class="rzc-langs">
          <span v-for="l in langList" :key="l.name" class="rzc-lang-chip" :data-tip="l.aliases">
            <code>{{ l.name }}</code><em>{{ l.label }}</em>
          </span>
        </div>
      </rizui_card>

      <!-- 高度限制与滚动 -->
      <rizui_card title="高度限制与滚动 | height" desc="设置 height 控制最大显示高度，超高时展示半透明主题色滚动条；超长单行支持横向滚动">
        <rizui_code block lang="js" :code="aronaJs" height="140px" />
        <rizui_code block lang="js" :code="longLine" height="100px" style="margin-top: 12px" />
        <rizui_code block lang="html" :code="card4Usage" style="margin-top: 12px" />
      </rizui_card>

      <!-- 颜色定制 -->
      <rizui_card title="配色定制 | color" desc="通过 color 指定边框与右上角复制按钮的重点色，为空时跟随全局主题色">
        <rizui_code block lang="py" :code="gameDevPy" color="#8b5cf6" />
        <rizui_code block lang="html" :code="card5Usage" style="margin-top: 12px" />
      </rizui_card>
    </section>

    <section class="sec-props">
      <h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table">
        <thead>
          <tr>
            <th>属性</th>
            <th>类型</th>
            <th>必填</th>
            <th>默认值</th>
            <th>说明</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="p in props" :key="p.name">
            <td><code>{{ p.name }}</code></td>
            <td><code>{{ p.type }}</code></td>
            <td>{{ p.required || '—' }}</td>
            <td><code>{{ p.default || '—' }}</code></td>
            <td>{{ p.desc }}</td>
          </tr>
        </tbody>
      </table>
    </section>

    <!-- 主题模式提示 -->
    <rizui_infobox type="info">
      <code>theme</code> 支持 <code>Dark</code> / <code>Light</code> / <code>auto</code>（默认）。<code>auto</code> 跟随顶栏主题开关（全局 <code>data-rizui-theme</code>）或系统 <code>prefers-color-scheme</code>；<code>Dark</code> / <code>Light</code> 可对本组件局部强制明暗。试试右上角主题按钮实时切换。
    </rizui_infobox>

    <!-- 主题色提示 -->
    <rizui_infobox type="info">
      未指定 <code>color</code> 时，边框与复制按钮自动跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>）。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </rizui_infobox>
  </main>
</template>

<style scoped>
.rzc-langs {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.rzc-lang-chip {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 4px 10px;
  border-radius: 8px;
  border: 1px solid var(--rz-border);
  background: color-mix(in srgb, var(--rz-text) 5%, var(--rz-bg));
  cursor: default;
}

.rzc-lang-chip code {
  font-size: 12px;
  font-weight: 700;
  color: var(--rizui_themecolor);
}

.rzc-lang-chip em {
  font-style: normal;
  font-size: 12px;
  color: var(--rz-subtext);
}
</style>

<style scoped>
.rzc-langs {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.rzc-lang-chip {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 4px 10px;
  border-radius: 8px;
  border: 1px solid var(--rz-border);
  background: color-mix(in srgb, var(--rz-text) 5%, var(--rz-bg));
  cursor: default;
  position: relative;
}

/* ── 自定义 tooltip（替代原生 title：悬停显示别名） ── */
.rzc-lang-chip[data-tip]::after {
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

.rzc-lang-chip[data-tip]:hover::after {
  opacity: 1;
  transform: translateX(-50%) translateY(0);
}

.rzc-lang-chip code {
  font-size: 12px;
  font-weight: 700;
  color: var(--rizui_themecolor);
}

.rzc-lang-chip em {
  font-style: normal;
  font-size: 11px;
  color: var(--rz-subtext);
}
</style>
