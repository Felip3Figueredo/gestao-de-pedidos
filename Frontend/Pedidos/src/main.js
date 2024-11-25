import { createApp, reactive } from 'vue';
import '@fortawesome/fontawesome-free/css/all.css';
import '@fortawesome/fontawesome-free/js/all.js';
import App from './App.vue';
import router from './router';
import configureLoading  from './services/api'; 
import LoadingSpinner from './components/shared/LoadingSpinner.vue';

const globalState = reactive({ isLoading: false });

const app = createApp(App);

app.provide('isLoading', globalState.isLoading);
app.provide('setLoading', (value) => (globalState.isLoading = value));
app.component('LoadingSpinner', LoadingSpinner);

configureLoading((value) => (globalState.isLoading = value));

app.use(router);

app.mount('#app');
