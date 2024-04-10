<template>
  <v-app>
    <v-main>
      <v-app-bar>
        <RouterLink to="/" class="app-bar-title-link" style="text-decoration: none; color: black; margin-left: 20px;">
          <v-app-bar-title> K-State Free Yoga </v-app-bar-title>
        </RouterLink>
         <v-spacer></v-spacer>
         <RouterLink to="/schedule" style="text-decoration: none; color: black; margin-right: 20px;">
          Shcueidle
         </RouterLink>
         <RouterLink to="/about" style="text-decoration: none; color: black; margin-right: 20px;">
          About
         </RouterLink>
      </v-app-bar>
      <router-view></router-view>

    </v-main>
    
  </v-app>
</template>

<script setup>
import ClassSchedule from './components/ClassSchedule.vue';

import Events from './views/Events.vue'
import FullCalendar from '@fullcalendar/vue3'

import { RouterLink } from 'vue-router'
import { Calendar } from '@fullcalendar/core'
import timeGridPlugin from '@fullcalendar/timegrid'



  //import axios
  //ref 
</script>

<style>
.router-link-active, .router-link-exact-active {
  text-decoration: none;
  color: black !important; /* Use important to override Vuetify styles if necessary */
}
</style>
