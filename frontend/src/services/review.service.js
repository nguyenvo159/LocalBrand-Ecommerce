import createApi from './api.service';

class ReviewService {
    constructor(baseUrl = "/api/review"){
        this.api = createApi(baseUrl);
    }

    async create(data){
        try {
            const response = await this.api.post('/', data);
            return response.data;
        }
        catch (error) {
            throw error;
        }
    }

    async update(data){
        try {
            const response = await this.api.put('/', data);
            return response.data;
        }
        catch (error) {
            throw error;
        }
    }

    async getById(id){
        try {
            const response = await this.api.get(`/${id}`);
            return response.data;
        }
        catch (error) {
            throw error;
        }
    }

    async getByProductId(id){
        try {
            const response = await this.api.get(`/list/${id}`);
            return response.data;
        }
        catch (error) {
            throw error;
        }
    }
    async delete(id){
        try {
            const response = await this.api.delete(`/${id}`);
            return response.data;
        }
        catch (error) {
            throw error;
        }
    }
}
export default new ReviewService();