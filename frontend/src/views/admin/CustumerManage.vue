<template>
    <div class="container-fluid">
        <div class="row">
            <DashBoard type="Custumer" />
            <div id="dv" class="col-lg-10 offset-lg-2 col-11 offset-1 admin-content">
                <h3 class="mb-4 text-uppercase">Quản lý khách hàng</h3>
                <div class="row row-cols-1 row-cols-md-2 row-cols-xl-4">
                    <div class="col  mb-3">
                        <div class="card radius-10 border-start border-0 border-3 border-info">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div>
                                        <p class="mb-0 text-secondary">Tổng số liên hệ</p>
                                        <h4 class="my-1 text-info">
                                            {{ contacts != null ? contacts.totalRecords : 0 }}</h4>
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
                                        <p class="mb-0 text-secondary">Liên hệ chưa phản hồi</p>
                                        <h4 class="my-1 text-danger">{{ contactNoReply }}</h4>
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
                                        <p class="mb-0 text-secondary">Mã giảm giá chưa sử dụng</p>
                                        <h4 class="my-1 text-success">
                                            {{ discounts != null ? discounts.totalRecords : 0 }}
                                        </h4>
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
                                        <p class="mb-0 text-secondary">Đánh giá tiêu cực</p>
                                        <h4 class="my-1 text-success">{{ reviewHate }}</h4>
                                    </div>
                                    <div class="widgets-icons-2 rounded-circle bg-gradient-blooker text-white ms-auto">
                                        <i class="fa fa-users"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12 p-0">
                        <ul class="d-flex" style="list-style: none;">
                            <li class="list-title" @click="display = 'contact'"
                                :class="{ 'active': display == 'contact' }">
                                <span>Liên hệ</span>
                            </li>
                            <li class="list-title" @click="display = 'review'"
                                :class="{ 'active': display == 'review' }">
                                <span>Đánh giá</span>
                            </li>
                            <li class="list-title" @click="display = 'discount'"
                                :class="{ 'active': display == 'discount' }">
                                <span>Mã giảm giá</span>
                            </li>

                        </ul>
                    </div>

                    <div v-if="display == 'contact'">
                        <div class="col-12">

                            <div v-if="contacts" v-for="contact in contacts.items"
                                class="card mb-3 radius-10 border-start border-0 border-5 "
                                :class="{ 'border-success': contact.userId, 'border-warning cursor-pointer': !contact.userId }">
                                <div class="card-body" @click="toggleFooter(contact.id, contact.userId)">
                                    <div class="d-flex align-items-center">
                                        <div>
                                            <p v-if="contact.userId" class="text-success" style="font-style: italic;">
                                                <i class="fa-regular fa-circle-check"></i> Đã được
                                                phản hồi bởi {{ contact.userName ?? '' }}
                                            </p>
                                            <p class="text-muted border-bottom" style="font-style: italic;">Ngày tạo: {{
                                                formatDate(contact.createdAt) }}</p>
                                            <h5 class="my-1 text-info">{{ contact.name }}
                                                <span class="text-muted h6" style="font-style: italic;">{{ contact.email
                                                    }}</span>
                                            </h5>

                                            <p class="mb-0 py-2 font-14" style="text-align: justify;">
                                                {{ contact.message }}
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <div v-show="showFooter[contact.id]" class="card-footer bg-transparent border-top">
                                    <div class="py-2">
                                        <div class="form-group w-100">
                                            <textarea v-model="contact.reply" class="form-control" rows="3"></textarea>
                                        </div>
                                        <button class="btn btn-primary" @click="updateContact(contact)">Xác
                                            nhận</button>
                                    </div>
                                </div>

                            </div>

                        </div>
                        <div class="my-3 px-4">
                            <nav aria-label="Page navigation example">
                                <ul class="pagination">
                                    <li class="page-item" @click="changePage(pageNumber - 1)"><a class="page-link"
                                            href="#">
                                            <i class="fa-solid fa-chevron-left"></i>
                                        </a>
                                    </li>
                                    <li class="page-item border-0" v-for="p in totalPages"
                                        :class="{ 'active': p == pageNumber }" @click="changePage(p)">
                                        <a class="page-link" href="#">{{ p }}</a>
                                    </li>

                                    <li class="page-item" @click="changePage(pageNumber + 1)"><a class="page-link"
                                            href="#">
                                            <i class="fa-solid fa-chevron-right"></i>
                                        </a></li>
                                </ul>
                            </nav>
                        </div>
                    </div>
                    <div v-if="display == 'review'">
                        <div class="col-12">
                            <div class="d-flex align-items-center">
                                <div class="btn-group form-group d-flex align-items-center justify-content-">
                                    <i class="fa-solid fa-filter mr-2"></i><span class="mr-3">Filter: </span>
                                    <select class="form-control rounded-pill" @change="fetchReviews"
                                        v-model="filterReview">
                                        <option value="">Tất cả</option>
                                        <option value="0">Tích cực</option>
                                        <option value="1">Tiêu cực</option>
                                    </select>
                                </div>
                            </div>
                            <div v-if="reviews" v-for="cmt in reviews.items"
                                class="card mb-3 radius-10 border-start border-0 border-5 border-warning">
                                <div class="card-body">
                                    <div class="d-flex align-items-center justify-content-between">
                                        <div>
                                            <p class="text-muted border-bottom">Ngày tạo: {{ formatDate(cmt.createdAt)
                                                }} </p>
                                            <p @click="pushDetailRoute(cmt.productId)"
                                                class="text-muted cursor-pointer"><span>{{ cmt.productName }}</span></p>
                                            <h5 class="my-1 text-info">{{ cmt.userName }}

                                                <span v-for="n in 5" :key="n" class="star">
                                                    <i :class="getStarClass(n, cmt.rating)" aria-hidden="true"></i>
                                                </span>
                                            </h5>
                                            <p class="mb-0 py-2 font-14" style="text-align: justify;">
                                                {{ cmt.comment }}
                                            </p>
                                        </div>
                                        <span v-if="cmt.rating < 4" class="fs-3 pr-5 cursor-pointer"
                                            @click="delteReview(cmt.id)">
                                            <i class="fa-solid fa-trash text-danger"></i></span>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="my-3 px-4">
                            <nav aria-label="Page navigation example">
                                <ul class="pagination">
                                    <li class="page-item" @click="changePageReview(pageNumberReview - 1)"><a
                                            class="page-link" href="#">
                                            <i class="fa-solid fa-chevron-left"></i>
                                        </a>
                                    </li>
                                    <li class="page-item border-0" v-for="p in totalPagesReview"
                                        :class="{ 'active': p == pageNumberReview }" @click="changePageReview(p)">
                                        <a class="page-link" href="#">{{ p }}</a>
                                    </li>

                                    <li class="page-item" @click="changePageReview(pageNumberReview + 1)"><a
                                            class="page-link" href="#">
                                            <i class="fa-solid fa-chevron-right"></i>
                                        </a></li>
                                </ul>
                            </nav>
                        </div>

                    </div>
                    <div v-if="display == 'discount'">
                        <div class="col-12">
                            <div class="mb-3 d-flex">
                                <button class="btn btn-primary" data-toggle="modal" data-target="#create-discount"><i
                                        class="fa-solid fa-plus mr-1"></i> Thêm</button>
                                <button @click="exportDiscount" class="btn btn-success ml-3"><i
                                        class="fa-solid fa-file-export mr-1"></i>
                                    Export</button>
                                <button @click="deleteDiscountExpired" class="btn btn-danger ml-3">
                                    <i class="fa-solid fa-trash-can"></i>
                                    Xóa mã hết hạn</button>
                            </div>
                            <div v-for="d in discounts.items"
                                class="card mb-3 radius-10 border-start border-0 border-5 border-success">
                                <div class="card-body" @click="toggleFooter(d.id)">
                                    <div class="d-flex align-items-center">
                                        <div>
                                            <h5 class="my-1 text-info">Mã: {{ d.code }} </h5>

                                            <p class="mb-0 py-2 font-14" style="text-align: justify;">
                                                Giảm {{ d.discountPercentage }} % cho đơn hàng từ
                                                <span class="price">{{ formatPrice(d.requireMoney) }}đ</span>, tối đa
                                                <span class="price">{{ formatPrice(d.maximumDiscount) }}đ</span>
                                            </p>
                                            <span class="text-muted h6" style="font-style: italic;">Ngày hết hạn:
                                                {{ formatDate(d.expiryDate) }}</span>
                                        </div>
                                    </div>
                                </div>
                                <div v-show="showFooter[d.id]"
                                    class="card-footer bg-transparent d-flex align-items-center">
                                    <input class="form-control w-50" type="email" v-model="d.email"
                                        placeholder="Nhập email người nhận...">
                                    <button class="my-2 ml-3 btn btn-primary" @click="sendDiscount(d)">Gửi</button>
                                </div>
                            </div>
                        </div>
                        <div class="my-3 px-4">
                            <nav aria-label="Page navigation example">
                                <ul class="pagination">
                                    <li class="page-item" @click="changePageDiscount(pageNumberDiscount - 1)"><a
                                            class="page-link" href="#">
                                            <i class="fa-solid fa-chevron-left"></i>
                                        </a>
                                    </li>
                                    <li class="page-item border-0" v-for="p in totalPagesDiscount"
                                        :class="{ 'active': p == pageNumberDiscount }" @click="changePageDiscount(p)">
                                        <a class="page-link" href="#">{{ p }}</a>
                                    </li>

                                    <li class="page-item" @click="changePageDiscount(pageNumberDiscount + 1)"><a
                                            class="page-link" href="#">
                                            <i class="fa-solid fa-chevron-right"></i>
                                        </a></li>
                                </ul>
                            </nav>
                        </div>

                        <!-- Modal -->
                        <div class="modal fade" id="create-discount" tabindex="-1" aria-labelledby="exampleModalLabel"
                            aria-hidden="true">
                            <div class="modal-dialog modal-lg">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalLabel">Thêm Mã Giảm Giá</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-md-6 form-group">
                                                <label for="amount">Số Lượng</label>
                                                <input v-model="discount.amount" type="text" class="form-control"
                                                    id="amount">
                                            </div>
                                            <div class="col-md-6 form-group">
                                                <label for="discountPercentage">Phần trăm giảm giá</label>
                                                <input v-model="discount.discountPercentage" type="text"
                                                    class="form-control" id="discountPercentage">
                                            </div>
                                            <div class="col-md-6 form-group">
                                                <label for="requireMoney">Số tiền tối thiểu</label>
                                                <input v-model="discount.requireMoney" type="text" class="form-control"
                                                    id="requireMoney">
                                            </div>
                                            <div class="col-md-6 form-group">
                                                <label for="maximumDiscount">Số tiền tối đa</label>
                                                <input v-model="discount.maximumDiscount" type="text"
                                                    class="form-control" id="maximumDiscount">
                                            </div>
                                            <div class="col-md-6 form-group">
                                                <label for="expiryDate">Ngày hết hạn</label>
                                                <input v-model="discount.expiryDate" type="date" class="form-control"
                                                    id="expiryDate">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary"
                                            data-dismiss="modal">Hủy</button>
                                        <button type="button" @click="createDiscount"
                                            class="btn btn-primary">Tạo</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <NotificationOption v-if="showToast" :visible="showToast" :type="toastType"
                        @close="showToast = false" :message="toastMessage" />
                </div>
            </div>
        </div>

    </div>
