import React from "react";
import profileService from "../services/profileService";
import ProfileCard from "../components/Card/ProfileCard";
import ActivityTimeline from "../components/Timeline/ActivityTimeline";
import { useParams } from "react-router-dom";


const Profile: React.FC = () => {

    const [username, setUsername] = React.useState("");
    const [avatarUrl, setAvatarUrl] = React.useState("");
    const [bio, setBio] = React.useState("");
    const [activityHistory, setActivityHistory] = React.useState([]);
    const userId = useParams<{ userId: string }>().userId;

    const fetchUserProfile = async () => {
        try {
            if (!userId) {
                console.error("User ID is not available");
                return;
            }
            const profileData = await profileService.getProfile(userId);
            setUsername(profileData.username);
            setAvatarUrl(profileData.avatarUrl);
            setBio(profileData.bio);
            console.log(profileData);
        } catch (error) {
            console.error("Error fetching user profile:", error);
        }
    };

    const fetchUserActivityHistory = async () => {
        try {
            if (!userId) {
                console.error("User ID is not available");
                return;
            }
            const historyData = await profileService.getUserActivityHistory(userId);
            setActivityHistory(historyData);
        } catch (error) {
            console.error("Error fetching user activity history:", error);
        }
    };

    React.useEffect(() => {
        fetchUserProfile();
        fetchUserActivityHistory();
    }, []);

    return (
        <div className="profile-page">
            <div className="profile-container">
                <h1>Welcome to your profile!</h1>
                <ProfileCard username={username} avatarUrl={avatarUrl} bio={bio} />
            </div>
            <div className="user-updates-history">
                <h2>Your activity history</h2>
                <ActivityTimeline activities={activityHistory} />
            </div>
            <div className="user-friends-list">
                <h2>Your Friends</h2>
            </div>
        </div>
    );
}
export default Profile;