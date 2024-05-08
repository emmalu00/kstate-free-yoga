<template>
    <v-row no-gutters>
        <v-col cols="12" justify="center">
            <h1> Class Schedule </h1>
            <v-divider class="border-opacity-25" :thickness="2" color="black" style="width: 90%; margin: 0 auto;"></v-divider>
        </v-col>
    </v-row>
    <v-row>
        <v-col cols="3">
            <v-sheet class="pa-2 ma-2">
                <FilterForm @filter="applyFilters" @reset="fetchEvents"></FilterForm>
                <v-divider style="margin: 7%;"></v-divider>
                <ClassFinderForm ></ClassFinderForm>
            </v-sheet>
           
        </v-col>
        <v-col cols="9"> 
            <v-overlay :model-value="overlayItem" class="align-center justify-center">
                <v-progress-circular indeterminate color="#bfaec2"></v-progress-circular>
            </v-overlay>
            <Calendar :YogaClasses="filteredEvents"></Calendar>
        </v-col>
    </v-row>
</template>

<script>
import Calendar from '../components/Calendar.vue';
import FilterForm from '../components/FilterForm.vue';
import ClassFinderForm from '../components/ClassFinderForm.vue';
import { useYogaClassesStore } from '@/stores/YogaClassStore.js'; 

export default {
    components: {
        Calendar, FilterForm, ClassFinderForm
    }, 
    data() {
    return {
      filteredEvents: [], 
      filters: { selectedLocation: null, selectedInstructor: null, matsProvided: null },
      overlayItem: true,
    }
  }, 
  methods: {
    async fetchEvents() {
        const yogaClassesStore = useYogaClassesStore();
        await yogaClassesStore.getClasses();
        this.filteredEvents = yogaClassesStore.filteredClasses.map((yogaClass) => {
            return {
                title: yogaClass.ClassName,
                date: new Date(`${yogaClass.ClassDate.substr(0, 10)}T${yogaClass.StartTime}`),
                extendedProps: {
                    yogaClass: yogaClass,
                },
            };
        });
        this.overlayItem = false;
    }, 
    async applyFilters(filters) {
        const yogaClassesStore = useYogaClassesStore();
        await yogaClassesStore.getClasses(filters);
        this.filteredEvents = yogaClassesStore.filteredClasses.map((yogaClass) => {
            return {
                title: yogaClass.ClassName,
                date: new Date(`${yogaClass.ClassDate.substr(0, 10)}T${yogaClass.StartTime}`),
                extendedProps: {
                    yogaClass: yogaClass,
                },
            };
        });
    }
  },   
  mounted() {
    this.fetchEvents();
  },
  
}

</script>

<style scoped>
h1 {
    color: #644874;
    text-align: center;
    margin: 1% auto;
}
</style>
