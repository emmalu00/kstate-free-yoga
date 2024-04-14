<template>
  <v-sheet class="pa-2 ma-2">
    <v-card variant="outlined">
    <v-form ref="form" class="filter-form">
      <h2> Filter </h2>

      <v-select variant="outlined"  label="Mats Provided?"
      :items="matsOptions" item-title="text" item-value="id" v-model="matsAvailable">
      </v-select>

      <v-select variant="outlined" label="Location"
      :items="locations" v-model="selectedLocation">
      </v-select>

      <v-select variant="outlined" label="Instructor"
      :items="teachers" v-model="selectedInstructor">
      </v-select>

      <div class="button-container">
        <v-btn class="filter-buttons" variant="outlined" @click="applyFilters"> Apply </v-btn>
        <v-btn class="filter-buttons" variant="outlined" @click="reset"> Reset Filters </v-btn>
      </div>
    </v-form>
    </v-card>
  </v-sheet>
</template>

<script>
import { useInstructorsStore } from '@/stores/Instructors';
import { useLocationsStore } from '@/stores/ClassLocations';

export default {
  data() {
    return {
      teachers: [], 
      selectedInstructor: null, 
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
      const instructorsStore = useInstructorsStore();
      const classLocationsStore = useLocationsStore();
      await instructorsStore.fetchInstructors();
      await classLocationsStore.fetchLocations();
      this.teachers = instructorsStore.instructors.map(teacher => `${teacher.FirstName} ${teacher.LastName}`);
      this.locations = classLocationsStore.classLocations.map(location => location.BuildingName);

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
