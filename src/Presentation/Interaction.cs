using System;
using System.Collections.Generic;
using OopWorkshop.Domain;
using OopWorkshop.Domain.Media;
using OopWorkshop.Domain.User;
using OopWorkshop.Domain.Interfaces;

namespace OopWorkshop.Presentation
{
	public class ConsoleUI
	{
		private LibraryCatalog catalog = new LibraryCatalog();
		private Admin admin;
		private Employee employee;
		private Borrower borrower;

		public void Start()
		{
			Console.WriteLine("Welcome to the OOP Library System");
			SelectRole();
		}

		private void SelectRole()
		{
			Console.WriteLine("Select your role:");
			Console.WriteLine("1. Admin");
			Console.WriteLine("2. Employee");
			Console.WriteLine("3. Borrower");
			string input = Console.ReadLine();
			switch (input)
			{
				case "1":
					admin = new Admin("Admin", 30, "000-00-0000");
					ShowAdminMenu();
					break;
				case "2":
					employee = new Employee("Employee", 25, "111-11-1111");
					ShowEmployeeMenu();
					break;
				case "3":
					borrower = new Borrower("Borrower", 20, "222-22-2222");
					ShowBorrowerMenu();
					break;
				default:
					Console.WriteLine("Invalid selection. Try again.");
					SelectRole();
					break;
			}
		}

		private void ShowAdminMenu()
		{
			while (true)
			{
				Console.WriteLine("\nAdmin Menu:");
				Console.WriteLine("1. View all users");
				Console.WriteLine("2. Create user");
				Console.WriteLine("3. Update user");
				Console.WriteLine("4. Delete user");
				Console.WriteLine("5. Media Management");
				Console.WriteLine("0. Exit");
				string input = Console.ReadLine();
				switch (input)
				{
					case "1": admin.ViewAllUsers(); break;
					case "2": CreateUserFlow(); break;
					case "3": UpdateUserFlow(); break;
					case "4": DeleteUserFlow(); break;
					case "5": ShowEmployeeMenu(admin); break;
					case "0": return;
					default: Console.WriteLine("Invalid selection."); break;
				}
			}
		}

		private void CreateUserFlow()
		{
			Console.Write("Name: "); string name = Console.ReadLine();
			Console.Write("Age: "); int age = ReadInt();
			Console.Write("SSN: "); string ssn = Console.ReadLine();
			Console.Write("Is Employee? (y/n): "); bool isEmployee = Console.ReadLine().ToLower() == "y";
			admin.CreateUser(name, age, ssn, isEmployee);
		}

		private void UpdateUserFlow()
		{
			Console.Write("Enter user name to update: "); string name = Console.ReadLine();
			var user = admin.Users.Find(u => u.Name == name);
			if (user == null) { Console.WriteLine("User not found."); return; }
			Console.Write("New Name: "); string newName = Console.ReadLine();
			Console.Write("New Age: "); int newAge = ReadInt();
			Console.Write("New SSN: "); string newSsn = Console.ReadLine();
			admin.UpdateUser(user, newName, newAge, newSsn);
		}

		private void DeleteUserFlow()
		{
			Console.Write("Enter user name to delete: "); string name = Console.ReadLine();
			var user = admin.Users.Find(u => u.Name == name);
			if (user == null) { Console.WriteLine("User not found."); return; }
			admin.DeleteUser(user);
		}

		private void ShowEmployeeMenu(Employee emp = null)
		{
			var user = emp ?? employee;
			while (true)
			{
				Console.WriteLine("\nEmployee Menu:");
				Console.WriteLine("1. Add media item");
				Console.WriteLine("2. Remove media item");
				Console.WriteLine("3. View media details");
				Console.WriteLine("0. Back");
				string input = Console.ReadLine();
				switch (input)
				{
					case "1": AddMediaFlow(user); break;
					case "2": RemoveMediaFlow(user); break;
					case "3": ViewMediaDetailsFlow(); break;
					case "0": return;
					default: Console.WriteLine("Invalid selection."); break;
				}
			}
		}

