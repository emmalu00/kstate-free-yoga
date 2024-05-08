/**
 * router/index.ts
 *
 * Automatic routes for `./src/pages/*.vue`
 */

// Composables
import { createRouter, createWebHistory } from 'vue-router'
import { setupLayouts } from 'virtual:generated-layouts'
import HomeView from '../pages/HomeView.vue'
import ScheduleView from '../pages/ScheduleView.vue'
import AboutView from '../pages/AboutView.vue'
import AccountView from '../pages/AccountView.vue';
import AdminView from '../pages/AdminView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  extendRoutes: setupLayouts,
  routes: [
    {
      path: '/', 
      name: 'home', 
      component: HomeView
    },
    {
      path: '/schedule', 
      name: 'schedule', 
      component: ScheduleView
    }, 
    {
      path: '/about', 
      name: 'about', 
      component: AboutView
    }, 
    {
      path: '/account', 
      name: 'account', 
      component: AccountView
    }, 
    {
      path: '/admin', 
      name: 'admin', 
      component: AdminView
    }
  ]
})

export default router
