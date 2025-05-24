import React from "react";
import { useAuth } from "../../context/AuthContext";

interface ProfileCardProps {
    username: string;
    avatarUrl: string;
    bio: string;
}
const ProfileCard: React.FC<ProfileCardProps> = ({ username, avatarUrl, bio }) => {
    const { user } = useAuth();

    return (
        <div className="profile-card">
            <img src={avatarUrl} alt={`${username}'s avatar`} />
            <h2>{username}</h2>
            <p>{bio}</p>
            {user && <button>Edit Profile</button>}
        </div>
    );
}

export default ProfileCard;