</template>

<script>
import DashBoard from '@/components/admin/DashBoard.vue';
import contactService from '@/services/contact.service';
import reviewService from '@/services/review.service';
import discountService from '@/services/discount.service';
import NotificationOption from '@/components/NotificationOption.vue';
import { formatDate, set } from 'date-fns';
export default {
    components: {
        DashBoard,
        NotificationOption
    },
    data() {
        return {
            showToast: false,
            toastType: 'success', // Kiểu toast ('success', 'error', 'info')
            toastMessage: '',
            display: 'contact',
            contacts: null,
            contactNoReply: 0,
            reviews: null,
            reviewHate: 0,
            discounts: null,
            pageNumber: 1,
            pageSize: 10,
            totalPages: 1,
            pageNumberReview: 1,
            pageSizeReview: 10,
            totalPagesReview: 1,
            filterReview: '',
            pageNumberDiscount: 1,
            pageSizeDiscount: 10,
            totalPagesDiscount: 1,
            showFooter: {},
            discount: {
                amount: 0,
                discountPercentage: 0,
                requireMoney: 0,
                maximumDiscount: 0,
                expiryDate: null
            }
        }
    },
    methods: {
        changeDisplay(type) {
            this.display = type;
            this.totalPages = 1;
            this.pageNumber = 1;
            this.pageSize = 10;
        },
        getStarClass(index, rating) {
            if (rating >= index) {
                return 'fa-solid fa-star';
            } else if (rating >= index - 0.5) {
                return 'fa-solid fa-star-half-stroke';
            } else {
                return 'fa-regular fa-star';
            }
        },
        formatPrice(price) {
            return price.toLocaleString('vi-VN');
        },
        formatDate(date) {
            return formatDate(new Date(date), 'hh:mm dd/MM/yyyy');
        },
        changePage(page) {
            if (page > 0 && page <= this.totalPages) {
                this.pageNumber = page;
                this.fetchContacts();
            }
        },
        changePageReview(page) {
            if (page > 0 && page <= this.totalPagesReview) {
                this.pageNumberReview = page;
                this.fetchReviews();
            }
        },
        changePageDiscount(page) {
            if (page > 0 && page <= this.totalPagesDiscount) {
                this.pageNumberDiscount = page;
                this.fetchDiscounts();
            }
        },
        toggleFooter(contactId, userId) {
            if (userId) {
                return;
            }
            if (!this.showFooter) {
                this.showFooter = {};
            }
            this.showFooter[contactId] = !this.showFooter[contactId];
        },
        pushDetailRoute(productId) {
            this.$router.push({ name: 'ProductDetail', params: { id: productId } });
        },

        async fetchContacts() {
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
                    pageNumber: this.pageNumber,
                    pageSize: this.pageSize
                }
                this.contacts = await contactService.getPaging(data);
                setTimeout(() => {
                    loader.hide();
                }, 500);
                this.totalPages = this.contacts.totalPages;
                this.contactNoReply = this.contacts.items.filter(c => !c.userId).length;
            } catch (error) {
                loader.hide();
                console.log(error);
            }
        },
        async fetchReviews() {
            try {
                var data = {
                    pageNumber: this.pageNumberReview,
                    pageSize: this.pageSizeReview,
                    filter: this.filterReview
                }
                this.reviews = await reviewService.getPaging(data);
                this.totalPagesReview = this.reviews.totalPages;
                this.reviewHate = this.reviews.items.filter(r => r.rating < 3).length;
            } catch (error) {
                console.error('Lỗi khi lấy danh sách đánh giá:', error);
            }
        },
        async delteReview(id) {
            let loader = this.$loading.show({
                container: null,
                width: 100,
                height: 100,
                color: '#808EF4',
                loader: 'bars',
                canCancel: true,
            });
            try {
                setTimeout(async () => {
                    await reviewService.delete(id);
                    this.fetchReviews();
                    loader.hide();
                }, 500);
            } catch (error) {
                console.error('Lỗi khi xóa đánh giá:', error);
            }
        },

        async fetchDiscounts() {
            try {
                var data = {
                    pageNumber: this.pageNumberDiscount,
                    pageSize: this.pageSizeDiscount
                }
                this.discounts = await discountService.getPaging(data);
                this.totalPagesDiscount = this.discounts.totalPages;
            } catch (error) {
                console.error('Lỗi khi lấy danh sách mã giảm giá:', error);
            }
        },
        async updateContact(contact) {
            try {
                var user = this.$store.getters.getUser;
                var data = {
                    id: contact.id,
                    userId: user.id,
                    reply: contact.reply
                }
                await contactService.update(data);
                this.showFooter[contact.id] = !this.showFooter[contact.id];
                this.fetchContacts();
            } catch (error) {
                console.log(error);
            }
        },
        async sendDiscount(discount) {
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
                    code: discount.code,
                    email: discount.email
                }
                await discountService.sendMail(data);
                loader.hide();
                discount.email = '';
                this.showFooter[discount.id] = !this.showFooter[discount.id];
            } catch (error) {
                loader.hide();
                console.log(error);
            }
        },
        async exportDiscount() {
            console.log('Export');

            try {
                await discountService.export();
            } catch (error) {
                console.error('Lỗi khi xuất file:', error);
            }
        },
        async createDiscount() {
            let loader = this.$loading.show({
                container: null,
                width: 100,
                height: 100,
                color: '#808EF4',
                loader: 'bars',
                canCancel: true,
            });
            try {
                this.discount.expiryDate = new Date(this.discount.expiryDate).toISOString();
                await discountService.create(this.discount);
                this.showToast = true;
                this.toastType = 'success';
                this.toastMessage = "Đã tạo thành công " + this.discount.amount + " mã giảm giá";
                loader.hide();
                this.fetchDiscounts();
                $('#create-discount').modal('hide');
            } catch (error) {
                loader.hide();
                console.error('Lỗi khi tạo mã giảm giá:', error);
            }
        },
        async deleteDiscountExpired() {
            let loader = this.$loading.show({
                container: null,
                width: 100,
                height: 100,
                color: '#808EF4',
                loader: 'bars',
                canCancel: true,
            });
            try {
                await discountService.deleteExpired();
                setTimeout(() => {
                    loader.hide();
                }, 500);
                this.fetchDiscounts();
            } catch (error) {
                console.error('Lỗi khi xóa mã giảm giá:', error);
                setTimeout(() => {
                    loader.hide();
                }, 500);
            }
        }
    },
    mounted() {
        this.fetchContacts();
        this.fetchReviews();
        this.fetchDiscounts();
    }
}
</script>

<style scoped>
.card {
    box-shadow: 0 2px 6px 0 rgb(218 218 253 / 65%), 0 2px 6px 0 rgb(206 206 238 / 54%);
}

.list-title {
    border-top: 3px solid transparent;
    border-left: 3px solid transparent;
    border-right: 3px solid transparent;
    border-bottom: 3px solid #e0e0e0;
    padding: 10px 20px;
    cursor: pointer;
}

.active {
    border-top: 3px solid #e0e0e0;
    border-left: 3px solid #e0e0e0;
    border-right: 3px solid #e0e0e0;
    border-bottom: none;
    border-radius: 0.5rem 0.5rem 0 0;
}
</style>