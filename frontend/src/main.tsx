import React from "react";
import ReactDOM from "react-dom/client";
import AppProvider from "./components/Layout/AppProvider";
import { AuthProvider } from "./context/AuthContext";
import AppRoutes from "./routes";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <AuthProvider>
      <AppProvider>
        <AppRoutes />
      </AppProvider>
    </AuthProvider>
  </React.StrictMode>
);
