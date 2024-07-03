using CaptionMaker.Core.Models;
using CaptionMaker.Core.Services;

namespace CaptionMaker.Tests.Services;

public class CaptionStructuriserTests
{
    [Theory]
    [MemberData(nameof(GetTestCases))]
    public void TestProcessCaptions(List<CaptionLine> captions, List<CaptionLine> expected)
    {
        var processor = new CaptionStructuriser();
        var result = processor.ProcessCaptions(captions);

        Assert.Equal(expected.Count, result.Count);
        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Text, result[i].Text);
        }
    }

    public static IEnumerable<object[]> GetTestCases()
    {
        yield return
        [
            new List<CaptionLine>
            {
                new() { Text = "Hello!!! my name is", Start = TimeSpan.FromSeconds(0), End = TimeSpan.FromSeconds(5) },
                new() { Text = "James. I want to tell you", Start = TimeSpan.FromSeconds(5), End = TimeSpan.FromSeconds(10) },
                new() { Text = "some interesting story.", Start = TimeSpan.FromSeconds(10), End = TimeSpan.FromSeconds(15) },
            },
            new List<CaptionLine>
            {
                new() { Text = "Hello!!! my name is James.", Start = TimeSpan.FromSeconds(1.5), End = TimeSpan.FromSeconds(6.5) },
                new() { Text = "I want to tell you some interesting story.", Start = TimeSpan.FromSeconds(6.5), End = TimeSpan.FromSeconds(15) }
            }
        ];

        yield return
        [
            new List<CaptionLine>
            {
                new() { Text = "This is a very long sentence that needs to be split.", Start = TimeSpan.FromSeconds(0), End = TimeSpan.FromSeconds(20) },
            },
            new List<CaptionLine>
            {
                new() { Text = "This is a very long", Start = TimeSpan.FromSeconds(0), End = TimeSpan.FromSeconds(10) },
                new() { Text = "sentence that needs to be split.", Start = TimeSpan.FromSeconds(10), End = TimeSpan.FromSeconds(20) }
            }
        ];
        
        yield return
        [
            new List<CaptionLine>
            {
                new() { Text = "Short sentence.", Start = TimeSpan.FromSeconds(0), End = TimeSpan.FromSeconds(2) },
                new() { Text = "Another one.", Start = TimeSpan.FromSeconds(2), End = TimeSpan.FromSeconds(4) },
                new() { Text = "Yet another short one.", Start = TimeSpan.FromSeconds(4), End = TimeSpan.FromSeconds(6) },
            },
            new List<CaptionLine>
            {
                new() { Text = "Short sentence. Another one.", Start = TimeSpan.FromSeconds(0), End = TimeSpan.FromSeconds(4) },
                new() { Text = "Yet another short one.", Start = TimeSpan.FromSeconds(4), End = TimeSpan.FromSeconds(6) }
            }
        ];

        yield return
        [
            new List<CaptionLine>
            {
                new() { Text = "A short segment.", Start = TimeSpan.FromSeconds(0), End = TimeSpan.FromSeconds(3) },
                new() { Text = "Another segment with more content and should be adjusted.", Start = TimeSpan.FromSeconds(3), End = TimeSpan.FromSeconds(15) },
            },
            new List<CaptionLine>
            {
                new() { Text = "A short segment.", Start = TimeSpan.FromSeconds(0), End = TimeSpan.FromSeconds(5) },
                new() { Text = "Another segment with more content and should", Start = TimeSpan.FromSeconds(5), End = TimeSpan.FromSeconds(10) },
                new() { Text = "be adjusted.", Start = TimeSpan.FromSeconds(10), End = TimeSpan.FromSeconds(15) }
            }
        ];
    }
}