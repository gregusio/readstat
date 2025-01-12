import React from "react";
import ReactDOM from "react-dom/client";
import { AuthProvider } from "./context/AuthContext";
import AppRoutes from "./routes";
import App from "./App";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <AuthProvider>
      <App>
        <AppRoutes />
      </App>
    </AuthProvider>
  </React.StrictMode>
);
