import { useState } from "react";
import { Link } from "react-router-dom";
import useFetch from "../../hooks/useFetch.jsx";
import NavBar from "../../components/NavbarComponent/NavBar.jsx";
import styles from "./SoftwarePage.module.css";

const SoftwarePage = () => {
    const { data: software, isPending, error } = useFetch("https://localhost:7050/api/software");
    const [query, setQuery] = useState("");

    return (
        <div>
            <NavBar />
        <div className={styles.software}>

            <h1>Software Page</h1>
            <div style={{textAlign:"center"}}><input
                className={styles.searchInput}
                placeholder="Enter a name"
                onChange={event => setQuery(event.target.value)}
            /></div>

            <div className={styles.softwareList}>
                {!isPending && !error && software && (
                    software.filter(specificSoftware => {
                        if (query === '') {
                            return specificSoftware;
                        } else if (specificSoftware.name.toLowerCase().includes(query.toLowerCase())) {
                            return specificSoftware;
                        }
                    }).map(specificSoftware => (
                        <Link
                            key={specificSoftware.id}
                            to={`/software/${specificSoftware.id}`}
                            className={styles.softwareLink}
                        >
                            <div className={styles.softwareItem}>
                                <h2>{specificSoftware.name}</h2>
                            </div>
                        </Link>
                    ))
                )}
            </div>
        </div>
        </div>
    );
};

export default SoftwarePage;
