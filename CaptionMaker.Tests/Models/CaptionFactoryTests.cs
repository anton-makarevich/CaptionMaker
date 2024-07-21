using CaptionMaker.Core.Models;

namespace CaptionMaker.Tests.Models;

public class CaptionFactoryTests
{
    private const string SrtContent = """

                                      1
                                      00:00:01,000 --> 00:00:03,000
                                      First caption line

                                      2
                                      00:00:04,000 --> 00:00:06,000
                                      Second caption line

                                      """;
    [Fact]
    public async Task CreateFromSrt_ParsesSrtFileCorrectly()
    {
        // Arrange
        var tempFilePath = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFilePath, SrtContent);

        try
        {
            // Act
            var result = await CaptionFactory.CreateFromSrt(tempFilePath);

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Equal("First caption line", result[0].Text);
            Assert.Equal(TimeSpan.FromSeconds(1), result[0].Start);
            Assert.Equal(TimeSpan.FromSeconds(3), result[0].End);

            Assert.Equal("Second caption line", result[1].Text);
            Assert.Equal(TimeSpan.FromSeconds(4), result[1].Start);
            Assert.Equal(TimeSpan.FromSeconds(6), result[1].End);
        }
        finally
        {
            // Clean up
            File.Delete(tempFilePath);
        }
    }
    
    
    [Fact]
    public async Task CreateFromSrt_ParsesSrtFileCorrectly_WithAdjustment()
    {
        // Arrange
        var tempFilePath = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFilePath, SrtContent);

        var adjustment = TimeSpan.FromSeconds(2);

        try
        {
            // Act
            var result = await CaptionFactory.CreateFromSrt(tempFilePath, adjustment);

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Equal("First caption line", result[0].Text);
            Assert.Equal(TimeSpan.FromSeconds(3), result[0].Start);  // Adjusted by 2 seconds
            Assert.Equal(TimeSpan.FromSeconds(5), result[0].End);    // Adjusted by 2 seconds

            Assert.Equal("Second caption line", result[1].Text);
            Assert.Equal(TimeSpan.FromSeconds(6), result[1].Start);  // Adjusted by 2 seconds
            Assert.Equal(TimeSpan.FromSeconds(8), result[1].End);    // Adjusted by 2 seconds
        }
        finally
        {
            // Clean up
            File.Delete(tempFilePath);
        }
    }
}