		private void AddMediaFlow(Employee user)
		{
			Console.WriteLine("Select media type: 1.EBook 2.Movie 3.Song 4.VideoGame 5.App 6.Podcast 7.ImageFile");
			string type = Console.ReadLine();
			MediaItem item = null;
			try
			{
				switch (type)
				{
					case "1": item = CreateEBook(); break;
					case "2": item = CreateMovie(); break;
					case "3": item = CreateSong(); break;
					case "4": item = CreateVideoGame(); break;
					case "5": item = CreateApp(); break;
					case "6": item = CreatePodcast(); break;
					case "7": item = CreateImageFile(); break;
					default: Console.WriteLine("Invalid type."); return;
				}
				user.AddMedia(catalog.MediaItems, item);
			}
			catch { Console.WriteLine("Error adding media. Check input."); }
		}

		private void RemoveMediaFlow(Employee user)
		{
			Console.Write("Enter media title to remove: "); string title = Console.ReadLine();
			var item = catalog.SearchByTitle(title);
			if (item == null) { Console.WriteLine("Media not found."); return; }
			user.RemoveMedia(catalog.MediaItems, item);
		}

		private void ViewMediaDetailsFlow()
		{
			Console.Write("Enter media title: "); string title = Console.ReadLine();
			var item = catalog.SearchByTitle(title);
			if (item == null) { Console.WriteLine("Media not found."); return; }
			item.ShowDetails();
		}

		private void ShowBorrowerMenu()
		{
			while (true)
			{
				Console.WriteLine("\nBorrower Menu:");
				Console.WriteLine("1. List media by type");
				Console.WriteLine("2. View media details");
				Console.WriteLine("3. Borrow media item");
				Console.WriteLine("4. Rate borrowed item");
				Console.WriteLine("5. Media actions");
				Console.WriteLine("0. Exit");
				string input = Console.ReadLine();
				switch (input)
				{
					case "1": ListMediaByTypeFlow(); break;
					case "2": ViewMediaDetailsFlow(); break;
					case "3": BorrowMediaFlow(); break;
					case "4": RateMediaFlow(); break;
					case "5": MediaActionFlow(); break;
					case "0": return;
					default: Console.WriteLine("Invalid selection."); break;
				}
			}
		}

		private void ListMediaByTypeFlow()
		{
			Console.WriteLine("Select type: 1.EBook 2.Movie 3.Song 4.VideoGame 5.App 6.Podcast 7.ImageFile");
			string type = Console.ReadLine();
			Type t = type switch
			{
				"1" => typeof(EBook),
				"2" => typeof(Movie),
				"3" => typeof(Song),
				"4" => typeof(VideoGame),
				"5" => typeof(App),
				"6" => typeof(Podcast),
				"7" => typeof(ImageFile),
				_ => null
			};
			if (t == null) { Console.WriteLine("Invalid type."); return; }
			borrower.ListMediaByType(catalog.MediaItems, t);
		}

		private void BorrowMediaFlow()
		{
			Console.Write("Enter media title to borrow: "); string title = Console.ReadLine();
			var item = catalog.SearchByTitle(title);
			if (item == null) { Console.WriteLine("Media not found."); return; }
			borrower.Borrow(item);
		}

		private void RateMediaFlow()
		{
			Console.Write("Enter media title to rate: "); string title = Console.ReadLine();
			var item = catalog.SearchByTitle(title);
			if (item == null) { Console.WriteLine("Media not found."); return; }
			Console.Write("Enter rating (1-5): "); int rating = ReadInt(1, 5);
			borrower.Rate(item, rating);
		}

		private void MediaActionFlow()
		{
			Console.Write("Enter media title: "); string title = Console.ReadLine();
			var item = catalog.SearchByTitle(title);
			if (item == null) { Console.WriteLine("Media not found."); return; }
			if (item is IReadable r) r.Read();
			if (item is IPlayable p) p.Play();
			if (item is IWatchable w) w.Watch();
			if (item is ICompletable c) c.Complete();
			if (item is IDownloadable d) d.Download();
			if (item is IExecutable e) e.Execute();
			if (item is IDisplayable disp) disp.Display();
		}

