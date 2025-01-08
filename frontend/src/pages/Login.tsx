import React from "react";
import LoginForm from "../components/Auth/LoginForm";
import "./Login.css";

const Login: React.FC = () => {
  return (
    <div className="login-page">
      <div className="login-container">
        <h1>Hello bookworm!</h1>
        <LoginForm />
      </div>
    </div>
  );
};

export default Login;