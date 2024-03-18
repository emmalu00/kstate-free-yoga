<template>
  <v-sheet class="pa-2 ma-2">
    <v-card
    :style="{ backgroundColor: '#f0f0f0' }">
    <v-form ref="form" class="filter-form">
      <h2> Filter </h2>

      <v-checkbox
      v-model="matsAvailable"
      label="Mats Available">
      </v-checkbox>
      
      <v-select
      :items="locations"
      label="Location"
      v-model="selectedLocation">
      </v-select>

      <v-select
      :items="teachers"
      label="Instructor"
      v-model="selectedInstructor">
      </v-select>

      <v-btn
      @click="applyFilters">
        Apply
      </v-btn>

      <v-btn
      @click="reset">
        Reset Filters
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
      matsAvailable: true
    }
  },
  methods: {
    async fetchEvents() {
      const yogaClassesStore = useYogaClassesStore();
      await yogaClassesStore.fetchTeacherNames();
      await yogaClassesStore.fetchLocations();
      this.teachers = yogaClassesStore.teacherNames.map(teacher => teacher.TeacherName);
      this.locations = yogaClassesStore.locations.map(location => location.BuildingName);
    },
    reset () {
        this.$refs.form.reset();
        this.$emit('ResetFilters', {
      matsAvailable: this.matsAvailable,
      selectedLocation: this.selectedLocation,
      selectedInstructor: this.selectedInstructor
    });
    },
    applyFilters()
    {
      this.$emit('FilteringYoga', {
      matsAvailable: this.matsAvailable,
      selectedLocation: this.selectedLocation,
      selectedInstructor: this.selectedInstructor
    });
    }
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
