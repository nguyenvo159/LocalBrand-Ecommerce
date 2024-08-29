<template>
    <div class="container-fluid pb-4" style="background-color: lightcyan;">
        <div class="row">
            <DashBoard type="User" />
            <div id="dv" class="col-lg-9 col-11 admin-content">
                <h1 class="mb-4">Quản lý Người Dùng </h1>

                <div class="d-flex">
                    <InputSearch v-model="searchText" />

                    <button class="btn pb-4 ml-3 m" style="box-shadow: none;" @click="refreshList()">
                        <i class="main-hover fa-solid fa-rotate-right" style="font-size: 24px;"></i></button>

                </div>

                <table class="table shadow bg-white">
                    <thead class="thead-light">
                        <tr>
                            <th class="align-middle text-center">STT</th>
                            <th>Tên</th>
                            <th>Email</th>
                            <th>Số điện thoại</th>
                            <th>Phân Quyền</th>
                            <th>Thao tác</th>
                        </tr>
                    </thead>
                    <div v-if="users.length == 0">No user available.</div>
                    <tbody v-else>

                        <tr v-for="(user, index) in filteredUsers" :key="user._id" class="product-item">
                            <td class="align-middle text-center">{{ index + 1 }}</td>
                            <td>{{ user.name }}</td>
                            <td>{{ user.email }}</td>
                            <td>{{ user.phone }}</td>
                            <td>{{ user.role}}</td>
                            <td>
                                <a class="cursor-pointer mr-2" data-toggle="modal" data-target="#update-user"
                                    @click="confirmUpdate(user)">Sửa</a>
                                <a class="cursor-pointer" data-toggle="modal" data-target="#delete-user"
                                    @click="confirmDelete(user)">Xóa</a>
                            </td>
                        </tr>

                    </tbody>

                </table>
                <!-- Sửa thông tin User -->
                <InputUser :user="user" @submit:user="updateUser" @close="closeModal" title="Cập Nhật Tài Khoản"
                    modalId="update-user" />
                <!-- Thông báo -->
                <NotificationModal modalId="delete-user" title="Xác Nhận Xóa" :message="message"
                    :confirmAction="deleteUser" :idToDelete="userToDelete" />
            </div>
        </div>
    </div>
</template>

<script>
import UserService from '@/services/user.service';
import InputSearch from '@/components/InputSearch.vue';
import InputUser from '@/components/admin/InputUser.vue';
import NotificationModal from '@/components/NotificationModal.vue';
import DashBoard from './DashBoard.vue';

export default {
    components: {
        DashBoard,
        InputSearch,
        InputUser,
        InputSearch,
        NotificationModal,
    },
    data() {
        return {
            users: [],
            activeIndex: -1,
            searchText: "",
            user: null,
            message: "",
            userToDelete: null,
        };
    },
    watch: {
        searchText() {
            this.activeIndex = -1;
        }
    },
    computed: {
        userStrings() {
            return this.users.map((user) => {
                const { name, email, phone } = user;
                return [name.toLowerCase(), email.toLowerCase(), phone].join("");
            });
        },
        filteredUsers() {
            if (!this.searchText.trim()) return this.users;
            return this.users.filter((_user, index) =>
                this.userStrings[index].includes(this.searchText.toLocaleLowerCase())
            );
        },
    },
    async created() {
        this.retrieveUsers();

    },
    methods: {
        refreshList() {
            this.retrieveUsers();
            this.activeIndex = -1;
            this.searchText = "";
        },
        closeModal() {
            $('#update-user').modal('hide');
        },

        confirmDelete(user) {
            this.userToDelete = user._id;
            this.message = `Bạn có chắn chắn muốn xóa \"${user ? user.name : ''}\" ?`;
        },
        confirmUpdate(user) {
            this.user = user;
        },

        async retrieveUsers() {
            try {
                this.users = await UserService.getAll();
                this.users.sort((a, b) => {
                    const pqA = a.isAdmin ? 'Admin' : 'User';
                    const pqB = b.isAdmin ? 'Admin' : 'User';
                    return pqA.localeCompare(pqB);
                });
            } catch (error) {
                console.log(error);
            }
        },


        async updateUser(data) {
            try {
                await UserService.update(this.user._id, data);
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
        this.refreshList();
    }
};
</script>
<style></style>