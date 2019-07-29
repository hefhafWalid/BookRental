﻿using System;
using System.Collections.Generic;

namespace BookRental.DataModels
{
    public class Book : IEntityBase
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        public string Director { get; set; }

        public string Writer { get; set; }

        public string Producer { get; set; }

        public DateTime ReleaseDate { get; set; }

        public byte Rating { get; set; }

        public string TrailerURI { get; set; }
        
        public virtual ICollection<Stock> Stocks { get; set; }

        public Book()
        {
            Stocks = new List<Stock>();
        }
    }
}
