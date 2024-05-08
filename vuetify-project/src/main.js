/**
 * main.js
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

// Plugins
import { registerPlugins } from '@/plugins'

// Components
import App from './App.vue'

import router from './router'


// Composables
import { createApp } from 'vue'

import GoogleSignInPlugin  from 'vue3-google-signin'


const app = createApp(App)

  
registerPlugins(app)

app.use(GoogleSignInPlugin  , {
    clientId: '1099353898213-senp1ppvp9hugivm11uid6ur7aksle2h.apps.googleusercontent.com',
  });

app.mount('#app')
