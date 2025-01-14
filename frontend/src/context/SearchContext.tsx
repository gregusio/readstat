import { createContext, useState, ReactNode } from "react";

type SearchContextType = {
  searchQuery: string;
  setSearchQuery: (value: string) => void;
  clearSearchQuery: () => void;
};

export const SearchContext = createContext<SearchContextType>({
  searchQuery: "",
  setSearchQuery: (_value: string) => {},
  clearSearchQuery: () => {},
});

export const SearchProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [searchQuery, setSearchQuery] = useState("");

  const clearSearchQuery = () => {
    setSearchQuery("");
  };

  return (
    <SearchContext.Provider
      value={{ searchQuery, setSearchQuery, clearSearchQuery }}
    >
      {children}
    </SearchContext.Provider>
  );
};
