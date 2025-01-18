using Backend.DTO;
using Backend.Interfaces;
using Backend.Services;
using Moq;

namespace Backend.test.Services;

public class StatisticServiceTest
{
    private Mock<IUserStatisticRepository> _userStatisticRepositoryMock;
    private StatisticService _statisticService;

    public StatisticServiceTest()
    {
        _userStatisticRepositoryMock = new Mock<IUserStatisticRepository>();
        _statisticService = new StatisticService(_userStatisticRepositoryMock.Object);
    }

    [Fact]
    public async Task GetStatisticsSummary_ShouldReturnStatisticsSummary()
    {
        // Arrange
        _userStatisticRepositoryMock.Setup(x => x.GetTotalBooksCountAsync(It.IsAny<int>())).ReturnsAsync(10);
        _userStatisticRepositoryMock.Setup(x => x.GetTotalReadBooksCountAsync(It.IsAny<int>())).ReturnsAsync(5);
        _userStatisticRepositoryMock.Setup(x => x.GetTotalReadingBooksCountAsync(It.IsAny<int>())).ReturnsAsync(3);
        _userStatisticRepositoryMock.Setup(x => x.GetTotalUnreadBooksAsync(It.IsAny<int>())).ReturnsAsync(2);

        // Act
        var result = await _statisticService.GetStatisticsSummary(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.TotalBooks);
        Assert.Equal(5, result.TotalReadBooks);
        Assert.Equal(3, result.TotalReadingBooks);
        Assert.Equal(2, result.TotalUnreadBooks);
    }

    [Fact]
    public async Task GetStatisticsMonthlyReadBookCountPerYear_ShouldReturnMonthlyReadBookCountPerYear()
    {
        // Arrange
        _userStatisticRepositoryMock.Setup(x => x.GetMonthlyReadBookCountPerYearAsync(It.IsAny<int>())).ReturnsAsync(new Dictionary<int, List<MonthlyStats>>
        {
            {
                2021, new List<MonthlyStats>
                {
                    new MonthlyStats
                    {
                        Month = "January",
                        Count = 1
                    }
                }
            }
        });

        // Act
        var result = await _statisticService.GetStatisticsMonthlyReadBookCountPerYear(1);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(2021, result.First().Key);
        Assert.Single(result.First().Value);

    }
}

