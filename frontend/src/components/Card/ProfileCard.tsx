import React from "react";
import { useAuth } from "../../context/AuthContext";

interface ProfileCardProps {
    username: string;
    bio: string;
}
const ProfileCard: React.FC<ProfileCardProps> = ({ username, bio }) => {
    const { user } = useAuth();

    return (
        <div className="profile-card">
            <h2>Username: {username}</h2>
            <p>{bio}</p>
            {user && <button>Edit Profile</button>}
        </div>
    );
}

export default ProfileCard;