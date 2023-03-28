import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import graphDataSlice from "graph/graphDataSlice";
import { graphsApi } from "graph/graphsApi";

const rootReducer = combineReducers({
    [graphDataSlice.name]: graphDataSlice.reducer,
    [graphsApi.reducerPath]: graphsApi.reducer,
});

const store = configureStore({
    reducer: rootReducer,
    middleware: getDefaultMiddleware =>
        getDefaultMiddleware().concat(graphsApi.middleware)
});

export type AppStore = typeof store
export type AppDispatch = AppStore["dispatch"]
export type RootState = ReturnType<typeof rootReducer>

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

export const dispatch = store.dispatch;

export default store;
