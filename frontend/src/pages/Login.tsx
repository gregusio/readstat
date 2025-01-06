import React from "react";
import LoginForm from "../components/Auth/LoginForm";
import "./Login.css";

const Login: React.FC = () => {
  return (
    <div className="login-container">
      <div className="login-box">
        <h1 className="login-title">Hello bookworm!</h1>
        <LoginForm />
      </div>
    </div>
  );
};

export default Login;