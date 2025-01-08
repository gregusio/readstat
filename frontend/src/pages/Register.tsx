import React from "react";
import RegisterForm from "../components/Auth/RegisterForm";
import "./Register.css";

const Register: React.FC = () => {
  return (
    <div className="register-page">
      <div className="register-container">
        <h1>Register</h1>
        <RegisterForm />
      </div>
    </div>
  );
};

export default Register;
