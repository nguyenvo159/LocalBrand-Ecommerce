import createApi from './api.service';

class ProductImageService {
    constructor(baseUrl = "/api/image"){
        this.api = createApi(baseUrl);
    }
    async uploadImage(images, productId){
        try {
            const response = await this.api.post("/upload", images, {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
                params: { productId }
            });
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async uploadImagesNoVector(images, productId){
        try {
            const response = await this.api.post("/upload-no-vector", images, {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
                params: { productId }
            });
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async update(data){
        try {
            const response = await this.api.put("/update", data);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async searchByImage(image){
        try {
            const response = await this.api.post("/search-by-image", image, {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
            });
            return response.data;
        } catch (error) {
            throw error;
        }
    }
    
}

export default new ProductImageService();