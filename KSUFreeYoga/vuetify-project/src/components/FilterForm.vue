<template>
  <v-sheet class="pa-2 ma-2">
    <v-card
    :style="{ backgroundColor: '#f0f0f0' }">
    <v-form ref="form" class="filter-form">
      <h2> Filter </h2>
      <v-checkbox
      label="Mats Available">
      </v-checkbox>
      
      <v-select
      color="purple"
      :items="locations"
      label="Location">
      </v-select>

      <v-select
      :items="teachers"
      label="Instructor"
      v-model="selectedInstructor">
      </v-select>

      <v-select
      label="Difficulty Level">
      </v-select>

      <v-btn>
        Apply
      </v-btn>

      <v-btn
      @click="reset">
        Reset
      </v-btn>
      
    </v-form>
    </v-card>
    
  </v-sheet>
</template>

<script>
import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file

export default {
  data() {
    return {
      teachers: [], // Data property to hold the teacher names
      selectedInstructor: null, // Data property for the selected instructor
      locations: [], 
      selectedLocation: null, 
    }
  },
  methods: {
    async fetchEvents() {
      const yogaClassesStore = useYogaClassesStore();
      await yogaClassesStore.fetchTeacherNames();
      await yogaClassesStore.fetchLocations();
      this.teachers = yogaClassesStore.teacherNames.map(teacher => teacher.TeacherName);
      this.locations = yogaClassesStore.locations.map(location => location.LocationID);
      console.log(this.locations);
    },
    reset () {
        this.$refs.form.reset()
    },
  },
  mounted() {
    this.fetchEvents(); // Call fetchTeacherNames when the component is mounted
  },
}
</script>

<style scoped>
.filter-form {
  padding: 2%
  
}

</style>
