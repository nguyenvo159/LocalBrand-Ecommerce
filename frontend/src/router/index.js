import { createWebHistory, createRouter } from "vue-router";
import store from "@/store/index.js";
import Home from "@/views/Home.vue";
import Search from "@/views/Search.vue";

//Admin
import UserManage from "@/views/admin/UserManage.vue";
import ProductManage from "@/views/admin/ProductManage.vue";
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
    path: "/search",
    name: "Search",
    component: Search,
    props: (route) => ({ keyword: route.query.keyword, results: route.query.results }),
  },
  
  {
    path: "/admin/user",
    name: "UserManage",
    component: UserManage,
    meta: {requiresAuth: true, requiredRole: ['Admin']},
  },  
  
  {
    path: "/admin/product",
    name: "ProductManage",
    component: ProductManage,
    meta: {requiresAuth: true, requiredRole: ['Admin']},
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

router.beforeEach((to, from, next) => {
  const isLogged = store.getters.isLogged;
  const userRole = store.getters.getUser? store.getters.getUser.role : null;

  //Check isLogged
  if(to.meta.requiresAuth ) {
    if(!isLogged) {
      next({name: 'Login'});
    } else {
      //Check Role
      if (to.matched.some(record => record.meta.requiredRole)){
        const requiredRole = to.meta.requiredRole;
        if (!requiredRole.includes(userRole)){
          next({name: 'Home'});
        }
      }
    }
  }
  next();
});

export default router;
