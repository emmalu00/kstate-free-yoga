//all router code here

import { createRouter, createWebHistory  } from 'vue-router';
import Events from "../views/Events.vue"
import HomeView from "../views/HomeView.vue"
//import Router from 'vue-router';

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/schedule', 
            name: 'schedule', 
            component: Events
        },
        {
            path: '/', 
            name: 'home', 
            component: HomeView
        },
    ]
})

export default router

