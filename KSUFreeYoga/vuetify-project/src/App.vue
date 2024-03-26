<template>
  <v-app>
    <v-main>
      <v-app-bar>
         <v-app-bar-title> K-State Free Yoga </v-app-bar-title>
         <v-spacer></v-spacer>
         <RouterLink to="/schedule" active-class="active">
          Shcueidle
         </RouterLink>
      </v-app-bar>
      <router-view></router-view>
    </v-main>
    
  </v-app>
</template>

<script setup>
import YogaCalandar from './components/YogaCalandar.vue';
import AddYogaClassForm from './components/AddYogaClassForm.vue';
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
