import NavBar from "../../components/NavbarComponent/NavBar.jsx";
import {useState} from "react";
import useFetch from "../../hooks/useFetch.jsx";
import {Link, useNavigate} from "react-router-dom";
import styles from './ClientsPage.module.css';

const ClientsPage = () => {
    const [isTypeOfClientPhysical, setIsTypeOfClientPhysical] = useState(true);
    const [url, setUrl] = useState('https://localhost:7050/api/clients/physical');
    const [query, setQuery] = useState("");

    const ToggleIsTypeOfClientPhysical = (bool) => {
        setIsTypeOfClientPhysical(bool);
        const newUrl = bool
            ? 'https://localhost:7050/api/clients/physical'
            : 'https://localhost:7050/api/clients/company';
        setUrl(newUrl);
    };

    const {data: clients, isPending, error} = useFetch(url);

    return (
        <div><NavBar/>
            <div className={styles.clients}>

                <div className={styles.content}>
                    <h1 className={styles.title}>Clients Page</h1>
                    <input
                        className={styles.searchInput}
                        placeholder="Enter First or Last Name"
                        onChange={event => setQuery(event.target.value)}
                    />
                    <div className={styles.options}>
                        <button
                            className={styles.button}
                            onClick={() => ToggleIsTypeOfClientPhysical(true)}
                        >
                            Physical clients
                        </button>
                        <button
                            className={styles.button}
                            onClick={() => ToggleIsTypeOfClientPhysical(false)}
                        >
                            Company clients
                        </button>
                    </div>
                    {!isPending && !error && isTypeOfClientPhysical && (
                        <div className={styles.physicalClients}>
                            {
                                clients.filter(client => {
                                    if (query === '') {
                                        return client;
                                    } else if ((client.firstName + " " + client.lastName).toLowerCase().includes(query.toLowerCase())) {
                                        return client;
                                    }
                                }).map((client) => (
                                    <Link
                                        key={client.id}
                                        to={`/clients/${client.id}`}
                                        state={{isTypeOfClientPhysical}}
                                        className={styles.clientLink}
                                    >
                                        <div className={styles.clientPreview}>
                                            <h2>{client.firstName} {client.lastName}</h2>
                                        </div>
                                    </Link>
                                ))
                            }
                        </div>
                    )}
                    {!isPending && !error && !isTypeOfClientPhysical && (
                        <div className={styles.companyClients}>
                            {clients.filter(client => {
                                if (query === '') {
                                    return client;
                                } else if (client.name.toLowerCase().includes(query.toLowerCase())) {
                                    return client;
                                }
                            }).map((client) => (
                                <Link
                                    key={client.id}
                                    to={`/clients/${client.id}`}
                                    state={{isTypeOfClientPhysical}}
                                    className={styles.clientLink}
                                >
                                    <div className={styles.clientPreview}>
                                        <h2>{client.name}</h2>
                                    </div>
                                </Link>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default ClientsPage;
