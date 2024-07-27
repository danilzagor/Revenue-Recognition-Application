import NavBar from "../../components/NavbarComponent/NavBar.jsx";
import './HomePage.css';
import useFetch from "../../hooks/useFetch.jsx";
import {useState} from "react";
import styles from "../SpecificSoftwarePage/SpecificSoftwarePage.module.css";

const HomePage = () => {

    const {
        data: actualRevenue,
        isPendingActualRevenue,
        errorActualRevenue
    } = useFetch(`https://localhost:7050/api/revenue/actual`);

    const {
        data: expectedRevenue,
        isPendingExpectedRevenue,
        errorExpectedRevenue
    } = useFetch(`https://localhost:7050/api/revenue/expected`);
    const [showActualRevenue, setActualRevenue] = useState(false);
    const [showExpectedRevenue, setExpectedRevenue] = useState(false);

    const onShowActualRevenue = (() => {
        setExpectedRevenue(false)
        setActualRevenue(!showActualRevenue)
    })
    const onShowExpectedRevenue = (() => {
        setActualRevenue(false)
        setExpectedRevenue(!showExpectedRevenue)
    })

    return (
        <div className="home">
            <nav className="navbar"><NavBar/></nav>
            <div className="content">
                <h1>Home Page</h1>
                <h3 style={{textAlign:"center"}}>My Revenue Recognition System front-end application offers a user-friendly interface for managing
                    customers, software products, contracts, and revenue calculations. Easily add or update individual
                    or corporate customer details, with options for soft deleting individual customers. Manage software
                    products by storing product information, including versions, categories and discounts. Create detailed contracts for software purchases,
                    specifying essential information like dates, prices, discounts, and update periods. Offer additional
                    support years for an extra fee. Ensure full payments are made within contract terms. Calculate both
                    current and expected revenue with options for currency conversion using public exchange rates. Enjoy
                    a seamless and efficient experience designed to maintain transparency and trust in financial
                    reporting.</h3>
                <div style={{textAlign: "center"}}>
                    <button onClick={() => onShowActualRevenue()} className={styles.countButton}>Show actual
                        revenue
                    </button>
                    <button onClick={() => onShowExpectedRevenue()} className={styles.countButton}>Show expected
                        revenue
                    </button>

                    {showActualRevenue && actualRevenue && (
                        <div style={{textAlign: "center"}}>
                            <h3>Current revenue for the company is: {actualRevenue}</h3>
                        </div>
                    )}

                    {showExpectedRevenue && expectedRevenue && (
                        <div style={{textAlign: "center"}}>
                            <h3>Expected revenue for the company is: {expectedRevenue}</h3>
                        </div>
                    )}

                </div>

            </div>

        </div>
    )
}
export default HomePage