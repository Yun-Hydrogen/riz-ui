/*
================================================================================
  RizUI — 通用 Vue3 UI 组件库

  使用：import { UiCard, UiConfigRow } from 'riz-ui'

================================================================================
*/

import '@fortawesome/fontawesome-free/css/fontawesome.min.css'
import '@fortawesome/fontawesome-free/css/solid.min.css'
import '@fortawesome/fontawesome-free/css/brands.min.css'
import './style.css'

/* 工具函数 */
export { openURL } from './utils.js'

export { default as rizui_card }        from './components/Container/rizui_card.vue'
export { default as rizui_cfgrow }     from './components/Container/rizui_cfgrow.vue'
export { default as rizui_infobox }    from './components/Container/rizui_infobox.vue'
export { default as rizui_dialog }     from './components/Container/rizui_dialog.vue'
export { default as rizui_toast }      from './components/Container/rizui_toast.vue'
export { default as rizui_code }       from './components/Container/rizui_code.vue'

export { default as rizui_switch }     from './components/Control/rizui_switch.vue'
export { default as rizui_slider }     from './components/Control/rizui_slider.vue'
export { default as rizui_dropdown }   from './components/Control/rizui_dropdown.vue'
export { default as rizui_colorpicker } from './components/Control/rizui_colorpicker.vue'
export { default as rizui_timeline }   from './components/Control/rizui_timeline.vue'
export { default as rizui_button }     from './components/Control/rizui_button.vue'
export { default as rizui_checkbox }   from './components/Control/rizui_checkbox.vue'

export { default as rizui_text }        from './components/Input/rizui_text.vue'
export { default as rizui_mdeditor }    from './components/Input/rizui_mdeditor.vue'

/* old — 已弃用（保留兼容） */
export { default as Ct_Card }          from './components/Container/rizui_card.vue'
export { default as UiCard }            from './components/old/UiCard.vue'
export { default as UiConfigRow }       from './components/old/UiConfigRow.vue'
export { default as UiSwitch }          from './components/old/UiSwitch.vue'
export { default as UiSlider }          from './components/old/UiSlider.vue'
export { default as UiTimeline }        from './components/old/UiTimeline.vue'
export { default as UiDropdown }        from './components/old/UiDropdown.vue'
export { default as UiColorPicker }     from './components/old/UiColorPicker.vue'
export { default as UiDialog }          from './components/old/UiDialog.vue'
export { default as NotReadyOverlay }   from './components/old/NotReadyOverlay.vue'
