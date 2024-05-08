<template>
  <v-sheet class="pa-2 ma-2">
      <FullCalendar ref="fullCalendar" :options="calendarOptions"/>
  </v-sheet>
  <v-dialog v-model="showClassInfo" persistent max-width="600px">
      <v-card>
        <v-card-item :title="selectedEvent.ClassName">
          <template v-slot:append>
            <span class="close" @click="showClassInfo = false">&times;</span>
          </template>
          <p style="color: grey; line-height: 1.0; font-size: 15px;" > {{selectedEvent.ClassDescription }}</p>
        </v-card-item>
        <v-card-item>
          <template v-slot:prepend>
            <v-icon class="me-1 pb-1" color="black" icon="$calendar"></v-icon>
            {{ formatDate(selectedEvent.ClassDate) }}
          </template>
        </v-card-item>
        <v-card-item>
          <template v-slot:prepend>
            <v-icon class="me-1 pb-1" color="black" icon="$clock"></v-icon>
            {{ formatTime(selectedEvent.StartTime) }} - {{ formatTime(selectedEvent.EndTime) }}
          </template>
        </v-card-item>
        <v-card-item>
          <v-divider></v-divider>
        </v-card-item>
        <v-card-item>
          <template v-slot:prepend>
            <v-icon class="me-1 pb-1" color="black" icon="$location"></v-icon>
            {{ selectedEvent.BuildingName }} - {{ selectedEvent.RoomNumber }}
          </template>
        </v-card-item>
        <v-card-item>
          <v-tooltip text="Open In Google Maps" location="bottom">
            <template v-slot:activator="{ props }">
              <a :href="`https://maps.google.com/?q=${encodeURIComponent(selectedEvent.BuildingName)}+${encodeURIComponent(selectedEvent.LocationAddress)}`" 
              target="_blank" style="color: inherit; text-decoration: none;" v-bind="props">
                {{ selectedEvent.LocationAddress }}
              </a>
            </template>
          </v-tooltip>
        </v-card-item>
        <v-card-item>
          <v-divider></v-divider>
        </v-card-item>
        <v-card-item>
          <template v-slot:prepend>
            <v-icon v-if="selectedEvent.MatsProvided" class="me-1 pb-1" color="#87ab87" icon="$yes"></v-icon>
            <v-icon v-else class="me-1 pb-1" color="red" icon="$no"></v-icon>
            Mats Provided
          </template>
        </v-card-item>
        <v-card-item>
          <template v-slot:prepend>
            <v-icon class="me-1 pb-1" color="black" icon="$instructor"></v-icon>
            <strong>Instructor: </strong>{{ selectedEvent.FirstName }}  {{ selectedEvent.LastName }}
          </template>
        </v-card-item>
        <v-card-item v-if="user && isFutureEvent(selectedEvent.ClassDate, selectedEvent.StartTime)">
          <v-divider></v-divider>
        </v-card-item >
        <v-list-item v-if="user && isFutureEvent(selectedEvent.ClassDate, selectedEvent.StartTime)">
          <v-btn block v-if="!selectedAttendanceID" @click="signUp" size="small" color="#769cbc"> 
            Sign Up 
          </v-btn>
          <v-btn block v-if="selectedAttendanceID" @click="confirmRemove = true" variant="tonal" size="small"
          color="#769cbc"> 
            Remove From My Schedule
          </v-btn>
        </v-list-item>
        <v-dialog v-model="confirmRemove" persistent max-width="400px">
          <v-card>
            <v-card-title>Confirm Delete</v-card-title>
            <v-card-text>Are you sure you want to remove this class from your schedule? </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="#769cbc" variant="tonal"  text @click="confirmRemove = false">Cancel</v-btn>
              <v-btn color="#df2525" variant="tonal" text @click="removeFromSchedule">Confirm</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-card>
    </v-dialog>
    <v-snackbar v-model="confirmationDialog.show" timeout="4000">
      {{ confirmationDialog.text }}
      <template v-slot:actions>
        <v-btn color="#bfc5e8" variant="text" @click="confirmationDialog.show = false">
          Close
        </v-btn>
      </template>
    </v-snackbar>
</template>

