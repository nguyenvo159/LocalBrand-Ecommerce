import userService from '@/services/user.service';
import {createStore} from 'vuex';

const store = createStore({
    state() {
        return {
            token: null,
            user: null,
        }
    },
    mutations: {
        setToken(state, token) {
            state.token = token;
        },
        setUser(state, user) {
            state.user = user;
        },
        logOut(state) {
            state.token = null;
            state.user = null;
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
              } catch (error) {
                console.error('Failed to load user profile:', error);
              }
            }
          },
        async logout ({ commit }) {
            commit('logOut');
            localStorage.removeItem('token');
        }
    },
    getters: {
        isLogged(state) {
            return state.token && state.user;
        },
        getUser(state) {
            return state.user;
        },
        getToken(state) {
            return state.token;
        }
    }
})

export default store;