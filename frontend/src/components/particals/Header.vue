<template>
    <div v-if="!isAdminRoute" class="" style="padding-bottom: 72px;">
        <nav class="navbar navbar-expand-lg navbar-light bg-white py-3 px-4 fixed-top border-bottom shadow-sm">
            <div class="row justify-content-center w-100">
                <div class="container">
                    <a class="navbar-brand d-flex justify-content-between align-items-center order-lg-0" href="/"
                        style="letter-spacing: 10px;">
                        <span class="text-uppercase fw-lighter ms-2"><b>AMIRI</b></span>
                    </a>
                    <div class="order-lg-2 nav-btns">
                        <button type="button" class="btn position-relative">
                            <i class="fa fa-search" @click="search"></i>
                        </button>
                        <button type="button" class="btn btn-click position-relative">
                            <router-link class="text-dark" :to="{ name: 'Cart' }">
                                <i class="fa-solid fa-bag-shopping"></i>
                            </router-link>
                            <span class="position-absolute rounded-circle top-10 translate-middle badge bg-primary">{{
                                itemCart }}</span>
                        </button>
                        <div class="btn position-relative">
                            <router-link v-if="!isLogged" class="text-dark" :to="{ name: 'Login' }">
                                <i class="fa fa-user"></i>
                            </router-link>
                            <div v-else class="dropdown">
                                <a class="btn dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                    {{ user.name }}
                                </a>
                                <div class="dropdown-menu dropdown-menu-right mt-4 p-0 border-0 shadow rounded-0">
                                    <router-link :to="{ name: 'Profile' }" class="dropdown-item py-2 ">
                                        <i class="fa-solid fa-user mr-2"></i>
                                        Thông tin cá nhân</router-link>
                                    <router-link :to="{ name: 'OrderList' }" class="dropdown-item py-2">
                                        <i class="fa-solid fa-receipt mr-2"></i>
                                        Đơn hàng
                                    </router-link>
                                    <a class="dropdown-item py-2" @click="logout">
                                        <i class="fa-solid fa-arrow-right-from-bracket mr-2"></i>
                                        Đăng xuất</a>
                                </div>
                            </div>
                        </div>

                    </div>

                    <button class="navbar-toggler border-0" type="button" data-toggle="collapse" data-target="#navMenu">
                        <span class="navbar-toggler-icon"></span>
                    </button>

                    <div class="collapse navbar-collapse order-lg-1" id="navMenu">
                        <ul class="navbar-nav mx-auto">
                            <li class="nav-item px-2 py-1">
                                <router-link :to="{ name: 'Home' }"
                                    class="nav-link text-uppercase text-dark nav-text">HOME</router-link>
                            </li>
                            <li class="nav-item px-2 py-1">
                                <router-link :to="{ name: 'ProductList', params: { category: 'all-collection' } }"
                                    class="nav-link text-uppercase text-dark nav-text">collection</router-link>
                            </li>
                            <li class="nav-item px-2 py-1">
                                <router-link :to="{ name: 'ProductSpecial', params: { category: 'all-collection' } }"
                                    class="nav-link text-uppercase text-dark nav-text" href="#special">
                                    <span class="fire-icon">
                                        <i class="fa-solid fa-fire"></i>
                                    </span> specials</router-link>

                            </li>
                            <li class="nav-item px-2 py-1">
                                <router-link :to="{ name: 'About' }" class="nav-link text-uppercase text-dark nav-text"
                                    href="#blogs">About us</router-link>
                            </li>
                            <li class="nav-item px-2 py-1 border-0">
                                <router-link :to="{ name: 'Contact' }"
                                    class="nav-link text-uppercase text-dark nav-text">Contact</router-link>
                            </li>
                            <li v-if="isAdmin" class="nav-item px-2 py-1">
                                <router-link :to="{ name: 'CustumerManage' }"
                                    class="nav-link text-uppercase text-dark nav-text">Admin</router-link>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="row">
                    <SearchEngine v-if="isSearch" @image-search-complete="search" />
                </div>
            </div>
        </nav>

    </div>
</template>

<script>
import SearchEngine from '@/components/SearchEngine.vue';
import { isAfter } from 'date-fns';
export default {
    components: {
        SearchEngine,
    },
    data() {
        return {
            isSearch: false,
            isAdminRoute: false,
        };
    },
    watch: {
        $route(to, from) {
            this.isAdminRoute = to.path.includes('admin');
        },
    },
    computed: {
        isLogged() {
            return this.$store.getters.isLogged;
        },
        isAdmin() {
            return this.$store.getters.isAdmin;
        },
        user() {
            return this.$store.getters.getUser;
        },
        itemCart() {
            if (this.$store.getters.getCart) {
                return this.$store.getters.getCart.cartItems.length;
            }
            return 0;
        }

    },
    mounted() {
        this.isAdminRoute = this.$route.path.includes('admin');
    },
    methods: {
        search() {
            this.isSearch = !this.isSearch;
        },
        logout() {
            this.$store.dispatch('logout');
            this.$router.push({ name: 'Home' });
        },
    },
}
</script>

<style scoped>
.btn {
    outline: none !important;
    box-shadow: none !important;
    border: none !important;
}

.nav-text:hover {
    color: #f8b500 !important;
}

i {
    font-size: 20px;
}

.dropdown-item>i {
    font-size: 15px;
}
</style>