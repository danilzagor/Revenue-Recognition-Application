import { Link } from 'react-router-dom';
import styles from './NavBar.module.css';

const NavBar = () => {
    return (
        <div className={styles.navigationBar}>
            <h1>Company</h1>
            <div className={styles.links}>
                <li><Link to="/software">Software</Link></li>
                <li><Link to="/clients">Clients</Link></li>
                <li><Link to="/">Home</Link></li>
            </div>
        </div>
    );
}

export default NavBar;