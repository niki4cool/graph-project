import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { BASE_URL } from "vars";
import userDataSlice from "auth/authSlice";
import graphDataSelector from "auth/authSlice";


export const graphsApi = createApi({
    reducerPath: "graphsApi",
    baseQuery: fetchBaseQuery({ baseUrl: `${BASE_URL}/api/v1/graph` }),
    keepUnusedDataFor: 0,
    endpoints: builder => ({
        getGraph: builder.query<void, string>({
            query(id) {
                var bearer = "Bearer " + graphDataSelector;
                return {
                    url: id,
                    method: "GET",
                    headers: {
                        'Authorization': bearer,
                    }
                }
            } 
        }),
        putGraph: builder.mutation<void, string>({
            query(id) {
                var bearer = "Bearer " + graphDataSelector;
                return {
                    url: id,
                    method: "PUT",
                    headers: {
                        'Authorization': bearer,
                    }
                }
            }
        }),
        deleteGraph: builder.mutation<void, string>({
            query(id) {
                var bearer = "Bearer " + graphDataSelector;
                return {
                    url: id,
                    method: "DELETE",
                    headers: {
                        'Authorization': bearer,
                    }
                }
            }
        })
    })
});