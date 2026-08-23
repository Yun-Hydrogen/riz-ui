# RizUI

一套轻量、精致的 **Vue 3** 通用 UI 组件库。

## ✨ 组件列表

| 组件 | 说明 |
|------|------|
| `Ctrl_Button` | 胶囊按钮，主题色填充 / 通栏 / 左右图标 / 禁用 |
| `Cntr_Card` | 卡片容器，标题 + 描述 + 内容插槽 |
| `Cntr_Cfgrow` | 配置行，左侧标签 + 右侧控件，支持 stack 布局 |
| `Cntr_Infobox` | 提示框，虚线边框 + 预设 info/warn/error 配色 |
| `Ctnr_Dialog` | 对话框，Teleport + Transition + 遮罩关闭 + 图标 |
| `Ctnr_Toast` | 通知，info/warn/error + 标题/说明 + 倒计时进度条，Teleport 右上角排队 |
| `Ctrl_Switch` | 纯 CSS 开关，支持 v-model 和主题色 |
| `Ctrl_Slider` | 步进滑条，支持拖拽 / 长按 ± 快速增减 |
| `Ctrl_Timeline` | 时间轴控件，拖拽定位 + 刻度线 |
| `Ctrl_Dropdown` | 下拉选择器，点击外部自动关闭 |
| `Ctrl_ColorPicker` | 颜色选择器，HSL 调色盘 + hex 输入 |
| `Inpt_Text` | 文本 / 数字输入框，支持 plaintext 多行模式 |
| `Inpt_mdeditor` | Markdown 编辑器，顶栏工具栏 + 实时预览 |

## 📦 安装

```bash
# 从 GitHub 安装
npm install github:YunHydrogen/riz-ui

# 指定版本
npm install github:YunHydrogen/riz-ui#v0.1.0
```

## 🚀 使用

```vue
<script setup>
import { UiCard, UiConfigRow, UiSwitch } from 'riz-ui'
</script>

<template>
  <UiCard title="设置面板" desc="调整应用参数">
    <UiConfigRow label="启用功能">
      <UiSwitch v-model="enabled" color="#39c5bb" />
    </UiConfigRow>
  </UiCard>
</template>
```

> 注意：样式中已包含组件样式，无需额外引入 CSS 文件。

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
import { Ctrl_Button } from 'riz-ui'
</script>

<template>
  <!-- color 省略时自动使用 --rizui_themecolor -->
  <Ctrl_Button text="保存" primary />

  <!-- 也可单独覆盖某个组件 -->
  <Ctrl_Button text="删除" color="#e05555" primary />
</template>
```

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

## 📄 License

[AGPLv3](https://www.gnu.org/licenses/agpl-3.0.html) — 开源免费使用，衍生作品须同样以 AGPLv3 开源。

如需闭源商业使用，请联系 YunHydrogenP-W@qq.com 获取商业许可。
