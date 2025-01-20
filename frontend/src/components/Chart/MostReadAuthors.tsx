import React, { useEffect, useState } from "react";
import { axisClasses, BarChart } from "@mui/x-charts";
import { Skeleton, Typography } from "@mui/material";
import statisticService from "../../services/statisticService";

const chartSetting = {
    yAxis: [
        {
            label: "Count",
        },
    ],
    height: 300,
    margin: { left: 60, right: 20 },
    sx: {
        [`.${axisClasses.left} .${axisClasses.label}`]: {
            transform: "translate(-20px, 0)",
        }
    },
};

const MostReadAuthors: React.FC = () => {
    const [chartData, setChartData] = useState<any>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        statisticService.getMostReadAuthors().then((data) => {
            const authors = Object.keys(data);

            const dataset = authors.map((author) => ({
                author: author,
                count: data[author],
            }));

            setChartData({
                labels: dataset,
                series: [{ type: "bar", dataKey: "count" }],
            });
            setLoading(false);
        });
    }, []);

    if (loading) {
        return <Skeleton variant="rectangular" width={500} height={300} />;
    }

    return (
        <>
            <Typography variant="h6">Most Read Authors</Typography>
            <BarChart
                xAxis={[{ dataKey: "author", scaleType: "band", label: "Author" }]}
                series={chartData.series}
                dataset={chartData.labels}
                {...chartSetting}
            />
        </>
    );
};

export default MostReadAuthors;
