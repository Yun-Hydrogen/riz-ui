import { createRouter, createWebHashHistory } from 'vue-router'

import HomePage       from './pages/HomePage.vue'
import PricingPage    from './pages/PricingPage.vue'

import Cntr_Card_demo   from './pages/demos/container/Cntr_Card_demo.vue'
import Cntr_Cfgrow_demo from './pages/demos/container/Cntr_Cfgrow_demo.vue'
import Ctrl_Switch_demo from './pages/demos/control/Ctrl_Switch_demo.vue'
import Ctrl_Slider_demo from './pages/demos/control/Ctrl_Slider_demo.vue'
import Ctrl_Dropdown_demo from './pages/demos/control/Ctrl_Dropdown_demo.vue'
import Ctrl_ColorPicker_demo from './pages/demos/control/Ctrl_ColorPicker_demo.vue'
import Ctrl_Timeline_demo from './pages/demos/control/Ctrl_Timeline_demo.vue'
import Inpt_demo from './pages/demos/input/Inpt_Text_demo.vue'
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
  { path: '/demo/cntr-card',           name: 'Cntr_Card',         component: Cntr_Card_demo },
  { path: '/demo/cntr-cfgrow',        name: 'Cntr_Cfgrow',      component: Cntr_Cfgrow_demo },
  { path: '/demo/ctrl-switch',        name: 'Ctrl_Switch',      component: Ctrl_Switch_demo },
  { path: '/demo/ctrl-slider',        name: 'Ctrl_Slider',      component: Ctrl_Slider_demo },
  { path: '/demo/ctrl-dropdown',      name: 'Ctrl_Dropdown',    component: Ctrl_Dropdown_demo },
  { path: '/demo/ctrl-color-picker',  name: 'Ctrl_ColorPicker', component: Ctrl_ColorPicker_demo },
  { path: '/demo/ctrl-timeline',      name: 'Ctrl_Timeline',   component: Ctrl_Timeline_demo },
  { path: '/demo/inpt',               name: 'Inpt',            component: Inpt_demo },
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
