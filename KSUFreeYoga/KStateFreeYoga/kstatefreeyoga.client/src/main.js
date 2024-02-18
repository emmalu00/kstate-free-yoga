/*import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'

createApp(App).mount('#app')*/
import { createApp } from 'vue'
import App from './App.vue'

// Vuetify
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
    components,
    directives,
})

createApp(App).use(vuetify).mount('#app')

/*import { createApp } from 'vue';
import App from './App.vue';
import './assets/main.css';

// Import Vuetify
import Vuetify from 'vuetify';
import 'vuetify/dist/vuetify.min.css';

const app = createApp(App);

// Use Vuetify
app.use(Vuetify);

app.mount('#app');*/