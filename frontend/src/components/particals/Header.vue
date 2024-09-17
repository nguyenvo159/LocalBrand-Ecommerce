<template>
    <div v-if="!isAdmin" class="" style="padding-bottom: 72px;">
        <nav class="navbar navbar-expand-lg navbar-light bg-white py-3 px-4 fixed-top border-bottom shadow-sm">
            <div class="row justify-content-center w-100">
                <div class="container">
                    <a class="navbar-brand d-flex justify-content-between align-items-center order-lg-0" href="/"
                        style="letter-spacing: 10px;">
                        <span class="text-uppercase fw-lighter ms-2"><b>AMIE</b></span>
                    </a>
                    <div class="order-lg-2 nav-btns">
                        <button type="button" class="btn position-relative">
                            <i class="fa fa-search" @click="search"></i>
                        </button>
                        <button type="button" class="btn btn-click position-relative">
                            <i class="fa fa-shopping-cart"></i>
                            <span class="position-absolute rounded-circle top-10 translate-middle badge bg-primary">{{
                                itemCart }}</span>
                        </button>
                        <button type="button" class="btn position-relative">
                            <router-link v-if="!isLogged" class="text-dark" :to="{ name: 'Login' }">
                                <i class="fa fa-user"></i>
                            </router-link>
                            <router-link v-else class="text-dark" :to="{ name: 'Register' }">
                                <i class="fa fa-user"></i>
                            </router-link>
                        </button>

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
                                <a class="nav-link text-uppercase text-dark nav-text" href="#special">specials</a>
                            </li>
                            <li class="nav-item px-2 py-1">
                                <router-link :to="{ name: 'About' }" class="nav-link text-uppercase text-dark nav-text"
                                    href="#blogs">About us</router-link>
                            </li>
                            <li class="nav-item px-2 py-1 border-0">
                                <router-link :to="{ name: 'Contact' }"
                                    class="nav-link text-uppercase text-dark nav-text">Contact</router-link>
                            </li>
                            <li class="nav-item px-2 py-1">
                                <router-link :to="{ name: 'UserManage' }"
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
export default {
    components: {
        SearchEngine,
    },
    data() {
        return {
            isSearch: false,
            isAdmin: false,
            itemCart: 0,
        };
    },
    watch: {
        $route(to, from) {
            this.isAdmin = to.path.includes('admin');
        },
    },
    computed: {
        isLogged() {
            return this.$store.getters.isLogged;
        },
    },
    mounted() {
        this.isAdmin = this.$route.path.includes('admin');
    },
    methods: {
        search() {
            this.isSearch = !this.isSearch;
        },
    },
}
</script>

<style scoped>
.btn:focus {
    outline: none !important;
    box-shadow: none !important;
}

.nav-text:hover {
    color: #f8b500 !important;
}
</style>