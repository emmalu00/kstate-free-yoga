/**
 * main.js
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

// Plugins
import { registerPlugins } from '@/plugins'

// Components
import App from './App.vue'

import vuetify from './plugins/vuetify';
import router from './router'

// Composables
import { createApp } from 'vue'
import { createPinia } from 'pinia';

const app = createApp(App)
app.use(createPinia());
app.use(router)
app.use(vuetify);


//registerPlugins(app)

app.mount('#app')
