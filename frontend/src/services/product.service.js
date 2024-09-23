import createApi from './api.service';

class ProductService {
    constructor(baseUrl = "/api/product"){
        this.api = createApi(baseUrl);
    }

    async getAll() {
        try {
            const response = await this.api.get("/");
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async getById(id) {
        try {
            const response = await this.api.get(`/${id}`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async getPaging(data){
        try {
            const response = await this.api.post("/paging", data);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async getImageByProductId(id) {
        try {
            const response = await this.api.get(`/image/${id}`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async getByCategory(categoryName) {
        try {
            const response = await this.api.get(`/category/${categoryName}`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async getBestRate(){
        try {
            const response = await this.api.get("/rate");
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async getBestSeller(){
        try {
            const response = await this.api.get("/best-sell");
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async search(keyword) {
        try {
            const response = await this.api.get(`/search/${keyword}`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async create(productCreateDto) {
        try {
            const response = await this.api.post("/", productCreateDto);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async update(productUpdateDto) {
        try {
            const response = await this.api.put("/", productUpdateDto);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async delete(id) {
        try {
            const response = await this.api.delete(`/${id}`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async import(file) {
        try {
            const response = await this.api.post("/import", file, {
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

export default new ProductService();