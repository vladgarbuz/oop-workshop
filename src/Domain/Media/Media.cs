using OopWorkshop.Domain.Interfaces;
using System;

namespace OopWorkshop.Domain.Media
{
    public abstract class MediaItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }

        public MediaItem(string title, string language)
        {
            Id = Guid.NewGuid();
            Title = title;
            Language = language;
        }

        public abstract void ShowDetails();
    }
    public class EBook : MediaItem, IReadable, IDownloadable
    {
        public string Author { get; set; }
        public int Pages { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }

        public EBook(string title, string author, string language, int pages, int year, string isbn)
            : base(title, language)
        {
            Author = author;
            Pages = pages;
            Year = year;
            ISBN = isbn;
        }

        public override void ShowDetails()
        {
            Console.WriteLine($"EBook: {Title}, Author: {Author}, Language: {Language}, Pages: {Pages}, Year: {Year}, ISBN: {ISBN}");
        }
        public void Read() => Console.WriteLine($"Reading {Title}");
        public void Download() => Console.WriteLine($"Downloading {Title}");
    }

    public class Movie : MediaItem, IWatchable, IDownloadable
    {
        public string Director { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public int Duration { get; set; }

        public Movie(string title, string director, string genre, int releaseYear, string language, int duration)
            : base(title, language)
        {
            Director = director;
            Genre = genre;
            ReleaseYear = releaseYear;
            Duration = duration;
        }

        public override void ShowDetails()
        {
            Console.WriteLine($"Movie: {Title}, Director: {Director}, Genre: {Genre}, Year: {ReleaseYear}, Language: {Language}, Duration: {Duration} min");
        }
        public void Watch() => Console.WriteLine($"Watching {Title}");
        public void Download() => Console.WriteLine($"Downloading {Title}");
    }

    public class Song : MediaItem, IPlayable, IDownloadable
    {
        public string Composer { get; set; }
        public string Singer { get; set; }
        public string Genre { get; set; }
        public string FileType { get; set; }
        public int Duration { get; set; }

        public Song(string title, string composer, string singer, string genre, string fileType, int duration, string language)
            : base(title, language)
        {
            Composer = composer;
            Singer = singer;
            Genre = genre;
            FileType = fileType;
            Duration = duration;
        }

        public override void ShowDetails()
        {
            Console.WriteLine($"Song: {Title}, Composer: {Composer}, Singer: {Singer}, Genre: {Genre}, FileType: {FileType}, Duration: {Duration} sec, Language: {Language}");
        }
        public void Play() => Console.WriteLine($"Playing {Title}");
        public void Download() => Console.WriteLine($"Downloading {Title}");
    }

    public class VideoGame : MediaItem, IPlayable, ICompletable, IDownloadable
    {
        public string Publisher { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public string Platform { get; set; }

        public VideoGame(string title, string genre, string publisher, int releaseYear, string platform, string language)
            : base(title, language)
        {
            Genre = genre;
            Publisher = publisher;
            ReleaseYear = releaseYear;
            Platform = platform;
        }

        public override void ShowDetails()
        {
            Console.WriteLine($"VideoGame: {Title}, Genre: {Genre}, Publisher: {Publisher}, Year: {ReleaseYear}, Platform: {Platform}, Language: {Language}");
        }
        public void Play() => Console.WriteLine($"Playing {Title}");
        public void Complete() => Console.WriteLine($"Completed {Title}");
        public void Download() => Console.WriteLine($"Downloading {Title}");
    }

    public class App : MediaItem, IDownloadable, IExecutable
    {
        public string Version { get; set; }
        public string Publisher { get; set; }
        public string Platform { get; set; }
        public double FileSize { get; set; }

        public App(string title, string version, string publisher, string platform, double fileSize, string language)
            : base(title, language)
        {
            Version = version;
            Publisher = publisher;
            Platform = platform;
            FileSize = fileSize;
        }

        public override void ShowDetails()
        {
            Console.WriteLine($"App: {Title}, Version: {Version}, Publisher: {Publisher}, Platform: {Platform}, FileSize: {FileSize} MB, Language: {Language}");
        }
        public void Download() => Console.WriteLine($"Downloading {Title}");
        public void Execute() => Console.WriteLine($"Executing {Title}");
    }

    public class Podcast : MediaItem, IPlayable, ICompletable, IDownloadable
    {
        public string Host { get; set; }
        public string Guest { get; set; }
        public int Episode { get; set; }
        public int ReleaseYear { get; set; }

        public Podcast(string title, string host, string guest, int episode, int releaseYear, string language)
            : base(title, language)
        {
            Host = host;
            Guest = guest;
            Episode = episode;
            ReleaseYear = releaseYear;
        }

        public override void ShowDetails()
        {
            Console.WriteLine($"Podcast: {Title}, Host: {Host}, Guest: {Guest}, Episode: {Episode}, Year: {ReleaseYear}, Language: {Language}");
        }
        public void Play() => Console.WriteLine($"Listening to {Title}");
        public void Complete() => Console.WriteLine($"Completed {Title}");
        public void Download() => Console.WriteLine($"Downloading {Title}");
    }

    public class ImageFile : MediaItem, IDisplayable, IDownloadable
    {
        public string Resolution { get; set; }
        public string FileFormat { get; set; }
        public double FileSize { get; set; }
        public string DateTaken { get; set; }

        public ImageFile(string title, string resolution, string fileFormat, double fileSize, string dateTaken, string language)
            : base(title, language)
        {
            Resolution = resolution;
            FileFormat = fileFormat;
            FileSize = fileSize;
            DateTaken = dateTaken;
        }

        public override void ShowDetails()
        {
            Console.WriteLine($"Image: {Title}, Resolution: {Resolution}, Format: {FileFormat}, Size: {FileSize} KB, Date Taken: {DateTaken}, Language: {Language}");
        }
        public void Display() => Console.WriteLine($"Displaying {Title}");
        public void Download() => Console.WriteLine($"Downloading {Title}");
    }
}
