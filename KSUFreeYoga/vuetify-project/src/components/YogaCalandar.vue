<template>
  <FullCalendar :options="calendarOptions" />
  <div v-if="showModal" class="modal">
    <div class="modal-content">
      <span class="close" @click="showModal = false">&times;</span>
      <h2>{{ selectedEvent.className }}</h2>
      <p><strong>Date:</strong> {{ selectedEvent.startStr }}</p>
      <p><strong>Start Time:</strong> {{ selectedEvent.startTime }}</p>
      <p><strong>Duration:</strong> {{ selectedEvent.duration }} minutes</p>
      <p><strong>Teacher:</strong> {{ selectedEvent.teacherName }}</p>
      <p><strong>Location ID:</strong> {{ selectedEvent.locationID }}</p>
      <p><strong>Mats Available:</strong> {{ selectedEvent.matsAvailable }}</p>
      <p><strong>Description:</strong> {{ selectedEvent.classDescription }}</p>
    </div>
  </div>

</template>

<script>
import FullCalendar from '@fullcalendar/vue3';
import dayGridPlugin from '@fullcalendar/daygrid';
import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file


export default {
  components: { FullCalendar },
  data() {
    return {
      showModal: false,
      selectedEvent: {},
      calendarOptions: {
        plugins: [dayGridPlugin],
        initialView: 'dayGridMonth',
        events: [], // Events will be set after fetching from the store
        eventClick: this.handleEventClick,
      },
    };
  },
  methods: {
    handleEventClick(clickInfo) {
      this.selectedEvent = {
        className: clickInfo.event.title,
        startStr: clickInfo.event.startStr,
        startTime: clickInfo.event.extendedProps.startTime,
        duration: clickInfo.event.extendedProps.duration,
        teacherName: clickInfo.event.extendedProps.teacherName,
        locationID: clickInfo.event.extendedProps.locationID,
        matsAvailable: clickInfo.event.extendedProps.matsAvailable ? 'Yes' : 'No',
        classDescription: clickInfo.event.extendedProps.classDescription,
      };
      this.showModal = true;
    },
    async fetchEvents() {
      const yogaClassesStore = useYogaClassesStore();
      await yogaClassesStore.fetchYogaClasses();
      console.log(yogaClassesStore.classes);
      this.calendarOptions.events = yogaClassesStore.classes.map((yogaClass) => {
        return {
          title: yogaClass.ClassName,
          date: yogaClass.ClassDate,
          extendedProps: {
            startTime: yogaClass.StartTime,
            duration: yogaClass.Duration,
            teacherName: yogaClass.TeacherName,
            locationID: yogaClass.LocationID,
            matsAvailable: yogaClass.MatsAvailable,
            classDescription: yogaClass.ClassDescription,
          },
        };
      });
    },
    combineDateTime(date, time) {
      // Assumes time is in 'HH:mm:ss' format and date is in 'YYYY-MM-DD' format
      // Adjust the format if your API returns them differently

      return `${date}T${time}`;
    },
    formatTime(time) {
      // Format the SQL time (which might be in 'HH:mm:ss' format) for display
      // You might not need this if the time is already in a nice format
      return time.substr(0, 5); // gets 'HH:mm' format
    },
  },
  mounted() {
    this.fetchEvents();
  },
};
</script>

<style scoped>
.modal {
  display: block;
  position: fixed;
  z-index: 1;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  overflow: auto;
  background-color: rgb(0, 0, 0);
  background-color: rgba(0, 0, 0, 0.4);
}

.modal-content {
  background-color: #fefefe;
  margin: 15% auto;
  padding: 20px;
  border: 1px solid #888;
  width: 80%;
}

.close {
  color: #aaa;
  float: right;
  font-size: 28px;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}
</style>
