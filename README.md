# RizUI

一套轻量、精致的 **Vue 3** 通用 UI 组件库。

## ✨ 组件列表

| 组件 | 说明 |
|------|------|
| `UiCard` | 卡片容器，标题 + 描述 + 内容插槽 |
| `UiConfigRow` | 配置行，左侧标签 + 右侧控件，支持 stack 布局 |
| `UiSwitch` | 纯 CSS 开关，支持 v-model 和主题色 |
| `UiSlider` | 步进滑条，支持拖拽 / 长按 ± 快速增减 |
| `UiTimeline` | 时间轴控件，拖拽定位 + 刻度线 |
| `UiDropdown` | 下拉选择器，点击外部自动关闭 |
| `UiColorPicker` | 颜色选择器，HSL 调色盘 + hex 输入 |
| `UiDialog` | 对话框，Teleport + Transition + 遮罩关闭 |
| `NotReadyOverlay` | 未就绪遮罩，毛玻璃 + 徽章提示 |

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
