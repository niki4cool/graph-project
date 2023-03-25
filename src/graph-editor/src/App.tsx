import React from "react";
import GraphPage from "graph/pages/GraphPage";
import { Navigate, Route, Routes, } from "react-router-dom";
import MainPage from "graph/pages/MainPage";
import LoginPage from "auth/pages/LoginPage";
import RegisterPage from "auth/pages/RegisterPage";
import UnauthorizedPage from "auth/pages/UnauthorizedPage";

function App() {
    return (
        <Routes>
            <Route path="*" element={<Navigate to="/" />} />
            <Route index element={<UnauthorizedPage />} />
            <Route path={"login/"} element={<LoginPage />} />
            <Route path={"main/"} element={<MainPage />} />
            <Route path={"register/"} element={<RegisterPage />} />
            <Route path={"graphs/:graphId"} element={<GraphPage />} />
        </Routes>
    );
}

export default App;
