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
                                        <h4 class="my-1 text-info">{{ contacts?.totalRecords }}</h4>
                                        <p class="mb-0 font-13">+ liên hệ trong 24 giờ qua</p>
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
                                        <p class="mb-0 text-secondary">Chưa phản hồi</p>
                                        <h4 class="my-1 text-danger">{{ contactNoReply }}</h4>
                                        <p class="mb-0 font-13">+ phản hồi trong 24 giờ qua</p>
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
                                        <h4 class="my-1 text-success">{{ discounts.length }}</h4>
                                        <p class="mb-0 font-13">+ mã đã dùng 24 giờ qua</p>
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
                                        <h4 class="my-1 text-success">{{ reviews?.totalRecords }}</h4>
                                        <p class="mb-0 font-13">+ đánh giá tiêu cực 24 giờ qua</p>
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
                                    <li class="page-item" v-for="p in totalPages" :class="{ 'active': p == pageNumber }"
                                        @click="changePage(p)">
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
                            <div v-if="reviews" v-for="cmt in reviews.items"
                                class="card mb-3 radius-10 border-start border-0 border-5 border-warning">
                                <div class="card-body">
                                    <div class="d-flex align-items-center">
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
                                    </div>
                                </div>
                            </div>

                        </div>


                    </div>
                    <div v-if="display == 'discount'">
                        <div class="col-12">
                            <div class="mb-3 d-flex">
                                <button class="btn btn-primary"><i class="fa-solid fa-plus mr-1"></i> Thêm</button>
                                <button @click="exportDiscount" class="btn btn-success ml-3"><i
                                        class="fa-solid fa-file-export mr-1"></i>
                                    Export</button>
                            </div>
                            <div v-for="d in discounts"
                                class="card mb-3 radius-10 border-start border-0 border-5 border-success">
                                <div class="card-body" @click="toggleFooter(d.id, null)">
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
                                <div v-show="showFooter[d.id]" class="card-footer d-flex align-items-center">
                                    <input class="form-control w-50" type="email" v-model="d.email"
                                        placeholder="Nhập email người nhận...">
                                    <button class="my-2 ml-3 btn btn-primary" @click="sendDiscount(d)">Gởi</button>
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
import DashBoard from '@/components/admin/DashBoard.vue';
import contactService from '@/services/contact.service';
import reviewService from '@/services/review.service';
import discountService from '@/services/discount.service';
import { formatDate, set } from 'date-fns';
export default {
    components: {
        DashBoard,
    },
    data() {
        return {
            display: 'contact',
            contacts: null,
            contactNoReply: 0,
            reviews: null,
            discounts: [],
            pageNumber: 1,
            pageSize: 10,
            totalPages: 1,
            showFooter: {}
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
                    pageNumber: this.pageNumner,
                    pageSize: this.pageSize
                }
                this.reviews = await reviewService.getPaging(data);
                this.totalPages = this.reviews.totalPages;
            } catch (error) {
                console.error('Lỗi khi lấy danh sách đánh giá:', error);
            }
        },
        async fetchDiscounts() {
            try {
                this.discounts = await discountService.getAll();
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