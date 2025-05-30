import React, { useState } from "react";
import { TextField, Button, Box, Typography } from "@mui/material";
import authService from "../../services/authService";
import { useNavigate } from "react-router-dom";

const RegisterForm: React.FC = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    if (password !== confirmPassword) {
      setError("Passwords do not match");
      return;
    }

    try {
      await authService.register({ username: username, password });
      setSuccess("Registration successful!");
      await new Promise((resolve) => setTimeout(resolve, 1000));
      navigate("/login");
    } catch (err) {
      setError("Registration failed. Please try again.");
    }
  };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{ maxWidth: 400, margin: "auto", textAlign: "center", mt: 4 }}
    >
      <TextField
        label="Username"
        variant="outlined"
        fullWidth
        margin="normal"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        required
      />
      <TextField
        label="Password"
        type="password"
        variant="outlined"
        fullWidth
        margin="normal"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        required
      />
      <TextField
        label="Confirm Password"
        type="password"
        variant="outlined"
        fullWidth
        margin="normal"
        value={confirmPassword}
        onChange={(e) => setConfirmPassword(e.target.value)}
        required
      />
      {error && (
        <Typography color="error" variant="body2" sx={{ mt: 1 }}>
          {error}
        </Typography>
      )}
      {success && (
        <Typography color="success" variant="body2" sx={{ mt: 1 }}>
          {success}
        </Typography>
      )}
      <Button
        type="submit"
        variant="contained"
        color="primary"
        fullWidth
        sx={{ mt: 2 }}
      >
        Register
      </Button>

      <Typography variant="body2" sx={{ mt: 2 }}>
        Already have an account?
        <Button
          color="primary"
          sx={{ textTransform: "none" }}
          onClick={() => navigate("/login")}
        >
          Login
        </Button>
      </Typography>
    </Box>
  );
};

export default RegisterForm;
