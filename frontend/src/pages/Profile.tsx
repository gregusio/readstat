import React from "react";
import profileService from "../services/profileService";
import ProfileCard from "../components/Card/ProfileCard";


const Profile: React.FC = () => {

    const [username, setUsername] = React.useState("");
    const [avatarUrl, setAvatarUrl] = React.useState("");
    const [bio, setBio] = React.useState("");

    const fetchUserProfile = async () => {
        try {
            const profileData = await profileService.getProfile();
            setUsername(profileData.username);
            setAvatarUrl(profileData.avatarUrl);
            setBio(profileData.bio);
            console.log(profileData);
        } catch (error) {
            console.error("Error fetching user profile:", error);
        }
    };

    React.useEffect(() => {
        fetchUserProfile();
    }, []);

    return (
        <div className="profile-page">
            <div className="profile-container">
                <h1>Welcome to your profile!</h1>
                <ProfileCard username={username} avatarUrl={avatarUrl} bio={bio} />
            </div>
            <div className="user-updates-history">
                <h2>Your Updates</h2>
            </div>
            <div className="user-friends-list">
                <h2>Your Friends</h2>
            </div>
        </div>
    );
}
export default Profile;