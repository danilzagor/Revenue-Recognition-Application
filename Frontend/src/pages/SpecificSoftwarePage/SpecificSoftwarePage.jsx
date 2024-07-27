import {useParams} from "react-router-dom";
import useFetch from "../../hooks/useFetch.jsx";
import {useEffect, useState} from "react";
import styles from "./SpecificSoftwarePage.module.css";
import NavBar from "../../components/NavbarComponent/NavBar.jsx";

const SpecificSoftwarePage = () => {
    const {id} = useParams();
    const {data: software, isPendingClient, errorClient} = useFetch(`https://localhost:7050/api/software/${id}`);
    const {
        data: actualRevenue,
        isPendingActualRevenue,
        errorActualRevenue
    } = useFetch(`https://localhost:7050/api/revenue/actual?productId=${id}`);

    const {
        data: expectedRevenue,
        isPendingExpectedRevenue,
        errorExpectedRevenue
    } = useFetch(`https://localhost:7050/api/revenue/expected?productId=${id}`);
    const [showActualRevenue, setActualRevenue] = useState(false);
    const [showExpectedRevenue, setExpectedRevenue] = useState(false);
    const [discount, setDiscount] = useState(null);

    useEffect(() => {
        if (software && software.softwareSales) {
            const maxDiscount = Math.max(
                ...software.softwareSales
                    .filter((sale) => {
                        const now = new Date();
                        return new Date(sale.startAt) <= now && new Date(sale.endAt) >= now;
                    })
                    .map((sale) => parseFloat(sale.value))
            );
            setDiscount(maxDiscount > 0 ? maxDiscount : null);
        }
    }, [software]);

    const onShowActualRevenue = (() => {
        setExpectedRevenue(false)
        setActualRevenue(!showActualRevenue)
    })
    const onShowExpectedRevenue = (() => {
        setActualRevenue(false)
        setExpectedRevenue(!showExpectedRevenue)
    })

    return (
        <div>
            <NavBar/>
            {isPendingClient && <div>Loading...</div>}
            {errorClient && <div>Error: {errorClient.message}</div>}
            {software && (
                <div className={styles.container}>
                    <h1>Software Details</h1>
                    <div className={styles.section}>
                        <h2>Software Information</h2>
                        <p><strong>Name:</strong> {software.name}</p>
                        <p><strong>Description:</strong> {software.description}</p>
                        <p className={styles.priceSection}>
                            <strong>Price:</strong>
                            {discount !== null ? (
                                <>
                                    <s> {software.price} PLN </s>
                                    <a style={{color: "#0f9308", paddingRight: "10px"}}>-{(discount) * 100}% </a>
                                    <a style={{fontSize: "125%"}}>{(software.price * (1 - discount)).toFixed(2)} PLN</a>
                                </>
                            ) : (
                                <a style={{marginLeft: "5px"}}> {software.price} PLN</a>
                            )}
                        </p>
                    </div>
                    <div className={styles.section}>
                        <h3>Categories</h3>
                        <ul>
                            {software.softwareCategories.map((category, index) => (
                                <li key={index} className={styles.tag}>{category}</li>
                            ))}
                        </ul>
                    </div>
                    <div className={styles.section}>
                        <h3>Versions</h3>
                        <ul>
                            {software.softwareVersions.map((version, index) => (
                                <li key={index}>{version.name}</li>
                            ))}
                        </ul>
                    </div>
                    <div style={{textAlign:"center"}}>
                        <button onClick={() => onShowActualRevenue()} className={styles.countButton}>Show actual
                            revenue
                        </button>
                        <button onClick={() => onShowExpectedRevenue()} className={styles.countButton}>Show expected
                            revenue
                        </button>
                        {showActualRevenue && actualRevenue && (
                            <div style={{textAlign: "center"}}>
                                <h3>Current revenue for this software is: {actualRevenue}</h3>
                            </div>
                        )}
                        {
                            showActualRevenue && !actualRevenue && (
                                <div style={{textAlign: "center"}}>
                                    <h3>Current software doesn't have any signed contracts</h3>
                                </div>
                            )
                        }

                        {showExpectedRevenue && expectedRevenue && (
                            <div style={{textAlign: "center"}}>
                                <h3>Expected revenue for this software is: {expectedRevenue}</h3>
                            </div>
                        )}
                        {
                            showExpectedRevenue && !expectedRevenue && (
                                <div style={{textAlign: "center"}}>
                                    <h3>Current software doesn't have any signed contracts</h3>
                                </div>
                            )
                        }
                    </div>
                </div>
            )
            }
        </div>
    )
        ;
};

export default SpecificSoftwarePage;
