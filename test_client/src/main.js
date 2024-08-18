import {createApp} from 'vue';
import App from './App.vue';
import router from './router';
import 'bootstrap/dist/css/bootstrap.min.css'; // Импорт стилей Bootstrap
import {BootstrapVue3} from 'bootstrap-vue-3'; // Импорт BootstrapVue

const app = createApp(App);
app.use(router);
app.use(BootstrapVue3);  // Подключение BootstrapVue
app.mount('#app');
