import {setHotelId, setNumberBookings} from "../../redux/HotelSlice.tsx";

;import React from 'react';
import Card from 'react-bootstrap/Card';
import { DeleteBooking, Room} from "../../api/AppApi.ts";
import {useAppDispatch, useAppSelector} from "../../redux/Hooks.tsx";
import {useNavigate} from "react-router-dom";

interface BookingCardToPageProps {
    BookingId : string;
    RoomId: string;
    CheckIn: Date;
    CheckOut: Date;
    Rooms : Array<Room>;
}


const BookingCardToPage: React.FC<BookingCardToPageProps> = ({ BookingId, RoomId, CheckIn, CheckOut, Rooms }) => {
    const user = useAppSelector((state) => state.user);
    const hotel = useAppSelector((state)=>state.hotel);
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    // Находим комнату, у которой RoomId совпадает с переданным RoomId
    const selectedRoom = Rooms.find((room: Room) => room.RoomId === RoomId);
    const typeRoom = ["Обычный", "Люкс", "Президентский"]


    const handleGoToRoom = () => {
        if (selectedRoom) {
            dispatch(setHotelId(selectedRoom.HotelId));
            console.log(hotel.hotelId);
            navigate("/rooms");
        }
        else{
            navigate("/hotels");
        }

    }

    const handleDelete = () => {
        (async () => {
            dispatch(setNumberBookings(hotel.numberBookings + 1));
            console.log(hotel.numberBookings);
            await DeleteBooking(BookingId, user.token);
        })();
    }


    return (
        <Card body>
            <div style={{ display: 'flex', gap: '20px', alignItems: 'flex-start' }}>
                <div style={{ flex: 1 }}>
                    Номер комнаты : {selectedRoom ? selectedRoom.RoomNumber.toString() : ""}
                    <br/>
                    Тип комнаты : {selectedRoom ? typeRoom[selectedRoom.RoomType - 1] : ""}
                    <br/>
                    Цена за одну ночь : {selectedRoom ? selectedRoom.PricePerNight.toString() : ""}
                    <br/>
                    Дата заезда : {CheckIn.toString()}
                    <br/>
                    Дата выезда : {CheckOut.toString()}
                    <br/>
                </div>
                <div>
                    {selectedRoom ? <img
                        src={`http://localhost:5045/${selectedRoom.ImgUrl}`}
                        alt="Image"
                        style={{ width: '320px', height: '180px', }} // Размер можно изменить
                    /> : "Нет картинки"}
                </div>
            </div>
            <div style={{ display: 'flex', gap: '10px', marginTop: '10px' }}>
                <button className="update-button" onClick={handleGoToRoom}>Перейти к отелю</button>
                <button className="delete-button" onClick={handleDelete}>Удалить бронь</button>
            </div>
        </Card>
    );
}

export default BookingCardToPage;