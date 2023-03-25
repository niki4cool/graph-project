import { createSlice } from "@reduxjs/toolkit"
import { RootState } from "store";


export interface UserData {
    userName: string;
    token: string;
}

const initialState = {
    userName: "",
    token: "",
} as UserData;

const userDataSlice = createSlice({
    name: "userData",
    initialState,
    reducers: {
        login(state, action) {
            return action.payload;
        },
        logout(state, action) {
            return initialState
        }
    },
});

export default userDataSlice;
export const userTokenSelector = (state: RootState) => state.userData.token;
