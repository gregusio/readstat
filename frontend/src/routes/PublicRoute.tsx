import React from "react";
import { useAuth } from "../context/AuthContext";
import { Navigate, Outlet } from "react-router-dom";

const PublicRoute: React.FC = () => {
  const { isAuthenticated } = useAuth();

  if (isAuthenticated) {
    console.log("Redirecting to books");
    return <Navigate to="/books" />;
  }

  return <Outlet />;
};

export default PublicRoute;