<script>
  import FullCalendar from '@fullcalendar/vue3'
  import dayGridPlugin from '@fullcalendar/daygrid'
  import listPlugin from '@fullcalendar/list';
  import { useUserStore } from '@/stores/UserStore';
  import { useClassAttendanceStore } from '@/stores/ClassAttendanceStore';

  export default {
    props: ['YogaClasses'],
    components: {
      FullCalendar
    },
    watch: {
      YogaClasses: {
        immediate: true,
        handler(newVal) {
          this.calendarOptions.events = newVal;
          this.$nextTick(() => {
              this.refetchCalendarEvents();
            });
        }
      }
    },
    data() {
      return {
        user: null, 
        selectedAttendanceID: 0, 
        showClassInfo: false,
        selectedEvent: {},
        calendarOptions: {
          plugins: [listPlugin, dayGridPlugin],
          initialView: 'dayGridMonth',
          events: [], 
          eventClick: this.handleEventClick,
          headerToolbar: { left: 'prev,next', center: 'title', right: 'dayGridMonth,listWeek'},
          buttonText: { dayGridMonth: 'Month', listWeek: 'List' },
          contentHeight: "auto", eventColor: '#644874', eventDisplay: 'block', fixedWeekCount: false,
        }, 
        confirmationDialog: { text: '', show: false},
        confirmRemove: false, 
      }
    },
    methods: {
      refetchCalendarEvents() {
        let calendarApi = this.$refs.fullCalendar.getApi();
        calendarApi.refetchEvents();
      }, 
      async handleEventClick(clickInfo) {
        this.selectedEvent = clickInfo.event.extendedProps.yogaClass;

        const token = localStorage.getItem('userToken');
        if (token) {
          await useClassAttendanceStore().fetchAttendanceRecords(null, this.user.UserID, this.selectedEvent.ClassID, null, null);
          if ( useClassAttendanceStore().attendanceRecords[0]) { this.selectedAttendanceID = useClassAttendanceStore().attendanceRecords[0].AttendanceID; }
          else { this.selectedAttendanceID = 0; }
        }
        this.showClassInfo = true;
      }, 
      async userInfo() {
        const token = localStorage.getItem('userToken');
        if (token) {
          await useUserStore().getUser();
          this.user = useUserStore().user[0];
        }
      },
      async signUp() {
        await useClassAttendanceStore().addAttendanceRecord(this.user.UserID, this.selectedEvent.ClassID);
        this.showClassInfo = false;
        this.confirmationDialog = { text: 'Class successfully added to schedule', show: true};
      },
      async removeFromSchedule() {
        await useClassAttendanceStore().deleteAttendanceRecord(this.selectedAttendanceID);
        this.showClassInfo = false;
        this.confirmRemove = false;
        this.confirmationDialog = { text: 'Class successfully removed from schedule', show: true};
      },
      formatDate(dateString) {
        return new Date(dateString.substr(0, 19)).toLocaleDateString('en-US', { year: 'numeric',month: 'long', day: 'numeric' });
      }, 
      formatTime(timeString) {
        const [hours24, minutes] = timeString.split(':');
        const hours24Num = parseInt(hours24, 10);
        return `${hours24Num % 12 || 12}:${minutes.padStart(2, '0')} ${hours24Num >= 12 ? 'PM' : 'AM'}`;
      },
      isFutureEvent(classDate, startTime) {
        const startDate = new Date(classDate);
        const [hours, minutes] = startTime.split(':');
        let startTimeHours = parseInt(hours);
        const isPM = startTime.includes('PM');
        if (isPM && startTimeHours !== 12) { startTimeHours += 12; } 
        else if (!isPM && startTimeHours === 12) { startTimeHours = 0; }
        startDate.setHours(startTimeHours, parseInt(minutes), 0, 0);
        return startDate > new Date();
      },
    }, 
    mounted() {
      this.userInfo();
    }
  }
</script>

<style scoped>
.close {
  color: #aaa;
  float: right;
  font-size: 2rem;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}

.v-card {
  padding-bottom: 3%;
}

:deep(.fc-next-button), :deep(.fc-prev-button)
{
  background-color: white !important;
  border-color: white !important;
  color: grey !important;
  border-radius: 0rem;
}

:deep(.fc-button) {
  height: 2.5rem !important;
  outline: none  !important;
  box-shadow: 0 0 0 0px !important;
  border-color: white !important;
}

:deep(.fc-listWeek-button), :deep(.fc-dayGridMonth-button)
{
  border-color: white !important;
  background-color: #bfaec2;
  outline: none  !important;
  box-shadow: 0 0 0 0px !important;
}

:deep(.fc-listWeek-button:not(.fc-button-active):hover), :deep(.fc-dayGridMonth-button:not(.fc-button-active):hover)
{
  background-color: #493455 !important;
}

:deep(.fc-button-active) {
  background-color: #927396 !important; 
  color: white; 
  border-color: white !important; 
}

:deep(.fc-day-today) {
  background-color: #f6f1f4 !important;
}

:deep(.fc-icon) {
  padding: 0rem 1.75rem 2rem .25rem;
}

:deep(.fc-icon:hover) {
  color: black !important; 
  background-color: lightgrey;
  border-radius: 50%;
}
</style>