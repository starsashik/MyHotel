import {useEffect, useState} from 'react';
import {getHotels, Hotel} from "../api/AppApi.ts";
import "../css/HotelsPage.css"
import Carousel from 'react-bootstrap/Carousel';
import {useNavigate} from "react-router-dom";
import {setHotelId, setRoomId} from "../redux/HotelSlice.tsx";
import {useAppDispatch} from "../redux/Hooks.tsx";
import {Button} from "react-bootstrap";


const HotelsPage = () => {
    //const MyHotel = useAppSelector((state) => state.hotel);
    const dispatch = useAppDispatch();
    const [hotels, setHotels] = useState<Hotel[]>([]);
    const [index, setIndex] = useState<number>(0);
    const navigate = useNavigate();

    useEffect(() => {
        (async () => {
            const data_3 = await getHotels();
            setHotels(data_3);
            console.log(data_3);

            dispatch(setRoomId(""));
        })();
    }, []);

    const handleSelect = (selectedIndex: number) => {
        setIndex(selectedIndex);
    };

    const handleBookNow = () => {
        dispatch(setHotelId(hotels[index]?.HotelId));
        navigate(`/rooms`);
    };


    return (
        <div className="hotels-rage">
            <div className="carousel-container">
                <Carousel activeIndex={index} onSelect={handleSelect} interval={5000} controls={true} indicators={true}>
                    {hotels.length > 0 ? (
                        hotels.map((hotel) => (
                            <Carousel.Item key={hotel.HotelId}>
                                <div className="carousel-image">
                                    <img
                                        src={`http://localhost:5045/${hotel.ImgUrl}`}
                                        alt={hotel.Name}
                                        className="carousel-img"
                                    />
                                </div>
                                <Carousel.Caption className="carousel-caption">
                                    <h4>{hotel.Name}</h4>
                                    <p>{hotel.Location}</p>
                                    <p>{hotel.Description}</p>
                                    <Button
                                        variant="danger"
                                        className="carousel-button"
                                        onClick={handleBookNow}
                                        disabled={hotels.length === 0}
                                    >
                                        Забронировать сейчас
                                    </Button>
                                </Carousel.Caption>
                            </Carousel.Item>
                        ))
                    ) : (
                        <Carousel.Item>
                            <div className="carousel-image">
                                <img
                                    src="http://localhost:5045/ImgHotel/default.png"
                                    alt="Default"
                                    className="carousel-img"
                                />
                            </div>
                            <Carousel.Caption className="carousel-caption">
                                <h3>No Hotels Available</h3>
                                <p>Please check back later.</p>
                                <p>Default message</p>
                            </Carousel.Caption>
                        </Carousel.Item>
                    )}
                </Carousel>
            </div>
        </div>
    );
};

export default HotelsPage;
