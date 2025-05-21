import React, { useEffect, useState } from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import axios from 'axios';
import {CurrentUserResponse, GetCurrentUser} from '../api/AppApi.ts';

interface PrivateRouteProps {
    IsAdmin: boolean;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({IsAdmin}) => {
    const [user, setUser] = useState<CurrentUserResponse | null | undefined>(undefined);

    useEffect(() => {
        (async () => {
            try {
                const data = await GetCurrentUser();
                if (data === null || data === undefined) {
                    setUser(null);
                }
                else {
                    setUser(data.data);
                    console.log(data);
                }
            } catch (err: any) {
                if (axios.isAxiosError(err) && err.response?.status === 401) {
                    setUser(null);
                }
                else{
                    setUser(null);
                }
            }
        })();
    }, []);

    if (user === undefined) {
        //return <Navigate to="/login"/>;
        return <p>Loading…</p>;
    }

    if (IsAdmin && user?.AsseccLvl != 2)
    {
        return <Navigate to="/home"/>;
    }
    if (user == null || user?.Name == "") {
        return <Navigate to="/login"/>;
    }

    return <Outlet />;
};

export default PrivateRoute;
