import React from 'react';
import { Card, CardContent, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';

interface BookCardProps {
    id: number;
    title: string;
    author: string;
    shelf: string;
}

const BookCard: React.FC<BookCardProps> = ({ id, title, author, shelf }) => {
    const navigate = useNavigate();

    const handleClick = () => {
        navigate(`/books/${id}`);
    }

    return (
        <Card onClick={handleClick} style={{ cursor: 'pointer' }}>
            <CardContent style={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-between' }}>
            <Typography variant="h5" style={{ flex: 1, textAlign: 'center' }}>
                {title}
            </Typography>
            <Typography variant="body2" color="text.secondary" style={{ flex: 1, textAlign: 'center' }}>
                {author}
            </Typography>
            <Typography variant="body2" color="text.secondary" style={{ flex: 1, textAlign: 'center' }}>
                {shelf}
            </Typography>
            </CardContent>
        </Card>
    );
};

export default BookCard;