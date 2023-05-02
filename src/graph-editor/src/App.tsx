import React from "react";
import GraphPage from "graph/pages/GraphPage";
import { Navigate, Route, Routes, } from "react-router-dom";
import MainPage from "graph/pages/MainPage";
import LoginPage from "auth/pages/LoginPage";
import RegisterPage from "auth/pages/RegisterPage";
import AccountPage from "auth/pages/AccountPage";

function App() {
    return (
        <Routes>
            <Route path="*" element={<Navigate to="/main" />} />
            <Route path={"login/"} element={<LoginPage />} />
            <Route path={"main/"} element={<MainPage />} />
            <Route path={"register/"} element={<RegisterPage />} />
            <Route path={"graphList/"} element={<AccountPage />} />
            <Route path={"graphs/:graphId"} element={<GraphPage />} />
        </Routes>
    );
}

export default App;
