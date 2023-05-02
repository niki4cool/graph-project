import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { BASE_URL } from "vars";
import { authHeader } from "../auth/authApi";

export const graphsApi = createApi({
    reducerPath: "graphsApi",
    baseQuery: fetchBaseQuery({ baseUrl: `${BASE_URL}/api/v1/graph` }),
    keepUnusedDataFor: 0,
    endpoints: builder => ({
        getGraph: builder.query<void, string>({
            query(id) {
                return {
                    url: id,
                    method: "GET",
                    headers: authHeader()
                }
            }
        }),
        putGraph: builder.mutation<void, string>({
            query(id) {
                return {
                    url: id,
                    body: { "graphTypeStr": "Regular" },
                    method: "PUT",
                    headers: authHeader()
                }
            }
        }),
        deleteGraph: builder.mutation<void, string>({
            query(id) {
                return {
                    url: id,
                    method: "DELETE",
                    headers: authHeader()
                }
            }
        }),
        getGraphs: builder.query<string[], void>({
            query() {
                return {
                    url: "list",
                    method: "GET",
                    headers: authHeader()
                }
            }
        })
    })
});