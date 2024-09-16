<template>
    <div class="modal fade" :id="modalId" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">{{ title }}</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form @submit.prevent="submitImage">
                        <!-- Image -->
                        <div class="row">
                            <div class="col-12 form-group row align-items-center">
                                <label for="name" class="col-2 m-0">ID: </label>
                                <input type="text" class="form-control col-9" id="name" name="name"
                                    v-model="productId" />
                            </div>
                            <div class="col-lg-6 form-group">
                                <label for="image">Ảnh</label>
                                <input type="file" class="form-control" id="image" name="image" multiple
                                    @change="handleFileChange" />
                                <small class="text-muted">Tối đa 4 ảnh.</small>
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
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" @click="resetForm"
                                data-dismiss="modal">Hủy</button>
                            <button type="submit" class="btn btn-primary">Lưu</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import ProductImageService from '@/services/image.service';

export default {
    props: {
        product: { type: Object, required: false, default: () => null },
        modalId: { type: String, required: true },
        title: { type: String, required: true }
    },
    data() {
        return {
            productId: null,
            imageFiles: [],
            imagePreviews: [],
        };
    },
    methods: {
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
        async submitImage() {
            if (this.imageFiles.length === 0) {
                alert('Chưa có ảnh nào để upload.');
                return;
            }
            const formData = new FormData();
            this.imageFiles.forEach(file => {
                formData.append('files', file);
            });

            try {
                await ProductImageService.uploadImagesNoVector(formData, this.productId);
                this.resetForm();
            } catch (error) {
                console.error('Lỗi khi upload ảnh:', error);
            }
        },
        resetForm() {
            this.imageFiles = [];
            this.imagePreviews = [];
        },
    },
};
</script>

<style scoped>
.image-preview-wrapper {
    position: relative;
    display: inline-block;
    cursor: pointer;
}

.delete-icon {
    font-size: 35px;
    position: absolute;
    top: 50%;
    right: 50%;
    transform: translate(50%, -50%);
    color: white;
    padding: 5px;
    cursor: pointer;
    display: none;
}

.image-preview-wrapper:hover .delete-icon {
    display: block;
}

.image-preview-wrapper:hover img {
    opacity: 0.6;

}

.image-preview-wrapper img {
    max-width: 100%;
    height: auto;
}
</style>