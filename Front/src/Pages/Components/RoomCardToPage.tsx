import React, {useEffect, useState} from 'react';
import Card from 'react-bootstrap/Card';
import {setRoomId} from "../../redux/HotelSlice.tsx";
import {useAppDispatch, useAppSelector} from "../../redux/Hooks.tsx";

interface RoomCardToPageProps {
    RoomId : string;
    RoomNumber : number;
    PricePerNight : number;
    ImgUrl : string;
}



const RoomCardToPage: React.FC<RoomCardToPageProps> = ({ RoomId, RoomNumber, PricePerNight, ImgUrl }) => {
    const hotel = useAppSelector((state) => state.hotel);
    const dispatch = useAppDispatch();
    const [choice, setChoice] = useState(false);

    useEffect(() => {
        (async () => {
            setChoice(RoomId == hotel.roomId);
        })();
    }, [hotel.roomId]);

    const handleChoice = () => {
        dispatch(setRoomId(RoomId));
        console.log(hotel.roomId);
    }

    return (
        <Card body>
            <div style={{ display: 'flex', gap: '20px', alignItems: 'flex-start' }}>
                <div style={{ flex: 1 }}>
                    Номер комнаты : {RoomNumber}
                    <br/>
                    Цена за одну ночь : {PricePerNight}
                </div>
                <div>
                    <img
                        src={`http://localhost:5045/${ImgUrl}`}
                        alt="Image"
                        style={{ width: '160px', height: '90px' }} // Размер можно изменить
                    />
                </div>
            </div>
            <div style={{ display: 'flex', gap: '10px', marginTop: '10px' }}>
                <button className="update-button" onClick={handleChoice}>Выбрать</button>
                {choice &&
                    <h4 style={{ margin: 'auto' }}>Выбран</h4>
                }
            </div>
        </Card>
    );
}

export default RoomCardToPage;