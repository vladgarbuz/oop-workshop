using System;
using System.Collections.Generic;
using System.IO;
using OopWorkshop.Domain.Media;
using OopWorkshop.Persistence;

namespace OopWorkshop.Tests;

public sealed class CsvMediaRepositoryTests : IDisposable
{
    private static readonly string[] HeaderColumns =
    {
        "Type","Title","Author","Director","Genre","Year","ISBN","Language","Pages","Duration","Singer","Composer","FileType","Publisher","Platform","Version","FileSize","Resolution","FileFormat","DateTaken","Host","Guest","Episode"
    };

    private static readonly string HeaderLine = string.Join(',', HeaderColumns);

    private readonly string tempFile = Path.Combine(Path.GetTempPath(), $"oop-workshop-{Guid.NewGuid():N}.csv");

    [Fact]
    public void LoadMedia_ReadsRowsIntoDomainObjects()
    {
        File.WriteAllLines(tempFile, new[]
        {
            HeaderLine,
            BuildRow(row =>
            {
                row[0] = "EBook";
                row[1] = "Sample Book";
                row[2] = "Jane Roe";
                row[4] = "Fiction";
                row[5] = "2000";
                row[6] = "1234567890";
                row[7] = "English";
                row[8] = "350";
            }),
            BuildRow(row =>
            {
                row[0] = "Movie";
                row[1] = "Sample Movie";
                row[3] = "John Director";
                row[4] = "Drama";
                row[5] = "2010";
                row[7] = "English";
                row[9] = "150";
            })
        });

        var repository = new CsvMediaRepository(tempFile);

        var items = repository.LoadMedia();

        Assert.Collection(items,
            first =>
            {
                var ebook = Assert.IsType<EBook>(first);
                Assert.Equal("Sample Book", ebook.Title);
                Assert.Equal(350, ebook.Pages);
            },
            second =>
            {
                var movie = Assert.IsType<Movie>(second);
                Assert.Equal("Sample Movie", movie.Title);
                Assert.Equal(150, movie.Duration);
            });
    }

    [Fact]
    public void SaveMedia_WritesHeaderAndRows()
    {
        var repository = new CsvMediaRepository(tempFile);
        var payload = new List<MediaItem>
        {
            new EBook("1984", "George Orwell", "English", 328, 1949, "9780452284234"),
            new Movie("Inception", "Christopher Nolan", "Sci-Fi", 2010, "English", 148)
        };

        repository.SaveMedia(payload);

        var lines = File.ReadAllLines(tempFile);
        Assert.Equal(3, lines.Length);
        Assert.Equal(HeaderLine, lines[0]);
        Assert.Contains("EBook", lines[1]);
        Assert.Contains("Movie", lines[2]);
    }

    public void Dispose()
    {
        if (File.Exists(tempFile))
            File.Delete(tempFile);
    }

    private static string BuildRow(Action<string[]> configure)
    {
        var row = new string[HeaderColumns.Length];
        for (var i = 0; i < row.Length; i++)
            row[i] = string.Empty;
        configure(row);
        return string.Join(',', row);
    }
}
