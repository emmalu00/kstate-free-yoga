<template>
    <v-sheet class="pa-2 ma-3">
        <v-btn block @click="showAddForm = true"> Add Class </v-btn>
    </v-sheet>
</template>

<script>
export default {
    data() {
        return {
            classInfo: null, 
            showUpdateFormDialog: false,
        }
    }, 
    methods: {
        openAddForm
    }

}
</script>