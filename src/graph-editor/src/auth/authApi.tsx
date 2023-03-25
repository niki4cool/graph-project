import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { BASE_URL } from "vars";

export interface RegisterUser {
    userName: string;
    email: string;
    password: string;
}

export interface LoginUser {
    userName: string;
    password: string;
}

export interface LoginResponse {
    jwt: string;
}


export const authApi = createApi({
    reducerPath: "authApi",
    baseQuery: fetchBaseQuery({ baseUrl: `${BASE_URL}/api/v1/User` }),
    keepUnusedDataFor: 0,
    endpoints: builder => ({
        registerUser: builder.mutation<void, RegisterUser>({
            query(user) {
                return {
                    url: '/register',
                    method: "PUT",
                    body: user,
                }
            }
        }),
        loginUser: builder.mutation<LoginResponse, LoginUser>({
            query(user) {
                return {
                    url: '/login',
                    method: "POST",
                    body: user,
                }
            },
        })
    })
});