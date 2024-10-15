import { createWebHistory, createRouter } from "vue-router";
import store from "@/store/index.js";

import Home from "@/views/Home.vue";
import Search from "@/views/Search.vue";
import Contact from "@/views/Contact.vue";
import About from "@/views/AboutUs.vue";
import NotFound404 from "@/views/NotFound404.vue";
import ProductDetail from "@/views/product/ProductDetail.vue";
import ProductList from "@/views/product/ProductList.vue";

import Cart from "@/views/cart/Cart.vue";
import Payment from "@/views/cart/Payment.vue";
import Profile from "@/views/auth/Profile.vue";
import OrderList from "@/views/order/OrderList.vue";
import OrderDetail from "@/views/order/OrderDetail.vue";

//Admin
import UserManage from "@/views/admin/UserManage.vue";
import ProductManage from "@/views/admin/ProductManage.vue";
import OrderManage from "@/views/admin/OrderManage.vue";

import Login from "@/views/auth/Login.vue";
import Register from "@/views/auth/Register.vue";
const routes = [
    
  //Admin
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
    path: "/admin/order",
    name: "OrderManage",
    component: OrderManage,
    // meta: {requiresAuth: true, requiredRole: ['Admin']},
  },
  // End - Admin

  // Guess
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
    path: "/contact",
    name: "Contact",
    component: Contact,
  },
  {
    path: "/about",
    name: "About",
    component: About,
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
    path: "/not-found",
    name: "NotFound404",
    component: NotFound404,
  },

  //Product
  {
    path: "/product/detail/:id",
    name: "ProductDetail",
    component: ProductDetail,
  },

  {
    path: "/product/:category",
    name: "ProductList",
    component: ProductList,
  },

  //Authorized
  {
    path: "/cart",
    name: "Cart",
    component: Cart,
    meta: {requiresAuth: true},
  },

  {
    path: "/payment",
    name: "Payment",
    component: Payment,
    meta: {requiresAuth: true},
  },

  {
    path: "/profile",
    name: "Profile",
    component: Profile,
    meta: {requiresAuth: true},
  },

  {
    path: "/order",
    name: "OrderList",
    component: OrderList,
  },

  {
    path: "/order/:id",
    name: "OrderDetail",
    component: OrderDetail,
    // meta: {requiresAuth: true},
  }



];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token');
  if (token && !store.getters.isLogged) {
      store.dispatch('loadUser');
  }

  const isLogged = store.getters.isLogged;
  const userRole = store.getters.getUser? store.getters.getUser.role : null;

  //Check isLogged and role
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
