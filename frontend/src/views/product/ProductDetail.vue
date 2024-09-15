<template>
    <div class="container">
        <div v-if="product" class="row">
            <div class="mt-3 mb-3 col-12">
                <h2>Sản phẩm
                    / {{ translatedCategory() }}
                </h2>
            </div>

            <!-- Ảnh  -->
            <div class="col-lg-7 col-12 mr-3">
                <div id="carouselExampleControls" class="carousel slide" data-ride="carousel" data-interval="4000">
                    <div class="carousel-inner">
                        <div v-for="(img, index) in product.imageUrls"
                            :class="['carousel-item', { active: index === 0 }]">
                            <img :src="img" class="w-100" style="object-fit: contain;" alt="image">
                        </div>
                    </div>
                    <button class="carousel-control-prev" type="button" data-target="#carouselExampleControls"
                        data-slide="prev">
                        <span class="" aria-hidden="true">
                            <i class="fa-solid fa-chevron-left" style="color: #000;"></i>
                        </span>
                        <span class="sr-only">Previous</span>
                    </button>
                    <button class="carousel-control-next" type="button" data-target="#carouselExampleControls"
                        data-slide="next">
                        <span class="" aria-hidden="true">
                            <i class="fa-solid fa-chevron-right" style="color: #000;"></i>
                        </span>
                        <span class="sr-only">Next</span>
                    </button>
                </div>
            </div>

            <div class="col-lg-4 col-12 mt-3">
                <h3 class="card-title">{{ product.name }}</h3>
                <div class="rating mt-2">
                    <span v-for="n in 5" :key="n" class="star">
                        <i :class="getStarClass(n, product.rating)" aria-hidden="true"></i>
                    </span>
                    <span class="text-muted">({{ product.rating }}) <i>{{ comment.length }} lượt đánh giá</i></span>
                </div>
                <hr>

                <h4><span class="price">{{ product.price }}đ</span></h4> <br>
                <div class="d-flex justify-content-around">
                    <div class="item-policy text-muted"><i class="fa-solid fa-repeat"></i> Đổi trả dễ dàng</div>
                    <div class="item-policy text-muted"><i class="fa-solid fa-check"></i> Chính hãng 100%</div>
                    <div class="item-policy text-muted"><i class="fa-solid fa-truck"></i> Giao toàn quốc</div>
                </div>

                <div class="mt-4">
                    <p class="m-0 font-new decreption" style="white-space: pre-line; ">Thông tin sản phẩm: <br>
                        {{ product.description }}</p>
                </div>

                <br>
                <div class="form-check p-0 mb-3">
                    <p class="pr-3 mb-2">Kích thước: </p>
                    <div v-if="product.categoryName !== 'accessory'" class="d-flex">
                        <div v-for="size in product.sizes" :key="size.name" class="ml-4">
                            <input required class="form-check-input d-none" type="radio" :id="size.name"
                                :value="size.name" :disabled="size.inventory === 0" @click="selectSize(size.name)"
                                v-model="selectedSize">
                            <label class="form-check-label size text-uppercase" :for="size.name"
                                :class="{ 'selected-size': selectedSize === size.name, 'disabled-size': size.inventory === 0 }">
                                {{ size.name }}
                            </label>
                        </div>
                    </div>

                    <div v-else class="ml-4">
                        <span>
                            <i>Free size</i>
                        </span>
                    </div>
                </div>
                <div v-if="errorSize">
                    <p class="error-feedback">Vui lòng chọn size.</p>
                </div>
                <p><a data-toggle="modal" data-target="#instruction"
                        class="cursor-pointer text-decoration-none error-feedback"> + Hướng dẫn chọn size</a></p>

                <p class="w-100 text-muted font-italic"><i class="fa-solid fa-truck-fast"></i> Miễn phí giao hàng cho
                    tất cả đơn hàng từ 300.000đ</p>

                <p>Số lượng:</p>
                <div class="row d-flex justify-content-center">
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-outline-dark border rounded-0 decrease-quantity"
                                @click="decreaseQuantity">
                                <i class="fas fa-minus"></i>
                            </button>
                        </div>
                        <input type="text" id="quantityDetail" name="quantity" class="m-0 form-control text-center"
                            value="1" @input="handleInput" min="1" max="100">
                        <div class=" input-group-append">
                            <button class="btn btn-outline-dark border rounded-0 increase-quantity"
                                @click="increaseQuantity">
                                <i class="fas fa-plus"></i>
                            </button>
                        </div>

                    </div>
                </div>

                <div v-if="inventory > 0" class="mt-3">
                    <button class="btn btn-dark py-3 px-5 rounded-0" @click="addToCart">Thêm vào giỏ</button>
                    <span class="ml-3 text-muted"><i>(Còn {{ inventory }})</i></span>
                </div>
                <div v-else class="mt-3">
                    <button class="btn btn-dark py-3 px-5 rounded-0">Hết hàng</button>
                    <span class="ml-3 text-muted"><i>(Còn {{ inventory }})</i></span>

                </div>
                <!-- </form> -->

                <br> <br>

            </div>

            <div class="col-lg-7 col-12 mt-5 row justify-content-center">
                <div class="w-100 mb-4 row justify-content-around">
                    <button class="col-5 btn border" :class="{ 'click-active': active == true }"
                        @click="clickActive(true)">MÔ TẢ</button>
                    <button class="col-5 btn border" :class="{ 'click-active': active == false }"
                        @click="clickActive(false)">ĐÁNH GIÁ</button>
                </div>
                <p v-if="active == true" class="p-2 pt-3 border-top text-justify font-new"><b>Hướng dẫn bảo quản sản
                        phẩm:</b> <br><br>

                    &#8722 Ngâm sản phẩm vào NƯỚC LẠNH có pha giấm hoặc phèn chua từ trong 2 tiếng đồng hồ<br><br>

                    &#8722 Giặt ở nhiệt độ bình thường, với đồ có màu tương tự.<br><br>

                    &#8722 Không dùng hóa chất tẩy.<br><br>

                    &#8722 Hạn chế sử dụng máy sấy và ủi (nếu có) thì ở nhiệt độ thích hợp.<br><br><br>


                    <b>Chính sách bảo hành:</b><br><br>

                    &#8722 Miễn phí đổi hàng cho khách mua ở shop trong trường hợp bị lỗi từ nhà sản xuất, giao nhầm
                    hàng, bị
                    hư
                    hỏng
                    trong quá trình vận chuyển hàng.<br><br>

                    &#8722 Sản phẩm đổi trong thời gian 3 ngày kể từ ngày nhận hàng<br><br>

                    &#8722 Sản phẩm còn mới nguyên tem, tags và mang theo hoá đơn mua hàng, sản phẩm chưa giặt và không
                    dơ
                    bẩn,
                    hư
                    hỏng bởi những tác nhân bên ngoài cửa hàng sau khi mua hàng.
                </p>

                <!-- Comment -->
                <div v-if="active == false" class="row justify-content-center font-new">
                    <div v-if="comment.length == 0" class="col-11 p-0 border row justify-content-center">
                        <div class="col-12 p-4 row justify-content-center bg-light">
                            <p class="w-100 text-center">Hiện tại chúng tôi chưa có đánh giá nào về sản phẩm này.</p>

                            <button class="btn btn-danger">Gửi đánh giá của bạn</button>
                        </div>

                    </div>
                    <div v-else class="col-11 py-4 border row justify-content-start align-items-start bg-light">
                        <div class="col-4 rating mt-2 text-center">
                            <h1>{{ product.rating }}/5 </h1>

                            <span v-for="n in 5" :key="n" class="star">
                                <i :class="getStarClass(n, product.rating)" aria-hidden="true"
                                    style="font-size: 24px;"></i>
                            </span>
                        </div>
                        <div class="col-8">
                            <button class="btn border mr-3 mt-2">Tất cả({{ comment.length }})</button>
                            <button class="btn border mr-3 mt-2">5 sao({{ countStar(5) }})</button>
                            <button class="btn border mr-3 mt-2">4 sao({{ countStar(4) }})</button>
                            <button class="btn border mr-3 mt-2">3 sao({{ countStar(3) }})</button>
                            <button class="btn border mr-3 mt-2">2 sao({{ countStar(2) }})</button>
                            <button class="btn border mr-3 mt-2">1 sao({{ countStar(1) }})</button>
                        </div>
                        <div class="mt-3">
                            <button class="btn btn-danger">Gửi đánh giá của bạn</button>

                        </div>
                    </div>
                    <div class="col-11 p-0 m-0 py-4 row justify-content-start align-items-start">
                        <div class="col-12 py-3 border-bottom" v-for="cmt in comment">
                            <div class="card border-0">
                                <h5 class="card-title mb-1 fw-bold">{{ cmt.userName }}</h5>
                                <div class="rating">
                                    <span v-for="n in 5" class="star">
                                        <i :class="getStarClass(n, cmt.rating)" aria-hidden="true"></i>
                                    </span>
                                </div>
                                <p class="card-text">{{ cmt.comment }}</p>
                            </div>
                        </div>
                    </div>


                </div>
            </div>

        </div>
        <div v-else>
            <p>Loading...</p>
        </div>
        <div class="modal fade" id="instruction" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="w-100 text-center" id="exampleModalLabel">Hướng dẫn chọn size</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <img class="w-100 img-fruid"
                            src="https://vuanem.com/blog/wp-content/uploads/2022/11/size-quan-ao-nam-owen.jpg" alt="">
                    </div>

                </div>
            </div>
        </div>
    </div>
