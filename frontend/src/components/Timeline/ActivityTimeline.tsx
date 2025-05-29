import Timeline from '@mui/lab/Timeline';
import { TimelineConnector, TimelineContent, TimelineDot, TimelineItem, TimelineOppositeContent, timelineOppositeContentClasses, TimelineSeparator } from '@mui/lab';
import React from 'react';

interface ActivityTimelineProps {
    activityType: string;
    description: string;
    activityDate: string;
}

const ActivityTimeline: React.FC<{ activities: ActivityTimelineProps[] }> = ({ activities }) => {
    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        return date.toISOString().slice(0, 16).replace('T', ' ');
    };
    
    return (
        <Timeline sx={{
        [`& .${timelineOppositeContentClasses.root}`]: {
          flex: 0.2,
        },
          '& .MuiTimelineItem-root': {
            minHeight: '50px',
            marginBottom: '-14px', 
        },
      }}>
            <h2>Activity Timeline</h2>
            {activities.map((activity, idx) => (
            <TimelineItem key={idx}>
                <TimelineOppositeContent color="text.secondary">
                    <div>{formatDate(activity.activityDate)}</div>
                    
                </TimelineOppositeContent>
                <TimelineSeparator>
                    <TimelineDot />
                    {/* <TimelineConnector /> */}
                </TimelineSeparator>
                <TimelineContent>
                    <div>
                        <strong>{activity.activityType}</strong>: {activity.description}
                    </div>
                </TimelineContent>
            </TimelineItem>
            ))}

            

        </Timeline>
    );
};

export default ActivityTimeline;