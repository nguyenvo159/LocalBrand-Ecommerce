import createApi from './api.service';

class CartService {
    constructor(baseUrl = "/api/cart"){
        this.api = createApi(baseUrl);
    }

    async getCart(id) {
        try {
            const response = await this.api.get(`/${id}`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async getByUserId(userId) {
        try {
            const response = await this.api.get(`/user/${userId}`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async addProductToCart(data){
        try {
            const response = await this.api.post("/", data);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async updateProductInCart(data){
        try {
            const response = await this.api.put("/", data);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async deleteItemCart(id){
        try {
            const response = await this.api.delete(`/cartitem/${id}`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async deleteCart(id){
        try {
            const response = await this.api.delete(`/${id}`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }
}

export default new CartService();