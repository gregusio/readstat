import React, { createContext, useState, useContext, useEffect } from "react";
import apiClient from "../services/apiClient";

interface User {
  id: string;
  username: string;
}

interface AuthContextType {
  isAuthenticated: boolean;
  user: User | null;
  setUser: (user: User | null) => void;
  loading: boolean;
  login: () => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [user, setUser] = useState<User | null>(null);
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchUser = async () => {
      const token = localStorage.getItem("accessToken");
      if (token) {
        try {
          const response = await apiClient.get("/Auth/me");
          setIsAuthenticated(true);
          setLoading(false);
          setUser(response.data);
        } catch (error) {
          console.error("Error fetching user:", error);
          setUser(null);
        }
      }
    };

    fetchUser();
  }, []);

  const login = async () => {
    setLoading(false);
    setIsAuthenticated(true);
    localStorage.removeItem("userBooks");
    const response = await apiClient.get("/Auth/me");
    if (response.data) {
      setUser(response.data);
    } else {
      console.error("Login failed, no user data returned");
      setUser(null);
    }
  };

  const logout = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("userBooks");
    setIsAuthenticated(false);
    setUser(null);
    setLoading(true);
  };

  return (
    <AuthContext.Provider
      value={{ isAuthenticated, user, setUser, loading, login, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
