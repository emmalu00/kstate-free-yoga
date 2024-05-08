/**
 * plugins/vuetify.js
 *
 * Framework documentation: https://vuetifyjs.com`
 */

// Styles
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

// Composables
import { createVuetify } from 'vuetify'
import { aliases, mdi } from 'vuetify/iconsets/mdi-svg'
import { mdiAccountCircleOutline, mdiCalendarBlank, 
        mdiClockOutline, mdiMapMarker, mdiMapMarkerPlus, mdiAccount, mdiAccountPlus,
        mdiText, mdiClose, mdiCheck, mdiHeart, mdiYoga, mdiTimerOutline, mdiOpenInNew} from '@mdi/js'

// https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides
export default createVuetify({
  icons: {
    defaultSet: 'mdi',
    aliases: {
      ...aliases,
      account: mdiAccountCircleOutline, 
      calendar: mdiCalendarBlank, 
      clock: mdiClockOutline, 
      location: mdiMapMarker, 
      addLocation: mdiMapMarkerPlus,
      instructor: mdiAccount, 
      addInstructor: mdiAccountPlus,
      description: mdiText, 
      no: mdiClose, 
      yes: mdiCheck, 
      heart: mdiHeart,
      yoga: mdiYoga,
      timer: mdiTimerOutline,
      newTab: mdiOpenInNew,
    },
    sets: {
      mdi,
    },
  },
})
