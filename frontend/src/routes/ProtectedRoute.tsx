import React from "react";
import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { Skeleton } from "@mui/material";

const ProtectedRoute: React.FC = () => {
  const { isAuthenticated, loading } = useAuth();

  if (loading) {
    return <Skeleton variant="rectangular" />;
  }

  if (!isAuthenticated) {
    console.log("Redirecting to login");
    return <Navigate to="/login" />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