		private int ReadInt(int min = int.MinValue, int max = int.MaxValue)
		{
			int val;
			while (!int.TryParse(Console.ReadLine(), out val) || val < min || val > max)
				Console.Write($"Enter a valid number ({min}-{max}): ");
			return val;
		}

		// Media creation helpers
		private EBook CreateEBook()
		{
			Console.Write("Title: "); string title = Console.ReadLine();
			Console.Write("Author: "); string author = Console.ReadLine();
			Console.Write("Language: "); string lang = Console.ReadLine();
			Console.Write("Pages: "); int pages = ReadInt(1);
			Console.Write("Year: "); int year = ReadInt(1000, 9999);
			Console.Write("ISBN: "); string isbn = Console.ReadLine();
			return new EBook(title, author, lang, pages, year, isbn);
		}
		private Movie CreateMovie()
		{
			Console.Write("Title: "); string title = Console.ReadLine();
			Console.Write("Director: "); string director = Console.ReadLine();
			Console.Write("Genre: "); string genre = Console.ReadLine();
			Console.Write("Year: "); int year = ReadInt(1000, 9999);
			Console.Write("Language: "); string lang = Console.ReadLine();
			Console.Write("Duration (min): "); int duration = ReadInt(1);
			return new Movie(title, director, genre, year, lang, duration);
		}
		private Song CreateSong()
		{
			Console.Write("Title: "); string title = Console.ReadLine();
			Console.Write("Composer: "); string composer = Console.ReadLine();
			Console.Write("Singer: "); string singer = Console.ReadLine();
			Console.Write("Genre: "); string genre = Console.ReadLine();
			Console.Write("FileType: "); string fileType = Console.ReadLine();
			Console.Write("Duration (sec): "); int duration = ReadInt(1);
			Console.Write("Language: "); string lang = Console.ReadLine();
			return new Song(title, composer, singer, genre, fileType, duration, lang);
		}
		private VideoGame CreateVideoGame()
		{
			Console.Write("Title: "); string title = Console.ReadLine();
			Console.Write("Genre: "); string genre = Console.ReadLine();
			Console.Write("Publisher: "); string publisher = Console.ReadLine();
			Console.Write("Year: "); int year = ReadInt(1000, 9999);
			Console.Write("Platform: "); string platform = Console.ReadLine();
			Console.Write("Language: "); string lang = Console.ReadLine();
			return new VideoGame(title, genre, publisher, year, platform, lang);
		}
		private App CreateApp()
		{
			Console.Write("Title: "); string title = Console.ReadLine();
			Console.Write("Version: "); string version = Console.ReadLine();
			Console.Write("Publisher: "); string publisher = Console.ReadLine();
			Console.Write("Platform: "); string platform = Console.ReadLine();
			Console.Write("FileSize (MB): "); double fileSize = double.Parse(Console.ReadLine());
			Console.Write("Language: "); string lang = Console.ReadLine();
			return new App(title, version, publisher, platform, fileSize, lang);
		}
		private Podcast CreatePodcast()
		{
			Console.Write("Title: "); string title = Console.ReadLine();
			Console.Write("Host: "); string host = Console.ReadLine();
			Console.Write("Guest: "); string guest = Console.ReadLine();
			Console.Write("Episode: "); int episode = ReadInt(1);
			Console.Write("Year: "); int year = ReadInt(1000, 9999);
			Console.Write("Language: "); string lang = Console.ReadLine();
			return new Podcast(title, host, guest, episode, year, lang);
		}
		private ImageFile CreateImageFile()
		{
			Console.Write("Title: "); string title = Console.ReadLine();
			Console.Write("Resolution: "); string resolution = Console.ReadLine();
			Console.Write("FileFormat: "); string format = Console.ReadLine();
			Console.Write("FileSize (KB): "); double fileSize = double.Parse(Console.ReadLine());
			Console.Write("DateTaken: "); string dateTaken = Console.ReadLine();
			Console.Write("Language: "); string lang = Console.ReadLine();
			return new ImageFile(title, resolution, format, fileSize, dateTaken, lang);
		}
	}
}
