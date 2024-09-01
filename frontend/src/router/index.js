import { createWebHistory, createRouter } from "vue-router";
import Home from "@/views/Home.vue";

//Admin
import UserManager from "@/views/admin/UserManager.vue";
import Login from "@/views/auth/Login.vue";
import Register from "@/views/auth/Register.vue";
import Product from "@/views/Product.vue";

const routes = [
  {
    path: "/",
    name: "Home",
    component: Home,
  },
  
  {
    path: "/admin/user",
    name: "UserManager",
    component: UserManager,
  },
  
  {
    path: "/auth/login",
    name: "Login",
    component: Login,
  },

  {
    path: "/auth/register",
    name: "Register",
    component: Register,
  },

  {
    path: "/product",
    name: "Product",
    component: Product,
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
