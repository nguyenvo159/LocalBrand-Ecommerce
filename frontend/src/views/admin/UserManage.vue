<template>
    <div class="container-fluid" style="height: 100vh;">
        <div class="row">
            <DashBoard type="User" />
            <div id="dv" class="col-lg-10 offset-lg-2 col-11 offset-1 admin-content">
                <h3 class="mb-4 text-uppercase">Quản lý Người Dùng </h3>
                <div class="d-flex justify-content-between mb-3">
                    <div>
                        <button class="btn btn-primary mb-2" data-toggle="modal" data-target="#create-user">
                            <i class="fa-solid fa-user-plus"></i> Thêm tài khoản</button>

                        <button class="btn ml-2 text-dark" style="box-shadow: none;" @click="refreshList()">
                            <i class=" fa-solid fa-rotate-right" style="font-size: 20px;"></i></button>
                    </div>
                    <div class="w-25">
                        <SearchInput v-model="searchText" />

                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12 mb-3">
                        <div class="card">
                            <div class="card-header">
                                <span><i class="bi bi-table me-2"></i></span> Data Table
                            </div>
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table table-hover table-striped bg-white">
                                        <thead class="">
                                            <tr>
                                                <th class="border-0 align-middle text-center">STT</th>
                                                <th class="border-0 align-middle">Tên</th>
                                                <th class="border-0 align-middle">Email</th>
                                                <th class="border-0 align-middle">Số điện thoại</th>
                                                <th class="border-0 align-middle text-center">Phân Quyền</th>
                                                <th class="border-0 align-middle text-center">Thao tác</th>
                                            </tr>
                                        </thead>
                                        <div v-if="users.length == 0">No user available.</div>
                                        <tbody v-else>

                                            <tr v-for="(user, index) in filteredUsers" :key="user.id"
                                                class="product-item">
                                                <td class="align-middle text-center">{{ index + 1 }}</td>
                                                <td class="align-middle">{{ user.name }}</td>
                                                <td class="align-middle">{{ user.email }}</td>
                                                <td class="align-middle">{{ user.phone }}</td>
                                                <td class="align-middle text-center">{{ user.role }}</td>
                                                <td class="align-middle">
                                                    <div class="d-md-flex d-sm-block justify-content-evenly">
                                                        <a class="cursor-pointer" data-toggle="modal"
                                                            data-target="#update-user" @click="confirmUpdate(user)"
                                                            style="font-size: 20px;"><i
                                                                class="fa-solid fa-pen-to-square"></i></a>
                                                        <a class="cursor-pointer" data-toggle="modal"
                                                            data-target="#delete-user"
                                                            @click="confirmDelete(user.name, user.id)"
                                                            style="font-size: 20px; color: red;"><i
                                                                class="fa-solid fa-trash"></i></a>
                                                    </div>
                                                </td>
                                            </tr>

                                        </tbody>

                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Sửa thông tin User -->
                <!-- <InputUser :user="user" @submit:user="updateUser" @close="closeModal" title="Cập Nhật Tài Khoản" -->
                <!-- modalId="update-user" /> -->
                <UserCreateUpdate modalId="update-user" :user="user" @submit:user="updateUser" @close="closeModal"
                    title="Cập Nhật Tài Khoản" />
                <UserCreateUpdate modalId="create-user" @submit:user="createUser" @close="closeModal"
                    title="Thêm Tài Khoản" />
                <!-- Thông báo -->
                <NotificationModal modalId="delete-user" title="Xác Nhận Xóa" :message="message"
                    :confirmAction="deleteUser" :idToDelete="userIdToDelete" />
            </div>
        </div>
    </div>
</template>

<script>
import UserService from '@/services/user.service';
import SearchInput from '@/components/SearchInput.vue';
import UserCreateUpdate from '@/components/admin/UserCreateUpdate.vue';
import NotificationModal from '@/components/NotificationModal.vue';
import DashBoard from '@/components/admin/DashBoard.vue';

export default {
    components: {
        DashBoard,
        UserCreateUpdate,
        SearchInput,
        NotificationModal,
    },
    data() {
        return {
            activeIndex: -1,
            searchText: "",
            user: null,
            message: "",
            userIdToDelete: null,
        };
    },
    watch: {
        searchText() {
            this.activeIndex = -1;
        }
    },
    computed: {
        users() {
            return this.$store.getters.getUsers;;
        },
        filteredUsers() {
            if (!this.searchText.trim()) return this.users;
            return this.users.filter(user =>
                (user.name + user.email + user.phone).toLowerCase().includes(this.searchText.toLowerCase())
            );
        },
    },
    methods: {
        refreshList() {
            this.retrieveUsers();
            this.activeIndex = -1;
            this.searchText = "";
        },
        closeModal() {
            $('#update-user').modal('hide');
            $('#create-user').modal('hide');
        },

        confirmDelete(name, id) {
            this.message = `Bạn có chắn chắn muốn xóa \"${name ?? ''}\" ?`;
            this.userIdToDelete = id;
        },
        confirmUpdate(user) {
            this.user = user;
        },

        async retrieveUsers() {
            try {
                this.$store.dispatch('fillUsers');
                this.users = this.$store.getters.getUsers;
                this.users.sort((a, b) => a.name.localeCompare(b.name));
            } catch (error) {
                console.log(error);
            }
        },

        async createUser(data) {
            try {
                await UserService.register(data);
                this.retrieveUsers();
            } catch (error) {
                console.log(error);
            }
        },

        async updateUser(data) {
            try {
                await UserService.update(data);
                this.retrieveUsers();
            } catch (error) {
                console.log(error);
            }
        },

        async deleteUser(id) {
            try {
                await UserService.delete(id);
                this.retrieveUsers();
            } catch (error) {
                console.log(error);
            }
        },
    },
    mounted() {
        if (this.users.length == 0) {
            this.retrieveUsers();
        }
    }

};
</script>
<style></style>