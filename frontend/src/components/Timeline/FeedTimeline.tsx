import React, { useState } from 'react';
import { Box, Typography, IconButton } from '@mui/material';
import FavoriteIcon from '@mui/icons-material/Favorite';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';

interface FeedTimelineProps {
    activityType: string;
    description: string;
    activityDate: string;
    username: string;
    userId: string;
    likeCount: number;
}

const FeedTimeline: React.FC<{ activities: FeedTimelineProps[] }> = ({ activities }) => {
    const [likedActivities, setLikedActivities] = useState<Set<number>>(new Set());
    const [likeCounts, setLikeCounts] = useState<{ [key: number]: number }>({});

    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        return date.toISOString().slice(0, 16).replace('T', ' ');
    };

    const handleLike = (index: number, initialLikeCount: number) => {
        const isLiked = likedActivities.has(index);
        const newLikedActivities = new Set(likedActivities);
        
        if (isLiked) {
            newLikedActivities.delete(index);
            setLikeCounts(prev => ({
                ...prev,
                [index]: (prev[index] !== undefined ? prev[index] : initialLikeCount) - 1
            }));
        } else {
            newLikedActivities.add(index);
            setLikeCounts(prev => ({
                ...prev,
                [index]: (prev[index] !== undefined ? prev[index] : initialLikeCount) + 1
            }));
        }
        
        setLikedActivities(newLikedActivities);
    };

    return (
        <Box sx={{ maxWidth: 600, margin: '0 auto', padding: 2 }}>
            <Typography variant="h4" component="h2" gutterBottom>
                Activity Feed
            </Typography>
            
            {activities.map((activity, idx) => {
                const isLiked = likedActivities.has(idx);
                // Ensure we have a valid number for like count
                const initialLikeCount = typeof activity.likeCount === 'number' ? activity.likeCount : 0;
                const currentLikeCount = likeCounts[idx] !== undefined ? likeCounts[idx] : initialLikeCount;
                
                return (
                    <Box
                        key={idx}
                        sx={{
                            border: '2px solid white',
                            borderRadius: 2,
                            padding: 2,
                            marginBottom: 2,
                            backgroundColor: 'background.default',
                            boxShadow: 1,
                        }}
                    >
                        {/* Date at the top */}
                        <Typography 
                            variant="caption" 
                            color="text.secondary"
                            sx={{ display: 'block', marginBottom: 1 }}
                        >
                            {formatDate(activity.activityDate)}
                        </Typography>
                        
                        {/* Username and activity */}
                        <Typography variant="body1" sx={{ marginBottom: 1 }}>
                            <strong>@{activity.username}</strong>
                        </Typography>
                        
                        <Typography variant="body2" sx={{ marginBottom: 2 }}>
                            <strong>{activity.activityType}</strong>: {activity.description}
                        </Typography>
                        
                        {/* Heart and like count at the bottom */}
                        <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'flex-end' }}>
                            <IconButton 
                                onClick={() => handleLike(idx, initialLikeCount)}
                                color={isLiked ? 'error' : 'default'}
                                size="small"
                            >
                                {isLiked ? <FavoriteIcon /> : <FavoriteBorderIcon />}
                            </IconButton>
                            <Typography variant="body2" color="text.secondary">
                                {currentLikeCount}
                            </Typography>
                        </Box>
                    </Box>
                );
            })}
        </Box>
    );
};

export default FeedTimeline;