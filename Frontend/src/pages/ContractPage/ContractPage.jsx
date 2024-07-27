import NavBar from "../../components/NavbarComponent/NavBar.jsx";
import './ContractPage.css';
import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
const ContractPage = () => {

    const {id} = useParams();
    const {data:contract, isPending, error} = useFetch('http://localhost:7050/api/contract/'+id);


    return (
        <div className="contract-page-container">
            <nav className="navbar"><NavBar/></nav>
            <div className="content">
                <h1>Contract Page</h1>

            </div>

        </div>
    )
}
export default ContractPage