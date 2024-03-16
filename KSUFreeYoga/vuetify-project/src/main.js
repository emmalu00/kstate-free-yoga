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


// Composables
import { createApp } from 'vue'
import { createPinia } from 'pinia';

const app = createApp(App)
app.use(createPinia());
app.use(vuetify);

//registerPlugins(app)

app.mount('#app')
