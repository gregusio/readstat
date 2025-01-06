import React from "react";
import RegisterForm from "../components/Auth/RegisterForm";
import "./Register.css";

const Register: React.FC = () => {
  return (
    <div className="register-container">
      <div className="register-box">
      <h1 className="register-title">Register</h1>
      <RegisterForm />
      </div>
    </div>
  );
};

export default Register;