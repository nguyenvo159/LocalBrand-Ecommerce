import './assets/main.css'
import {LoadingPlugin} from 'vue-loading-overlay';
import 'vue-loading-overlay/dist/css/index.css';
import { createApp } from 'vue'
import App from './App.vue'
import store from './store/index.js'

import "bootstrap/dist/css/bootstrap.min.css";
import "@fortawesome/fontawesome-free/css/all.min.css";

import router from "./router"
createApp(App)
.use(LoadingPlugin)
.use(store)
.use(router)
.mount('#app')
