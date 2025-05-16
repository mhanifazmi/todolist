import React from "react";
import { Routes, Route, Link } from "react-router-dom";
import TodoPage from "./pages/todo/Index.jsx";

function App() {
    return (
        <div className="App">
            <div className="container mt-5">
                <Routes>
                    <Route path="/" element={<TodoPage />} />
                </Routes>
            </div>
        </div>
    );
}

export default App;
