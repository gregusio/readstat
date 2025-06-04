import React from "react";
import { useAuth } from "../../context/AuthContext";
import { Button, Grid, TextField } from "@mui/material";
import profileService from "../../services/profileService";
import { useParams } from "react-router-dom";

interface ProfileCardProps {
    name: string;
}

const ProfileCard: React.FC<ProfileCardProps> = ({ name }) => {
    const { user } = useAuth();
    const userId = useParams<{ userId: string }>().userId;
    const isOwnProfile = user && String(user.id) === userId;
    const [isEditing, setIsEditing] = React.useState(false);
    const [editedUsername, setEditedUsername] = React.useState(name);
    const [username, setUsername] = React.useState(name);

    React.useEffect(() => {
        setEditedUsername(name);
        setUsername(name);
    }, [name]);


    const handleEditProfile = () => {
        setIsEditing(true);
    }

    const handleSaveProfile = async () => {
        setIsEditing(false);
        try {
            const response = await profileService.updateProfile({ username: editedUsername });
            if (response) {
                alert("Profile updated successfully!");
            } else {
                alert("Failed to update profile. Please try again.");
            }
        } catch (error) {
            alert("Failed to update profile. Please try again.");
        }
        setUsername(editedUsername);
    }


    return (
        <div className="profile-card">
            <Grid container spacing={5} direction="row" alignItems="left">
                {isOwnProfile ? (
                    isEditing ? (
                        <>
                            <TextField
                                id="username"
                                label="Username"
                                value={editedUsername}
                                onChange={(e) => { setEditedUsername(e.target.value); }}
                            />
                            <Button onClick={handleSaveProfile}>Save</Button>
                        </>
                    ) : (
                        <>
                            <h2>Username: {username}</h2>
                            <Button onClick={handleEditProfile}>Edit Profile</Button>
                        </>
                    )
                ) : (
                    <h2>Username: {username}</h2>
                )}
            </Grid>

        </div>
    );
}

export default ProfileCard;