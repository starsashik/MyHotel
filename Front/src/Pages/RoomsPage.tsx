import React, {FormEvent, useEffect, useState} from "react";
import {
    CreateBooking,
    getFilteredRooms,
    Room
} from "../api/AppApi.ts";
import '../css/RoomsPage.css';
import {useAppDispatch, useAppSelector} from "../redux/Hooks.tsx";
import {Button} from "react-bootstrap";
import RoomCardToPage from "./Components/RoomCardToPage.tsx";
import {setRoomId} from "../redux/HotelSlice.tsx";

export function BookingsPage() {
    const user = useAppSelector((state) => state.user);
    const hotel = useAppSelector((state) => state.hotel);
    const dispatch = useAppDispatch();

    const [FilteredRooms_1, setFilteredRooms_1] = useState<Room[]>([]);
    const [FilteredRooms_2, setFilteredRooms_2] = useState<Room[]>([]);
    const [FilteredRooms_3, setFilteredRooms_3] = useState<Room[]>([]);

    const [DateInToCreate, setDateInToCreate] = useState<Date>(new Date());
    const [DateOutToCreate, setDateOutToCreate] = useState<Date>(new Date());

    const [errorMessage, setErrorMessage] = useState<string>("");

    useEffect(() => {
        (async () => {
            try {
                const formData = new FormData();
                formData.append('HotelId', hotel.hotelId);
                formData.append('RoomType', "1");
                const response = await getFilteredRooms(formData, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }
                const result: Room[] = await response.json();
                console.error(result)
                setFilteredRooms_1(result);

                setErrorMessage('');
            } catch (error: any) {
                if (error.name === "401") {
                    setErrorMessage('Неверный токен');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    }, []);
    useEffect(() => {
        (async () => {
            try {
                const formData = new FormData();

                formData.append('HotelId', hotel.hotelId);
                formData.append('RoomType', "2");
                const response = await getFilteredRooms(formData, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }
                const result: Room[] = await response.json();
                console.error(result)
                setFilteredRooms_2(result);

                setErrorMessage('');
            } catch (error: any) {
                if (error.name === "401") {
                    setErrorMessage('Неверный токен');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    }, []);
    useEffect(() => {
        (async () => {
            try {
                const formData = new FormData();

                formData.append('HotelId', hotel.hotelId);
                formData.append('RoomType', "3");
                const response = await getFilteredRooms(formData, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }
                const result: Room[] = await response.json();
                console.error(result)
                setFilteredRooms_3(result);

                setErrorMessage('');
            } catch (error: any) {
                if (error.name === "401") {
                    setErrorMessage('Неверный токен');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    }, []);

    const handleSubmit_createBooking = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            dispatch(setRoomId(""));
            try {
                const formData = new FormData();
                formData.append('UserId', user.userId);
                formData.append('RoomId', hotel.roomId);
                formData.append('CheckIn', formatDateForInput(DateInToCreate).toString());
                formData.append('CheckOut', formatDateForInput(DateOutToCreate).toString());

                const response = await CreateBooking(formData, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }
                alert("Вы забронировали номер!")
                setErrorMessage('');
            } catch (error: any) {
                if (error.name === "401" || error.name === "400") {
                    setErrorMessage('Неверные данные.');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleDateChangeIn = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newDate = new Date(e.target.value);
        if (!isNaN(newDate.getTime())) { // Проверяем, что дата валидная
            setDateInToCreate(newDate);
        }
    };

    const handleDateChangeOut = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newDate = new Date(e.target.value);
        if (!isNaN(newDate.getTime())) { // Проверяем, что дата валидная
            setDateOutToCreate(newDate);
        }
    };

    const formatDateForInput = (date: Date): string => {
        return date.toISOString().split('T')[0]; // Например, "2025-05-20"
    };

    return (
        <div className="rooms-page">
            <h1 style={{display: 'flex', alignItems: 'center', justifyContent: 'center', gap: '5px'}}>Панель
                администратора</h1>
            <br/>
            <div className="err-message">
                {errorMessage !== "" && (
                    <b>{errorMessage}</b>
                )}
            </div>
            <div style={{display: 'flex', gap: '50px', alignItems: 'flex-start'}}>
                <div style={{flex: 1}}>
                    <h1>Обычные номера:</h1>
                    <div>
                            {FilteredRooms_1.length > 0 ? (
                                <div>
                                    {FilteredRooms_1.map((room: Room) => (
                                        <li key={room.RoomId}>
                                            <RoomCardToPage RoomId={room.RoomId} RoomNumber={room.RoomNumber} PricePerNight={room.PricePerNight} ImgUrl={room.ImgUrl} />
                                        </li>
                                    ))}
                                </div>
                            ) : (
                                <div>
                                    <p>Нет номеров.</p>
                                </div>
                            )}
                        </div>
                    <br/>
                    <h1>Люкс номера:</h1>
                    <div>
                        {FilteredRooms_2.length > 0 ? (
                            <div>
                                {FilteredRooms_2.map((room: Room) => (
                                    <li key={room.RoomId}>
                                        <RoomCardToPage RoomId={room.RoomId} RoomNumber={room.RoomNumber} PricePerNight={room.PricePerNight} ImgUrl={room.ImgUrl} />
                                    </li>
                                ))}
                            </div>
                        ) : (
                            <div>
                                <p>Нет номеров.</p>
                            </div>
                        )}
                    </div>
                    <br/>
                    <h1>Президентские номера:</h1>
                    <div>
                        {FilteredRooms_3.length > 0 ? (
                            <div>
                                {FilteredRooms_3.map((room: Room) => (
                                    <li key={room.RoomId}>
                                        <RoomCardToPage RoomId={room.RoomId} RoomNumber={room.RoomNumber} PricePerNight={room.PricePerNight} ImgUrl={room.ImgUrl} />
                                    </li>
                                ))}
                            </div>
                        ) : (
                            <div>
                                <p>Нет номеров.</p>
                            </div>
                        )}
                    </div>
                </div>
                <div style={{flex: 0.5}}>
                    {hotel.roomId != "" && user.userId != "" ? (
                        <div>
                            <h4>Создание брони</h4>
                            <form className="form" onSubmit={handleSubmit_createBooking}>
                                <div className="form-group">
                                    <label>
                                        Дата въезда
                                    </label>
                                    <input
                                        type="date"
                                        placeholder="Дата"
                                        value={formatDateForInput(DateInToCreate)}
                                        onChange={handleDateChangeIn}
                                        required
                                    />
                                </div>
                                <div className="form-group">
                                    <label>
                                        Дата выезда
                                    </label>
                                    <input
                                        type="date"
                                        placeholder="Дата"
                                        value={formatDateForInput(DateOutToCreate)}
                                        onChange={handleDateChangeOut}
                                        required
                                    />
                                </div>
                                <Button type="submit" className="update-button">
                                    Создать бронь
                                </Button>
                            </form>
                        </div>
                    ) : (
                        <div>
                            <p>Выберите комнату</p>
                        </div>
                    )}

                </div>
            </div>
        </div>
    );

}

export default BookingsPage;