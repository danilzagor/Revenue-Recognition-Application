import React, { useState, useEffect } from 'react';
import { useParams, useLocation, useNavigate } from 'react-router-dom';
import useFetch from "../../hooks/useFetch.jsx";
import NavBar from "../../components/NavbarComponent/NavBar.jsx";
import styles from './SpecificClientPage.module.css';
import { useAuth } from "../../context/AuthContext.jsx";
import axiosInstance from "../../utils/axiosInstance.jsx";
import ConfirmationPrompt from "../../components/ConfirmationPromptComponent/ConfirmationPrompt.jsx";

const SpecificClientPage = () => {
    const { user } = useAuth();
    const { id } = useParams();
    const location = useLocation();
    const { isTypeOfClientPhysical } = location.state;
    const [showContracts, setShowContracts] = useState(false);
    const [showEditingDetails, setShowEditingDetails] = useState(false);
    const [url, setUrl] = useState(`https://localhost:7050/api/clients/${isTypeOfClientPhysical ? 'physical' : 'company'}/${id}`);
    const { data: client, isPendingClient, errorClient } = useFetch(url);
    const { data: contracts, isPendingContracts, errorContracts } = useFetch(`https://localhost:7050/api/contract/client/${id}`);
    const navigate = useNavigate();
    const [requestError, setRequestError] = useState(null);

    const [name, setName] = useState("");
    const [lastName, setLastName] = useState("");
    const [address, setAddress] = useState("");
    const [phone, setPhone] = useState("");
    const [email, setEmail] = useState("");
    const [showConfirmation, setShowConfirmation] = useState(false);
    const [confirmationAction, setConfirmationAction] = useState(null);

    useEffect(() => {
        setUrl(`https://localhost:7050/api/clients/${isTypeOfClientPhysical ? 'physical' : 'company'}/${id}`);
        if (client !== null) {
            if (isTypeOfClientPhysical) {
                setName(client.firstName)
                setLastName(client.lastName)
            } else {
                setName(client.name)
            }
            setAddress(client.address)
            setEmail(client.email)
            setPhone(client.phoneNumber)
        }
    }, [client, id, isTypeOfClientPhysical]);

    const handleActionWithConfirmation = (action) => {
        setConfirmationAction(() => action);
        setShowConfirmation(true);
    };

    const handleConfirm = async () => {
        if (confirmationAction) {
            await confirmationAction();
        }
        setShowConfirmation(false);
        setConfirmationAction(null);
    };

    const handleCancel = () => {
        setShowConfirmation(false);
        setConfirmationAction(null);
    };

    const onContractButtonClick = () => {
        setShowEditingDetails(false);
        setShowContracts(!showContracts);
    }

    const onEditButtonClick = () => {
        setShowContracts(false);
        setShowEditingDetails(!showEditingDetails);
    }

    const onDeleteButtonClick = async () => {
        try {
            const response = await axiosInstance.delete(url);
            if (response.status === 204) {
                alert('Client deleted successfully!');
                navigate(`/clients`);
            } else {
                setRequestError(response.data);
            }
        } catch (error) {
            setRequestError(`Error: ${error.response.data}`);
        }
    }

    const handleSubmit = async () => {
        const data = isTypeOfClientPhysical ? {
            name, lastName, address, email, phone
        } : {
            name, address, email, phone
        };
        try {
            const response = await axiosInstance.put(url, data);
            if (response.status === 204) {
                alert('Client updated successfully!');
                navigate(`/clients`);
            } else {
                setRequestError(response.data);
            }
        } catch (error) {
            setRequestError(`Error: ${error.response.data}`);
        }
    }

    return (
        <div className={styles.clientContainer}>
            <NavBar />
            {showConfirmation && (
                <ConfirmationPrompt
                    message="Are you sure you want to proceed?"
                    onConfirm={handleConfirm}
                    onCancel={handleCancel}
                />
            )}
            {isPendingClient && <div>Loading...</div>}
            {errorClient && <div>Error: {errorClient.message}</div>}
            {client && (
                <div>
                    <div className={styles.clientDetails}>
                        {isTypeOfClientPhysical ? (
                            <div>
                                <h2>{client.firstName} {client.lastName}</h2>
                                <h3>Email: {client.email}</h3>
                                <h3>Phone number: {client.phoneNumber}</h3>
                                <h3>Address: {client.address}</h3>
                                <h3>PESEL: {client.pesel}</h3>
                            </div>
                        ) : (
                            <div>
                                <h2>{client.name}</h2>
                                <h3>Email: {client.email}</h3>
                                <h3>Phone number: {client.phoneNumber}</h3>
                                <h3>Address: {client.address}</h3>
                                <h3>KRS: {client.krs}</h3>
                            </div>
                        )}
                        <button className={styles.button} onClick={onContractButtonClick}>All contracts</button>
                        <button className={styles.button} onClick={() => navigate(`/clients/${id}/createContract`)}>Create contract</button>
                        <div>
                            {user.roles.includes('admin') && <h2>These are admin options</h2>}
                            {user.roles.includes('admin') && <button className={styles.button} onClick={onEditButtonClick}>Edit personal details</button>}
                            {user.roles.includes('admin') && <button className={styles.button} onClick={() => handleActionWithConfirmation(onDeleteButtonClick)}>Delete client</button>}
                        </div>
                    </div>
                    {showContracts && (
                        <div className={styles.contracts}>
                            {contracts && contracts.map((contract) => (
                                <div className={styles.contract} key={contract.contract.id}>
                                    <div className={styles.section}>
                                        <h3>Contract of {contract.contract.beginningDate} with {contract.software.name}</h3>
                                        <p><strong>Beginning Date:</strong> {contract.contract.beginningDate}</p>
                                        <p><strong>Ending Date:</strong> {contract.contract.endingDate}</p>
                                        <p><strong>Contract Status:</strong> {contract.contract.contractStatus}</p>
                                        <p><strong>Price:</strong> {contract.contract.price}</p>
                                        <p><strong>Actualisation Period:</strong> {contract.contract.actualisationPeriod}</p>
                                        <p><strong>Paid Amount:</strong> {contract.paidAmount}</p>
                                    </div>
                                    <div className={styles.section}>
                                        <h3>Software Information</h3>
                                        <p><strong>Company name:</strong> {contract.software.name}</p>
                                        <p><strong>Description:</strong> {contract.software.description}</p>
                                        <p><strong>Price:</strong> {contract.software.price}</p>
                                        <p><strong>Version:</strong> {contract.softwareVersion}</p>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                    {user.roles.includes('admin') && showEditingDetails && (
                        <div className={styles.editFormContainer}>
                            <form onSubmit={(e) => { e.preventDefault(); handleActionWithConfirmation(handleSubmit); }}>
                                <div className={styles.formGroup}>
                                    {isTypeOfClientPhysical ? <label>First name:</label> : <label>Company name:</label>}
                                    <input type="text" value={name} onChange={(e) => setName(e.target.value)} required />
                                </div>
                                {isTypeOfClientPhysical && (
                                    <div className={styles.formGroup}>
                                        <label>Last name:</label>
                                        <input type="text" value={lastName} onChange={(e) => setLastName(e.target.value)} required />
                                    </div>
                                )}
                                <div className={styles.formGroup}>
                                    <label>Email:</label>
                                    <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
                                </div>
                                <div className={styles.formGroup}>
                                    <label>Phone number:</label>
                                    <input type="tel" value={phone} onChange={(e) => setPhone(e.target.value)} required />
                                </div>
                                <div className={styles.formGroup}>
                                    <label>Address:</label>
                                    <input type="text" value={address} onChange={(e) => setAddress(e.target.value)} required />
                                </div>
                                {requestError && <div className={styles.errorMessage}>{requestError}</div>}
                                <div className={styles.formGroup}>
                                    <button type="submit" className={styles.button}>Save Changes</button>
                                </div>
                            </form>
                        </div>
                    )}
                </div>
            )}
        </div>
    );
};

export default SpecificClientPage;
