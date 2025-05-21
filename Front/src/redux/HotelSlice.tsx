import {createSlice, type PayloadAction} from '@reduxjs/toolkit'

export interface HotelState {
    hotelId: string;
    roomId: string;
    numberBookings : number;
}

const initialState: HotelState = {
    hotelId: "",
    roomId: "",
    numberBookings : 0
}

export const hotelSlice = createSlice({
    name: 'hotel',
    initialState,
    reducers: {
        setHotelId: (state, action: PayloadAction<string>) => {
            state.hotelId = action.payload;
        },
        setRoomId: (state, action: PayloadAction<string>) => {
            state.roomId = action.payload;
        },
        setNumberBookings: (state, action: PayloadAction<number>) => {
            state.numberBookings = action.payload;
        }
    }
})

export const {setHotelId, setRoomId, setNumberBookings} = hotelSlice.actions