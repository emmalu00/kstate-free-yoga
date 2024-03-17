<template>
        <v-sheet class="pa-2 ma-2">
          <v-card>
            <FullCalendar class="calendar" :options="calendarOptions" />
          </v-card>
        </v-sheet>
        <div v-if="showModal" class="modal">
            <div class="modal-content">
                <span class="close" @click="showModal = false">&times;</span>
                <h2>{{ selectedEvent.className }}</h2>
                <p><strong>Date:</strong> {{ selectedEvent.startStr }}</p>
                <p><strong>Start Time:</strong> {{ selectedEvent.startTime }}</p>
                <p><strong>Duration:</strong> {{ selectedEvent.duration }} minutes</p>
                <p><strong>Teacher:</strong> {{ selectedEvent.teacherName }}</p>
                <p><strong>Building Name:</strong> {{ selectedEvent.building }}</p>
                <p><strong>Room:</strong> {{ selectedEvent.room }}</p>
                <p><strong>Address:</strong> {{ selectedEvent.address }}</p>
                <p><strong>Mats Available:</strong> {{ selectedEvent.matsAvailable }}</p>
                <p><strong>Description:</strong> {{ selectedEvent.classDescription }}</p>
            </div>
        </div>
  </template>

  <script>
  import FullCalendar from '@fullcalendar/vue3'
  import dayGridPlugin from '@fullcalendar/daygrid'
  import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file
 
  export default {
    components: {
      FullCalendar 
    },
    data: function() {
      return {
       showModal: false,
       selectedEvent: {},
        calendarOptions: {
          plugins: [dayGridPlugin],
          initialView: 'dayGridMonth',
          events: [], 
          eventClick: this.handleEventClick,
        }
      }
    }, 
    methods: {
      handleEventClick(clickInfo) {
        this.selectedEvent = {
          className: clickInfo.event.title,
          startTime: this.formatTime(clickInfo.event.extendedProps.startTime),
          duration: clickInfo.event.extendedProps.duration,
          startStr: this.formatDate(clickInfo.event.startStr),
          teacherName: clickInfo.event.extendedProps.teacherName,
          building: clickInfo.event.extendedProps.building,
          room: clickInfo.event.extendedProps.room,
          address: clickInfo.event.extendedProps.address,
          matsAvailable: clickInfo.event.extendedProps.matsAvailable ? 'Yes' : 'No',
          classDescription: clickInfo.event.extendedProps.classDescription,
        };
        this.showModal = true;
        console.log(clickInfo.event.extendedProps.locationInfo);
      },
      async fetchEvents() {
        const yogaClassesStore = useYogaClassesStore();
        await yogaClassesStore.fetchYogaClasses();
        this.calendarOptions.events = yogaClassesStore.classes.map((yogaClass) => {
          return {
            title: yogaClass.ClassName,
            date: this.combineDateTime(yogaClass.ClassDate, yogaClass.StartTime), 
            extendedProps: {
              startTime: yogaClass.StartTime,
              duration: yogaClass.Duration,
              teacherName: yogaClass.TeacherName,
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
      formatDate(dateString) {
          const newDate = dateString.substr(0, 19);
          const date = new Date(newDate);
          const formattedDate = date.toLocaleDateString('en-US', {
          year: 'numeric',
          month: 'long',
          day: 'numeric'
          });
          return formattedDate;
      },
      formatTime(time24) {
          const [hours24, minutes] = time24.split(':');
          const hours24Num = parseInt(hours24, 10);
          const minutesNum = parseInt(minutes, 10);
          const suffix = hours24Num >= 12 ? 'PM' : 'AM';
          const hours12Num = hours24Num % 12 || 12; // Convert "0" to "12"
          const paddedMinutes = minutesNum < 10 ? '0' + minutesNum : minutesNum;
          const time12 = `${hours12Num}:${paddedMinutes} ${suffix}`;
          return time12;
      },
    },
    mounted() {
      this.fetchEvents();
    },
  }
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

.calendar {
  padding: 1%;
}


</style>
