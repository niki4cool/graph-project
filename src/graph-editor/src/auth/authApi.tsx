import { BASE_URL } from "vars";

export interface RegisterUser {
    userName: string;
    password: string;
}

export interface LoginUser {
    userName: string;
    password: string;
}

export interface LoginResponse {
    jwt: string;
}

export const userService = {
    login,
    logout,
    register,
};

export function authUser() {
    let user = JSON.parse(localStorage.getItem('user') as string);
    if (user === null)
        return {};
    return user;
}

export function authHeader() {
    let user = authUser();

    if (user && user.token) {
        return { 'Authorization': 'Bearer ' + user.token };
    } else {
        return {};
    }
}

function login(user: LoginUser) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    };

    return fetch(`${BASE_URL}/api/v1/User/login`, requestOptions)
        .then(handleResponse)
        .then(user => {
            localStorage.setItem('user', JSON.stringify(user));
            return user;
        });
}

function register(user: RegisterUser) {
    const requestOptions = {
        method: "PUT",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    };
    return fetch(`${BASE_URL}/api/v1/User/register`, requestOptions)
        .then(handleResponse);
}

function logout() {
    localStorage.removeItem('user');
}

function handleResponse(response: Response) {
    return response.text().then(text => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                logout();
            }
            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }
        return data;
    });
}