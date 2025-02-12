import React from "react";
import { Routes, Route } from "react-router-dom";
import Home from "../pages/Home";
import Login from "../pages/Login";
import Register from "../pages/Register";
import ProtectedRoute from "./ProtectedRoute";
import Books from "../pages/Books";
import BookDetail from "../pages/BookDetail";
import Statistics from "../pages/Statistics";
import AddBook from "../pages/AddBook";
import DrawBook from "../pages/DrawBook";
import PublicRoute from "./PublicRoute";

const AppRoutes: React.FC = () => {
  return (
    <Routes>
      <Route element={<ProtectedRoute />}>
        <Route path="*" element={<Home />} />
        <Route path="/home" element={<Home />} />
        <Route path="/books" element={<Books />} />
        <Route path="/books/:id" element={<BookDetail />} />
        <Route path="/statistics" element={<Statistics />} />
        <Route path="/add-book" element={<AddBook />} />
        <Route path="/drawbook" element={<DrawBook />} />
      </Route>

      <Route element={<PublicRoute />}>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
      </Route>
    </Routes>
  );
};

export default AppRoutes;
