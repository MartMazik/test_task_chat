import {createRouter, createWebHistory} from 'vue-router';
import Sender from '../components/Sender.vue';
import Socket from '../components/Socket.vue';
import History from '../components/History.vue';

const routes = [
    {path: '/', component: Sender},
    {path: '/socket', component: Socket},
    {path: '/history', component: History},
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;
