import {NavLink, useNavigate} from "react-router-dom";
import {setAccessLvl, setEmail, setIsLoggedIn, setToken, setUserId} from "../redux/UserSlice.tsx";
import {GetCurrentUser, LogoutUser} from "../api/AppApi.ts";
import {useAppDispatch, useAppSelector} from "../redux/Hooks.tsx";
import {useEffect, useState} from "react";

const Header = () => {
    const user = useAppSelector((state) => state.user);
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const [isLogged, setIsLogged] = useState<boolean>(false);
    const [isAdminMode, setIsAdminMode] = useState<boolean>(false);

    useEffect(() => {
        console.error(user.accessLvl)
        console.error(user.isLoggedIn)
        if (user.isLoggedIn) {
            setIsLogged(true);
            if (user.accessLvl == 2) {
                setIsAdminMode(true);
            } else {
                setIsAdminMode(false);
            }
        } else {
            setIsLogged(false);
            setIsAdminMode(false);
        }
    }, [user.accessLvl])

    const handleLogout = () => {
        LogoutUser().then(() => {
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
                dispatch(setUserId(res.data.UserId));
            }).then(() => {
                navigate("/");
            })
        });
    }

    return (
        <header>
            <div>
                <span className="logo">
                    <NavLink to={"/home"} style={{textDecoration: "none", color: "black"}}>MyHotels</NavLink>
                </span>

                {!isLogged ? (
                    <ul className="nav">
                        <li>
                            <NavLink to={"/help"} style={{textDecoration: "none", color: "black"}}>Помощь</NavLink>
                        </li>
                        <li>
                            <NavLink to={"/login"}
                                     style={{textDecoration: "none", color: "black"}}>Авторизация</NavLink>
                        </li>
                        <li>
                            <NavLink to={"/register"}
                                     style={{textDecoration: "none", color: "black"}}>Регистрация</NavLink>
                        </li>
                    </ul>
                ) : (
                    <ul className="nav">
                        <li>
                            <NavLink to={"/help"} style={{textDecoration: "none", color: "black"}}>Помощь</NavLink>
                        </li>
                        <li>
                            <NavLink to={"/profile"}
                                     style={{textDecoration: "none", color: "black"}}>{user.email}</NavLink>
                        </li>
                        <li>
                            <NavLink to={"/bookings"} style={{textDecoration: "none", color: "black"}}>Брони</NavLink>
                        </li>
                        <li>
                            <button
                                onClick={handleLogout}
                                style={{
                                    textDecoration: "none",
                                    color: "black",
                                    background: "none",
                                    border: "none",
                                    cursor: "pointer"
                                }}>Выйти
                            </button>
                        </li>
                        <li>
                            {isAdminMode && (
                                <NavLink to={"/admin"} style={{textDecoration: "none", color: "black"}}>Панель
                                    администратора</NavLink>
                            )}
                        </li>
                    </ul>
                )}

            </div>
        </header>
    );
}

export default Header;