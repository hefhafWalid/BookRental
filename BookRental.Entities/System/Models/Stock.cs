using System;
using System.Collections.Generic;

namespace BookRental.Entities
{
    public class Stock : IEntityBase
    {
        public int ID { get; set; }

        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        public Guid UniqueKey { get; set; }

        public bool IsAvailable { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }

        public Stock()
        {
            Rentals = new List<Rental>();
        }
    }
}
