<template>
    <section class="pt-3" style="background-color:#f1f3f7 ;">
        <div class="container">
            <div class="row">
                <div class="col-lg-4">
                    <div class="card">
                        <div class="card-body">
                            <div class="d-flex flex-column align-items-center text-center">
                                <img src="https://static.vecteezy.com/system/resources/previews/005/005/788/original/user-icon-in-trendy-flat-style-isolated-on-grey-background-user-symbol-for-your-web-site-design-logo-app-ui-illustration-eps10-free-vector.jpg"
                                    alt="Admin" class="rounded-circle p-1 bg-primary"
                                    style="width: 100px; height: 100px; object-fit: contain;">
                                <div class="mt-3 row justify-content-center">
                                    <h4>{{ user.name }}</h4>
                                    <p class="text-secondary mb-3">{{ user.role == 'User' ? 'Khách hàng' : 'Nhân viên'
                                        }}
                                    </p>
                                    <button class="btn btn-success w-75 mb-2">Đơn Hàng</button>
                                    <a class="btn btn-primary w-75 mb-2 text-white main-hover" href="#change-password">
                                        Đổi Mật Khẩu</a>
                                    <button @click="logout" class="btn btn-outline-danger w-75 mb-2">Đăng Xuất</button>
                                </div>
                            </div>
                            <hr class="my-4">
                            <ul class="list-group list-group-flush">
                                <h5>Liên hệ chúng tôi</h5>
                                <li class="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                                    <h6 class="mb-0"><i class="fa-brands fa-facebook mr-2" style="font-size: 24px;"></i>
                                        Facebook
                                    </h6>
                                    <a href="https://www.facebook.com/nguyenph2212/" target="_blank"
                                        class="text-secondary main-hover">fb.com/nguyenph2212</a>
                                </li>
                                <li class="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                                    <h6 class="mb-0"><i class="fa-solid fa-phone mr-2" style="font-size: 24px;"></i>
                                        Hotline
                                    </h6>
                                    <span class="text-secondary">0763962680</span>
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

                    <!-- Password -->
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="d-flex align-items-center mb-3"><b>Đổi Mật Khẩu</b></h5>
                                    <Form @submit="changePassword" ref="passwordForm"
                                        :validation-schema="passwordSchema">
                                        <div class="row mb-3">
                                            <div class="col-sm-3 d-flex align-items-center">
                                                <h6 class="mb-0">Mật Khẩu Cũ</h6>
                                            </div>
                                            <div class="col-sm-9 text-secondary">
                                                <Field type="password" name="password" class="form-control"
                                                    @focus="clearFieldError('password')" />
                                                <ErrorMessage name="password" class="text-danger" />
                                            </div>
                                        </div>
                                        <div class="row mb-3">
                                            <div class="col-sm-3 d-flex align-items-center">
                                                <h6 class="mb-0">Mật Khẩu Mới</h6>
                                            </div>
                                            <div class="col-sm-9 text-secondary">
                                                <Field type="password" name="newPassword" class="form-control"
                                                    @focus="clearFieldError('newPassword')" />
                                                <ErrorMessage name="newPassword" class="text-danger" />
                                            </div>
                                        </div>
                                        <div class="row mb-3">
                                            <div class="col-sm-3 d-flex align-items-center">
                                                <h6 class="mb-0">Nhập Lại Mật Khẩu</h6>
                                            </div>
                                            <div class="col-sm-9 text-secondary">
                                                <Field type="password" name="rePassword" class="form-control"
                                                    @focus="clearFieldError('rePassword')" />
                                                <ErrorMessage name="rePassword" class="text-danger" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-3"></div>
                                            <div class="col-sm-9 text-secondary">
                                                <button type="submit" class="btn btn-primary px-4">Đổi Mật Khẩu</button>
                                            </div>
                                        </div>
                                    </Form>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
import * as yup from 'yup';
import { Form, Field, ErrorMessage } from 'vee-validate';
import UserService from '@/services/user.service';

export default {
    components: {
        Form, Field, ErrorMessage
    },
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
        clearFieldError(fieldName) {
            this.$refs.passwordForm.setFieldError(fieldName, null);
        },
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
        async changePassword(values) {
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
                    oldPassword: values.password,
                    newPassword: values.newPassword,
                });
                loader.hide();
                this.refresh();
                this.$refs.passwordForm.resetForm();
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
    setup() {
        const passwordSchema = yup.object().shape({
            password: yup.string().required('Mật khẩu cũ là bắt buộc')
                .min(6, 'Mật khẩu mới phải có ít nhất 6 ký tự'),

            newPassword: yup.string()
                .required('Mật khẩu mới là bắt buộc')
                .min(6, 'Mật khẩu mới phải có ít nhất 6 ký tự'),
            rePassword: yup.string()
                .oneOf([yup.ref('newPassword'), null], 'Mật khẩu nhập lại không khớp')
                .required('Xác nhận mật khẩu là bắt buộc')
        });

        return {
            passwordSchema
        };
    }
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