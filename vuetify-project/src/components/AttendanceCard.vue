<template>
  <v-card v-for="event in eventList" :key="event.AttendanceID" variant="flat" color="white">
    <v-card-item :title="event.ClassName"> 
      <template v-slot:append>
        <v-btn :class="event.Favorited ? 'text-red' : 'text-grey'" icon="$heart" variant="text"
          @click="updateFavorited(event.AttendanceID, event.Favorited)">
        </v-btn>
        </template>
      <template v-slot:subtitle>
        <v-icon class="me-1 pb-1" color="black" icon="$clock" size="18"></v-icon>
        {{ formatDate(event.ClassDate) }} | {{ formatTime(event.StartTime) }} - {{ formatTime(event.EndTime) }} 
      </template>
    </v-card-item>
    <v-list-item>
      <template v-slot:title>
        <v-icon class="me- pb-1" color="black" icon="$location" size="18"></v-icon>
        {{ event.BuildingName}} - {{ event.RoomNumber }}
      </template>
      <v-tooltip text="Open In Google Maps" location="bottom">
        <template v-slot:activator="{ props }">
          <a :href="`https://maps.google.com/?q=${encodeURIComponent(event.BuildingName)}+${encodeURIComponent(event.LocationAddress)}`" 
          target="_blank" style="color: inherit; text-decoration: none;" v-bind="props">
            {{ event.LocationAddress }}
          </a>
        </template>
      </v-tooltip>
    </v-list-item>
    <v-list-item> Class Instructor: {{ event.FirstName }} {{ event.LastName }} </v-list-item>
    <v-list-item>
      <template v-slot:prepend>
        <v-icon v-if="event.MatsProvided" class="me-1 pb-1" color="#87ab87" icon="$yes" size="18"></v-icon>
        <v-icon v-else class="me-1 pb-1" color="#df2525" icon="$no" size="18"></v-icon>
        Mats Provided
      </template>
      <template v-slot:append>
        <v-btn @click="confirmRemove = true; this.selectedAttendanceID = event.AttendanceID;" v-if="listInfo === 'upcoming'"
        size="small" density="compact" variant="tonal">
          Remove From Schedule
        </v-btn>
        <v-btn @click="confirmRemove = true; this.selectedAttendanceID = event.AttendanceID;" v-if="listInfo === 'previous'"
        size="small" density="compact" variant="tonal">
          I did not attend this class
        </v-btn>
      </template>
    </v-list-item>
    <v-dialog v-model="confirmRemove" persistent max-width="400px">
        <v-card>
          <v-card-title>Confirm Remove</v-card-title>
          <v-card-text>Are you sure you want to remove this class from your schedule? </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="#769cbc" variant="tonal"  text @click="confirmRemove = false">Cancel</v-btn>
            <v-btn color="#df2525" variant="tonal" text @click="removeClass(selectedAttendanceID)">Confirm</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
  </v-card>
</template>

<script>

export default {
    props: ['eventList', 'listInfo'], 
    emits: ['removeClass', 'updateFavorites'],
    data() {
        return {
            confirmRemove: false,
            selectedAttendanceID: -1,
        }
    }, 
    methods: {
        async removeClass(id) {
          console.log(id)
          const index = this.eventList.findIndex(e => e.AttendanceID === id);
          this.eventList.splice(index, 1);
          this.confirmRemove = false;
          this.$emit('removeClass', id);
        }, 
        async updateFavorited(id, favorited) 
        {
          const classToUpdate = this.eventList.find(c => c.AttendanceID === id);
          if (classToUpdate) {classToUpdate.Favorited = !favorited; }
          console.log(classToUpdate);
          this.$emit('updateFavorited', classToUpdate);
        },
        formatDate(dateString) {
          return new Date(dateString.substr(0, 19)).toLocaleDateString('en-US', {year: 'numeric',  month: 'long',   day: 'numeric' });
        }, 
        formatTime(timeString) {
          const [hours24, minutes] = timeString.split(':');
          const hours24Num = parseInt(hours24, 10);
          return `${hours24Num % 12 || 12}:${minutes.padStart(2, '0')} ${hours24Num >= 12 ? 'PM' : 'AM'}`;
      },
    }
}

</script>