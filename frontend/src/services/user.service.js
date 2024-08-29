import createApi from './api.service';

class UserService {
    constructor(baseUrl = "/api/auth"){
        this.api = createApi(baseUrl);
    }

    async register(registerDto) {
        try {
            const response = await this.api.post("/register", registerDto);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async login(loginDto) {
        try {
            const response = await this.api.post("/login", loginDto);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async profile() {
        try {
            const response = await this.api.get("/profile");
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async changePassword(changePasswordDto) {
        try {
            const response = await this.api.post("/change-password", changePasswordDto);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async getAll() {
        try {
            const response = await this.api.get("/all");
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

    async getByEmail(email) {
        try {
            const response = await this.api.get(`/email/${email}`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async update(userUpdateDto) {
        try {
            const response = await this.api.put("/update", userUpdateDto);
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
}

export default new UserService();