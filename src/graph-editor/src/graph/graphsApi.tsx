import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import {BASE_URL} from "vars";

export const graphsApi = createApi({
  reducerPath: "graphsApi",
  baseQuery: fetchBaseQuery({baseUrl: `${BASE_URL}/api/v1/graph`}),
  keepUnusedDataFor: 0,
  endpoints: builder => ({
    getGraph: builder.query<void, string>({
      query: (id) => id
    }),
    putGraph: builder.mutation<void, string>({
      query: id => ({
        url: id,
        method: "PUT"
      })
    }),
    deleteGraph: builder.mutation<void, string>({
      query: id => ({
        url: id,
        method: "DELETE"
      })
    })
  })
});