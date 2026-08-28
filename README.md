# RizUI

一套轻量、精致的 **Vue 3** 通用 UI 组件库。

## ✨ 组件列表

| 组件 | 说明 |
|------|------|
| `rizui_card` | 卡片容器，标题 + 描述 + 内容插槽 |
| `rizui_cfgrow` | 配置行，左侧标签 + 右侧控件，支持 stack 布局 |
| `rizui_infobox` | 提示框，虚线边框 + 预设 info/warn/error 配色 |
| `rizui_dialog` | 对话框，Teleport + Transition + 遮罩关闭 + 图标 |
| `rizui_toast` | 通知，info/warn/error + 标题/说明 + 倒计时进度条，Teleport 右上角排队 |
| `rizui_code` | 代码块，行内胶囊 / 大代码块，22 种语言高亮 + 复制按钮 |
| `rizui_button` | 胶囊按钮，主题色填充 / 通栏 / 左右图标 / 禁用 |
| `rizui_switch` | 纯 CSS 开关，支持 v-model 和主题色 |
| `rizui_checkbox` | 纯 CSS 复选框，支持 v-model、主题色和 label 文字 |
| `rizui_slider` | 步进滑条，支持拖拽 / 长按 ± 快速增减 |
| `rizui_timeline` | 时间轴控件，拖拽定位 + 刻度线 |
| `rizui_dropdown` | 下拉选择器，点击外部自动关闭 |
| `rizui_colorpicker` | 颜色选择器，HSL 调色盘 + hex 输入 |
| `rizui_text` | 文本 / 数字输入框，支持 plaintext 多行模式 |
| `rizui_mdeditor` | Markdown 编辑器，顶栏工具栏 + 实时预览 |

**工具函数**

| 函数 | 说明 |
|------|------|
| `openURL(url, target?)` | 动态合成 `<a>` 并触发真实点击，规避 async / Teleport 场景下 `window.open` 被拦截 |

## 📦 安装

```bash
# 从 GitHub 安装
npm install github:Yun-Hydrogen/riz-ui

# 指定版本
npm install github:Yun-Hydrogen/riz-ui#v0.1.0
```

## 🚀 使用

```vue
<script setup>
import { rizui_card, rizui_cfgrow, rizui_switch } from 'riz-ui'
</script>

<template>
  <rizui_card title="设置面板" desc="调整应用参数">
    <rizui_cfgrow label="启用功能">
      <rizui_switch v-model="enabled" color="#39c5bb" />
    </rizui_cfgrow>
  </rizui_card>
</template>
```

> **零配置开箱即用**：组件样式 + **Font Awesome 图标（含字体）已全部内置**。
> - ✅ 无需单独安装 `@fortawesome/fontawesome-free`
> - ✅ 无需额外引入 CSS 文件（JS 自动注入样式与字体）
> - ✅ 组件中直接使用 `fa-*` class（如 `fa-solid fa-house`）即可，字体自动加载

## 🎨 主题色定制

所有支持 `color` 属性的控件，在**未显式指定 `color`** 时，默认跟随全局主题色变量 `--rizui_themecolor`（默认 `#66CCFF`）。

只需在全局样式中覆盖该变量即可一键换色，无需逐个组件设置：

```css
:root {
  --rizui_themecolor: #66CCFF;
}
```

```vue
<script setup>
import { rizui_button } from 'riz-ui'
</script>

<template>
  <!-- color 省略时自动使用 --rizui_themecolor -->
  <rizui_button text="保存" primary />

  <!-- 也可单独覆盖某个组件 -->
  <rizui_button text="删除" color="#e05555" primary />
</template>
```

## 🌙 暗色主题

组件库内置完整的亮 / 暗色语义变量体系，无需任何配置即可自动适配：

- **跟随系统**：默认 `auto` 模式，根据 `prefers-color-scheme` 自动切换
- **全局强制**：在 `<html>` 上设置 `data-rizui-theme="dark"`（或 `"light"`）即可锁定全局明暗
- **局部覆盖**：每个组件支持 `theme="Dark" / "Light" / "auto"`（默认），可单独强制明暗

```html
<!-- 全局强制暗色 -->
<html data-rizui-theme="dark">
```

```vue
<!-- 仅本组件强制暗色 -->
<rizui_card theme="Dark" title="暗色卡片" />
```

所有明暗均由语义变量（`--rz-bg` / `--rz-border` / `--rz-text` / `--rz-subtext` 等）驱动，覆盖这些变量即可深度定制配色。

## 🛠️ 本地开发

```bash
# 安装依赖
npm install

# 启动 Demo 预览页 (localhost:3000)
npm run dev

# 构建组件库 (dist/)
npm run build

# 构建 Demo 静态站 (demo-dist/)
npm run build:demo
```

## 🖀️ 在线 Demo

Demo 静态站通过 Cloudflare Pages 从本仓库自动构建部署，[访问](https://rizui.yunhydrogen.top/#/) `https://rizui.yunhydrogen.top/#/` 即可预览。

## 📄 License

[AGPLv3](https://www.gnu.org/licenses/agpl-3.0.html) — 开源免费使用，衍生作品须同样以 AGPLv3 开源。

如需闭源商业使用，请联系 YunHydrogenP-W@qq.com 获取商业许可。
