import React from 'react';
import Card from 'react-bootstrap/Card';

interface BookingCardProps {
    BookingId : string;
    UserId : string;
    RoomId: string;
    CheckIn: Date;
    CheckOut: Date;
}


const BookingCard: React.FC<BookingCardProps> = ({ BookingId, UserId, RoomId, CheckIn, CheckOut }) => {
    return (
        <Card body>
            Id : {BookingId}
            <br/>
            Посетитель : {UserId}
            <br/>
            Комната : {RoomId}
            <br/>
            {CheckIn.toString()} --- {CheckOut.toString()}
        </Card>
    );
}

export default BookingCard;