<template>
        <v-sheet class="pa-2 ma-2">
          <v-card variant="outlined">
            <FullCalendar ref="fullCalendar"  :options="calendarOptions" style="height: 30;"/>
          </v-card>
        </v-sheet>
        <div v-if="showModal" class="modal">
            <div class="modal-content">
                <span class="close" @click="showModal = false">&times;</span>
                <h2>{{ selectedEvent.className }}</h2>
                <p> <strong>{{ selectedEvent.startStr }}</strong>   |    <strong>{{ selectedEvent.startTime }} - {{ selectedEvent.endTime }}</strong></p>
                <v-divider></v-divider>
                <p><strong> {{ selectedEvent.building }}</strong> - <strong> {{ selectedEvent.room }}</strong> </p>
                <p> {{ selectedEvent.address }}</p>
                <v-divider></v-divider>
                <p><strong>Instructor: </strong> {{ selectedEvent.instructorName }}</p>
                <p><strong>Mats Provided:</strong> {{ selectedEvent.matsAvailable }}</p>
                <p><strong>Description:</strong> {{ selectedEvent.classDescription }}</p>
                <v-divider></v-divider>
                <v-row>
                  <v-col cols="auto" @click="deleteClassFromSchedule">
                    <v-btn> Delete this class </v-btn>
                  </v-col>
                  <v-col cols="auto">
                    <v-btn> Update this class </v-btn>
                  </v-col>
                </v-row>
                
            </div>
        </div>
  </template>

  <script>
  import FullCalendar from '@fullcalendar/vue3'
  import dayGridPlugin from '@fullcalendar/daygrid'
  import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file
  //import TimeGrid from './components/TimeGrid.vue';
  import timeGridPlugin from '@fullcalendar/timegrid'
  import listPlugin from '@fullcalendar/list';



  export default {
    props: ['Yogaevents'],
    components: {
      FullCalendar 
    },
    watch: {
    Yogaevents: {
      immediate: true,
      handler(newVal) {
        this.calendarOptions.events = newVal;
        this.$nextTick(() => {
            this.refetchCalendarEvents();
          });
      }
    }
  },
    data: function() {
      return {
       showModal: false,
       selectedEvent: {},
       calendarOptions: {
          plugins: [listPlugin, dayGridPlugin],
          initialView: 'dayGridMonth',
          events: [], 
          eventClick: this.handleEventClick,
          headerToolbar: {
            left: 'prev,next',
            center: 'title',
            right: 'dayGridMonth,listWeek' // user can switch between the two
          },
          buttonText: {
            dayGridMonth: 'Month View', // Custom text for the 'dayGridMonth' button
            listWeek: 'List View' // Custom text for the 'listWeek' button
          },
          height: 750, 
          eventColor: '#644874',
          eventDisplay: 'block',
        }
      }
    }, 
    methods: {
      refetchCalendarEvents() {
      let calendarApi = this.$refs.fullCalendar.getApi();
      calendarApi.refetchEvents();
      //console.log(this.calendarOptions.events);
    },
      handleEventClick(clickInfo) {
        this.selectedEvent = {
          classID: clickInfo.event.extendedProps.classID,
          className: clickInfo.event.title,
          startTime: this.formatTime(clickInfo.event.extendedProps.startTime),
          endTime: this.formatTime(clickInfo.event.extendedProps.endTime),
          startStr: this.formatDate(clickInfo.event.startStr),
          instructorName: clickInfo.event.extendedProps.instructorName,
          building: clickInfo.event.extendedProps.building,
          room: clickInfo.event.extendedProps.room,
          address: clickInfo.event.extendedProps.address,
          matsAvailable: clickInfo.event.extendedProps.matsAvailable ? 'Yes' : 'No',
          classDescription: clickInfo.event.extendedProps.classDescription,
        };
        this.showModal = true;
      },
      async deleteClassFromSchedule()
      {
        console.log(this.selectedEvent.classID);
        const yogaClassesStore = useYogaClassesStore();
        await yogaClassesStore.deleteClass(this.selectedEvent.classID);
        this.refetchCalendarEvents();
        console.log(this.selectedEvent.classID);
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
  width: 30%;
  border-radius: 10px;
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

p {
  line-height: 2.0
}

h2 {
  color: 	#644874
}

.fc {
  padding: 1%;
}

::v-deep .fc-button
{
  background-color: #BF98B2  !important;
}

::v-deep .fc-button-active {
  background-color: #927396 !important; /* Even darker blue */
  color: white; /* White text */
  border-color: #004085; /* Corresponding border color */
}

</style>
