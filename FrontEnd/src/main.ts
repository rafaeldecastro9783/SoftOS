import './assets/main.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia';
import router from 'plugins/router';
import vuetify from 'plugins/vuetify';
import axios from 'plugins/axios';

import App from './App.vue';

const app = createApp(App);

app.use(createPinia());
app.use(axios);
app.use(vuetify);
app.use(router);

app.mount('#app');
