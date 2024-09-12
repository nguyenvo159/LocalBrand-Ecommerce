<template>
    <div class="container-fluid" style="background-color: lightcyan; height: 100vh">
        <div class="row">
            <DashBoard type="Product" />
            <div id="dv" class="col-lg-9 col-11 admin-content">
                <h1 class="mb-4">Quản lý Sản Phẩm </h1>
                
                <div class="d-flex mb-3">
                    <SearchInput v-model="searchText" />
                    <button class="btn ml-2" style="box-shadow: none;" @click="refreshList()">
                        <i class="main-hover fa-solid fa-rotate-right" style="font-size: 20px;"></i></button>
                </div>
                <button class="btn btn-primary mb-2 mr-3" data-toggle="modal" data-target="#add-product">
                    <i class="fa-solid fa-plus"></i> Thêm mới</button>
                <button class="btn btn-primary mb-2" data-toggle="modal" data-target="#upload-img">
                    <i class="fa-solid fa-upload"></i> Upload</button>

                <table class="table table-hover shadow bg-white">
                    <thead class="thead-light">
                        <tr>
                            <th class="align-middle text-center">STT</th>
                            <th class="align-middle"><a class="cursor-pointer text-decoration-none"
                                    style="color: #495057; user-select: none;" @click="sortProductsByName()">Tên</a>
                            </th>
                            <th class="align-middle"><a class="cursor-pointer text-decoration-none"
                                    style="color: #495057; user-select: none;"
                                    @click="sortProductsByCategory()">Loại</a></th>
                            <th class="align-middle">Giá</th>
                            <th class="align-middle">Ngày thêm</th>
                            <th class="align-middle cursor-pointer" @click="sortDate()" style="user-select: none;">Ngày sửa</th>
                            <th class="align-middle text-center text-nowrap">Số lượng</th>
                            <th class="align-middle text-center text-nowrap">Thao tác</th>
                        </tr>
                    </thead>
                    <div v-if="products.length === 0">No products available.</div>
                    <tbody v-else>

                        <tr v-for="(product, index) in filteredProducts" :key="product.id" class="product-item">
                            <td class="align-middle text-center">{{ index + 1 }}</td>
                            <td class="align-middle">
                                {{ product.name }}
                            </td>
                            <td class="align-middle">{{ capitalizeFirstLetter(product.categoryName) }}</td>
                            <td class="align-middle">{{ product.price }}</td>
                            <td class="align-middle">{{ formatDate(product.createdAt) }}</td>
                            <td class="align-middle">{{ formatDate(product.updatedAt) }}</td>

                            <td class="align-middle text-center"> {{totalInventory(product.sizes)}}</td>
                            <td class="align-middle">
                                <div class="d-md-flex d-sm-block justify-content-evenly align-items-center">
                                    <a class="cursor-pointer" data-toggle="modal" data-target="#update-product" style="font-size: 20px;">
                                        <i class="fa-solid fa-pen-to-square"></i></a>
                                    <a class="cursor-pointer" data-toggle="modal" data-target="#delete-product" @click="confirmDelete(product.name, product.id)" style="font-size: 20px; color: red;">
                                        <i class="fa-solid fa-trash"></i></a>
                                </div>
                            </td>
                        </tr>

                    </tbody>

                </table>
                <!-- Thêm sản phẩm -->
                <!-- <InputProduct :product="newProduct" @submit:product="createProduct" @close="closeModal"
                    title="Thêm Sản Phẩm" modalId="add-product" /> -->
                <ProductCreateUpdate modalId="add-product" @submit:product="createProduct" @close="closeModal"
                    title="Thêm Sản Phẩm" />
                <!-- Upload ảnh -->
                <ProductImage modalId="upload-img" title="Upload Ảnh" />
                <!-- Sửa sản phẩm -->
                <!-- <InputProduct :product="product" @submit:product="updateProduct" @close="closeModal"
                    title="Chỉnh Sửa Sản Phẩm" modalId="update-product" /> -->

                <!-- Thông báo -->
                <NotificationModal modalId="delete-product" title="Xác Nhận Xóa" :message="message"
                    :confirmAction="deleteProduct" :idToDelete="productToDelete" />
            </div>
        </div>
    </div>
