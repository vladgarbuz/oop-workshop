using System;
using System.Collections.Generic;
using OopWorkshop.Domain.Media;

namespace OopWorkshop.Domain.User
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string SSN { get; set; }

        public User(string name, int age, string ssn)
        {
            Id = Guid.NewGuid();
            Name = name;
            Age = age;
            SSN = ssn;
        }
    }

    public class Borrower : User
    {
        public List<MediaItem> BorrowedItems { get; set; } = new List<MediaItem>();
        public Dictionary<Guid, int> Ratings { get; set; } = new Dictionary<Guid, int>();

        public Borrower(string name, int age, string ssn) : base(name, age, ssn) { }

        public void ListMediaByType(IEnumerable<MediaItem> items, Type type)
        {
            foreach (var item in items)
                if (item.GetType() == type)
                    item.ShowDetails();
        }

        public void Borrow(MediaItem item)
        {
            BorrowedItems.Add(item);
            Console.WriteLine($"Borrowed {item.Title}");
        }

        public void Rate(MediaItem item, int rating)
        {
            if (BorrowedItems.Contains(item))
            {
                Ratings[item.Id] = rating;
                Console.WriteLine($"Rated {item.Title} with {rating}");
            }
            else
                Console.WriteLine("You can only rate borrowed items.");
        }
    }

    public class Employee : User
    {
        public Employee(string name, int age, string ssn) : base(name, age, ssn) { }

        public void AddMedia(List<MediaItem> catalog, MediaItem item)
        {
            catalog.Add(item);
            Console.WriteLine($"Added {item.Title}");
        }

        public void RemoveMedia(List<MediaItem> catalog, MediaItem item)
        {
            catalog.Remove(item);
            Console.WriteLine($"Removed {item.Title}");
        }

        public void ViewMediaDetails(MediaItem item)
        {
            item.ShowDetails();
        }
    }

    public class Admin : Employee
    {
        public List<User> Users { get; set; } = new List<User>();

        public Admin(string name, int age, string ssn) : base(name, age, ssn) { }

        public void ViewAllUsers()
        {
            foreach (var user in Users)
                Console.WriteLine($"User: {user.Name}, Age: {user.Age}, SSN: {user.SSN}");
        }

        public void CreateUser(string name, int age, string ssn, bool isEmployee)
        {
            User user = isEmployee ? new Employee(name, age, ssn) : new Borrower(name, age, ssn);
            Users.Add(user);
            Console.WriteLine($"Created {(isEmployee ? "Employee" : "Borrower")}: {name}");
        }

        public void UpdateUser(User user, string name, int age, string ssn)
        {
            user.Name = name;
            user.Age = age;
            user.SSN = ssn;
            Console.WriteLine($"Updated user {name}");
        }

        public void DeleteUser(User user)
        {
            Users.Remove(user);
            Console.WriteLine($"Deleted user {user.Name}");
        }
    }
}
