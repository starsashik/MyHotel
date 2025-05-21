import {createSlice, type PayloadAction} from '@reduxjs/toolkit'

export interface UserState {
    isLoggedIn: boolean;
    email: string;
    accessLvl: number;
    token: string;
    userId: string;
}

const initialState: UserState = {
    isLoggedIn: false,
    email: "",
    accessLvl: -1,
    token: "",
    userId: ""
}

export const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        setIsLoggedIn: (state, action: PayloadAction<boolean>) => {
            state.isLoggedIn = action.payload;
        },
        setEmail: (state, action: PayloadAction<string>) => {
            state.email = action.payload;
        },
        setAccessLvl: (state, action: PayloadAction<number>) => {
            state.accessLvl = action.payload;
        },
        setToken: (state, action: PayloadAction<string>) => {
            state.token = action.payload;
        },
        setUserId: (state, action: PayloadAction<string>) => {
            state.userId = action.payload;
        }
    }
})

export const {setIsLoggedIn, setEmail, setAccessLvl, setToken, setUserId} = userSlice.actions