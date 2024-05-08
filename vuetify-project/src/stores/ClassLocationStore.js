import { defineStore } from 'pinia';
import api from '@/router/api';

export const useLocationsStore = defineStore('locations', {
  state: () => ({
    buildingsList: [
      { building: 'Anderson Hall', address: '919 Mid-Campus Dr, Manhattan, KS'},
      { building: "Alumni Center", address: "1720 Anderson Ave, Manhattan, KS" },
      { building: "Ahearn Field House", address: "919 N M.L.K. Jr. Dr, Manhattan, KS" },
      { building: "Ackert Hall", address: "1717 Claflin Rd, Manhattan, KS" },
      { building: "Chalmers Hall", address: "1711 Claflin Rd, Manhattan, KS" },
      { building: "Beach Art Museum", address: "701 Beach Ln, Manhattan, KS" },
      { building: "Bluemont Hall", address: "1114 Mid-Campus Dr, Manhattan, KS" },
      { building: "Burt Hall", address: "1220 N M.L.K. Jr. Dr, Manhattan, KS" },
      { building: "Bushnell Hall", address: "1615 Claflin Rd, Manhattan, KS" },
      { building: "Business Building", address: "1301 Lovers Lane, Manhattan, KS" },
      { building: "Calvin Hall", address: "802 Mid-Campus Dr, Manhattan, KS" },
      { building: "Chemistry/Biochemistry Building", address: "1212 Mid-Campus Dr, Manhattan, KS" },
      { building: "Call Hall", address: "1530 North Mid-Campus Drive, Manhattan KS" },
      { building: "Cardwell Hall", address: "1228 N M.L.K. Jr. Dr, Manhattan, KS" },
      { building: "Dickens Hall", address: "1116 Mid-Campus Dr North, Manhattan, KS" },
      { building: "Dole Hall", address: "1525 Mid-Campus Dr, Manhattan, KS" },
      { building: "Engineering Hall", address: "1701 Platt St, Manhattan, KS 66506" },
      { building: "English/Counseling Services", address: "1612 Steam Pl, Manhattan, KS" },
      { building: "Eisenhower Hall", address: "1013 Mid-Campus Dr, Manhattan, KS" },
      { building: "Campus Creek Complex", address: "1405 Campus Creek Rd, Manhattan, KS" },
      { building: "Fairchild Hall", address: "1601 Vattier Street, Manhattan, KS" },
      { building: "Holton Hall North", address: "1101 Mid-Campus Dr, Manhattan, KS" },
      { building: "Hale Library", address: "1117 Mid-Campus Drive North, Manhattan, KS" },
      { building: "Holtz Hall", address: "1005 Mid-Campus Dr, Manhattan, KS" },
      { building: "International Student Center", address: "1414 Mid-Campus Dr, Manhattan, KS" },
      { building: "Justin Hall", address: "1324 Lovers Ln, Manhattan, KS" },
      { building: "Kedzie Hall", address: "828 Mid-Campus Dr, Manhattan, KS" },
      { building: "King Hall", address: "1220 Mid-Campus Dr, Manhattan, KS" },
      { building: "Leasure Hall", address: "1128 N M.L.K. Jr. Dr, Manhattan, KS" },
      { building: "Leadership Studies Building", address: "1300 Mid-Campus Drive N., Manhattan, KS" },
      { building: "McCain Auditorium", address: "1501 Goldstein Circle, Manhattan, KS" },
      { building: "Nichols Hall", address: "702 Mid-Campus Dr, Manhattan, KS" },
      { building: "Chester E. Peters Recreation Complex", address: "1831 Olympic Dr, Manhattan, KS" },
      { building: "Seaton Hall", address: "920 N M.L.K. Jr. Dr, Manhattan, KS" },
      { building: "UFM Community Learning Center", address: "1221 Thurston St, Manhattan, KS" },
      { building: "Shellenberger Hall", address: "1301 Mid-Campus Dr, Manhattan, KS" },
      { building: "Thompson Hall", address: "1428 Anderson Ave, Manhattan, KS" },
      { building: "Throckmorton Plant Sciences Center", address: "1712 Claflin Rd, Manhattan, KS" },
      { building: "K-State Student Union", address: "918 N M.L.K. Jr. Dr, Manhattan, KS" },
      { building: "Willard Hall", address: "1211 Mid-Campus Dr North, Manhattan, KS" },
      { building: "Waters Hall", address: "1603 Old Claflin Rd, Manhattan, KS" },
      { building: "Weber Hall", address: "1424 Claflin Rd, Manhattan, KS" }
    ], 
    classLocations: [], 
    classLocationID: 0,
  }),
  actions: {
    async fetchLocations() 
    {
      try {
        const response = await api.get(`/classlocation`);
        this.classLocations = response.data;
      } 
      catch (error) { console.error('Error fetching yoga classes:', error); }
    },
    async addClassLocation(newClassLocation)
    {
      console.log(newClassLocation);
      try {
        const response = await api.post('/classlocation', newClassLocation);
        this.classLocationID = response.data;
        if (response.data) { console.log('Location added successfully:', response.data); }
      } 
      catch (error) { console.error('Error adding class location:', error); }
    },
  },
});