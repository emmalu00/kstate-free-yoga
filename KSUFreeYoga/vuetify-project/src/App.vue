<template>
  <v-app>
    <v-main>
      <v-app-bar>
        <template v-slot:prepend>
          <v-app-bar-nav-icon></v-app-bar-nav-icon>
        </template>
         <v-app-bar-title> K-State Free Yoga </v-app-bar-title>
      </v-app-bar>
     <ClassSchedule></ClassSchedule>
    
    </v-main>
  </v-app>
</template>

<script setup>
import YogaCalandar from './components/YogaCalandar.vue';
import AddYogaClassForm from './components/AddYogaClassForm.vue';
import ClassSchedule from './components/ClassSchedule.vue';
import CalanderSample from './components/CalanderSample.vue';
  //import axios
  //ref 
</script>
