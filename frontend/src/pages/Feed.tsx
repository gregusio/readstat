import React from "react";
import feedService from "../services/feedService";
import FeedTimeline from "../components/Timeline/FeedTimeline";
import { useNavigate, useParams } from "react-router-dom";


const Feed: React.FC = () => {

    const [feedContent, setFeedContent] = React.useState([]);
    const userId = useParams<{ userId: string }>().userId;


    const fetchFeed = async () => {
        try {
            if (!userId) {
                console.error("User ID is not available");
                return;
            }
            const feedData = await feedService.getFeed();
            setFeedContent(feedData);
        } catch (error) {
            console.error("Error fetching user activity history:", error);
        }
    };

    React.useEffect(() => {
        fetchFeed()
    }, []);


       return (
        <div className="feed-page">
            <FeedTimeline activities={feedContent} />
        </div>
    );
}
export default Feed;