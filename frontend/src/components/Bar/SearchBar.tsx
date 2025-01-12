import { Box } from "@mui/material";
import React, { useContext, useRef } from "react";
import { SearchContext } from "../../context/SearchContext";
import { useNavigate } from "react-router-dom";

const SearchBar = () => {
  const { setSearchQuery } = useContext(SearchContext);
  const navigate = useNavigate();
  const inputRef = useRef<HTMLInputElement>(null);

  const handleSearch = (event: React.ChangeEvent<HTMLInputElement> | React.KeyboardEvent<HTMLInputElement>) => {
    const target = event.target as HTMLInputElement;
    setSearchQuery(target.value);
    navigate('/books');
  };

  const clearSearch = () => {
    if (inputRef.current) {
      inputRef.current.value = '';
    }
  };

  return (
    <Box
      sx={{
        flexGrow: 1,
        display: "flex",
        justifyContent: "right",
        marginRight: "20px",
      }}
    >
      <input
        ref={inputRef}
        type="text"
        placeholder="Search books..."
        onKeyDown={(event) => {
          if (event.key === 'Enter') {
            handleSearch(event);
            clearSearch();
          }
        }}
      />
    </Box>
  );
};

export default SearchBar;