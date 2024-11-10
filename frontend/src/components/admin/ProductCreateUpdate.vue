<template>
    <div class="modal fade" :id="modalId" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="w-100 modal-title text-uppercase text-center py-3" id="exampleModalLabel">{{ title }}
                    </h3>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <Form @submit="submitProduct" :validation-schema="productFormSchema">
                        <div class="row">
                            <div class="col-lg-6 form-group">
                                <label for="name">Tên sản phẩm</label>
                                <Field class="form-control" name="name" v-model="productLocal.name" type="text"
                                    label="Tên sản phẩm" />
                                <ErrorMessage class="error-feedback" name="name" />
                            </div>

                            <div class="col-lg-6 form-group">
                                <label for="category">Loại sản phẩm</label>
                                <Field class="form-control text-capitalize" name="category"
                                    v-model="productLocal.categoryId" as="select" label="Loại sản phẩm">
                                    <option v-for="category in categories" :value="category.id" class="text-capitalize">
                                        {{ category.name }}</option>
                                </Field>
                                <ErrorMessage class="error-feedback" name="category" />
                            </div>

                            <div class="col-lg-6 form-group">
                                <label for="price">Giá</label>
                                <Field class="form-control" name="price" v-model.number="productLocal.price"
                                    type="number" label="Giá sản phẩm" />
                                <ErrorMessage class="error-feedback" name="price" />
                            </div>

                            <div class="col-12 form-group">
                                <label>Chọn kích thước và số lượng</label>
                                <div v-if="isFreesize">
                                    <div class="form-check d-flex align-items-center">
                                        <input type="checkbox" id="freesize" v-model="selectedSizes.freesize"
                                            class="form-check-input" />
                                        <label for="freesize" class="form-check-label p-2">Freesize</label>
                                        <input v-if="selectedSizes.freesize" type="number" class="form-control w-25"
                                            v-model="inventory.freesize" placeholder="Nhập số lượng" />
                                    </div>
                                </div>

                                <div v-else>
                                    <div class="form-check">
                                        <table>
                                            <tr v-for="size in sizeOptions" :key="size">
                                                <td class="align-middle d-flex align-items-center">
                                                    <input type="checkbox" :id="size" v-model="selectedSizes[size]"
                                                        class="form-check-input mb-1" />
                                                    <label :for="size" class="form-check-label p-2">{{
                                                        size.toUpperCase() }}</label>
                                                </td>
                                                <td>
                                                    <input v-if="selectedSizes[size]" type="number"
                                                        class="form-control w-50" v-model="inventory[size]"
                                                        placeholder="Nhập số lượng" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>

                            <div class="col-12 form-group">
                                <label for="description">Mô tả</label>
                                <Field class="form-control" name="description" as="textarea" rows="5"
                                    v-model="productLocal.description" label="Mô tả" />
                                <ErrorMessage class="error-feedback" name="description" />
                            </div>
                        </div>
                    </Form>
                    <hr>
                    <!-- Image -->

                    <form @submit.prevent="submitImage">
                        <!-- Image -->
                        <div class="row">
                            <div class="col-lg-6 form-group">
                                <label for="image" class="h5 py-2">Upload Ảnh</label>
                                <input type="file" class="form-control" id="image" name="image" multiple
                                    @change="handleFileChange" />
                                <div class="form-check d-flex align-items-center">
                                    <input type="checkbox" id="vector" class="form-check-input"
                                        v-model="isVectorized" />
                                    <label for="vector" class="form-check-label p-2">Hỗ trợ tìm kiếm bằng hình
                                        ảnh</label>
                                </div>
                                <small class="text-muted"><i>Tối đa 4 ảnh.</i></small>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-3" v-for="(image, index) in imagePreviews" :key="index">
                                <div class="image-preview-wrapper">
                                    <img :src="image" class="img-thumbnail" alt="Image Preview" />
                                    <span class="delete-icon" @click="removeImage(index)">
                                        <i class="fa-solid fa-trash"></i>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </form>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" @click="resetForm"
                            data-dismiss="modal">Hủy</button>
                        <button type="" @click="submitProduct" class="btn btn-primary">Lưu</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import * as yup from 'yup';
