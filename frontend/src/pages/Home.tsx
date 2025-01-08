import { Button } from "@mui/material";
import React from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

const Home: React.FC = () => {
  const navigate = useNavigate();
  const { logout } = useAuth();

  return (
    <div>
      <h1>Welcome Home!</h1>
      <Button
        onClick={() => {
          logout();
          navigate("/login");
        }}
      >
        Logout
      </Button>
    </div>
  );
};

export default Home;
