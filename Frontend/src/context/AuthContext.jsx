// src/AuthContext.js
import React, {createContext, useState, useEffect, useContext} from 'react';
import Cookies from 'js-cookie';

export const AuthContext = createContext({
    user: null,
    setUser: () => {},
    isAuthenticated: false,
    setIsAuthenticated: () => {},
});

const AuthProvider = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [user, setUser] = useState(null);

    useEffect(() => {
        const accessToken = Cookies.get('access_token');
        if (accessToken) {
            setIsAuthenticated(true);
        }
    }, []);

    const logout = () => {
        Cookies.remove('access_token');
        Cookies.remove('refresh_token');
        setIsAuthenticated(false);
    };

    return (
        <AuthContext.Provider value={{ user, setUser, isAuthenticated, setIsAuthenticated, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);

export default AuthProvider;