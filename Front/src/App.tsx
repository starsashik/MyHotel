import {useEffect} from "react";
import {BrowserRouter, Navigate, Route, Routes} from "react-router-dom";
import HotelsPage from "./Pages/HotelsPage.tsx";
import {useAppDispatch, useAppSelector} from "./redux/Hooks.tsx";
import Registration from "./Pages/RegistrationPage.tsx";
import LoginPage from "./Pages/LoginPage.tsx";
import PrivateRoute from "./Components/PrivateRoute.tsx";
import './index.css';
import Header from "./Components/Header.tsx";
import Footer from "./Components/Footer.tsx";
import {GetCurrentUser} from "./api/AppApi.ts";
import {setAccessLvl, setEmail, setIsLoggedIn, setToken, setUserId} from "./redux/UserSlice.tsx";
import {setHotelId, setRoomId} from "./redux/HotelSlice.tsx";
import StartPage from "./Pages/StartPage.tsx";
import AdminPage from "./Pages/AdminPage.tsx";
import ProfilePage from "./Pages/ProfilePage.tsx";
import BookingsPage from "./Pages/BookingsPage.tsx";
import RoomsPage from "./Pages/RoomsPage.tsx";

function App() {
    const user = useAppSelector((state) => state.user);
    const dispatch = useAppDispatch();


    useEffect(() => {
        //SetEmptyCookiesApi().then(() => {
            GetCurrentUser().then((res) => {
                if (res.data.Email != "")
                {
                    dispatch(setIsLoggedIn(true));
                }
                else
                {
                    dispatch(setIsLoggedIn(false));
                }
                dispatch(setEmail(res.data.Email));
                dispatch(setAccessLvl(res.data.AsseccLvl));
                dispatch(setToken(res.data.Token));
                dispatch(setHotelId(""));
                dispatch(setRoomId(""));
                dispatch(setUserId(res.data.UserId));
                console.log(user);
            });
        //});
    }, [])

    return (
        <div className='wrapper'>
            <BrowserRouter>
                <Header/>
                <Routes>
                    <Route path="/" element={<Navigate to="/home"/>}/>

                    <Route path="/home" element={
                        <StartPage/>
                    }/>

                    <Route path={'/register'} element={
                        <Registration/>}
                    />
                    <Route path={'/login'} element={
                        <LoginPage/>}
                    />

                    <Route element={<PrivateRoute IsAdmin={true}/>}>
                        <Route path={'/admin'} element={
                            <AdminPage/>}
                        />
                    </Route>

                    <Route element={<PrivateRoute IsAdmin={false}/>}>
                        <Route path="/hotels" element={
                            <HotelsPage/>}
                            />
                    </Route>

                    <Route element={<PrivateRoute IsAdmin={false}/>}>
                        <Route path="/profile" element={
                            <ProfilePage/>}
                        />
                    </Route>

                    <Route element={<PrivateRoute IsAdmin={false}/>}>
                        <Route path="/bookings" element={
                            <BookingsPage/>}
                        />
                    </Route>

                    <Route element={<PrivateRoute IsAdmin={false}/>}>
                        <Route path="/rooms" element={
                            <RoomsPage/>}
                        />
                    </Route>

                    <Route path="/help" element={
                        <div className="help">
                            <h1>Популярные вопросы:</h1>
                            <br/><br/>
                            <h3>Если возникнут какие-либо ошибки - вы увидите соответствующее сообщение. Если проблема в
                                сведениях заказа, то перепроверьте их корректность еще раз. Если же нет
                                то свяжитесь с администратором системы по телефону 8(800)555-35-35, и сообщите о данной
                                проблеме.</h3>
                            <br/>
                        </div>
                    }/>

                    <Route path="*" element={<Navigate to="/home"/>}/>
                </Routes>
                <Footer/>
            </BrowserRouter>
        </div>
    )
}

export default App
