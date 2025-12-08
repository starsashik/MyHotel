import React, {ReactNode, useEffect} from 'react';
import '../styles/index.css';
import Header from "../../Components/Header.tsx";
import Footer from "../../Components/Footer.tsx";
import {useAppDispatch, useAppSelector} from "../../redux/Hooks.tsx";
import {GetCurrentUser} from "../../api/AppApi.ts";
import {setAccessLvl, setEmail, setIsLoggedIn, setToken, setUserId} from "../../redux/UserSlice.tsx";

interface LayoutProps {
    children: ReactNode;
}

export const Layout: React.FC<LayoutProps> = ({ children }) => {
    const user = useAppSelector((state) => state.user);
    const dispatch = useAppDispatch();

    useEffect(() => {
        const fetchUser = async () => {
            const res = await GetCurrentUser();
            console.log(res.data);
            if (res.data.Email) {
                dispatch(setIsLoggedIn(true));
            } else {
                dispatch(setIsLoggedIn(false));
            }

            dispatch(setEmail(res.data.Email || ""));
            dispatch(setAccessLvl(res.data.AsseccLvl ?? -1));
            dispatch(setToken(res.data.Token || ""));
            dispatch(setUserId(res.data.UserId || ""));
        }

        fetchUser();
    }, [dispatch, user.update]);

    return (
        <div className="layout">
            <Header />
                <main className="layout__main">{children}</main>
            <Footer />
        </div>
    );
};
