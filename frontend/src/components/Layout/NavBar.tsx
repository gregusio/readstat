import React from "react";
import { AppBar, Toolbar, Typography, Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

const NavBar: React.FC = () => {
  const { isAuthenticated, logout } = useAuth();

  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <AppBar position="fixed">
      <Toolbar>  
        <img src="/book_icon.png" alt="book icon" style={{ width: "30px", height: "30px", marginRight: "10px", marginBottom: "5px" }} />
        <Typography variant="h6" sx={{ flexGrow: 1 }}>
          Readstat
        </Typography>
        {isAuthenticated ? (
          <>
            <Button color="inherit" onClick={() => navigate("/profile")}>
              Profile
            </Button>
            <Button color="inherit" onClick={handleLogout}>
              Logout
            </Button>
          </>
        ) : (
          <Button color="inherit" onClick={() => navigate("/login")}>
            Login
          </Button>
        )}
      </Toolbar>
    </AppBar>
  );
};

export default NavBar;