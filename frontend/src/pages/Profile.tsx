import React from "react";
import profileService from "../services/profileService";
import ProfileCard from "../components/Card/ProfileCard";
import ActivityTimeline from "../components/Timeline/ActivityTimeline";
import { useNavigate, useParams } from "react-router-dom";
import Button from '@mui/material/Button'
import "./Profile.css";
import { useAuth } from "../context/AuthContext";


const Profile: React.FC = () => {

    const [username, setUsername] = React.useState("");
    const [bio, setBio] = React.useState("");
    const [activityHistory, setActivityHistory] = React.useState([]);
    const userId = useParams<{ userId: string }>().userId;
    const navigate = useNavigate();
    const { user } = useAuth();

    const fetchUserProfile = async () => {
        try {
            if (!userId) {
                console.error("User ID is not available");
                return;
            }
            const profileData = await profileService.getProfile(userId);
            setUsername(profileData.username);
            setBio(profileData.bio);
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

    // Check if the profile being viewed is the current user's profile
    const isOwnProfile = String(user?.id) === userId;

    return (
        <div className="profile-page">
            <div className="profile-container">
                <h1>Welcome to your profile!</h1>
                <ProfileCard username={username} bio={bio} />
            </div>
                <div className="user-friends-list">
                {isOwnProfile && (
                    <Button variant="text" onClick={() => navigate(`/${userId}/following`)}>
                        View Following
                    </Button>
                )}
                <Button variant="text" onClick={() => navigate(`/${userId}/books`)}>
                    View Books
                </Button>
                <Button variant="text" onClick={() => navigate(`/${userId}/statistics`)}>
                    View Statistics
                </Button>
            </div>
            <div className="user-updates-history">
                <h2>Your activity history</h2>
                <ActivityTimeline activities={activityHistory} />
            </div>
        </div>
    );
}
export default Profile;