</template>

<script>
import { format } from 'date-fns';
import ProductService from '@/services/product.service';
import SearchInput from '@/components/SearchInput.vue';
import ProductCreateUpdate from '@/components/admin/ProductCreateUpdate.vue';
import ProductImage from '@/components/admin/ProductImage.vue';
import NotificationModal from '@/components/NotificationModal.vue';
import DashBoard from '@/components/admin/DashBoard.vue';

export default {
    components: {
        DashBoard,
        SearchInput,
        ProductCreateUpdate,
        ProductImage,
        NotificationModal, 
    },
    data() {
        return {
            searchText: "",
            message: "",
            productToDelete: null,
        };
    },
    computed: {
        products() {
            return this.$store.getters.getProducts;
        },
        filteredProducts() {
            if (!this.searchText.trim()) return this.products;
            return this.products.filter(product => 
                (product.name + product.categoryName + product.description).toLowerCase().includes(this.searchText.toLowerCase())
            );
        },
    },
    methods: {
        refreshList() {
            this.searchText= "";
            this.retrieveProducts();
        },

        formatDate(date) {
            const formattedDate = format(new Date(date), "dd/MM/yyyy HH:mm");
            return formattedDate;
        },
        capitalizeFirstLetter(text) {
            if (!text) return '';
            return text.charAt(0).toUpperCase() + text.slice(1).toLowerCase();
        },
        totalInventory(data) {
            return data.reduce((total, product) => total + product.inventory, 0);
        },

        // Sắp xếp
        sortProductsByName() {
            this.products.sort((a, b) => {
                if (this.sortByNameAsc) {
                    return a.name.localeCompare(b.name);
                } else {
                    return b.name.localeCompare(a.name);
                }
            });
            this.sortByNameAsc = !this.sortByNameAsc;
        },
        sortProductsByCategory() {
            this.products.sort((a, b) => {
                if (this.sortByCategoryAsc) {
                    return a.category.localeCompare(b.categoryName);
                } else {
                    return b.category.localeCompare(a.categoryName);
                }
            });
            this.sortByCategoryAsc = !this.sortByCategoryAsc;
        },

        sortDate() {
            this.products.sort((a, b) => {
                const dateA = new Date(a.updatedAt);
                const dateB = new Date(b.updatedAt);
                if (this.sortByDateAsc) {
                    return dateA - dateB;
                } else {
                    return dateB - dateA;
                }
            });
            this.sortByDateAsc = !this.sortByDateAsc;
        },
        closeModal() {
            $('#add-product').modal('hide');
            $('#update-product').modal('hide');
        },
        confirmDelete(name, id) {
            this.message = `Bạn có chắn chắn muốn xóa \"${name ?? ''}\" ?`;
            this.productToDelete = id;
        },
        async retrieveProducts() {
            try {
                this.$store.dispatch('fillProducts');
                this.products = this.$store.getters.getProducts;

            } catch (error) {
                console.log(error);
            }
        },

        async createProduct(data) {
            try {
                await ProductService.create(data);
                this.refreshList();
            } catch (error) {
                console.log(error);
            }
        },

        async updateProduct(data) {
            try {
                await ProductService.update(data);
                this.retrieveProducts();
            } catch (error) {
                console.log(error);
            }
        }
        ,

        async deleteProduct(id) {
            try {
                await ProductService.delete(id);
                this.retrieveProducts();
            } catch (error) {
                console.log(error);
            }
        },

    },
    mounted() {
        if (this.products.length === 0){
            this.retrieveProducts();
        }
    }
};
</script>
<style></style>