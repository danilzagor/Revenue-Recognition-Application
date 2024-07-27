import { Link, useNavigate, useParams } from "react-router-dom";
import useFetch from "../../hooks/useFetch.jsx";
import { useState } from "react";
import axiosInstance from "../../utils/axiosInstance.jsx";
import styles from './CreateContractPage.module.css';
import NavBar from "../../components/NavbarComponent/NavBar.jsx";

const CreateContractPage = () => {
    const { id } = useParams();
    const { data: software, isPending, error } = useFetch("https://localhost:7050/api/software");
    const [query, setQuery] = useState("");
    const [selectedSoftware, setSelectedSoftware] = useState(null);
    const [selectedVersion, setSelectedVersion] = useState(null);
    const [beginningDate, setBeginningDate] = useState('');
    const [endingDate, setEndingDate] = useState('');
    const [actualisationPeriod, setActualisationPeriod] = useState('');
    const navigate = useNavigate();
    const [requestError, setRequestError] = useState(null);

    const handleSoftwareClick = (specificSoftware) => {
        setSelectedSoftware(specificSoftware);
        setSelectedVersion(null);
    };

    const handleVersionClick = (version) => {
        setSelectedVersion(version);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!selectedSoftware || !selectedVersion) {
            alert("Please select a software and version.");
            return;
        }

        const contractData = {
            beginningDate,
            endingDate,
            actualisationPeriod,
            softwareId: selectedSoftware.id,
            softwareVersionId: selectedVersion.id
        };
        try {
            const response = await axiosInstance.post(`https://localhost:7050/api/contract/${id}`, contractData);

            if (response.status === 201) {
                alert('Contract created successfully!');
                navigate(`/clients/${id}`);
            } else {
                setRequestError(response.data);
            }
        } catch (error) {
            setRequestError(`Error: ${error.response.data}`);
        }
    };

    return (
        <div>
            <NavBar></NavBar>
        <div className={styles.container}>
            <form onSubmit={handleSubmit}>
                <div style={{textAlign:"center",display:"flex",alignItems:"center",flexDirection:"column",marginBottom:"10px"}}>
                <label>Beginning Date of the contract</label>
                <input type="date" value={beginningDate} onChange={(e) => setBeginningDate(e.target.value)} required/>

                <label>Ending Date of the contract</label>
                <input type="date" value={endingDate} onChange={(e) => setEndingDate(e.target.value)} required/>

                <label>Actualisation Period of the software</label>
                <select value={actualisationPeriod} onChange={(e) => setActualisationPeriod(e.target.value)} required>
                    <option value="1" defaultValue>1 year</option>
                    <option value="2">2 years</option>
                    <option value="3">3 years</option>
                    <option value="4">4 years</option>
                </select>
                </div>
                <div style={{textAlign: "center"}}>
                    <input
                        className={styles.searchInput}
                        placeholder="Search software"
                        onChange={event => setQuery(event.target.value)}
                    />
                </div>

                <div className={styles.softwareList}>
                    {!isPending && !error && software && (
                        software.filter(specificSoftware => {
                            if (query === '') {
                                return specificSoftware;
                            } else if (specificSoftware.name.toLowerCase().includes(query.toLowerCase())) {
                                return specificSoftware;
                            }
                        }).map(specificSoftware => (
                            <div
                                onClick={() => handleSoftwareClick(specificSoftware)}
                                className={`${styles.softwareItem} ${selectedSoftware && selectedSoftware.id === specificSoftware.id ? styles.selected : ''}`}
                                key={specificSoftware.id}
                            >
                                <h2>{specificSoftware.name}</h2>
                            </div>
                        ))
                    )}
                </div>

                {selectedSoftware && (
                    <div className={styles.versionList}>
                        <h3>Select a Version for {selectedSoftware.name}</h3>
                        <ul>
                            {selectedSoftware.softwareVersions.map(version => (
                                <li
                                    key={version.id}
                                    onClick={() => handleVersionClick(version)}
                                    className={selectedVersion && selectedVersion.id === version.id ? styles.selected : ''}
                                >
                                    {version.name}
                                </li>
                            ))}
                        </ul>
                    </div>
                )}


                {requestError && <div className={styles.errorMessage}>{requestError}</div>}

                <input type="submit" value="Submit" className={styles.submitButton}/>
            </form>
        </div>
        </div>
    );
};

export default CreateContractPage;
