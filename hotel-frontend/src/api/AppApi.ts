import axios from "axios";

const baseURL = 'http://localhost:5045';

export const appApiIns = axios.create({
    baseURL: 'http://localhost:5045',
    headers: {
        'Content-Type': 'application/json'
    },
    withCredentials: true
})

// Models
export interface User{
    UserId : string;
    Email: string;
    Name: string;
    Role: string;
    ImgUrl : string;
}
export interface Role{
    Id: string;
    Name: string;
    AccessLevel: number;
}

export interface Hotel{
    HotelId : string;
    Name : string;
    Location : string;
    Description: string;
    ImgUrl : string;
}

export interface Room{
    RoomId : string;
    RoomNumber : number;
    RoomType : number;
    PricePerNight : number;
    ImgUrl : string;
    HotelId : string;
}

export interface Booking{
    BookingId : string;
    UserId : string;
    RoomId : string;
    CheckIn : Date;
    CheckOut : Date;
}

// Responses
export interface CurrentUserResponse {
    Token : string;
    Name : string;
    Email : string;
    AsseccLvl : number;
    ImgUrl : string;
    UserId : string;
}

export interface LoginResponse {
    token: string;
}

export interface RegistrationResponse {
    token: string;
}


// Authorization
export function LoginUser(email: string, password: string){
    const payload = { email, password };
    return appApiIns.post<LoginResponse>('/Authorization/LoginUser', payload);
}

export function RegistrationUser(name: string, email: string, password: string){
    const payload = { name, email, password };
    return appApiIns.post<RegistrationResponse>('/Authorization/RegisterUser', payload);
}

export function GetCurrentUser(){
    return appApiIns.get<CurrentUserResponse>('/Authorization/GetCurrentDataUser', { withCredentials: true});
}

export function LogoutUser(){
    return  appApiIns.post('/Authorization/LogoutUser');
}

export function SetEmptyCookiesApi() {
    return appApiIns.post("/Authorization/SetEmptyCookies");
}


// Roles
export const getRoles = async () => {
    try {
        const response = await appApiIns.get<Role[]>('/Roles/TestGetRoles');
        return response.data;
    } catch (error: any) {
        return error;
    }
};

export function CreateRole(name: string, accessLevel: number, token : string){
    const requestOptions = {
        method: "POST",
        headers: {"Content-Type" : "application/json",
            "Authorization" : `Bearer ${token}`},
        body: JSON.stringify({
            "Name": name,
            "AccessLevel": accessLevel
        })
    };
    return fetch(`${baseURL}/Roles/CreateRole`, requestOptions)
}

export function DeleteRole(roleId: string, token : string){
    const requestOptions = {
        method: "DELETE",
        headers: {"Content-Type" : "application/json",
            "Authorization" : `Bearer ${token}`}
        }
    return fetch(`${baseURL}/Roles/DeleteRole?RoleId=${roleId}`, requestOptions)
}


// Users
export const getUsers = async () => {
    try {
        const response = await appApiIns.get<User[]>('/Users/TestGetUsers');
        return response.data;
    } catch (error: any) {
        return error;
    }
};

export function CreateUser(name: string, email: string, password: string, role: string, token : string){
    const requestOptions = {
        method: "POST",
        headers: {"Content-Type" : "application/json",
            "Authorization" : `Bearer ${token}`},
        body: JSON.stringify({
            "name": name,
            "email": email,
            "password": password,
            "role": role
        })
    };
    return fetch(`${baseURL}/Users/CreateUser`, requestOptions)
}

export function UpdateUser(data: FormData, token : string){
    const requestOptions = {
        method: "POST",
        headers: {"Authorization" : `Bearer ${token}`},
        body: data
    }
    return fetch(`${baseURL}/Users/UpdateUserSmallParam`, requestOptions)
}

export function DeleteUser(userId: string, token : string){
    const requestOptions = {
        method: "DELETE",
        headers: {"Content-Type" : "application/json",
            "Authorization" : `Bearer ${token}`}
    }
    return fetch(`${baseURL}/Users/DeleteUser?UserId=${userId}`, requestOptions)
}


// Hotels
export const getHotels = async () => {
    try {
        const response = await appApiIns.get<Hotel[]>('/Hotels/TestGetHotels');
        return response.data;
    }  catch (error: any)
    {
        return error;
    }
};

export function CreateHotel(data: FormData, token : string){
    const requestOptions = {
        method: "POST",
        headers: {"Authorization" : `Bearer ${token}`},
        body: data
    }
    return fetch(`${baseURL}/Hotels/CreateHotel`, requestOptions)
}

export function UpdateHotel(data: FormData, token : string){
    const requestOptions = {
        method: "POST",
        headers: {"Authorization" : `Bearer ${token}`},
        body: data
    }
    return fetch(`${baseURL}/Hotels/UpdateHotel`, requestOptions)
}

export function DeleteHotel(hotelId: string, token : string){
    const requestOptions = {
        method: "DELETE",
        headers: {"Content-Type" : "application/json",
            "Authorization" : `Bearer ${token}`}
    }
    return fetch(`${baseURL}/Hotels/DeleteHotel?HotelId=${hotelId}`, requestOptions)
}


// Rooms
export const getAllRooms = async () => {
    try {
        const response = await appApiIns.get<Room[]>('/Rooms/TestGetRooms');
        return response.data;
    }  catch (error: any)
    {
        return error;
    }
};

export function getFilteredRooms(data: FormData, token : string){
    const requestOptions = {
        method: "POST",
        headers: {"Authorization" : `Bearer ${token}`},
        body: data
    }
    return fetch(`${baseURL}/Rooms/GetFilteredRooms`, requestOptions)

}

export function CreateRoom(data: FormData, token : string){
    const requestOptions = {
        method: "POST",
        headers: {"Authorization" : `Bearer ${token}`},
        body: data
    }
    return fetch(`${baseURL}/Rooms/CreateRoom`, requestOptions)
}

export function UpdateRoom(data: FormData, token : string){
    const requestOptions = {
        method: "POST",
        headers: {"Authorization" : `Bearer ${token}`},
        body: data
    }
    return fetch(`${baseURL}/Rooms/UpdateRoom`, requestOptions)
}

export function DeleteRoom(roomId: string, token : string){
    const requestOptions = {
        method: "DELETE",
        headers: {"Content-Type" : "application/json",
            "Authorization" : `Bearer ${token}`}
    }
    return fetch(`${baseURL}/Rooms/DeleteRoom?RoomId=${roomId}`, requestOptions)
}


// Booking
export const getAllBooking = async () => {
    try {
        const response = await appApiIns.get<Booking[]>('/Bookings/TestGetBookings');
        return response.data;
    }  catch (error: any)
    {
        return error;
    }
};

export function getFilteredBookings(data: FormData, token : string){
    const requestOptions = {
        method: "POST",
        headers: {"Authorization" : `Bearer ${token}`},
        body: data
    }
    return fetch(`${baseURL}/Bookings/GetFilteredBookings`, requestOptions)
}

export function CreateBooking(data: FormData, token : string){
    const requestOptions = {
        method: "POST",
        headers: {"Authorization" : `Bearer ${token}`},
        body: data
    }
    return fetch(`${baseURL}/Bookings/CreateBooking`, requestOptions)
}

export function DeleteBooking(bookingId: string, token : string){
    const requestOptions = {
        method: "DELETE",
        headers: {"Content-Type" : "application/json",
            "Authorization" : `Bearer ${token}`}
    }
    return fetch(`${baseURL}/Bookings/DeleteBooking?BookingId=${bookingId}`, requestOptions)
}