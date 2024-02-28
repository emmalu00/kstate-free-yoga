<template>
    <div>
      <button @click="fetchClasses">Load Yoga Classes</button>
      <ul>
        <li v-for="yogaClass in yogaClasses" :key="yogaClass.id">
          {{ yogaClass.ClassName }} - {{ yogaClass.ClassDescription }}
        </li>
      </ul>
    </div>
  </template>
  
  <script>
  import { useYogaClassesStore } from '@/stores/YogaClasses';
  
  export default {
    name: 'YogaClassesComponent',
  
    computed: {
      yogaClasses() {
        const store = useYogaClassesStore();
        return store.classes;
      },
    },
  
    methods: {
      fetchClasses() {
        const store = useYogaClassesStore();
        store.fetchYogaClasses();
      },
    },
  };
  </script>