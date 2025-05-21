import React from 'react';
import Card from 'react-bootstrap/Card';

interface UserCardProps {
    email: string;
    name: string;
    needRole: boolean;
    role: string;
    needId: boolean;
    Id: string;
    ImgUrl: string;
}


const UserCard: React.FC<UserCardProps> = ({ email, name, needRole,role, needId, Id, ImgUrl }) => {
    return (
        <Card body>
            <img
            src={`http://localhost:5045/${ImgUrl}`}
            alt="Image"
            style={{ width: '40px', height: '40px' }} // Размер можно изменить
            />
            <br/>
            {needId && <div>Id : {Id} <br/></div>}
            Почта : {email}
            <br/>
            Имя : {name}
            <br/>
            {needRole && <div>Id роли : {role}</div>}

        </Card>
    );
}

export default UserCard;