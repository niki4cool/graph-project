import React from "react";
import GraphPage from "graph/GraphPage";
import {Navigate, Route, Routes,} from "react-router-dom";
import MainPage from "graph/pages/MainPage";

function App() {
  return (
    <Routes>
      <Route path="*" element={<Navigate to="/"/>}/>
      <Route index element={<MainPage/>}/>
      <Route path={"graphs/:graphId"} element={<GraphPage/>}/>
    </Routes>
  );
}

export default App;
