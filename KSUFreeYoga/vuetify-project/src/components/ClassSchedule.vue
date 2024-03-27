<template>
    <div> 
        <h1> Class Schedule </h1> 
    </div>
    <div>
        <v-row no-gutters>
        <v-col cols="2">
          <FilterForm @FilteringYoga="receiveFilters" @ResetFilters="fetchEvents"> </FilterForm>
        </v-col>
        <v-col>
            <Calendar :Yogaevents="testEvents"></Calendar>
        </v-col>
        </v-row>
    </div>
</template>
  
<script>
import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file
import FilterForm from './FilterForm.vue';
import Calendar from './Calendar.vue';
import dayGridPlugin from '@fullcalendar/daygrid'

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
      filteredEvents: []
    }
  }, 
  methods: {
    async fetchEvents() {
        const yogaClassesStore = useYogaClassesStore();
        await yogaClassesStore.fetchYogaClasses();
        this.testEvents = yogaClassesStore.classes.map((yogaClass) => {
          return {
            title: yogaClass.ClassName,
            date: this.combineDateTime(yogaClass.ClassDate, yogaClass.StartTime), 
            extendedProps: {
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
        const yogaClassesStore = useYogaClassesStore();
        await yogaClassesStore.filterYogaClasses(filters.selectedLocation, filters.selectedInstructor, filters.matsAvailable);
        this.filteredEvents = yogaClassesStore.filteredClasses.map((yogaClass) => {
          return {
            title: yogaClass.ClassName,
            date: this.combineDateTime(yogaClass.ClassDate, yogaClass.StartTime), 
            extendedProps: {
              startTime: yogaClass.StartTime,
              endTime: yogaClass.endTime,
              instructorName: this.getInstructorName(yogaClass.FirstName, yogaClass.LastName),
              building: yogaClass.BuildingName,
              room: yogaClass.RoomNumber, 
              address: yogaClass.LocationAddress,
              matsAvailable: yogaClass.MatsAvailable,
              classDescription: yogaClass.ClassDescription,
            },
          };
        });;
        console.log(this.filteredEvents);
        this.testEvents = this.filteredEvents;
      }, 
      getInstructorName(first, last)
      {
        return `${first} ${last}`;
      }
  },
  mounted() {
    this.fetchEvents();
  },
}
</script>

<style scoped>
h1 {
    color: #b195c1;
    text-align: center;
}
</style>
