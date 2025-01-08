import React from "react";
import { ThemeProvider } from "@mui/material/styles";
import { CssBaseline } from "@mui/material";
import { BrowserRouter as Router } from "react-router-dom";
import theme from "../../theme/theme";
import DashboardLayout from "./DashboardLayout";


interface AppProviderProps {
  children: React.ReactNode;
}

const AppProvider: React.FC<AppProviderProps> = ({ children }) => {
return (
    <ThemeProvider theme={theme}>
        <CssBaseline />
        <Router>
            <DashboardLayout>
            {children}
            </DashboardLayout>
        </Router>
    </ThemeProvider>
);
};

export default AppProvider;