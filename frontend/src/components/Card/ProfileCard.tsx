import React from "react";
import { useAuth } from "../../context/AuthContext";
import { useParams } from "react-router-dom";

interface ProfileCardProps {
    username: string;
    bio: string;
}
const ProfileCard: React.FC<ProfileCardProps> = ({ username, bio }) => {
    const { user } = useAuth();
    const userId = useParams<{ userId: string }>().userId;
    const isOwnProfile = user && String(user.id) === userId;

    return (
        <div className="profile-card">
            <h2>Username: {username}</h2>
            <p>{bio}</p>
            {isOwnProfile && <button>Edit Profile</button>}
        </div>
    );
}

export default ProfileCard;