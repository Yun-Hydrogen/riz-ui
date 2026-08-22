/*
================================================================================
  RizUI — 通用 Vue3 UI 组件库

  使用：import { UiCard, UiConfigRow } from 'riz-ui'

================================================================================
*/

import '@fortawesome/fontawesome-free/css/fontawesome.min.css'
import '@fortawesome/fontawesome-free/css/solid.min.css'
import './style.css'

export { default as Cntr_Card }        from './components/Container/Cntr_Card.vue'
export { default as Cntr_Cfgrow }     from './components/Container/Cntr_Cfgrow.vue'
export { default as Cntr_Infobox }    from './components/Container/Cntr_Infobox.vue'
export { default as Ctnr_Dialog }     from './components/Container/Ctnr_Dialog.vue'

export { default as Ctrl_Switch }     from './components/Control/Ctrl_Switch.vue'
export { default as Ctrl_Slider }     from './components/Control/Ctrl_Slider.vue'
export { default as Ctrl_Dropdown }   from './components/Control/Ctrl_Dropdown.vue'
export { default as Ctrl_ColorPicker } from './components/Control/Ctrl_ColorPicker.vue'
export { default as Ctrl_Timeline }   from './components/Control/Ctrl_Timeline.vue'
export { default as Ctrl_Button }     from './components/Control/Ctrl_Button.vue'

export { default as Inpt_Text }        from './components/Input/Inpt_Text.vue'
export { default as Inpt_mdeditor }    from './components/Input/Inpt_mdeditor.vue'

/* old — 已弃用（保留兼容） */
export { default as Ct_Card }          from './components/Container/Cntr_Card.vue'
export { default as UiCard }            from './components/old/UiCard.vue'
export { default as UiConfigRow }       from './components/old/UiConfigRow.vue'
export { default as UiSwitch }          from './components/old/UiSwitch.vue'
export { default as UiSlider }          from './components/old/UiSlider.vue'
export { default as UiTimeline }        from './components/old/UiTimeline.vue'
export { default as UiDropdown }        from './components/old/UiDropdown.vue'
export { default as UiColorPicker }     from './components/old/UiColorPicker.vue'
export { default as UiDialog }          from './components/old/UiDialog.vue'
export { default as NotReadyOverlay }   from './components/old/NotReadyOverlay.vue'
