import React from "react";
import { Card, CardContent, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

interface BookCardProps {
  userId: string;
  id: number;
  title: string;
  author: string;
  shelf: string;
}

const BookCard: React.FC<BookCardProps> = ({ userId, id, title, author, shelf }) => {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(`/${userId}/books/${id}`);
  };

  return (
    <Card onClick={handleClick} style={{ cursor: "pointer", height: "250px" }}>
      <CardContent
      style={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        height: "100%",
        gap: "10px",
      }}
      >
      <Typography variant="h5" style={{ textAlign: "center" }}>
        {title}
      </Typography>
      <Typography
        variant="body1"
        color="text.secondary"
        style={{ textAlign: "center" }}
      >
        {author}
      </Typography>
      <Typography
        variant="body2"
        color="text.secondary"
        style={{ textAlign: "center" }}
      >
        {shelf}
      </Typography>
      </CardContent>
    </Card>
  );
};

export default BookCard;
