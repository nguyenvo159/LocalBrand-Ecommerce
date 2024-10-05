import userService from '@/services/user.service';
import productService from '@/services/product.service';
import categoryService from '@/services/category.service';
import cartService from '@/services/cart.service';

import { set } from 'date-fns';
import {createStore} from 'vuex';

const store = createStore({
    state() {
        return {
            token: null,
            user: null,
            searchResults: [],
            users: [],
            products: [],
            categories: [],
            cart: null,
        }
    },
    mutations: {
        setSearchResults(state, results) {
            state.searchResults = results;
        },
        setToken(state, token) {
            state.token = token;
        },
        setUser(state, user) {
            state.user = user;
        },
        setUsers(state, users) {
            state.users = users;
        },
        setProducts(state, products) {
            state.products = products;
        },
        setCategories(state, categories) {
            state.categories = categories;
        },
        setCart(state, cart) {
            state.cart = cart;
        },
        logOut(state) {
            state.token = null;
            state.user = null;
            state.cart = null;
        }
    },
    actions: {
        async loadUser({ commit }) {
            const token = localStorage.getItem('token');
            if (token) {
              commit('setToken', token);
              try {
                const userProfile = await userService.profile();
                commit('setUser', userProfile);
                if(userProfile){
                    const cart = await cartService.getByUserId(userProfile.id);
                    commit('setCart', cart);
                }
              } catch (error) {
                console.error('Failed to load user profile:', error);
              }
            }
        },
        async fillUsers({ commit }) {
            const users = await userService.getAll();
            users.sort((a, b) => a.role.localeCompare(b.role));
            commit('setUsers', users);
        },
        async fillProducts({ commit }) {
            const products = await productService.getAll();
            commit('setProducts', products);
        },
        async fillCategories({ commit }) {
            const categories = await categoryService.getAll();
            categories.sort((a, b) => a.name.localeCompare(b.name));
            commit('setCategories', categories);
        },
        async fillCart({ commit }) {
            if(this.getters.isLogged) {
                const cart = await cartService.getByUserId(this.state.user.id);
                commit('setCart', cart);
            }
        },
        async logout ({ commit }) {
            commit('logOut');
            localStorage.removeItem('token');
        }
    },
    getters: {
        isLogged(state) {
            return !!state.token && !!state.user;
        },
        isAdmin(state) {
            return state.user && (state.user.role === 'Admin' || state.user.role === 'Staff');
        },
        getSearchResults(state) {
            return state.searchResults;
        },
        getUser(state) {
            return state.user;
        },
        getToken(state) {
            return state.token;
        },
        getUsers(state) {
            return state.users;
        },
        getProducts(state) {
            return state.products;
        },
        getCategories(state) {
            return state.categories;
        },
        getCart(state) {
            return state.cart;
        }
    }
})

export default store;