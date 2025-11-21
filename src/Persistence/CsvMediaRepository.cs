using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using OopWorkshop.Domain.Media;

namespace OopWorkshop.Persistence
{
    public class CsvMediaRepository
    {
        private static readonly string[] HeaderColumns = new[]
        {
            "Type","Title","Author","Director","Genre","Year","ISBN","Language","Pages","Duration","Singer","Composer","FileType","Publisher","Platform","Version","FileSize","Resolution","FileFormat","DateTaken","Host","Guest","Episode"
        };

        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        public string FilePath { get; }

        public CsvMediaRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("A CSV path is required.", nameof(filePath));

            FilePath = Path.GetFullPath(filePath);
        }

        public List<MediaItem> LoadMedia()
        {
            var items = new List<MediaItem>();
            if (!File.Exists(FilePath))
            {
                EnsureDirectoryExists();
                return items;
            }

            using var reader = new StreamReader(FilePath);
            reader.ReadLine(); // discard header
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var columns = SplitCsvLine(line);
                var media = CreateMedia(columns);
                if (media != null)
                    items.Add(media);
            }

            return items;
        }

        public void SaveMedia(IEnumerable<MediaItem> items)
        {
            EnsureDirectoryExists();
            using var writer = new StreamWriter(FilePath, false, Encoding.UTF8);
            writer.WriteLine(string.Join(',', HeaderColumns));
            foreach (var media in items)
            {
                var row = Serialize(media);
                writer.WriteLine(ToCsvLine(row));
            }
        }

        private void EnsureDirectoryExists()
        {
            var directory = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        private static MediaItem? CreateMedia(string[] columns)
        {
            if (columns.Length < HeaderColumns.Length)
                columns = PadColumns(columns, HeaderColumns.Length);

            var type = columns[0]?.Trim();
            var title = columns[1]?.Trim();
            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(title))
                return null;

            var normalized = type.ToLowerInvariant();
            return normalized switch
            {
                "ebook" => CreateEBook(columns),
                "movie" => CreateMovie(columns),
                "song" => CreateSong(columns),
                "videogame" => CreateVideoGame(columns),
                "app" => CreateApp(columns),
                "podcast" => CreatePodcast(columns),
                "image" => CreateImage(columns),
                "imagefile" => CreateImage(columns),
                _ => null
            };
        }

        private static MediaItem? CreateEBook(string[] c)
        {
            var year = ParseInt(c[5]);
            return new EBook(
                c[1].Trim(),
                OrDefault(c[2], "Unknown Author"),
                OrDefault(c[7], "Unknown"),
                Math.Max(0, ParseInt(c[8])),
                year == 0 ? DateTime.Now.Year : year,
                OrDefault(c[6], string.Empty));
        }

        private static MediaItem? CreateMovie(string[] c)
        {
            var releaseYear = ParseInt(c[5]);
            var duration = ParseInt(c[9]);
            return new Movie(
                c[1].Trim(),
                OrDefault(c[3], "Unknown Director"),
                OrDefault(c[4], "Unknown"),
                releaseYear == 0 ? DateTime.Now.Year : releaseYear,
                OrDefault(c[7], "Unknown"),
                duration == 0 ? 90 : duration);
        }

        private static MediaItem? CreateSong(string[] c)
        {
            return new Song(
                c[1].Trim(),
                OrDefault(c[11], "Unknown Composer"),
                OrDefault(c[10], "Unknown Singer"),
                OrDefault(c[4], "Unknown"),
                OrDefault(c[12], "MP3"),
                Math.Max(0, ParseInt(c[9])),
                OrDefault(c[7], "Unknown"));
        }

        private static MediaItem? CreateVideoGame(string[] c)
        {
            var releaseYear = ParseInt(c[5]);
            return new VideoGame(
                c[1].Trim(),
                OrDefault(c[4], "Unknown"),
                OrDefault(c[13], "Unknown"),
                releaseYear == 0 ? DateTime.Now.Year : releaseYear,
                OrDefault(c[14], "Unknown"),
                OrDefault(c[7], "English"));
        }

        private static MediaItem? CreateApp(string[] c)
        {
            return new App(
                c[1].Trim(),
                OrDefault(c[15], "1.0"),
                OrDefault(c[13], "Unknown"),
                OrDefault(c[14], "Unknown"),
                ParseDouble(c[16]),
                OrDefault(c[7], "English"));
        }

        private static MediaItem? CreatePodcast(string[] c)
        {
            var releaseYear = ParseInt(c[5]);
            return new Podcast(
                c[1].Trim(),
                OrDefault(c[20], "Unknown Host"),
                OrDefault(c[21], "Unknown Guest"),
                Math.Max(1, ParseInt(c[22])),
                releaseYear == 0 ? DateTime.Now.Year : releaseYear,
                OrDefault(c[7], "English"));
        }

        private static MediaItem? CreateImage(string[] c)
        {
            return new ImageFile(
                c[1].Trim(),
                OrDefault(c[17], ""),
                OrDefault(c[18], ""),
                ParseDouble(c[16]),
                OrDefault(c[19], string.Empty),
                OrDefault(c[7], string.Empty));
        }

        private static string[] Serialize(MediaItem item)
        {
            var row = new string[HeaderColumns.Length];
            row[0] = GetTypeLabel(item);
            row[1] = item.Title;
            row[7] = item.Language;

            switch (item)
            {
                case EBook ebook:
                    row[2] = ebook.Author;
                    row[5] = ebook.Year.ToString(Culture);
                    row[6] = ebook.ISBN;
                    row[8] = ebook.Pages.ToString(Culture);
                    break;
                case Movie movie:
                    row[3] = movie.Director;
                    row[4] = movie.Genre;
                    row[5] = movie.ReleaseYear.ToString(Culture);
                    row[9] = movie.Duration.ToString(Culture);
                    break;
                case Song song:
                    row[4] = song.Genre;
                    row[9] = song.Duration.ToString(Culture);
                    row[10] = song.Singer;
                    row[11] = song.Composer;
                    row[12] = song.FileType;
                    break;
                case VideoGame game:
                    row[4] = game.Genre;
                    row[5] = game.ReleaseYear.ToString(Culture);
                    row[13] = game.Publisher;
                    row[14] = game.Platform;
                    break;
                case App app:
                    row[13] = app.Publisher;
                    row[14] = app.Platform;
                    row[15] = app.Version;
                    row[16] = app.FileSize.ToString(Culture);
                    break;
                case Podcast podcast:
                    row[4] = "";
                    row[5] = podcast.ReleaseYear.ToString(Culture);
                    row[20] = podcast.Host;
                    row[21] = podcast.Guest;
                    row[22] = podcast.Episode.ToString(Culture);
                    break;
                case ImageFile image:
                    row[16] = image.FileSize.ToString(Culture);
                    row[17] = image.Resolution;
                    row[18] = image.FileFormat;
                    row[19] = image.DateTaken;
                    break;
            }

            return row;
        }

        private static string GetTypeLabel(MediaItem item) => item switch
        {
            EBook => "EBook",
            Movie => "Movie",
            Song => "Song",
            VideoGame => "VideoGame",
            App => "App",
            Podcast => "Podcast",
            ImageFile => "Image",
            _ => item.GetType().Name
        };

        private static int ParseInt(string? input) => int.TryParse(input, NumberStyles.Integer, Culture, out var value) ? value : 0;

        private static double ParseDouble(string? input) => double.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, Culture, out var value) ? value : 0d;

        private static string OrDefault(string? value, string fallback) => string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

        private static string[] SplitCsvLine(string line)
        {
            var values = new List<string>();
            var current = new StringBuilder();
            var inQuotes = false;
            for (var i = 0; i < line.Length; i++)
            {
                var ch = line[i];
                if (ch == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (ch == ',' && !inQuotes)
                {
                    values.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(ch);
                }
            }
            values.Add(current.ToString());

            if (values.Count < HeaderColumns.Length)
            {
                values.AddRange(Enumerable.Repeat(string.Empty, HeaderColumns.Length - values.Count));
            }
            else if (values.Count > HeaderColumns.Length)
            {
                values = values.Take(HeaderColumns.Length).ToList();
            }

            return values.ToArray();
        }

        private static string ToCsvLine(string[] columns) => string.Join(',', columns.Select(Escape));

        private static string Escape(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Contains('"'))
                value = value.Replace("\"", "\"\"");

            return value.IndexOfAny(new[] { ',', '\n', '\r', '"' }) >= 0 ? $"\"{value}\"" : value;
        }

        private static string[] PadColumns(IReadOnlyCollection<string> existing, int desired)
        {
            var result = new string[desired];
            var index = 0;
            foreach (var value in existing)
            {
                if (index >= desired)
                    break;
                result[index++] = value;
            }
            for (; index < desired; index++)
                result[index] = string.Empty;
            return result;
        }
    }
}
