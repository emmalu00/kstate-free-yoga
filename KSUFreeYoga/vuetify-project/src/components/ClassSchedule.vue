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
            <Calendar :Yogaevents="testEvents" @deletingClass="deleteClass"></Calendar>
        </v-col>
        </v-row>
    </div>
    <v-snackbar v-model="snackbarAdd" :timeout="3000">
      Class successfully added. 
      <template v-slot:actions>
        <v-btn color="blue" variant="text"  @click="snackbar = false"> Close </v-btn>
      </template>
    </v-snackbar>
    <v-snackbar v-model="snackbarDelete" :timeout="3000">
      Class successfully deleted. 
      <template v-slot:actions>
        <v-btn color="blue" variant="text"  @click="snackbar = false"> Close </v-btn>
      </template>
    </v-snackbar>
</template>
  
<script>
import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file
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
      testEvents: [],
      filteredEvents: [], 
      showAddForm: false,
      snackbarAdd: false, 
      snackbarDelete: false,
    }
  }, 
  methods: {
    async fetchEvents() {
        const yogaClassesStore = useYogaClassesStore();
        await yogaClassesStore.filterYogaClasses(null, null, null, null);
        this.testEvents = yogaClassesStore.filteredClasses.map((yogaClass) => {
          return {
            title: yogaClass.ClassName,
            date: this.combineDateTime(yogaClass.ClassDate, yogaClass.StartTime), 
            extendedProps: {
              classID: yogaClass.ClassID,
              startTime: yogaClass.StartTime,
              endTime: yogaClass.EndTime,
              instructorName: this.getInstructorName(yogaClass.FirstName, yogaClass.LastName),
              building: yogaClass.BuildingName,
              room: yogaClass.RoomNumber, 
              address: yogaClass.LocationAddress,
              matsAvailable: yogaClass.MatsAvailable,
              classDescription: yogaClass.ClassDescription,
            },
          };
        });
      },
      combineDateTime(date, time) {
        const newDate = date.substr(0, 10);
        const dateTimeStr = `${newDate}T${time}`;
        const dateTime = new Date(dateTimeStr);
        return dateTime;
      },
      async receiveFilters(filters) {
        console.log(filters);
        const firstName = this.getFirstName(filters.selectedInstructor);
        const lastName = this.getLastName(filters.selectedInstructor);
        const yogaClassesStore = useYogaClassesStore();
        await yogaClassesStore.filterYogaClasses(filters.selectedLocation, firstName, lastName, filters.matsAvailable);
        this.filteredEvents = yogaClassesStore.filteredClasses.map((yogaClass) => {
          return {
            title: yogaClass.ClassName,
            date: this.combineDateTime(yogaClass.ClassDate, yogaClass.StartTime), 
            extendedProps: {
              classID: yogaClass.ClassID,
              startTime: yogaClass.StartTime,
              endTime: yogaClass.EndTime,
              instructorName: this.getInstructorName(yogaClass.FirstName, yogaClass.LastName),
              building: yogaClass.BuildingName,
              room: yogaClass.RoomNumber, 
              address: yogaClass.LocationAddress,
              matsAvailable: yogaClass.MatsAvailable,
              classDescription: yogaClass.ClassDescription,
            },
          };
        });;
        this.testEvents = this.filteredEvents;
        console.log(this.filteredEvents);
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
      getInstructorName(first, last)
      {
        return `${first} ${last}`;
      },
      getFirstName(fullName)
      {
        if (!fullName) return '';
        return fullName.split(" ")[0];
      }, 
      getLastName(fullName)
      {
        if (!fullName) return '';
        return fullName.split(" ")[1];;
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
    margin-top: 1%;
}
</style>