</template>

<script>
import ProductService from "@/services/product.service";
import reviewService from "@/services/review.service";
// import CartService from "@/services/cart.service";

export default {
    data() {
        return {
            product: null,
            comment: [],
            selectedSize: 'free-size',
            errorSize: false,
            active: true,
            inventory: 0,
        };
    },
    methods: {
        translatedCategory() {
            switch (this.product.categoryName) {
                case 'shirt':
                    return 'Áo';
                case 'pant':
                    return 'Quần';
                case 'polo':
                    return 'Phụ kiện';
                default:
                    return this.product.category;
            }
        },
        countStar(i) {
            let count = 0;
            this.comment.forEach((c) => {
                if (c.rating === i) {
                    count++;
                }
            });
            return count;
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
        clickActive(key) {
            this.active = key;
        },
        selectSize(size) {
            if (this.getSizeInventory(size) === 0) {
                this.errorSize = true;
                return;
            }
            this.errorSize = false;
            this.selectedSize = size;
        },
        getSizeInventory(sizeName) {
            const size = this.product.sizes.find(s => s.name === sizeName);
            return size ? size.inventory : 0;
        },
        async addToCart() {
            console.log('add to cart');
        },

        handleInput() {
            const quantityInput = document.getElementById('quantityDetail');
            if (quantityInput < 1) {
                quantityInput.value = 1;
            }
            if (quantityInput.value > this.getSizeInventory(this.selectedSize)) {
                quantityInput.value = this.getSizeInventory(this.selectedSize);
            }
            if (quantityInput.value > 100) {
                quantityInput = 100;
            }
        },

        decreaseQuantity() {
            const quantityInput = document.getElementById('quantityDetail');
            let currentQuantity = parseInt(quantityInput.value);
            if (currentQuantity > 1) {
                currentQuantity--;
                quantityInput.value = currentQuantity;
            }
        },
        increaseQuantity() {
            100
            const quantityInput = document.getElementById('quantityDetail');
            let currentQuantity = parseInt(quantityInput.value);
            if (currentQuantity < 100) {
                currentQuantity++;
                quantityInput.value = currentQuantity;
            }

        },

    },
    async mounted() {
        try {
            const id = this.$route.params.id;
            this.product = await ProductService.getById(id);
            this.inventory = this.product.sizes.reduce((acc, size) => acc + size.inventory, 0);
            this.comment = await reviewService.getByProductId(id);
        } catch (error) {
            console.error(error);
        }
    },
};
</script>


<style>
#quantityDetail {
    max-width: 52px;
}

.size {
    display: table;
    width: 35px;
    height: 35px;
    line-height: 30px;
    text-align: center;
    border-radius: 50%;
    border: 1px solid #75757520;
    cursor: pointer;
    transition: ease 0.3s;
}

.selected-size {
    background-color: #000;
    color: #fff;
}

.disabled-size {
    background-color: #e0e0e0;
    color: #b0b0b0;
    cursor: not-allowed;
    pointer-events: none;
}

.font-new {
    font-family: 'Futura', sans-serif;
}

.item-policy {
    font-size: 15px;
    font-style: italic;
}

.description {
    line-height: 1.9;
}

.click-active {
    border: 1px #000 solid !important;
}

.star {
    margin-right: 2px;
    color: #FFD700;
}

.full-star {
    color: #FFD700;
}

.half-star {
    color: #FFD700;
}
</style>