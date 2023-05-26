import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { BASE_URL } from "vars";
import { authHeader } from "../auth/authApi";
import { Graph } from "./graphDataSlice";

export const graphsApi = createApi({
    reducerPath: "graphsApi",
    baseQuery: fetchBaseQuery({ baseUrl: `${BASE_URL}/api/v1/graph` }),
    keepUnusedDataFor: 0,
    endpoints: builder => ({
        getGraph: builder.query<void, string>({
            query(id) {
                return {
                    url: "id/" + id,
                    method: "GET",
                    headers: authHeader()
                }
            }
        }),
        putGraph: builder.mutation<void, { id: string, type: string, classId: string}>({
            query(data) {
                return {
                    url: "id/" + data.id,
                    body: { "graphType": data.type, "GraphClassId": data.classId },
                    method: "PUT",
                    headers: authHeader()
                }
            }
        }),
        deleteGraph: builder.mutation<void, string>({
            query(id) {
                return {
                    url: "id/" + id,
                    method: "DELETE",
                    headers: authHeader()
                }
            }
        }),
        getGraphs: builder.query<Graph[], void>({
            query: () => ({
                url: "list",
                method: "GET",
                headers: authHeader()
            }),
            transformResponse: (response: { graphs: string[] }) => {
                console.log(response);
                return response.graphs.map(j => JSON.parse(j) as Graph);
            }
        })
    })
});