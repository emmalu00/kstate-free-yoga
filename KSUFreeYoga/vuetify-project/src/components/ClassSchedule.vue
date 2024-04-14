<template>
    <div> 
        <h1> Class Schedule </h1> 
    </div>
    <div>
      <v-row no-gutters>
        <v-col cols="3">
          <FilterForm @FilteringYoga="receiveFilters" @ResetFilters="fetchEvents"> </FilterForm>
          <v-sheet class="pa-2 ma-2">
            <v-card>
              <v-btn @click="showAddForm = !showAddForm" block variant="outlined"> Add Class </v-btn>
            </v-card>
          </v-sheet>
          <v-dialog v-model="showAddForm" persistent style="width: 75%;">
            <AddForm @addingClass="addClassToSchedule" @close="showAddForm = false"></AddForm>
          </v-dialog>
        </v-col>
        <v-col>
            <Calendar :Yogaevents="filteredEvents" @deletingClass="deleteClass"></Calendar>
        </v-col>
      </v-row>
    </div>
    <v-snackbar v-model="snackbarAdd" :timeout="3000"> Class successfully added. 
      <template v-slot:actions>
        <v-btn color="blue" variant="text"  @click="snackbar = false"> Close </v-btn>
      </template>
    </v-snackbar>
    <v-snackbar v-model="snackbarDelete" :timeout="3000"> Class successfully deleted. 
      <template v-slot:actions>
        <v-btn color="blue" variant="text"  @click="snackbar = false"> Close </v-btn>
      </template>
    </v-snackbar>
</template>
  
<script>
import { useYogaClassesStore } from '@/stores/YogaClasses'; 
import FilterForm from './FilterForm.vue';
import Calendar from './Calendar.vue';
import dayGridPlugin from '@fullcalendar/daygrid'
import AddForm from './AddForm.vue';

export default {
  components: {
    FilterForm, 
    Calendar
  },
  data: function() {
    return {
      calendarOptions: {
          plugins: [dayGridPlugin],
          initialView: 'dayGridMonth',
          events: [], 
          eventClick: null,
        },
      filteredEvents: [], 
      showAddForm: false,
      snackbarAdd: false, 
      snackbarDelete: false,
      filters: { selectedInstructor: null, selectedInstructor: null, matsAvailable: null },
    }
  }, 
  methods: {
      async receiveFilters() 
      {
        const firstName = this.filters.selectedInstructor ? this.filters.selectedInstructor.split(" ")[0] : null;
        const lastName = this.filters.selectedInstructor ? this.filters.selectedInstructor.split(" ")[1] : null;
        const yogaClassesStore = useYogaClassesStore();
        await yogaClassesStore.filterYogaClasses(this.filters.selectedLocation, firstName, lastName, this.filters.matsAvailable);
        this.filteredEvents = yogaClassesStore.filteredClasses.map((yogaClass) => {
          return {
            title: yogaClass.ClassName,
            date: new Date(`${yogaClass.ClassDate.substr(0, 10)}T${yogaClass.StartTime}`),
            extendedProps: {
              yogaClass: yogaClass,
            },
          };
        });
      }, 
      async addClassToSchedule(newClass)
      {
        const yogaClassesStore = useYogaClassesStore();
        await yogaClassesStore.addClass(newClass);
        this.snackbarAdd = true;
        this.fetchEvents();
      },
      async deleteClass(classID) {
        const yogaClassesStore = useYogaClassesStore();
        await yogaClassesStore.deleteClass(classID);
        this.fetchEvents();
        this.snackbarDelete = true;
      },
  },
  mounted() {
    this.receiveFilters();
  },
}
</script>

<style scoped>
h1 {
    color: #644874;
    text-align: center;
    margin-top: 1%;
}
</style>
