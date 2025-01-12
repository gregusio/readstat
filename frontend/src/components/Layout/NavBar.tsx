import React, { useContext } from "react";
import {
  AppBar,
  Toolbar,
  Typography,
  Box,
  Tooltip,
  IconButton,
  Avatar,
  Menu,
  MenuItem,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import InputFileUpload from "../Button/InputFileButton";
import SearchBar from "../Bar/SearchBar";
import { SearchContext } from "../../context/SearchContext";

const pages = ["Home", "Books", "Statistics"];

const NavBar: React.FC = () => {
  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(
    null
  );
  const { isAuthenticated, logout } = useAuth();

  const navigate = useNavigate();
  const { clearSearchQuery } = useContext(SearchContext);

  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  const handleGoToPage = (page: string) => {
    clearSearchQuery();
    navigate(`/${page.toLowerCase()}`);
    handleCloseUserMenu();
  };

  const handleLogout = () => {
    logout();
    handleCloseUserMenu();
    navigate("/login");
  };

  const settings = [
    { label: "Profile", action: () => handleGoToPage("Profile") },
    { label: "Logout", action: handleLogout },
  ];

  return (
    <AppBar position="fixed">
      <Toolbar>
        <img
          src="/book_icon.png"
          alt="book icon"
          style={{
            width: "30px",
            height: "30px",
            marginRight: "10px",
            marginBottom: "5px",
          }}
        />
        <Typography variant="h6" sx={{ marginRight: "20px" }}>
          Readstat
        </Typography>
        {isAuthenticated ? (
          <>
            <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "left" }}>
              {pages.map((page) => (
                <MenuItem key={page} onClick={handleGoToPage.bind(null, page)}>
                  <Typography sx={{ textAlign: "center" }}>{page}</Typography>
                </MenuItem>
              ))}
            </Box>

            <SearchBar />

            <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
              <InputFileUpload />
              <Tooltip title="Click it!" placement="bottom">
              <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                <Avatar alt="Remy Sharp" src="/book_icon.png" />
              </IconButton>
              </Tooltip>
              <Menu
              sx={{ mt: "45px" }}
              id="menu-appbar"
              anchorEl={anchorElUser}
              anchorOrigin={{
                vertical: "top",
                horizontal: "right",
              }}
              keepMounted
              transformOrigin={{
                vertical: "top",
                horizontal: "right",
              }}
              open={Boolean(anchorElUser)}
              onClose={handleCloseUserMenu}
              >
              {settings.map((setting) => (
                <MenuItem key={setting.label} onClick={setting.action}>
                <Typography sx={{ textAlign: "center" }}>
                  {setting.label}
                </Typography>
                </MenuItem>
              ))}
              </Menu>
            </Box>
          </>
        ) : null}
      </Toolbar>
    </AppBar>
  );
};

export default NavBar;
