import axios from "axios";


const getToken = () => {
    const token = localStorage.getItem('token');
    return token ? `Bearer ${token}` : '';
};

const commonConfig = {
    headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
        Authorization: getToken()
    },
};

export default (baseURL) => {
    return axios.create({
        baseURL,
        ...commonConfig,
    });
};