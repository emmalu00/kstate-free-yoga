<template>
    <v-sheet class="pa-2 ma-2">
      <h3> Filter </h3>
      <v-form ref="form">
        <v-btn>
            Add Class
        </v-btn>
        <v-btn>
            Remove Class
        </v-btn>
        <v-btn>
            Add Class
        </v-btn>
      </v-form>
    </v-sheet>
  </template>
  
  <script>
  import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file
  
  export default {
    data() {
      return {
        
      }
    },
    methods: {
      
    }
  }
  </script>
  