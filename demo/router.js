import { createRouter, createWebHashHistory } from 'vue-router'

import HomePage       from './pages/HomePage.vue'
import PricingPage    from './pages/PricingPage.vue'

import rizui_card_demo   from './pages/demos/container/rizui_card_demo.vue'
import rizui_cfgrow_demo from './pages/demos/container/rizui_cfgrow_demo.vue'
import rizui_infobox_demo from './pages/demos/container/rizui_infobox_demo.vue'
import rizui_dialog_demo from './pages/demos/container/rizui_dialog_demo.vue'
import rizui_toast_demo from './pages/demos/container/rizui_toast_demo.vue'
import rizui_code_demo from './pages/demos/container/rizui_code_demo.vue'
import rizui_nav_demo from './pages/demos/container/rizui_nav_demo.vue'
import ThemeColor_demo from './pages/demos/other/ThemeColor_demo.vue'
import OpenURL_demo from './pages/demos/other/OpenURL_demo.vue'
import rizui_switch_demo from './pages/demos/control/rizui_switch_demo.vue'
import rizui_slider_demo from './pages/demos/control/rizui_slider_demo.vue'
import rizui_dropdown_demo from './pages/demos/control/rizui_dropdown_demo.vue'
import rizui_colorpicker_demo from './pages/demos/control/rizui_colorpicker_demo.vue'
import rizui_timeline_demo from './pages/demos/control/rizui_timeline_demo.vue'
import rizui_button_demo from './pages/demos/control/rizui_button_demo.vue'
import rizui_checkbox_demo from './pages/demos/control/rizui_checkbox_demo.vue'
import rizui_progress_demo from './pages/demos/control/rizui_progress_demo.vue'
import rizui_circle_progress_demo from './pages/demos/control/rizui_circle_progress_demo.vue'
import rizui_text_demo from './pages/demos/input/rizui_text_demo.vue'
import rizui_mdeditor_demo from './pages/demos/input/rizui_mdeditor_demo.vue'
import UiCard_demo    from './pages/demos/old/UiCard_demo.vue'
import UiConfigRow_demo from './pages/demos/old/UiConfigRow_demo.vue'
import UiSwitch_demo  from './pages/demos/old/UiSwitch_demo.vue'
import UiSlider_demo  from './pages/demos/old/UiSlider_demo.vue'
import UiDropdown_demo from './pages/demos/old/UiDropdown_demo.vue'
import UiColorPicker_demo from './pages/demos/old/UiColorPicker_demo.vue'
import UiTimeline_demo from './pages/demos/old/UiTimeline_demo.vue'
import UiDialog_demo  from './pages/demos/old/UiDialog_demo.vue'
import NotReadyOverlay_demo from './pages/demos/old/NotReadyOverlay_demo.vue'

const routes = [
  { path: '/',                         name: 'home',              component: HomePage },
  { path: '/pricing',                  name: 'pricing',           component: PricingPage },
  { path: '/demo/rizui-card',          name: 'rizui_card',         component: rizui_card_demo },
  { path: '/demo/rizui-cfgrow',       name: 'rizui_cfgrow',      component: rizui_cfgrow_demo },
  { path: '/demo/rizui-infobox',      name: 'rizui_infobox',     component: rizui_infobox_demo },
  { path: '/demo/rizui-dialog',       name: 'rizui_dialog',       component: rizui_dialog_demo },
  { path: '/demo/rizui-toast',        name: 'rizui_toast',      component: rizui_toast_demo },
  { path: '/demo/rizui-code',         name: 'rizui_code',       component: rizui_code_demo },
  { path: '/demo/rizui-nav',          name: 'rizui_nav',        component: rizui_nav_demo },
  { path: '/demo/theme-color',        name: 'ThemeColor',      component: ThemeColor_demo },
  { path: '/demo/open-url',           name: 'OpenURL',         component: OpenURL_demo },
  { path: '/demo/rizui-switch',       name: 'rizui_switch',      component: rizui_switch_demo },
  { path: '/demo/rizui-slider',       name: 'rizui_slider',      component: rizui_slider_demo },
  { path: '/demo/rizui-dropdown',     name: 'rizui_dropdown',    component: rizui_dropdown_demo },
  { path: '/demo/rizui-colorpicker',  name: 'rizui_colorpicker', component: rizui_colorpicker_demo },
  { path: '/demo/rizui-timeline',     name: 'rizui_timeline',   component: rizui_timeline_demo },
  { path: '/demo/rizui-button',       name: 'rizui_button',     component: rizui_button_demo },
  { path: '/demo/rizui-checkbox',     name: 'rizui_checkbox',   component: rizui_checkbox_demo },
  { path: '/demo/rizui-progress',     name: 'rizui_progress',   component: rizui_progress_demo },
  { path: '/demo/rizui-circle-progress', name: 'rizui_circle_progress', component: rizui_circle_progress_demo },
  { path: '/demo/rizui-text',         name: 'rizui_text',       component: rizui_text_demo },
  { path: '/demo/rizui-mdeditor',     name: 'rizui_mdeditor',   component: rizui_mdeditor_demo },
  { path: '/demo/ui-card',             name: 'UiCard',            component: UiCard_demo },
  { path: '/demo/ui-config-row',       name: 'UiConfigRow',       component: UiConfigRow_demo },
  { path: '/demo/ui-switch',           name: 'UiSwitch',          component: UiSwitch_demo },
  { path: '/demo/ui-slider',           name: 'UiSlider',          component: UiSlider_demo },
  { path: '/demo/ui-dropdown',         name: 'UiDropdown',        component: UiDropdown_demo },
  { path: '/demo/ui-color-picker',     name: 'UiColorPicker',     component: UiColorPicker_demo },
  { path: '/demo/ui-timeline',         name: 'UiTimeline',        component: UiTimeline_demo },
  { path: '/demo/ui-dialog',           name: 'UiDialog',          component: UiDialog_demo },
  { path: '/demo/not-ready-overlay',   name: 'NotReadyOverlay',   component: NotReadyOverlay_demo }
]

export function createDemoRouter() {
  return createRouter({
    history: createWebHashHistory(),
    routes
  })
}