import { Form, Field, ErrorMessage } from 'vee-validate';
import ProductService from '@/services/product.service';
import ProductImageService from '@/services/image.service';
export default {
    components: {
        Form,
        Field,
        ErrorMessage,
    },
    data() {
        return {
            modalId: this.modalId,
            productLocal: { ...this.product },
            selectedSizes: {
                freesize: false,
                s: false,
                m: false,
                l: false,
                xl: false,
                xxl: false,
            },
            inventory: {
                freesize: 0,
                s: 0,
                m: 0,
                l: 0,
                xl: 0,
                xxl: 0,
            },
            productFormSchema: yup.object().shape({
                name: yup.string().required('Tên phải có giá trị.').min(2, 'Tên phải ít nhất 2 ký tự.').max(100, 'Tên có nhiều nhất 100 ký tự.'),
                category: yup.string().required('Loại sản phẩm là bắt buộc.'),
                price: yup.number(),
                inventory: yup.number().min(1, "Số lượng tối thiểu là 1.").max(999, 'Không thể vượt quá 999.'),
                description: yup.string().max(1000, 'Mô tả tối đa 1000 ký tự.'),
                imgURL: yup.string().required('Link ảnh là bắt buộc.').matches(/\.(jpg|jpeg|png)$/, 'Link ảnh phải có định dạng hợp lệ (jpg, jpeg, png).'),
            }),
            imageFiles: [],
            imagePreviews: [],
            isVectorized: false,
        };
    },
    computed: {
        categories() {
            return this.$store.getters.getCategories;
        },
        isFreesize() {
            return this.productLocal.categoryId === '9e9fcc33-66bb-44ac-8862-3a20fecb8cf6';
        },
        sizeOptions() {
            return ['s', 'm', 'l', 'xl', 'xxl'];
        }
    },
    props: {
        product: { type: Object, required: false, default: () => null },
        submitFunction: { type: Function, required: true },
        modalId: { type: String, required: true },
        title: { type: String, required: true }
    },
    watch: {
        product: {
            handler(newVal) {
                this.productLocal = { ...newVal };
                this.selectedSizes = {
                    freesize: false,
                    s: false,
                    m: false,
                    l: false,
                    xl: false,
                    xxl: false,
                };
                this.inventory = {
                    freesize: 0,
                    s: 0,
                    m: 0,
                    l: 0,
                    xl: 0,
                    xxl: 0,
                };

                // Nếu product có kích thước
                if (newVal && newVal.sizes) {
                    newVal.sizes.forEach(size => {
                        if (size.name === 'freesize') {
                            this.selectedSizes.freesize = true;
                            this.inventory.freesize = size.inventory;
                        } else {
                            this.selectedSizes[size.name] = true;
                            this.inventory[size.name] = size.inventory;
                        }
                    });
                }
                if (newVal && newVal.imageUrls) {
                    this.imagePreviews = newVal.imageUrls;
                }
            },
            deep: true
        }
    },
    methods: {
        async submitProduct() {
            let loader = this.$loading.show({
                container: null,
                width: 100,
                height: 100,
                color: '#808EF4',
                loader: 'bars',
                canCancel: true,
            });
            try {
                var sizes = Object.keys(this.selectedSizes)
                    .filter(size => this.selectedSizes[size])
                    .map(size => ({
                        name: size,
                        inventory: this.inventory[size]
                    }));
                this.productLocal.sizes = sizes;
                var product = null;

                if (this.modalId == 'add-product') {
                    product = await ProductService.create(this.productLocal);
                }
                if (this.modalId == 'update-product') {
                    product = await ProductService.update(this.productLocal);
                }
                if (!product && this.modalId == 'add-product') {
                    alert('Có lỗi xảy ra khi lưu sản phẩm.');
                }
                await this.submitImage(product.id);
                loader.hide();
                this.$emit('close');

            } catch (error) {
                console.error('Lỗi khi lưu sản phẩm:', error);
                loader.hide();
                this.$emit('close');
            }
        },
        handleFileChange(event) {
            const files = Array.from(event.target.files);

            if (this.imageFiles.length + files.length > 4) {
                alert('Chỉ có thể upload tối đa 4 ảnh.');
                return;
            }

            files.forEach(file => {
                const reader = new FileReader();
                reader.onload = (e) => {
                    this.imagePreviews.push(e.target.result);
                    this.imageFiles.push(file);
                };
                reader.readAsDataURL(file);
            });
        },
        removeImage(index) {
            this.imagePreviews.splice(index, 1);
            this.imageFiles.splice(index, 1);
        },
        async submitImage(productId) {
            // if (this.imageFiles.length === 0) {
            //     // alert('Chưa có ảnh nào để upload.');
            //     return;
            // }
            const formData = new FormData();
            this.imageFiles.forEach(file => {
                formData.append('files', file);
            });

            try {
                if (this.isVectorized) {
                    await ProductImageService.uploadImage(formData, productId);
                } else {
                    await ProductImageService.uploadImagesNoVector(formData, productId);
                }
                this.resetForm();
            } catch (error) {
                console.error('Lỗi khi upload ảnh:', error);
            }
        },
        resetForm() {
            this.productLocal = { ...this.product };
            this.selectedSizes = {
                freesize: false,
                s: false,
                m: false,
                l: false,
                xl: false,
                xxl: false,
            };
            this.inventory = {
                freesize: 0,
                s: 0,
                m: 0,
                l: 0,
                xl: 0,
                xxl: 0,
            };
            if (this.productLocal && this.productLocal.sizes) {
                this.productLocal.sizes.forEach(size => {
                    if (size.name === 'freesize') {
                        this.selectedSizes.freesize = true;
                        this.inventory.freesize = size.inventory;
                    } else {
                        this.selectedSizes[size.name] = true;
                        this.inventory[size.name] = size.inventory;
                    }
                });
            }
            this.imageFiles = [];
            this.imagePreviews = [];
        },
    },
    mounted() {

    },
};
</script>