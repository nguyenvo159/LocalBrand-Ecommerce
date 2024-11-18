import createApi from './api.service';

class DiscountService {
    constructor(baseUrl = "/api/discount"){
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

    async getByCode(code) {
        try {
            const response = await this.api.get(`/code/${code}`);
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

    async create(data){
        try {
            const response = await this.api.post("/", data);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async delete(id){
        try {
            const response = await this.api.delete(`/${id}`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async deleteExpired(){
        try {
            const response = await this.api.delete("/delete-expired");
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async export() {
        try {
            const response = await this.api.get("/export", {
                responseType: 'blob', 
            });
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', 'Discounts.xlsx'); 
            document.body.appendChild(link);
            link.click(); 
            document.body.removeChild(link); 
        } catch (error) {
            throw error;
        }
    }

    async sendMail(data) {
        try {
            const response = await this.api.post("/send", data);
            return response.data;
        } catch (error) {
            throw error;
        }
    }
}

export default new DiscountService();