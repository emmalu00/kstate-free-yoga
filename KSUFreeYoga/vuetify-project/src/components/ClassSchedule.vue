<template>
    <div>  
        <h1> Class Schedule </h1> 
    </div>
    <div>
        <v-row no-gutters>
        <v-col cols="2">
          <FilterForm ></FilterForm>
        </v-col>
        <v-col>
            <Calendar></Calendar>
        </v-col>
        </v-row>
    </div>
</template>
  
<script>
import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file
import FilterForm from './FilterForm.vue';
import Calendar from './Calendar.vue';

export default {
  components: {
    FilterForm, 
    Calendar
  },
  data: function() {
    return {
     
    }
  }, 
  methods: {
    
  },
  mounted() {
    
  },
}
</script>

<style scoped>
h1 {
    color: #b195c1;
    text-align: center;
}
</style>
