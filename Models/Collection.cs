using System.Collections.Generic;

namespace CollectionSystem.Models
{
    public class Collection
    {
        public string Name { get; set; }
        public List<CollectionItem> Items { get; set; } = new List<CollectionItem>();
    }
}
