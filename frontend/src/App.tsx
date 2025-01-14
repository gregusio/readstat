import React from "react";
import { ThemeProvider } from "@mui/material/styles";
import { CssBaseline } from "@mui/material";
import { BrowserRouter as Router } from "react-router-dom";
import DashboardLayout from "./components/Layout/DashboardLayout";
import theme from "./theme/theme";
import { SearchProvider } from "./context/SearchContext";

interface AppProviderProps {
  children: React.ReactNode;
}

const App: React.FC<AppProviderProps> = ({ children }) => {
  return (
    <SearchProvider>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <Router>
          <DashboardLayout>{children}</DashboardLayout>
        </Router>
      </ThemeProvider>
    </SearchProvider>
  );
};

export default App;
