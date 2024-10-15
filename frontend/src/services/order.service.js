import createApi from './api.service';

class OrderService {
    constructor(baseUrl = "/api/order") {
        this.api = createApi(baseUrl);
    }

    async getAll() {
        try {
            const response = await this.api.get('/');
            return response.data;
        } catch (error) {
            console.error("Error fetching orders:", error);
            throw error; 
        }
    }

    async getByUserId(userId) {
        try {
            const response = await this.api.get(`/user/${userId}`);
            return response.data;
        } catch (error) {
            console.error("Error fetching orders for user:", error);
            throw error;
        }
    }

    async getById(id) {
        try {
            const response = await this.api.get(`/${id}`);
            return response.data;
        } catch (error) {
            console.error("Error fetching order by id:", error);
            throw error;
        }
    }

    async getPaging(data) {
        try {
            const response = await this.api.post('/paging', data);
            return response.data;
        } catch (error) {
            console.error("Error fetching orders with paging:", error);
            throw error;
        }
    }

    async create(data) {
        try {
            const response = await this.api.post('/', data);
            return response.data;
        } catch (error) {
            console.error("Error creating order:", error);
            throw error;
        }
    }

    async update(data) {
        try {
            const response = await this.api.put('/', data);
            return response.data;
        } catch (error) {
            console.error("Error updating order:", error);
            throw error;
        }
    }

    async delete(id) {
        try {
            const response = await this.api.delete(`/${id}`);
            return response.data;
        } catch (error) {
            console.error("Error deleting order:", error);
            throw error;
        }
    }
}

export default new OrderService();
