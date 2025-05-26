import Timeline from '@mui/lab/Timeline';
import { TimelineConnector, TimelineContent, TimelineDot, TimelineItem, TimelineOppositeContent, timelineOppositeContentClasses, TimelineSeparator } from '@mui/lab';
import React from 'react';

interface ActivityTimelineProps {
    activityType: string;
    description: string;
    activityDate: string;
}

const ActivityTimeline: React.FC<{ activities: ActivityTimelineProps[] }> = ({ activities }) => {
    return (
        <Timeline sx={{
        [`& .${timelineOppositeContentClasses.root}`]: {
          flex: 0.2,
        },
      }}>
            <h2>Activity Timeline</h2>
            {activities.map((activity, idx) => (
            <TimelineItem key={idx}>
                <TimelineOppositeContent color="text.secondary">
                    <div>{activity.activityDate}</div>
                    
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