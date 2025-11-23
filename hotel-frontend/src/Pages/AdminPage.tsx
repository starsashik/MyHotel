import {FormEvent, useEffect, useState} from "react";
import {
    CreateRole,
    CreateUser,
    DeleteRole,
    DeleteUser,
    getRoles,
    getUsers,
    Hotel,
    Role,
    User,
    Room,
    getHotels,
    CreateHotel,
    DeleteHotel,
    UpdateHotel,
    getAllRooms,
    getFilteredRooms,
    CreateRoom,
    UpdateRoom,
    DeleteRoom,
    Booking, getAllBooking, getFilteredBookings, CreateBooking, DeleteBooking
} from "../api/AppApi.ts";
import UserCard from "./Components/UserCard.tsx";
import RoleCard from "./Components/RoleCard.tsx";
import {Button} from "react-bootstrap";
import {useAppSelector} from "../redux/Hooks.tsx";
import "../css/AdminPage.css"
import HotelCard from "./Components/HotelCard.tsx";
import RoomCard from "./Components/RoomCard.tsx";
import BookingCard from "./Components/BookingCard.tsx";

export function AdminPage() {
    const user = useAppSelector((state) => state.user);

    // Users
    const [showUsers, setShowUsers] = useState(false);
    const [showUsersRole, setShowUsersRole] = useState(false);
    const [showUsersId, setShowUsersId] = useState(false);
    const [showCreteUser, setShowCreteUser] = useState(false);
    const [showDeleteUser, setShowDeleteUser] = useState(false);

    // Roles
    const [showRoles, setShowRoles] = useState(false);
    const [showCreateRoles, setShowCreateRoles] = useState(false);
    const [showDeleteRoles, setShowDeleteRoles] = useState(false);

    // Hotels
    const [showHotels, setShowHotels] = useState(false);
    const [showCreateHotels, setShowCreateHotels] = useState(false);
    const [showUpdateHotels, setShowUpdateHotels] = useState(false);
    const [showDeleteHotels, setShowDeleteHotels] = useState(false);

    // Rooms
    const [showRooms, setShowRooms] = useState(false);
    const [showFilteredRooms, setShowFilteredRooms] = useState(false);
    const [showCreateRooms, setShowCreateRooms] = useState(false);
    const [showUpdateRooms, setShowUpdateRooms] = useState(false);
    const [showDeleteRooms, setShowDeleteRooms] = useState(false);

    // Bookings
    const [showBookings, setShowBookings] = useState(false);
    const [showFilteredBookings, setShowFilteredBookings] = useState(false);
    const [showCreateBookings, setShowCreateBookings] = useState(false);
    const [showDeleteBookings, setShowDeleteBookings] = useState(false);

    // list models
    const [Users, setUsers] = useState<User[]>([]);
    const [Roles, setRoles] = useState<Role[]>([]);
    const [Hotels, setHotels] = useState<Hotel[]>([]);
    const [Rooms, setRooms] = useState<Room[]>([]);
    const [FilteredRooms, setFilteredRooms] = useState<Room[]>([]);
    const [Bookings, setBookings] = useState<Booking[]>([]);
    const [FilteredBookings, setFilteredBookings] = useState<Booking[]>([]);

    // Users
    const [username, setUsername] = useState('');
    const [email, setEmail_2] = useState('');
    const [password, setPassword] = useState('');
    const [RoleId, setRoleId] = useState('');
    const [UserIdDel, setUserIdDel] = useState('');

    // Roles
    const [RoleName, setRoleName] = useState('');
    const [AcceessLvl, setAccessLevelAd] = useState<number>(0);
    const [RoleIdDel, setRoleIdDel] = useState('');

    // Hotels
    const [HotelName, setHotelName] = useState('');
    const [HotelLocation, setHotelLocation] = useState('');
    const [HotelDescription, setHotelDescription] = useState('');
    const [PreviewUrlHotel, setPreviewUrlHotel] = useState<string | null>(null);
    const [ImageHotel, setImageHotel] = useState<File | null>(null);
    const [HotelIdDel, setHotelIdDel] = useState('');
    const [HotelIdUpdate, setHotelIdUpdate] = useState('');
    const [NewHotelName, setNewHotelName] = useState('');
    const [NewHotelLocation, setNewHotelLocation] = useState('');
    const [NewHotelDescription, setNewHotelDescription] = useState('');
    const [NewPreviewUrlHotel, setNewPreviewUrlHotel] = useState<string | null>(null);
    const [NewImageHotel, setNewImageHotel] = useState<File | null>(null);

    // Rooms
    const [HotelIdToFilter, setHotelIdToFilter] = useState('');
    const [RoomTypeToFilter, setRoomTypeToFilter] = useState<number>(0);
    const [HotelIdToCreateRoom, setHotelIdToCreateRoom] = useState('');
    const [RoomNumber, setRoomNumber] = useState<number>(0);
    const [RoomType, setRoomType] = useState<number>(0);
    const [PricePerNight, setPricePerNight] = useState<number>(0);
    const [PreviewUrlRoom, setPreviewUrlRoom] = useState<string | null>(null);
    const [ImageRoom, setImageRoom] = useState<File | null>(null);
    const [RoomIdUpdate, setRoomIdUpdate] = useState('');
    const [RoomIdDel, setRoomIdDel] = useState('');
    const [NewRoomNumber, setNewRoomNumber] = useState<number>(0);
    const [NewRoomType, setNewRoomType] = useState<number>(0);
    const [NewPricePerNight, setNewPricePerNight] = useState<number>(0);
    const [NewPreviewUrlRoom, setNewPreviewUrlRoom] = useState<string | null>(null);
    const [NewImageRoom, setNewImageRoom] = useState<File | null>(null);

    // Bookings
    const [UserIdToFilter, setUserIdToFilter] = useState('');
    const [DateToFilter, setDateToFilter] = useState<Date>(new Date());
    const [UserIdToCreate, setUserIdToCreate] = useState('');
    const [RoomIdToCreate, setRoomIdToCreate] = useState('');
    const [DateInToCreate, setDateInToCreate] = useState<Date>(new Date());
    const [DateOutToCreate, setDateOutToCreate] = useState<Date>(new Date());
    const [BookingIdDel, setBookingIdDel] = useState('');


    // Dop
    const [errorMessage, setErrorMessage] = useState<string>("");
    const [UpdatePage, setUpdatePage] = useState(false);

    /*_____________________________________________________________*/

    const incrementRole = () => {
        if (AcceessLvl < 7) {
            setAccessLevelAd(AcceessLvl + 1);
        }
    };

    const decrementRole = () => {
        if (AcceessLvl > 0) {
            setAccessLevelAd(AcceessLvl - 1);
        }
    };

    const incrementRoomFilter = () => {
        if (RoomTypeToFilter + 1 < 4) {
            setRoomTypeToFilter(RoomTypeToFilter + 1);
        }
    };

    const decrementRoomFilter = () => {
        if (RoomTypeToFilter > 0) {
            setRoomTypeToFilter(RoomTypeToFilter - 1);
        }
    };

    useEffect(() => {
        (async () => {
            const data = await getUsers();
            setUsers(data);
            console.log(data)
        })();
    }, [showUsers, UpdatePage]);

    useEffect(() => {
        (async () => {
            const data_2 = await getRoles();
            setRoles(data_2);
            console.log(data_2);
        })();
    }, [showRoles, UpdatePage]);

    useEffect(() => {
        (async () => {
            const data_3 = await getHotels();
            setHotels(data_3);
            console.log(data_3);
        })();
    }, [showHotels, UpdatePage]);

    useEffect(() => {
        (async () => {
            const data_4 = await getAllRooms();
            setRooms(data_4);
            console.log(data_4);
        })();
    }, [showRooms, UpdatePage]);

    useEffect(() => {
        (async () => {
            const data_5 = await getAllBooking();
            setBookings(data_5);
            console.log(data_5);
        })();
    }, [showBookings, UpdatePage]);


    // Users
    const handleSubmit_CreateUser = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const response = await CreateUser(username, email, password, RoleId, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401" || error.name === "400") {
                    setErrorMessage('Неверные данные');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleSubmit_deleteUser = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const response = await DeleteUser(UserIdDel, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401") {
                    setErrorMessage('Неверный токен');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    // Role
    const handleSubmit_createRole = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const response = await CreateRole(RoleName, AcceessLvl, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401" || error.name === "400") {
                    setErrorMessage('Неверные данные');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleSubmit_deleteRole = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const response = await DeleteRole(RoleIdDel, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401") {
                    setErrorMessage('Неверный токен');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    // Hotel
    const handleSubmit_createHotel = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const formData = new FormData();
                formData.append('Name', HotelName);
                formData.append('Location', HotelLocation);
                formData.append('Description', HotelDescription);
                if (ImageHotel) {
                    formData.append('ImageFile', ImageHotel);
                }
                const response = await CreateHotel(formData, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401" || error.name === "400") {
                    setErrorMessage('Неверные данные.');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleSubmit_updateHotel = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const formData = new FormData();
                formData.append('HotelId', HotelIdUpdate);
                if (NewHotelName != ""){
                    formData.append('NewName', NewHotelName);
                }
                if (NewHotelLocation != ""){
                    formData.append('NewLocation', NewHotelLocation);
                }
                if (NewHotelDescription != ""){
                    formData.append('NewDescription', NewHotelDescription);
                }
                if (NewImageHotel) {
                    formData.append('ImageFile', NewImageHotel);
                }
                const response = await UpdateHotel(formData, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401" || error.name === "400") {
                    setErrorMessage('Неверные данные.');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleSubmit_deleteHotel = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const response = await DeleteHotel(HotelIdDel, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401") {
                    setErrorMessage('Неверный токен');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleImageChangeHotel = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            const selectedImage = e.target.files[0];
            setImageHotel(selectedImage);
            setPreviewUrlHotel(URL.createObjectURL(selectedImage)); // Обновляем предпросмотр
        }
    };

    const handleImageChangeHotelUpdate = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            const selectedImage = e.target.files[0];
            setNewImageHotel(selectedImage);
            setNewPreviewUrlHotel(URL.createObjectURL(selectedImage)); // Обновляем предпросмотр
        }
    };

    // Room
    const handleSubmit_filterRooms = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const formData = new FormData();
                if (HotelIdToFilter != ""){
                    formData.append('HotelId', HotelIdToFilter);
                }
                if (RoomTypeToFilter > 0){
                    formData.append('RoomType', RoomTypeToFilter.toString());
                }
                const response = await getFilteredRooms(formData, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }
                const result: Room[] = await response.json();
                console.error(result)
                setFilteredRooms(result);

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401") {
                    setErrorMessage('Неверный токен');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleSubmit_createRoom = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const formData = new FormData();
                formData.append('HotelId', HotelIdToCreateRoom);
                formData.append('RoomNumber', RoomNumber.toString());
                formData.append('RoomType', RoomType.toString());
                formData.append('PricePerNight', PricePerNight.toString());
                if (ImageRoom) {
                    formData.append('ImageFile', ImageRoom);
                }
                const response = await CreateRoom(formData, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401" || error.name === "400") {
                    setErrorMessage('Неверные данные.');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleSubmit_updateRoom = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const formData = new FormData();
                formData.append('Id', RoomIdUpdate);
                if (NewRoomNumber != 0)
                {
                    formData.append('NewRoomNumber', NewRoomNumber.toString());
                }
                if (NewRoomType != 0)
                {
                    formData.append('NewRoomType', NewRoomType.toString());
                }
                if (NewPricePerNight != 0)
                {
                    formData.append('NewPricePerNight', NewPricePerNight.toString());
                }
                if (NewImageRoom) {
                    formData.append('ImageFile', NewImageRoom);
                }
                const response = await UpdateRoom(formData, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401" || error.name === "400") {
                    setErrorMessage('Неверные данные.');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleSubmit_deleteRoom = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const response = await DeleteRoom(RoomIdDel, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401") {
                    setErrorMessage('Неверный токен');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleImageChangeRooms = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            const selectedImage = e.target.files[0];
            setImageRoom(selectedImage);
            setPreviewUrlRoom(URL.createObjectURL(selectedImage)); // Обновляем предпросмотр
        }
    };

    const handleImageChangeRoomsUpdate = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            const selectedImage = e.target.files[0];
            setNewImageRoom(selectedImage);
            setNewPreviewUrlRoom(URL.createObjectURL(selectedImage)); // Обновляем предпросмотр
        }
    };

    // Booking
    const handleSubmit_filterBookings = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const formData = new FormData();
                if (UserIdToFilter != ""){
                    formData.append('UserId', UserIdToFilter);
                }
                formData.append('DateInRange', formatDateForInput(DateToFilter).toString());
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
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401") {
                    setErrorMessage('Неверный токен');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleSubmit_createBooking = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const formData = new FormData();
                formData.append('UserId', UserIdToCreate);
                formData.append('RoomId', RoomIdToCreate);
                formData.append('CheckIn', formatDateForInput(DateInToCreate).toString());
                formData.append('CheckOut', formatDateForInput(DateOutToCreate).toString());

                const response = await CreateBooking(formData, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401" || error.name === "400") {
                    setErrorMessage('Неверные данные.');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleSubmit_deleteBooking = (e: FormEvent) => {
        e.preventDefault();
        (async () => {
            try {
                const response = await DeleteBooking(BookingIdDel, user.token);
                if (!response.ok) {
                    const errorData = await response.json();
                    const error = new Error(errorData.message || `HTTP error! Status: ${response.status}`);
                    error.name = response.status.toString();
                    throw error;
                }

                setErrorMessage('');
                setUpdatePage(!UpdatePage);
            } catch (error: any) {
                if (error.name === "401" || error.name === "400") {
                    setErrorMessage('Неверные данные.');
                } else {
                    setErrorMessage(`Произошла ошибка: ${error.message}`);
                }
            }
        })();
    };

    const handleDateChangeFilter = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newDate = new Date(e.target.value);
        if (!isNaN(newDate.getTime())) { // Проверяем, что дата валидная
            setDateToFilter(newDate);
        }
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

    /*_____________________________________________________________*/

    return (
        <div className="admin-page">
            <h1 style={{display: 'flex', alignItems: 'center', justifyContent: 'center', gap: '5px'}}>Панель администратора</h1>
            <br/>
            <div className="err-message">
                {errorMessage !== "" && (
                    <b>{errorMessage}</b>
                )}
            </div>
            <div>
                <h1>Пользователи</h1>
                <div style={{display: 'flex', alignItems: 'left', justifyContent: 'left', gap: '20px'}}>
                    <label>
                        <input
                            type="checkbox"
                            checked={showUsers}
                            onChange={() => setShowUsers(!showUsers)}
                        />
                        Показывать пользователей
                    </label>
                    <label>
                        <input
                            type="checkbox"
                            checked={showUsersRole}
                            onChange={() => setShowUsersRole(!showUsersRole)}
                        />
                        Показывать Роли
                    </label>
                    <label>
                        <input
                            type="checkbox"
                            checked={showUsersId}
                            onChange={() => setShowUsersId(!showUsersId)}
                        />
                        Показывать ID пользователя
                    </label>
                </div>
                {showUsers &&
                    <div>
                        {Users.length > 0 ? (
                            <div>
                                {Users.map((user: User) => (
                                    <li key={user.Email}>
                                        <UserCard email={user.Email} name={user.Name} needRole={showUsersRole} role={user.Role} needId={showUsersId} Id={user.UserId} ImgUrl={user.ImgUrl} />
                                    </li>
                                ))}
                            </div>
                        ) : (
                            <div>
                                <p>Нет пользователей.</p>
                            </div>
                        )}
                    </div>
                }
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showCreteUser}
                        onChange={() => setShowCreteUser(!showCreteUser)}
                    />
                    Показывать создание пользователя
                </label>
                {showCreteUser &&
                    <div>
                        <br/>
                        <h4>Создание пользователя</h4>
                        <form className="form" onSubmit={handleSubmit_CreateUser}>
                            <div className="form-group">
                                <label>
                                    Имя
                                </label>
                                <input
                                    
                                    type="text"
                                    placeholder="Имя пользователся"
                                    value={username}
                                    onChange={(e) => setUsername(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Email
                                </label>
                                <input
                                    
                                    type="text"
                                    placeholder="Email пользователся"
                                    value={email}
                                    onChange={(e) => setEmail_2(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Пароль
                                </label>
                                <input
                                    
                                    type="password"
                                    placeholder="Минимум 8 символов"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Guid Роли
                                </label>
                                <input
                                    
                                    type="text"
                                    placeholder="Guid роли"
                                    value={RoleId}
                                    onChange={(e) => setRoleId(e.target.value)}
                                    required
                                />
                            </div>
                            <Button type="submit" className="update-button">
                                Создать пользователя
                            </Button>
                        </form>
                    </div>
                }
                <br/>
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showDeleteUser}
                        onChange={() => setShowDeleteUser(!showDeleteUser)}
                    />
                    Показывать удаление пользователей
                </label>
                {showDeleteUser &&
                    <div>
                        <br/>
                        <h4>Удаление пользователя</h4>
                        <form className="form" onSubmit={handleSubmit_deleteUser}>
                            <div className="form-group">
                                <label>
                                    Guid пользователя
                                </label>
                                <input
                                    
                                    type="text"
                                    placeholder="Guid пользователся"
                                    value={UserIdDel}
                                    onChange={(e) => setUserIdDel(e.target.value)}
                                    required
                                />
                            </div>
                            <Button type="submit" className="update-button">
                                Удалить пользователя
                            </Button>
                        </form>
                    </div>
                }
            </div>
            <br/>
            <div>
                <h1>Роли</h1>
                <label>
                    <input
                        type="checkbox"
                        checked={showRoles}
                        onChange={() => setShowRoles(!showRoles)}
                    />
                    Показывать роли
                </label>
                <br/>
                {showRoles &&
                    <div>
                        {Roles.length > 0 ? (
                            <div>
                                {Roles.map((role: Role) => (
                                    <li key={role.Id}>
                                        <RoleCard role={role.Id} name={role.Name} accessLvl={role.AccessLevel}/>
                                    </li>
                                ))}
                            </div>
                        ) : (
                            <div>
                                <p>Нет ролей.</p>
                            </div>
                        )}
                    </div>
                }
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showCreateRoles}
                        onChange={() => setShowCreateRoles(!showCreateRoles)}
                    />
                    Показывать создание роли
                </label>
                {showCreateRoles &&
                    <div>
                        <br/>
                        <h4>Создание роли</h4>
                        <form className="form" onSubmit={handleSubmit_createRole}>
                            <div className="form-group">
                                <label>
                                    Имя роли
                                </label>
                                <input
                                    
                                    type="text"
                                    placeholder="Имя роли"
                                    value={RoleName}
                                    onChange={(e) => setRoleName(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Уровень доступа (0 : superuser, 1-5 : admin, 6 : editor, 7 : commonuser)
                                </label>
                                <div style={{display: 'flex', alignItems: 'left', justifyContent: 'left', gap: '5px'}}>
                                    <Button variant="outline-secondary"
                                            onClick={decrementRole}
                                            style={{fontSize: '1.8rem',}}
                                    >-</Button>
                                    <span style={{
                                        fontSize: '1.5rem',
                                        padding: '10px 20px',
                                        border: '1px solid #ccc',
                                        borderRadius: '4px',
                                        backgroundColor: '#f8f9fa',
                                    }}>{AcceessLvl}</span>
                                    <Button variant="outline-secondary"
                                            onClick={incrementRole}
                                            style={{fontSize: '1.8rem',}}
                                    >+</Button>
                                </div>
                            </div>
                            <Button type="submit" className="update-button">
                                Создать роль
                            </Button>
                        </form>
                    </div>
                }
                <br/>
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showDeleteRoles}
                        onChange={() => setShowDeleteRoles(!showDeleteRoles)}
                    />
                    Показывать удаление роли
                </label>
                {showDeleteRoles &&
                    <div>
                        <br/>
                        <h4>Удаление роли</h4>
                        <form className="form" onSubmit={handleSubmit_deleteRole}>
                            <div className="form-group">
                                <label>
                                    Guid роли
                                </label>
                                <input
                                    
                                    type="text"
                                    placeholder="Guid роли"
                                    value={RoleIdDel}
                                    onChange={(e) => setRoleIdDel(e.target.value)}
                                    required
                                />
                            </div>
                            <Button type="submit" className="update-button">
                                Удалить роль
                            </Button>
                        </form>
                    </div>
                }
            </div>
            <br/>
            <div>
                <h1>Отели</h1>
                <label>
                    <input
                        type="checkbox"
                        checked={showHotels}
                        onChange={() => setShowHotels(!showHotels)}
                    />
                    Показывать отели
                </label>
                <br/>
                {showHotels &&
                    <div>
                        {Hotels.length > 0 ? (
                            <div>
                                {Hotels.map((hotel: Hotel) => (
                                    <li key={hotel.HotelId}>
                                        <HotelCard HotelId={hotel.HotelId} Name={hotel.Name} Location={hotel.Location} Description={hotel.Description} ImgUrl={hotel.ImgUrl}/>
                                    </li>
                                ))}
                            </div>
                        ) : (
                            <div>
                                <p>Нет Отелей.</p>
                            </div>
                        )}
                    </div>
                }
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showCreateHotels}
                        onChange={() => setShowCreateHotels(!showCreateHotels)}
                    />
                    Показывать создание отеля
                </label>
                {showCreateHotels &&
                    <div>
                        <br/>
                        <h4>Создание отеля</h4>
                        <form className="form" onSubmit={handleSubmit_createHotel}>
                            <div className="form-group">
                                <label>
                                    Название отеля
                                </label>
                                <input
                                    
                                    type="text"
                                    placeholder="Название отеля"
                                    value={HotelName}
                                    onChange={(e) => setHotelName(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Местоположение отеля
                                </label>
                                <input
                                    
                                    type="text"
                                    placeholder="Местоположение отеля"
                                    value={HotelLocation}
                                    onChange={(e) => setHotelLocation(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Описание отеля
                                </label>
                                <input
                                    
                                    type="text"
                                    placeholder="Описание отеля"
                                    value={HotelDescription}
                                    onChange={(e) => setHotelDescription(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Фотография:</label>
                                {PreviewUrlHotel && <img src={PreviewUrlHotel} alt="Hotel Preview" />}
                                <input
                                    
                                    type="file"
                                    accept="image/*"
                                    onChange={handleImageChangeHotel}
                                />
                            </div>
                            <Button type="submit" className="update-button">
                                Создать Отель
                            </Button>
                        </form>
                    </div>
                }
                <br/>
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showUpdateHotels}
                        onChange={() => setShowUpdateHotels(!showUpdateHotels)}
                    />
                    Показывать обновление отеля
                </label>
                {showUpdateHotels &&
                    <div>
                        <br/>
                        <h4>Обновление отеля</h4>
                        <form className="form" onSubmit={handleSubmit_updateHotel}>
                            <div className="form-group">
                                <label>
                                    Guid отеля, который надо обновить
                                </label>
                                <input
                                    type="text"
                                    placeholder="Guid отеля"
                                    value={HotelIdUpdate}
                                    onChange={(e) => setHotelIdUpdate(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Новое название отеля
                                </label>
                                <input

                                    type="text"
                                    placeholder="Название отеля"
                                    value={NewHotelName}
                                    onChange={(e) => setNewHotelName(e.target.value)}
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Новое местоположение отеля
                                </label>
                                <input

                                    type="text"
                                    placeholder="Местоположение отеля"
                                    value={NewHotelLocation}
                                    onChange={(e) => setNewHotelLocation(e.target.value)}
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Новое описание отеля
                                </label>
                                <input

                                    type="text"
                                    placeholder="Описание отеля"
                                    value={NewHotelDescription}
                                    onChange={(e) => setNewHotelDescription(e.target.value)}
                                />
                            </div>
                            <div className="form-group">
                                <label>Новая фотография:</label>
                                {NewPreviewUrlHotel && <img src={NewPreviewUrlHotel} alt="Hotel Preview" />}
                                <input
                                    type="file"
                                    accept="image/*"
                                    onChange={handleImageChangeHotelUpdate}
                                />
                            </div>
                            <Button type="submit" className="update-button">
                                Обновить отель
                            </Button>
                        </form>
                    </div>
                }
                <br/>
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showDeleteHotels}
                        onChange={() => setShowDeleteHotels(!showDeleteHotels)}
                    />
                    Показывать удаление отеля
                </label>
                {showDeleteHotels &&
                    <div>
                        <br/>
                        <h4>Удаление отеля</h4>
                        <form className="form" onSubmit={handleSubmit_deleteHotel}>
                            <div className="form-group">
                                <label>
                                    Guid отеля
                                </label>
                                <input
                                    type="text"
                                    placeholder="Guid отеля"
                                    value={HotelIdDel}
                                    onChange={(e) => setHotelIdDel(e.target.value)}
                                    required
                                />
                            </div>
                            <Button type="submit" className="update-button">
                                Удалить отель
                            </Button>
                        </form>
                    </div>
                }
            </div>
            <br/>
            <div>
                <h1>Комнаты</h1>
                <label>
                    <input
                        type="checkbox"
                        checked={showRooms}
                        onChange={() => setShowRooms(!showRooms)}
                    />
                    Показывать номера в отелях
                </label>
                <br/>
                {showRooms &&
                    <div>
                        {Rooms.length > 0 ? (
                            <div>
                                {Rooms.map((room: Room) => (
                                    <li key={room.RoomId}>
                                        <RoomCard RoomId={room.RoomId} RoomNumber={room.RoomNumber} RoomType={room.RoomType} PricePerNight={room.PricePerNight} ImgUrl={room.ImgUrl} />
                                    </li>
                                ))}
                            </div>
                        ) : (
                            <div>
                                <p>Нет комнат.</p>
                            </div>
                        )}
                    </div>
                }
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showFilteredRooms}
                        onChange={() => setShowFilteredRooms(!showFilteredRooms)}
                    />
                    Показывать отфильтрованные номера
                </label>
                <br/>
                {showFilteredRooms &&
                    <div>
                        <div>
                            <br/>
                            <h4>Фильтр</h4>
                            <form className="form" onSubmit={handleSubmit_filterRooms}>
                                <div className="form-group">
                                    <label>
                                        Guid отеля
                                    </label>
                                    <input
                                        type="text"
                                        placeholder="Guid отеля"
                                        value={HotelIdToFilter}
                                        onChange={(e) => setHotelIdToFilter(e.target.value)}
                                    />
                                </div>
                                <div className="form-group">
                                    <label>
                                        Тип комнаты (0 : не фильтровать по типу, 1 : Обычная, 2 : Люкс, 3 : Президентский)
                                    </label>
                                    <div style={{display: 'flex', alignItems: 'left', justifyContent: 'left', gap: '5px'}}>
                                        <Button variant="outline-secondary"
                                                onClick={decrementRoomFilter}
                                                style={{fontSize: '1.8rem',}}
                                        >-</Button>
                                        <span style={{
                                            fontSize: '1.5rem',
                                            padding: '10px 20px',
                                            border: '1px solid #ccc',
                                            borderRadius: '4px',
                                            backgroundColor: '#f8f9fa',
                                        }}>{RoomTypeToFilter}</span>
                                        <Button variant="outline-secondary"
                                                onClick={incrementRoomFilter}
                                                style={{fontSize: '1.8rem',}}
                                        >+</Button>
                                    </div>
                                </div>
                                <Button type="submit" className="update-button">
                                    Показать комнаты
                                </Button>
                            </form>
                        </div>
                        <div>
                            {FilteredRooms.length > 0 ? (
                                <div>
                                    {FilteredRooms.map((room: Room) => (
                                        <li key={room.RoomId}>
                                            <RoomCard RoomId={room.RoomId} RoomNumber={room.RoomNumber} RoomType={room.RoomType} PricePerNight={room.PricePerNight} ImgUrl={room.ImgUrl} />
                                        </li>
                                    ))}
                                </div>
                            ) : (
                                <div>
                                    <p>Нет комнат.</p>
                                </div>
                            )}
                        </div>
                    </div>
                }
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showCreateRooms}
                        onChange={() => setShowCreateRooms(!showCreateRooms)}
                    />
                    Показывать создание комнаты
                </label>
                {showCreateRooms &&
                    <div>
                        <br/>
                        <h4>Создание комнаты</h4>
                        <form className="form" onSubmit={handleSubmit_createRoom}>
                            <div className="form-group">
                                <label>
                                    Guid отеля
                                </label>
                                <input
                                    type="text"
                                    placeholder="Guid отеля"
                                    value={HotelIdToCreateRoom}
                                    onChange={(e) => setHotelIdToCreateRoom(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Номер комнаты
                                </label>
                                <input
                                    type="number"
                                    placeholder="Число от 1 до 1000"
                                    min="1" max="1000" step="1"
                                    value={RoomNumber}
                                    onChange={(e) => setRoomNumber(parseInt(e.target.value) || 0)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Тип комнаты
                                </label>
                                <input
                                    type="number"
                                    placeholder="1 : Обычная, 2 : Люкс, 3 : Президентский"
                                    min="1" max="3" step="1"
                                    value={RoomType}
                                    onChange={(e) => setRoomType(parseInt(e.target.value) || 0)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Цена за ночь
                                </label>
                                <input
                                    type="number"
                                    placeholder="Число от 1 до 10000"
                                    min="1" max="10000" step="1"
                                    value={PricePerNight}
                                    onChange={(e) => setPricePerNight(parseInt(e.target.value) || 0)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Фотография:</label>
                                {PreviewUrlRoom && <img src={PreviewUrlRoom} alt="Hotel Preview" />}
                                <input

                                    type="file"
                                    accept="image/*"
                                    onChange={handleImageChangeRooms}
                                />
                            </div>
                            <Button type="submit" className="update-button">
                                Создать комнату
                            </Button>
                        </form>
                    </div>
                }
                <br/>
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showUpdateRooms}
                        onChange={() => setShowUpdateRooms(!showUpdateRooms)}
                    />
                    Показывать обновление номера
                </label>
                {showUpdateRooms &&
                    <div>
                        <br/>
                        <h4>Обновление номера</h4>
                        <form className="form" onSubmit={handleSubmit_updateRoom}>
                            <div className="form-group">
                                <label>
                                    Guid номера, который надо обновить
                                </label>
                                <input
                                    type="text"
                                    placeholder="Guid номера"
                                    value={RoomIdUpdate}
                                    onChange={(e) => setRoomIdUpdate(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Новый номер комнаты
                                </label>
                                <input
                                    type="number"
                                    placeholder="Число от 1 до 1000"
                                    min="0" max="1000" step="1"
                                    value={NewRoomNumber}
                                    onChange={(e) => setNewRoomNumber(parseInt(e.target.value) || 0)}
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Новый тип комнаты
                                </label>
                                <input
                                    type="number"
                                    placeholder="1 : Обычная, 2 : Люкс, 3 : Президентский"
                                    min="0" max="3" step="1"
                                    value={NewRoomType}
                                    onChange={(e) => setNewRoomType(parseInt(e.target.value) || 0)}
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Новая цена за ночь
                                </label>
                                <input
                                    type="number"
                                    placeholder="Число от 1 до 10000"
                                    min="0" max="10000" step="1"
                                    value={NewPricePerNight}
                                    onChange={(e) => setNewPricePerNight(parseInt(e.target.value) || 0)}
                                />
                            </div>
                            <div className="form-group">
                                <label>Новая фотография:</label>
                                {NewPreviewUrlRoom && <img src={NewPreviewUrlRoom} alt="Hotel Preview" />}
                                <input
                                    type="file"
                                    accept="image/*"
                                    onChange={handleImageChangeRoomsUpdate}
                                />
                            </div>
                            <Button type="submit" className="update-button">
                                Обновить номеру
                            </Button>
                        </form>
                    </div>
                }
                <br/>
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showDeleteRooms}
                        onChange={() => setShowDeleteRooms(!showDeleteRooms)}
                    />
                    Показывать удаление номера
                </label>
                {showDeleteRooms &&
                    <div>
                        <br/>
                        <h4>Удаление номера</h4>
                        <form className="form" onSubmit={handleSubmit_deleteRoom}>
                            <div className="form-group">
                                <label>
                                    Guid номера
                                </label>
                                <input
                                    type="text"
                                    placeholder="Guid номера"
                                    value={RoomIdDel}
                                    onChange={(e) => setRoomIdDel(e.target.value)}
                                    required
                                />
                            </div>
                            <Button type="submit" className="update-button">
                                Удалить номер
                            </Button>
                        </form>
                    </div>
                }
            </div>
            <br/>
            <div>
                <h1>Брони</h1>
                <label>
                    <input
                        type="checkbox"
                        checked={showBookings}
                        onChange={() => setShowBookings(!showBookings)}
                    />
                    Показывать все брони
                </label>
                <br/>
                {showBookings &&
                    <div>
                        {Bookings.length > 0 ? (
                            <div>
                                {Bookings.map((booking: Booking) => (
                                    <li key={booking.BookingId}>
                                        <BookingCard BookingId={booking.BookingId} UserId={booking.UserId} RoomId={booking.RoomId} CheckIn={booking.CheckIn} CheckOut={booking.CheckOut} />
                                    </li>
                                ))}
                            </div>
                        ) : (
                            <div>
                                <p>Нет броней.</p>
                            </div>
                        )}
                    </div>
                }
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showFilteredBookings}
                        onChange={() => setShowFilteredBookings(!showFilteredBookings)}
                    />
                    Показывать отфильтрованные брони
                </label>
                <br/>
                {showFilteredBookings &&
                    <div>
                        <div>
                            <br/>
                            <h4>Фильтр</h4>
                            <form className="form" onSubmit={handleSubmit_filterBookings}>
                                <div className="form-group">
                                    <label>
                                        Guid пользователя
                                    </label>
                                    <input
                                        type="text"
                                        placeholder="Guid пользователя"
                                        value={UserIdToFilter}
                                        onChange={(e) => setUserIdToFilter(e.target.value)}
                                    />
                                </div>
                                <div className="form-group">
                                    <label>
                                        Дата в промежутке посещения
                                    </label>
                                    <input
                                        type="date"
                                        placeholder="Дата"
                                        value={formatDateForInput(DateToFilter)}
                                        onChange={handleDateChangeFilter}
                                    />
                                </div>
                                <Button type="submit" className="update-button">
                                    Показать комнаты
                                </Button>
                            </form>
                        </div>
                        <div>
                            {FilteredBookings.length > 0 ? (
                                <div>
                                    {FilteredBookings.map((booking: Booking) => (
                                        <li key={booking.BookingId}>
                                            <BookingCard BookingId={booking.BookingId} UserId={booking.UserId} RoomId={booking.RoomId} CheckIn={booking.CheckIn} CheckOut={booking.CheckOut} />
                                        </li>
                                    ))}
                                </div>
                            ) : (
                                <div>
                                    <p>Нет комнат.</p>
                                </div>
                            )}
                        </div>
                    </div>
                }
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showCreateBookings}
                        onChange={() => setShowCreateBookings(!showCreateBookings)}
                    />
                    Показывать создание брони
                </label>
                {showCreateBookings &&
                    <div>
                        <br/>
                        <h4>Создание брони</h4>
                        <form className="form" onSubmit={handleSubmit_createBooking}>
                            <div className="form-group">
                                <label>
                                    Guid пользователя
                                </label>
                                <input
                                    type="text"
                                    placeholder="Guid пользователя"
                                    value={UserIdToCreate}
                                    onChange={(e) => setUserIdToCreate(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>
                                    Guid номера
                                </label>
                                <input
                                    type="text"
                                    placeholder="Guid номера"
                                    value={RoomIdToCreate}
                                    onChange={(e) => setRoomIdToCreate(e.target.value)}
                                    required
                                />
                            </div>
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
                }
                <br/>
                <br/>
                <label>
                    <input
                        type="checkbox"
                        checked={showDeleteBookings}
                        onChange={() => setShowDeleteBookings(!showDeleteBookings)}
                    />
                    Показывать удаление брони
                </label>
                {showDeleteBookings &&
                    <div>
                        <br/>
                        <h4>Удаление брони</h4>
                        <form className="form" onSubmit={handleSubmit_deleteBooking}>
                            <div className="form-group">
                                <label>
                                    Guid брони
                                </label>
                                <input
                                    type="text"
                                    placeholder="Guid брони"
                                    value={BookingIdDel}
                                    onChange={(e) => setBookingIdDel(e.target.value)}
                                    required
                                />
                            </div>
                            <Button type="submit" className="update-button">
                                Удалить бронь
                            </Button>
                        </form>
                    </div>
                }
            </div>
        </div>
    );
};

export default AdminPage;