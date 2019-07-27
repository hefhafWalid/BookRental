using System.Collections.Generic;

namespace BookRental.Entities
{
    public class Genre : IEntityBase
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public Genre()
        {
            Books = new List<Book>();
        }
    }
}
