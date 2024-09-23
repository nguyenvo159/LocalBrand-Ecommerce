<template>
    <div class="container mt-4">
        <div class="row">
            <div class="col-lg-4">
                <div class="card">
                    <div class="card-body">
                        <div class="d-flex flex-column align-items-center text-center">
                            <img src="https://bootdey.com/img/Content/avatar/avatar6.png" alt="Admin"
                                class="rounded-circle p-1 bg-primary"
                                style="width: 100px; height: 100px; object-fit: contain;">
                            <div class="mt-3 row justify-content-center">
                                <h4>{{ user.name }}</h4>
                                <p class="text-secondary mb-3">{{ user.role == 'User' ? 'Khách hàng' : 'Nhân viên' }}
                                </p>
                                <button class="btn btn-success w-75 mb-2">Đơn Hàng</button>
                                <a class="btn btn-primary w-75 mb-2 text-white main-hover" href="#change-password">
                                    Đổi Mật Khẩu</a>
                                <button @click="logout" class="btn btn-outline-danger w-75 mb-2">Đăng Xuất</button>
                            </div>
                        </div>
                        <hr class="my-4">
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                                <h6 class="mb-0"><i class="fa-solid fa-globe" style="font-size: 24px;"></i> Website</h6>
                                <span class="text-secondary">abc.com</span>
                            </li>
                            <li class="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                                <h6 class="mb-0"><i class="fa-brands fa-github" style="font-size: 24px;"></i> Github
                                </h6>
                                <span class="text-secondary">bootdey</span>
                            </li>

                        </ul>
                    </div>
                </div>
            </div>
            <div class="col-lg-8">
                <div class="card">
                    <div class="card-body">
                        <div class="row mb-3">
                            <div class="col-sm-2 d-flex align-items-center">
                                <h6 class="mb-0">Full Name</h6>
                            </div>
                            <div class="col-sm-9 text-secondary">
                                <input type="text" class="form-control" v-model="user.name">
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-sm-2 d-flex align-items-center">
                                <h6 class="mb-0">Email</h6>
                            </div>
                            <div class="col-sm-9 text-secondary">
                                <input type="text" class="form-control" v-model="user.email">
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-sm-2 d-flex align-items-center">
                                <h6 class="mb-0">Phone</h6>
                            </div>
                            <div class="col-sm-9 text-secondary">
                                <input type="text" class="form-control" v-model="user.phone">
                            </div>
                        </div>
                        <div id="change-password"></div>
                        <div class="row mb-3">
                            <div class="col-sm-2 d-flex align-items-center">
                                <h6 class="mb-0">Address</h6>
                            </div>
                            <div class="col-sm-9 text-secondary">
                                <input type="text" class="form-control" value="Hưng Lợi, Ninh Kiều, Cần Thơ">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2"></div>
                            <div class="col-sm-9 text-secondary">
                                <button @click="update" type="button" class="btn btn-primary px-4">Lưu thay đổi
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="card">
                            <div class="card-body">
                                <h5 class="d-flex align-items-center mb-3"><b>Đổi Mật Khẩu</b></h5>
                                <div class="row mb-3">
                                    <div class="col-sm-3 d-flex align-items-center">
                                        <h6 class="mb-0">Mật Khẩu Cũ</h6>
                                    </div>
                                    <div class="col-sm-9 text-secondary">
                                        <input type="password" v-model="password" class="form-control">
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="col-sm-3 d-flex align-items-center">
                                        <h6 class="mb-0">Mật Khẩu Mới</h6>
                                    </div>
                                    <div class="col-sm-9 text-secondary">
                                        <input type="password" v-model="newPassword" class="form-control">
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="col-sm-3 d-flex align-items-center">
                                        <h6 class="mb-0">Nhập Lại Mật Khẩu</h6>
                                    </div>
                                    <div class="col-sm-9 text-secondary">
                                        <input type="password" v-model="rePassword" class="form-control">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3"></div>
                                    <div class="col-sm-9 text-secondary">
                                        <button @click="changePassword" type="button" class="btn btn-primary px-4">
                                            Đổi Mật Khẩu
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import UserService from '@/services/user.service';
import { ref } from 'vue';

export default {
    data() {
        return {
            password: '',
            newPassword: '',
            rePassword: '',
        };
    },
    computed: {
        user() {
            return this.$store.getters.getUser;
        }
    },
    methods: {
        async logout() {
            this.$store.dispatch('logout');
            this.$router.push({ name: 'Home' });
        },
        async refresh() {
            this.user = await UserService.profile();
            this.$store.commit('setUser', this.user);
        },
        async update() {
            let loader = this.$loading;
            try {
                loader = loader.show({
                    container: null,
                    canCancel: true,
                    color: '#808EF4',
                    loader: 'dots',
                    width: 100,
                    height: 100,
                });
                await UserService.update(this.user);
                loader.hide();
                this.refresh();
            } catch (error) {
                loader.hide();

                console.error('Lỗi khi cập nhật thông tin cá nhân:', error);
            } finally {
                loader.hide();
            }
        },
        async changePassword() {
            let loader = this.$loading;
            try {
                loader = loader.show({
                    container: null,
                    canCancel: true,
                    color: '#808EF4',
                    loader: 'dots',
                    width: 100,
                    height: 100,
                });
                let r = await UserService.changePassword({
                    oldPassword: this.password,
                    newPassword: this.newPassword,
                });
                loader.hide();
                this.refresh();
            } catch (error) {
                loader.hide();
                alert('Lỗi khi cập nhật mật khẩu:', error.response.data.message);
            } finally {
                loader.hide();
            }
        }
    },
    async mounted() {
        window.scrollTo(0, 0);
        if (!this.$store.getters.isLogged) {
            this.$router.push({ name: 'Login' });
        } else {
            this.refresh();
        }
    },
};
</script>

<style scoped>
.card {
    position: relative;
    display: flex;
    flex-direction: column;
    min-width: 0;
    word-wrap: break-word;
    background-color: #fff;
    background-clip: border-box;
    border: 0 solid transparent;
    border-radius: .25rem;
    margin-bottom: 1.5rem;
    box-shadow: 0 2px 6px 0 rgb(218 218 253 / 65%), 0 2px 6px 0 rgb(206 206 238 / 54%);
}

.me-2 {
    margin-right: .5rem !important;
}
</style>