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
import Profile from "../pages/Profile";
import Following from "../pages/Following";

const AppRoutes: React.FC = () => {
  return (
    <Routes>
      <Route element={<ProtectedRoute />}>
        <Route path="*" element={<Home />} />
        <Route path="/home" element={<Home />} />
        <Route path="/:userId/books" element={<Books />} />
        <Route path="/:userId/books/:bookId" element={<BookDetail />} />
        <Route path="/:userId/statistics" element={<Statistics />} />
        <Route path="/:userId/books/add" element={<AddBook />} />
        <Route path="/:userId/drawbook" element={<DrawBook />} />
        <Route path="/:userId/profile" element={<Profile />} />
        <Route path="/:userId/following" element={<Following />} />
      </Route>

      <Route element={<PublicRoute />}>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
      </Route>
    </Routes>
  );
};

export default AppRoutes;
