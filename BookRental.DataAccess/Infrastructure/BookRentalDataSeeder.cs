using BookRental.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace BookRental.DataAccess.Infrastructure
{
    public class BookRentalDataSeeder
    {
        BookRentalContext context;
        public BookRentalDataSeeder(BookRentalContext brContext)
        {
            context = brContext;
        }

        public void Seed()
        {
            //  create genres
            context.GenreSet.AddOrUpdate(g => g.Name, GenerateGenres());

            // create Books
            context.BookSet.AddOrUpdate(m => m.Title, GenerateBooks());

            //// create stocks
            context.StockSet.AddOrUpdate(GenerateStocks());

            // create customers
            context.CustomerSet.AddOrUpdate(GenerateCustomers());

            // create roles
            context.RoleSet.AddOrUpdate(r => r.Name, GenerateRoles());

           

            // // create user-admin
            context.UserRoleSet.AddOrUpdate(new UserRole[] {
                new UserRole() {
                    RoleId = 1, // admin
                    UserId = 1  // hWalid
                }
            });
        }

        private Genre[] GenerateGenres()
        {
            Genre[] genres = new Genre[] {
                //Instances ..
            };

            return genres;
        }
        private Book[] GenerateBooks()
        {
            Book[] Books = new Book[] {
                
            };

            return Books;
        }
        private Stock[] GenerateStocks()
        {
            List<Stock> stocks = new List<Stock>();

            for (int i = 1; i <= 15; i++)
            {
                // Three stocks for each Book
                for (int j = 0; j < 3; j++)
                {
                    Stock stock = new Stock()
                    {
                        BookId = i,
                        UniqueKey = Guid.NewGuid(),
                        IsAvailable = true
                    };
                    stocks.Add(stock);
                }
            }

            return stocks.ToArray();
        }
        private Customer[] GenerateCustomers()
        {
            List<Customer> _customers = new List<Customer>();

            // Create 20 customers
            for (int i = 0; i < 20; i++)
            {
                Customer customer = new Customer()
                {
                   
                };

                _customers.Add(customer);
            }

            return _customers.ToArray();
        }
        private Role[] GenerateRoles()
        {
            Role[] _roles = new Role[]{
                new Role()
                {
                    Name="Admin"
                }
            };

            return _roles;
        }
    }
}
