import { useState, useContext } from 'react';
import axiosInstance from '../../utils/axiosInstance.jsx';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../../context/AuthContext.jsx';
import styles from './LoginPage.module.css';

const LoginPage = () => {
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();
    const { setIsAuthenticated, setUser } = useContext(AuthContext);

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await axiosInstance.post('/auth/login', { login, password });
            if(response.status === 200) {
                setIsAuthenticated(true);
                const claims = response.data;
                const roles = claims.map(claim => claim.value);
                setUser({ ...response.data.user, roles });
                navigate('/');
            }
        } catch (error) {
            if (error.response) {
                console.error('Login failed:', error.response.data);
            } else {
                console.error('Login failed:', error.message);
            }
        }
    };

    return (
        <div className={styles.loginPageContainer}>
            <form className={styles.loginForm} onSubmit={handleLogin}>
                <input
                    type="text"
                    className={styles.loginInput}
                    value={login}
                    onChange={(e) => setLogin(e.target.value)}
                    placeholder="Login"
                    required
                />
                <input
                    type="password"
                    className={styles.loginInput}
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Password"
                    required
                />
                <button type="submit" className={styles.loginButton}>Login</button>
            </form>
        </div>
    );
};

export default LoginPage;
