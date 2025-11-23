import {useEffect, useState} from "react";
import {Booking, getAllRooms,  getFilteredBookings, Room} from "../api/AppApi.ts";
import '../css/BookingsPage.css';
import {useAppSelector} from "../redux/Hooks.tsx";
import BookingCardToPage from "./Components/BookingCardToPage.tsx";
import {useNavigate} from "react-router-dom";

export function BookingsPage() {
    const user = useAppSelector((state) => state.user);
    const hotel = useAppSelector((state)=>state.hotel);
    const navigate = useNavigate();
    const [FilteredBookings, setFilteredBookings] = useState<Booking[]>([]);
    const [Rooms, setRooms] = useState<Room[]>([]);
    const [errorMessage, setErrorMessage] = useState<string>("");

    useEffect(() => {
        (async () => {
            const data_4 = await getAllRooms();
            setRooms(data_4);
            console.log(data_4);
        })();
    }, [hotel.numberBookings]);

    useEffect(() => {
        (async () => {
            try {
                const formData = new FormData();
                if (user.userId != ""){
                    formData.append('UserId', user.userId);
                }
                else {
                    navigate("/");
                }
                const response = await getFilteredBookings(formData, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }
                const result: Booking[] = await response.json();
                console.error(result)
                setFilteredBookings(result);

                setErrorMessage('');
            } catch (error: any) {
                if (error.name === "401") {
                    setErrorMessage('Неверный токен');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    }, [hotel.numberBookings]);

    return (
        <div className="bookings-page">
            <h1 style={{display: 'flex', alignItems: 'Left', justifyContent: 'Left', gap: '5px'}}>Брони:</h1>
            <br/>
            <div className="err-message">
                {errorMessage !== "" && (
                    <b>{errorMessage}</b>
                )}
            </div>
            <div>
                {FilteredBookings.length > 0 ? (
                    <div>
                        {FilteredBookings.map((booking: Booking) => (
                            <li key={booking.BookingId}>
                                <BookingCardToPage BookingId={booking.BookingId} RoomId={booking.RoomId} CheckIn={booking.CheckIn} CheckOut={booking.CheckOut} Rooms={Rooms} />
                            </li>
                        ))}
                    </div>
                ) : (
                    <div>
                        <p>Нет броней.</p>
                    </div>
                )}
            </div>
        </div>
    );

}
export default BookingsPage;