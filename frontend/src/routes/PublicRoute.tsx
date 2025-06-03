import React from "react";
import { useAuth } from "../context/AuthContext";
import { Navigate, Outlet } from "react-router-dom";

const PublicRoute: React.FC = () => {
  const { isAuthenticated } = useAuth();

  if (isAuthenticated) {
    console.log("Redirecting to /home");
    return <Navigate to="/home" />;
  }

  return <Outlet />;
};

export default PublicRoute;
