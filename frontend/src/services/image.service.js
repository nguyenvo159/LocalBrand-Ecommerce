import createApi from './api.service';

class ProductImageService {
    constructor(baseUrl = "/api/image"){
        this.api = createApi(baseUrl);
    }
    async uploadImage(images, id){
        try {
            const response = await this.api.post("/upload", images, id, {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
            });
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async uploadImagesNoVector(images, id){
        try {
            const response = await this.api.post("/upload-no-vector", images, id, {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
            });
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