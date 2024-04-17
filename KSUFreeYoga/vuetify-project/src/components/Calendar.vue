<template>
        <v-sheet class="pa-2 ma-2">
          <v-card variant="outlined">
            <FullCalendar ref="fullCalendar"  :options="calendarOptions" style="height: 30;"/>
          </v-card>
        </v-sheet>
        <div v-if="showModal" class="modal">
            <div class="modal-content">
              <p> {{ selectedEvent.yogaClass}} </p>
                <span class="close" @click="showModal = false">&times;</span>
                <h2>{{ selectedEvent.className }}</h2>
                <p style="color: grey; line-height: 1.0; font-size: 15px; " > {{selectedEvent.classDescription }}</p>
                <div class="event-container">
                  <v-icon icon="fa-regular fa-calendar"></v-icon>
                  <p><strong>{{ selectedEvent.startStr }}</strong></p>
                </div>
                <div class="event-container">
                  <v-icon icon="fa-regular fa-clock"> </v-icon>
                  <p><strong>{{ selectedEvent.startTime }} - {{ selectedEvent.endTime }}</strong></p>
                </div>
                <v-divider></v-divider>
                <div class="event-container">
                  <v-icon icon="fa-solid fa-location-dot"> </v-icon>
                  <p><strong>{{ selectedEvent.building }}</strong> - <strong>{{ selectedEvent.room }}</strong></p>
                </div>
                <p style="margin-left: 35px;"> {{ selectedEvent.address }}</p>
                <v-divider></v-divider>
                <div class="event-container">
                  <v-icon color="green" v-if="selectedEvent.matsAvailable" icon="fa-solid fa-check"></v-icon>
                  <v-icon color="red" v-else icon="fa-solid fa-xmark"></v-icon>
                  <p><strong>Mats Provided</strong></p>
                </div>
                <div class="event-container">
                  <v-icon icon="fa-regular fa-user"> </v-icon>
                  <p><strong>Instructor: </strong>{{ selectedEvent.instructorName }}</p>
                </div>
                <v-divider></v-divider>
                <v-btn style="margin-top: 10px" variant="outlined" @click="deleteClassFromSchedule" block> Delete this class </v-btn>
            </div>
        </div>
  </template>

  <script>
  import FullCalendar from '@fullcalendar/vue3'
  import dayGridPlugin from '@fullcalendar/daygrid'
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
            right: 'dayGridMonth,listWeek' 
          },
          buttonText: {
            dayGridMonth: 'Month View', 
            listWeek: 'List View'
          },
          height: 750, 
          eventColor: '#644874',
          eventDisplay: 'block',
        }
      }
    }, 
    methods: {
      refetchCalendarEvents() 
      {
        let calendarApi = this.$refs.fullCalendar.getApi();
        calendarApi.refetchEvents();
      },
      handleEventClick(clickInfo) 
      {
        this.selectedEvent = {
          classID: clickInfo.event.extendedProps.yogaClass.ClassID,
          className: clickInfo.event.title,
          startTime: this.formatTime(clickInfo.event.extendedProps.yogaClass.StartTime),
          endTime: this.formatTime(clickInfo.event.extendedProps.yogaClass.EndTime),
          startStr: this.formatDate(clickInfo.event.startStr),
          instructorName: this.getInstructorName(clickInfo.event.extendedProps.yogaClass.FirstName, clickInfo.event.extendedProps.yogaClass.LastName),
          building: clickInfo.event.extendedProps.yogaClass.BuildingName,
          room: clickInfo.event.extendedProps.yogaClass.RoomNumber,
          address: clickInfo.event.extendedProps.yogaClass.LocationAddress,
          matsAvailable: clickInfo.event.extendedProps.yogaClass.MatsAvailable,
          classDescription: clickInfo.event.extendedProps.yogaClass.ClassDescription,
        };
        this.showModal = true;
      },
      async deleteClassFromSchedule()
      {
        this.$emit('deletingClass', this.selectedEvent.classID);
        this.showModal = false;
      },
      formatDate(dateString) 
      {
        return new Date(dateString.substr(0, 19)).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
        });
      },
      formatTime(time24) {
        const [hours24, minutes] = time24.split(':');
        const hours24Num = parseInt(hours24, 10);
        return `${hours24Num % 12 || 12}:${minutes.padStart(2, '0')} ${hours24Num >= 12 ? 'PM' : 'AM'}`;
      },
      getInstructorName(first, last)
      {
        return `${first} ${last}`;
      },
    },
             // await instructorsStore.fetchInstructorByID(instructorsStore.instructorID);
            // this.selectedInstructor = {
            //     text: `${instructorsStore.instructor[0].FirstName} ${instructorsStore.instructor[0].LastName}`, 
            //     id: instructorsStore.instructorID
            // }
            // this.$refs.instructorForm.reset();
            // this.showDropdownInstructor = false;
            // this.fetchEvents();
            // console.log(this.selectedInstructor);
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
  width: 35%;
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

:deep(.fc-button)
{
  background-color: #BF98B2  !important;
}

:deep(.fc-button-active) {
  background-color: #927396 !important; /* Even darker blue */
  color: white; /* White text */
  border-color: #004085; /* Corresponding border color */
}

.event-container {
  display: flex; /* This will layout the children (v-icon and p) in a row */
  align-items: center; /* This will vertically center the children in the container */
  gap: 10px;
  margin-top: 5px;
}

</style>
