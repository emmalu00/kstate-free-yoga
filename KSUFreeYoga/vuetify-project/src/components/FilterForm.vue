<template>
  <v-sheet class="pa-2 ma-2">
    <v-card variant="outlined">
    <v-form ref="form" class="filter-form">
      <h2> Filter </h2>
      <v-select
      variant="outlined"
      :items="matsOptions"
      item-title="text"
      item-value="id"
      label="Mats Provided?"
      v-model="matsAvailable" >
      </v-select>

      <v-select
      variant="outlined"
      :items="locations"
      label="Location"
      v-model="selectedLocation">
      </v-select>

      <v-select
      :items="teachers"
      variant="outlined"
      label="Instructor"
      v-model="selectedInstructor">
      </v-select>

      <div class="button-container">
        <v-btn
          class="filter-buttons"
          variant="outlined"
          @click="applyFilters">
            Apply
        </v-btn>
      
        <v-btn
        class="filter-buttons"
          variant="outlined"
          @click="reset">
            Reset Filters
        </v-btn>
      </div>
      
    </v-form>
    </v-card>
    
  </v-sheet>
</template>

<script>
import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file
import { useInstructorsStore } from '@/stores/Instructors';

export default {
  data() {
    return {
      teachers: [], // Data property to hold the teacher names
      selectedInstructor: null, // Data property for the selected instructor
      locations: [], 
      selectedLocation: null, 
      matsAvailable: null,
      matsOptions: [ 
        {text: 'Yes', id: true},
        {text: 'No', id: false}
      ]
    }
  },
  methods: {
    async fetchEvents() {
      const yogaClassesStore = useYogaClassesStore();
      const instructorsStore = useInstructorsStore();

      await yogaClassesStore.fetchLocations();
      await instructorsStore.fetchInstructors();

      this.teachers = instructorsStore.instructors.map(teacher => `${teacher.FirstName} ${teacher.LastName}`);
      this.locations = yogaClassesStore.locations.map(location => location.BuildingName);
      console.log(this.matsOptions);
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
      console.log(this.selectedInstructor);
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
      const parts = fullName.split(" ");
      if (parts.length === 1) return "";
      return parts[parts.length - 1];
    }
  },
  mounted() {
    this.fetchEvents(); 
  },
}
</script>

<style scoped>
.filter-form {
  padding: 2%;
  background-color: #e5d5e0
}

.button-container {
  display: flex;
  justify-content: space-between; 
}

:deep(.v-field__overlay) {
  background-color: white !important;
}

.filter-buttons {
  background-color: #927396;
}
</style>
