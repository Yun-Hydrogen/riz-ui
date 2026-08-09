import { createApp } from 'vue'
import App from './App.vue'
import { createDemoRouter } from './router.js'

const app = createApp(App)
app.use(createDemoRouter())
app.mount('#app')
