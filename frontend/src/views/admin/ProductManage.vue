<template>
    <div class="container-fluid" style=" height: 100vh">
        <div class="row">
            <DashBoard type="Product" />
            <div id="dv" class="col-lg-10 offset-lg-2 col-11 offset-1 admin-content">
                <h3 class="mb-4 text-uppercase">Quản lý Sản Phẩm </h3>
                <div class="row row-cols-1 row-cols-md-2 row-cols-xl-4">
                    <div class="col  mb-3">
                        <div class="card radius-10 border-start border-0 border-3 border-info">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div>
                                        <p class="mb-0 text-secondary">Tổng sản phẩm</p>
                                        <h4 class="my-1 text-info">{{ analytics.totalProduct }}</h4>
                                        <p class="mb-0 font-13 text-muted"><i>tồn tại trong kho</i></p>
                                    </div>
                                    <div class="widgets-icons-2 rounded-circle bg-gradient-scooter text-white ms-auto">
                                        <i class="fa fa-shopping-cart"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col  mb-3">
                        <div class="card radius-10 border-start border-0 border-3 border-danger">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div>
                                        <p class="mb-0 text-secondary">Tổng sản phẩm</p>
                                        <h4 class="my-1 text-danger">{{ analytics.totalProductSold }}</h4>
                                        <p class="mb-0 font-13 text-muted"><i>đã bán trong hôm nay</i></p>
                                    </div>
                                    <div class="widgets-icons-2 rounded-circle bg-gradient-bloody text-white ms-auto"><i
                                            class="fa fa-dollar"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col  mb-3">
                        <div class="card radius-10 border-start border-0 border-3 border-success">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div>
                                        <p class="mb-0 text-secondary">Tổng sản phẩm</p>
                                        <h4 class="my-1 text-success">{{ analytics.totalProductSoldForWeek }}</h4>
                                        <p class="mb-0 font-13 text-muted"><i>đã bán trong 7 ngày qua</i></p>

                                    </div>
                                    <div
                                        class="widgets-icons-2 rounded-circle bg-gradient-ohhappiness text-white ms-auto">
                                        <i class="fa fa-bar-chart"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col  mb-3">
                        <div class="card radius-10 border-start border-0 border-3 border-warning">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div>
                                        <p class="mb-0 text-secondary">Tổng sản phẩm</p>
                                        <h4 class="my-1 text-warning">{{ analytics.totalProductSoldForMonth }}</h4>
                                        <p class="mb-0 font-13 text-muted"><i>đã bán trong 30 ngày qua</i></p>

                                    </div>
                                    <div class="widgets-icons-2 rounded-circle bg-gradient-blooker text-white ms-auto">
                                        <i class="fa fa-users"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="d-flex justify-content-between mb-1">
                    <div class="">
                        <button class="btn btn-primary mb-2 mr-3" data-toggle="modal" data-target="#add-product">
                            <i class="fa-solid fa-plus"></i> Thêm mới</button>

                        <input ref="fileImport" type="file" class="d-none" @change="importProduct" />
                        <button class="btn btn-primary mb-2" type="button" @click="openFileDialog">
                            <i class=" fa-solid fa-upload"></i> Import</button>
                        <button class="btn ml-2" style="box-shadow: none;" @click="refreshList()">
                            <i class=" fa-solid fa-rotate-right" style="font-size: 20px;"></i></button>
                    </div>
                    <div class="w-25">
                        <!-- <SearchInput v-model="searchText" /> -->
                        <div id="search-input" class="w-100 input-group d-flex align-items-center">
                            <span class="pr-2">Search: </span>
                            <input type="text" class="form-control rounded-pill" placeholder="Nhập thông tin cần tìm..."
                                v-model="searchText" @keyup.enter="fetchProduct" style="box-shadow: none;" />

                        </div>
                    </div>
                </div>
                <div class="input-group">
                    <input type="number" class="form-control d-inline-block" v-model="discountPercentage"
                        placeholder="Phần trăm giảm giá" style="max-width: 80px;" />
                    <div class="input-group-append">
                        <button class="btn btn-danger mb-2" @click="confirmDiscount"> Áp dụng giảm giá</button>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12 mb-3">
                        <div class="card">
                            <div class="card-header">
                                <span><i class="bi bi-table "></i></span> Danh sách sản phẩm
                            </div>
                            <div class="d-flex align-items-center px-3 pt-3">
                                <label class="m-0 pr-2">Hiển thị </label>
                                <select name="example_length" aria-controls="example" class="form-select form-select-sm"
                                    v-model="size" @change="changePage(1)" style="max-width: 70px;">
                                    <option value="10">10</option>
                                    <option value="25">25</option>
                                    <option value="50">50</option>
                                    <option value="100">100</option>
                                </select>
                                <div class="btn-group form-group d-flex align-items-center m-0 pl-3">
                                    <i class="fa-solid fa-filter mr-2"></i><span class="mr-3">Filter: </span>
                                    <select class="form-control rounded-pill" @change="fetchProduct" v-model="sortBy">
                                        <option value="">Mặc định</option>
                                        <option value="0">Giá: Tăng dần</option>
                                        <option value="1">Giá: Giảm dần</option>
                                        <option value="2">Kho: Tăng dần</option>
                                        <option value="3">Kho: Giảm dần</option>
                                    </select>
                                </div>
                            </div>
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table table-hover table-striped bg-white">
                                        <thead class="">
                                            <tr>
                                                <th class="border-0">
                                                    <div class="form-check d-flex align-items-center">
                                                        <input class="form-check-input" type="checkbox"
                                                            @click="toggleSelectAll" :checked="isAllSelected" />
                                                    </div>
                                                </th>

                                                <th class="border-0 align-middle text-center">STT</th>
                                                <th class="border-0 align-middle cursor-pointer"
                                                    style="user-select: none;" @click="sortProductsByName()">Tên</th>
                                                <th class="border-0 align-middle cursor-pointer"
                                                    style="user-select: none;" @click="sortProductsByCategory()">Loại
                                                </th>
                                                <th class="border-0 align-middle">Giá</th>
                                                <!-- <th class="border-0 align-middle">Ngày thêm</th> -->
                                                <th class="border-0 align-middle text-dark cursor-pointer"
                                                    @click="sortDate()" style="user-select: none;">Ngày
                                                    sửa</th>
                                                <th class="border-0 align-middle text-center text-nowrap">Số lượng</th>
                                                <th class="border-0 align-middle text-center text-nowrap">Thao tác</th>
                                            </tr>
                                        </thead>
                                        <div v-if="products.length === 0">No products available.</div>
                                        <tbody v-else>

                                            <tr v-for="(product, index) in filteredProducts" :key="product.id"
                                                class="product-item">
                                                <td>
                                                    <div class="form-check d-flex align-items-center">
                                                        <input type="checkbox" class="form-check-input"
                                                            v-model="selectedProducts" :value="product.id" />
                                                    </div>
                                                </td>
                                                <td class="align-middle text-center">{{ index + 1 }}</td>
                                                <td class="align-middle">
                                                    {{ product.name }}
                                                    <span v-if="product.percentage != 0"
                                                        class="fire-animation px-2 py-1 rounded">
                                                        - {{ product.percentage }}%
                                                    </span>
                                                </td>
                                                <td class="align-middle">{{ capitalizeFirstLetter(product.categoryName)
                                                    }}</td>
                                                <td class="align-middle">
                                                    {{ formatPrice(product.price - product.price *
                                                        product.percentage /
                                                        100) }}
                                                </td>
                                                <!-- <td class="align-middle">{{ formatDate(product.createdAt) }}</td> -->
                                                <td class="align-middle">{{ formatDate(product.updatedAt) }}</td>

                                                <td class="align-middle text-center"> {{ totalInventory(product.sizes)
                                                    }}</td>
                                                <td class="align-middle">
                                                    <div
                                                        class="d-md-flex d-sm-block justify-content-evenly align-items-center">
                                                        <a class="cursor-pointer" data-toggle="modal"
                                                            data-target="#update-product"
                                                            @click="confirmUpdate(product)" style="font-size: 20px;">
                                                            <i class="fa-solid fa-pen-to-square"></i></a>
                                                        <a class="cursor-pointer" data-toggle="modal"
                                                            data-target="#delete-product"
                                                            @click="confirmDelete(product.name, product.id)"
                                                            style="font-size: 20px; color: red;">
                                                            <i class="fa-solid fa-trash"></i></a>
                                                    </div>
                                                </td>
                                            </tr>

                                        </tbody>

                                    </table>
                                </div>
                                <div>
                                    <nav aria-label="Page navigation example">
                                        <ul class="pagination">
                                            <li class="page-item" @click="changePage(page - 1)"><a class="page-link"
                                                    href="#">
                                                    <i class="fa-solid fa-chevron-left"></i>
                                                </a>
                                            </li>
                                            <li class="page-item" v-for="p in totalPage"
                                                :class="{ 'active': p == page }" @click="changePage(p)">
                                                <a class="page-link" href="#">{{ p }}</a>
                                            </li>

                                            <li class="page-item" @click="changePage(page + 1)"><a class="page-link"
                                                    href="#">
                                                    <i class="fa-solid fa-chevron-right"></i>
                                                </a></li>
                                        </ul>
                                    </nav>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <!-- Thêm sản phẩm -->
            </div>
        </div>
    </div>
    <!-- Thêm sản phẩm -->
    <ProductCreateUpdate modalId="add-product" @close="closeModal" title="Thêm Sản Phẩm" />

    <!-- Sửa sản phẩm -->
    <ProductCreateUpdate :product="product" @close="closeModal" title="Chỉnh Sửa Sản Phẩm" modalId="update-product" />

    <!-- Thông báo -->
    <NotificationModal modalId="delete-product" title="Xác Nhận Xóa" :message="message" :confirmAction="deleteProduct"
        :idToDelete="productToDelete" />

    <!-- Upload ảnh -->

