import React from 'react';
import {BrowserRouter as Router, Routes, Route} from 'react-router-dom';
import SoftwarePage from "./pages/SoftwarePage/SoftwarePage.jsx";
import LoginPage from "./pages/LoginPage/LoginPage.jsx";
import ProtectedRoute from "./ProtectedRoute.jsx";
import AuthProvider from "./context/AuthContext.jsx";
import HomePage from "./pages/HomePage/HomePage.jsx";
import ContractPage from "./pages/ContractPage/ContractPage.jsx";
import ClientsPage from "./pages/ClientsPage/ClientsPage.jsx";
import SpecificClientPage from "./pages/SpecificClientPage/SpecificClientPage.jsx";
import SpecificSoftwarePage from "./pages/SpecificSoftwarePage/SpecificSoftwarePage.jsx";
import CreateContractPage from "./pages/CreateContractPage/CreateContractPage.jsx";

const App = () => {
    return (
        <AuthProvider>
            <Router>
                <Routes>
                    <Route path="/login" element={<LoginPage/>}/>
                    <Route path="/" element={<ProtectedRoute><HomePage/></ProtectedRoute>}/>
                    <Route path="/software/:id" element={<ProtectedRoute><SpecificSoftwarePage/></ProtectedRoute>} />
                    <Route path="/software" element={<ProtectedRoute><SoftwarePage/></ProtectedRoute>}/>
                    <Route path="/clients/:id/createContract" element={<ProtectedRoute><CreateContractPage/></ProtectedRoute>} />
                    <Route path="/clients/:id" element={<ProtectedRoute><SpecificClientPage/></ProtectedRoute>}/>
                    <Route path="/clients" element={<ProtectedRoute><ClientsPage/></ProtectedRoute>} />


                </Routes>
            </Router>
        </AuthProvider>
    );
};

export default App;
