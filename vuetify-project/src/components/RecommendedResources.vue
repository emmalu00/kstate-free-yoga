<template>
    <v-card>
         <v-card-item title="Additional Resources">
            <v-card-subtitle> Recommended based on your favorites. </v-card-subtitle>
         </v-card-item>
         <v-list-item v-if="!recommendations.length"> 
            Sorry, no current recommendations.
        </v-list-item>
         <v-list-item v-for="recommendation in recommendations">
            <v-list-item-subtitle>
                <a :href="recommendation.link" target="_blank">{{ recommendation.name }}</a>
            </v-list-item-subtitle>
         </v-list-item>
    </v-card>
</template>

<script>

export default {
    props: ['favorites'],
    data() {
        return {
            recommendations: [], 
            keyWordsResources: [
                { name: 'Restorative Yoga', words: ['restorative', 'restful'], 
                    link: 'https://www.youtube.com/playlist?list=PLui6Eyny-UzxghGvVE7V_6YsZ7rh5r1Fx' },
                { name: 'Gentle Yoga Practices', words: ['gentle', 'slow'], 
                    link: 'https://www.youtube.com/playlist?list=PLui6Eyny-Uzzfg2uL--Z_2Ep2Is9kRInT' },
                { name: 'Stretching & Flexibility', words: ['stretch', 'deep'], 
                    link: 'https://www.youtube.com/playlist?list=PLui6Eyny-UzwZs3GBQeOoAp31-_IBq4Ky' },
                { name: 'Guided Meditations', words: ['relaxation', 'breathing'], 
                    link: 'https://www.youtube.com/playlist?list=PLui6Eyny-UzzG5qB0LNxyVh3Mu6GjYJC_' },
                { name: 'Intermediate Yoga Practices', words: ['intermediate', 'advanced', 'challenging'], 
                    link: 'https://www.youtube.com/playlist?list=PLui6Eyny-Uzw8CEkrdtRgxXAx8hPtQFpc' },
                { name: 'Various Full Yoga Practices', words: ['noontime'], 
                    link: 'https://www.youtube.com/playlist?list=PLui6Eyny-Uzwzd-9fi_cmhz3UW9gS1raf'}
            ],
            timeRangesResources: [
                { name: 'Morning Yoga Practices', start: 6, end: 10, link: 'https://www.youtube.com/playlist?list=PLui6Eyny-UzxMFVoPmxcPX1MOeLyV5uKQ'}, 
                { name: 'Evening Yoga Classes', start: 18, end: 21, link: 'https://www.youtube.com/playlist?list=PLui6Eyny-UzxlcWgUFYAcnNInS3SNe6Fs'},
            ],
        }
    },
    methods: {
        generateRecommendations() {
            this.favorites.forEach(favorite => {
                const classDate = new Date(favorite.ClassDate);
                        
                const thirtyDaysAgo = new Date();
                thirtyDaysAgo.setDate(thirtyDaysAgo.getDate());

                if (classDate >= thirtyDaysAgo) {
                    this.keyWordsResources.forEach(resource => {
                        resource.words.forEach(word => {
                            if (favorite.ClassName.toLowerCase().includes(word) || favorite.ClassDescription.toLowerCase().includes(word)) {
                                this.recommendations.push({ name: resource.name, link: resource.link });
                            }
                        });
                    });
                const startTimeHour = parseInt(favorite.StartTime.split(':')[0]);
                this.timeRangesResources.forEach(resource => {
                    if (startTimeHour >= resource.start && startTimeHour < resource.end) {
                        this.recommendations.push({ name: resource.name, link: resource.link });
                    }
                });
                }
            });
            const uniqueClassesMap = this.recommendations.reduce((map, obj) => {
                map.set(obj.name, obj);
                return map;
            }, new Map());
            this.recommendations = Array.from(uniqueClassesMap.values());
        }
        
    }, 
    mounted() {
        this.generateRecommendations();
    }      
}

</script>