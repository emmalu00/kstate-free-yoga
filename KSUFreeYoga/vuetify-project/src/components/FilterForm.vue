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
      matsAvailable: null
    }
  },
  methods: {
    async fetchEvents() {
      const yogaClassesStore = useYogaClassesStore();
      await yogaClassesStore.fetchTeacherNames();
      await yogaClassesStore.fetchLocations();
      this.teachers = yogaClassesStore.teacherNames.map(teacher => `${teacher.FirstName} ${teacher.LastName}`);
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
      console.log(this.selectedLocation);
      this.$emit('FilteringYoga', {
      matsAvailable: this.matsAvailable,
      selectedLocation: this.selectedLocation,
      selectedInstructor: this.selectedInstructor
    });
    }, 
    getFirstName(fullName)
    {
      return fullName.split(" ")[0];
    }, 
    getLastName(fullName)
    {
      // Split the fullName string into an array of words
      const parts = fullName.split(" ");
      // If there's only one part, return an empty string for the last name
      if (parts.length === 1) return "";
      // Return the last element of the array as the last name
      // This simplistic approach assumes the last name is the last part of the fullName
      return parts[parts.length - 1];
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