</template>

<script>
import { format, set } from 'date-fns';
import ProductService from '@/services/product.service';
import SearchInput from '@/components/SearchInput.vue';
import ProductCreateUpdate from '@/components/admin/ProductCreateUpdate.vue';
import NotificationModal from '@/components/NotificationModal.vue';
import DashBoard from '@/components/admin/DashBoard.vue';

export default {
    components: {
        DashBoard,
        SearchInput,
        ProductCreateUpdate,
        NotificationModal,
    },
    data() {
        return {
            size: 10,
            page: 1,
            totalPage: 1,
            products: [],
            items: [],
            searchText: "",
            message: "",
            productToDelete: null,
            product: null,
            analytics: {
                totalProduct: 0,
                totalProductSold: 0,
                totalProductSoldForWeek: 0,
                totalProductSoldForMonth: 0
            },
            sortBy: '',
            selectedProducts: [], // Danh sách sản phẩm được chọn
            discountPercentage: 0,
        };
    },
    computed: {
        filteredProducts() {
            return this.products;
        },
        isAllSelected() {
            return this.selectedProducts.length === this.filteredProducts.length && this.filteredProducts.length > 0;
        },
    },
    methods: {
        refreshList() {
            this.searchText = "";
            this.page = 1;
            this.fetchProduct();
            // this.retrieveProducts();
        },
        toggleSelectAll(event) {
            if (event.target.checked) {
                this.selectedProducts = this.filteredProducts.map(product => product.id);
            } else {
                this.selectedProducts = [];
            }
        },

        formatDate(date) {
            const formattedDate = format(new Date(date), "HH:mm dd/MM/yyyy");
            return formattedDate;
        },
        formatPrice(price) {
            return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
        },
        capitalizeFirstLetter(text) {
            if (!text) return '';
            return text.charAt(0).toUpperCase() + text.slice(1).toLowerCase();
        },
        totalInventory(data) {
            return data.reduce((total, product) => total + product.inventory, 0);
        },
        openFileDialog() {
            this.$refs.fileImport.click();
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
                    return a.categoryName.localeCompare(b.categoryName);
                } else {
                    return b.categoryName.localeCompare(a.categoryName);
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
            this.retrieveProducts();
        },
        confirmDelete(name, id) {
            this.message = `Bạn có chắn chắn muốn xóa \"${name ?? ''}\" ?`;
            this.productToDelete = id;
        },
        confirmUpdate(product) {
            this.product = product;
        },
        changePage(page) {
            if (page > 0 && page <= this.totalPage) {
                this.page = page;
                this.fetchProduct();
            }
        },
        async retrieveProducts() {
            try {
                this.products = await ProductService.getAll();
                this.products.sort((a, b) => new Date(b.updatedAt) - new Date(a.updatedAt));
                this.fetchProduct();
            } catch (error) {
                console.log(error);
            }
        },
        async fetchProduct() {
            let loader = this.$loading.show({
                container: null,
                width: 100,
                height: 100,
                color: '#808EF4',
                loader: 'bars',
                canCancel: true,
            });
            try {
                var data = {
                    pageNumber: this.page,
                    pageSize: this.size,
                    search: this.searchText,
                    orderByPrice: this.sortBy
                }
                var result = await ProductService.getPaging(data);
                this.analytics = await ProductService.analytics();
                setTimeout(() => {
                    loader.hide();
                }, 500);
                this.totalPage = result.totalPages;
                this.products = result.items;
                this.items = result.items;
                this.selectedProducts = [];
                this.discountPercentage = 0;

            } catch (error) {
                loader.hide();
                console.log(error);
            }
        },
        async confirmDiscount() {
            try {
                var data = {
                    productIds: this.selectedProducts,
                    percentage: this.discountPercentage
                }
                await ProductService.updateSpecial(data);
                this.fetchProduct();
            } catch (error) {
                console.error(error);
            }
        },

        async deleteProduct(id) {
            try {
                await ProductService.delete(id);
                this.retrieveProducts();
            } catch (error) {
                console.log(error);
            }
        },

        async importProduct(event) {
            const file = event.target.files[0];
            if (file) {
                const formData = new FormData();
                formData.append('file', file);
                try {
                    await ProductService.import(formData);
                    this.retrieveProducts();
                } catch (error) {
                    console.log(error);
                }
            }
        },
    },
    mounted() {
        // this.retrieveProducts();
        this.fetchProduct();
        if (this.$store.getters.getCategories.length == 0) {
            this.$store.dispatch('fillCategories');
        }
    }
}
</script>
<style scoped>
.page-link {
    outline: none !important;
    box-shadow: none !important;
}

.card {
    box-shadow: 0 2px 6px 0 rgb(218 218 253 / 65%), 0 2px 6px 0 rgb(206 206 238 / 54%);
}

.fire-animation {
    animation: flicker 1s infinite;
}

@keyframes flicker {
    0% {
        transform: scale(1);
        opacity: 1;
        background-color: #ff4500;
        color: white;
    }

    50% {
        transform: scale(1.2);
        opacity: 0.8;
        background-color: #f8b500;
        color: white;
    }

    100% {
        transform: scale(1);
        opacity: 1;
        background-color: #a52c00;
        color: white;
    }
}
</style>