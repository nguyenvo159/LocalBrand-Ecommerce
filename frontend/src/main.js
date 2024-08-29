import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import store from './store/index.js'

import "bootstrap/dist/css/bootstrap.min.css";
import "@fortawesome/fontawesome-free/css/all.min.css";

import router from "./router"
createApp(App).use(store).use(router).mount('#app')
