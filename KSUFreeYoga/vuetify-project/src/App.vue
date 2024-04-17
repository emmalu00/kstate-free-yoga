<template>
  <v-app>
    <v-main>
      <v-app-bar>
        <RouterLink to="/" class="app-bar-title"> 
          <v-app-bar-title> K-State Free Yoga </v-app-bar-title> 
        </RouterLink>
        <v-spacer></v-spacer>
        <RouterLink to="/schedule" class="menu-option"> Shcueidle </RouterLink>
        <RouterLink to="/about" class="menu-option"> About </RouterLink>
      </v-app-bar>
      <router-view></router-view>
    </v-main>
  </v-app>
</template>

<script setup>
import { RouterLink } from 'vue-router'

</script>

<style>
.menu-option, .app-bar-title {
  text-decoration: none;
  color: black !important; 
  margin-right: 20px;
}
</style>
