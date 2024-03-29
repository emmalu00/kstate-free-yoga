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
import YogaCalandar from './components/YogaCalandar.vue';
import ClassSchedule from './components/ClassSchedule.vue';
import CalanderSample from './components/CalanderSample.vue';
import Events from './views/Events.vue'
import { RouterLink } from 'vue-router'

  //import axios
  //ref 
</script>

<style>
.router-link-active, .router-link-exact-active {
  text-decoration: none;
  color: black !important; /* Use important to override Vuetify styles if necessary */
}
</style>
