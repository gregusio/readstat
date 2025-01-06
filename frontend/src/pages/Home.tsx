import { Button } from "@mui/material";
import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import authService from "../services/authService";

const Home: React.FC = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("accessToken");
    if (!token) {
      navigate("/login");
    }
  }, [navigate]);

  return (
    <div>
      <h1>Welcome Home!</h1>
      <Button
        onClick={() => {
          authService.logout();
        }}
      >
        Logout
      </Button>
    </div>
  );
};

export default Home;
