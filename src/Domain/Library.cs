using System;
using System.Collections.Generic;
using System.Linq;
using OopWorkshop.Domain.Media;

namespace OopWorkshop.Domain
{
    public class LibraryCatalog
    {
        public List<MediaItem> MediaItems { get; set; } = new List<MediaItem>();

        public void AddMedia(MediaItem item)
        {
            MediaItems.Add(item);
        }

        public void RemoveMedia(MediaItem item)
        {
            MediaItems.Remove(item);
        }

        public IEnumerable<MediaItem> ListItemsByType(Type type)
        {
            return MediaItems.Where(m => m.GetType() == type);
        }

        public MediaItem SearchById(Guid id)
        {
            return MediaItems.FirstOrDefault(m => m.Id == id);
        }

        public MediaItem SearchByTitle(string title)
        {
            return MediaItems.FirstOrDefault(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }
    }
}
