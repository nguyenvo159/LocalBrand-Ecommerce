import createApi from './api.service';

class ContactService {
    constructor(baseUrl = "/api/contact"){
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

    async getPaging(data) {
        try {
            const response = await this.api.post("/paging", data);
            return response.data;
        } catch (error) {
            throw error;
        }
        
    }

    async create(data) {
        try {
            const response = await this.api.post("/create", data);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async update(data) {
        try {
            const response = await this.api.put("/update", data);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async delete(id) {
        try {
            const response = await this.api.delete(`/delete/${id}`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }
}

export default new ContactService();