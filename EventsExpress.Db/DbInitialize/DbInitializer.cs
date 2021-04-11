using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventsExpress.Db.DataPath;
using EventsExpress.Db.EF;
using EventsExpress.Db.Entities;
using EventsExpress.Db.Enums;
using EventsExpress.Db.Helpers;

namespace EventsExpress.Db.DbInitialize
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext dbContext)
        {
           // dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            // Look for any users
            if (dbContext.Users.Any())
            {
                return; // DB has been seeded
            }

            Role adminRole = new Role { Name = "Admin" };
            Role userRole = new Role { Name = "User" };
            dbContext.Roles.AddRange(new Role[] { adminRole, userRole });
            Guid idUser = Guid.NewGuid();
            Guid idAdmin = Guid.NewGuid();

            var saltDef = PasswordHasher.GenerateSalt();
            var users = new User[]
            {
                 new User
                 {
                     Id = idAdmin,
                     Name = "Admin",
                     PasswordHash = PasswordHasher.GenerateHash("1qaz1qaz", saltDef),
                     Salt = saltDef,
                     Email = "admin@gmail.com",
                     EmailConfirmed = true,
                     Phone = "+380974293583",
                     Birthday = DateTime.Parse("2000-01-01"),
                     Gender = Gender.Male,
                     IsBlocked = false,
                     Role = adminRole,
                 },

                 new User
                  {
                      Id = idUser,
                      Name = "User",
                      PasswordHash = PasswordHasher.GenerateHash("1qaz1qaz", saltDef),
                      Salt = saltDef,
                      Email = "user@gmail.com",
                      EmailConfirmed = true,
                      Phone = "+380970101013",
                      Birthday = DateTime.Parse("2000-01-01"),
                      Gender = Gender.Male,
                      IsBlocked = false,
                      Role = userRole,
                  },
            };

            dbContext.Users.AddRange(users);

            var categories = CategoryData.GetCategoriesFromFile();
            dbContext.AddRange(categories);

            var photoData = new PhotoData();
            var photos = photoData.GetPhotossFromFile();
            var photo = photos.FirstOrDefault();
            dbContext.Photos.Add(photo);

            var locations = LocationData.GetEventsLocationFromFile();
            dbContext.AddRange(locations);
            var location = locations.FirstOrDefault();

            // var categories = new Category[]
            // {
            //    new Category { Name = "Sea" },
            //    new Category { Name = "Mount" },
            //    new Category { Name = "Summer" },
            //    new Category { Name = "Golf" },
            //    new Category { Name = "Team-Building" },
            //    new Category { Name = "Swimming" },
            //    new Category { Name = "Gaming" },
            //    new Category { Name = "Fishing" },
            //    new Category { Name = "Trips" },
            //    new Category { Name = "Meeting" },
            //    new Category { Name = "Sport" },
            //    new Category { Name = "ABA" },
            // };
            // dbContext.Categories.AddRange(categories);
            Guid idEventOneDay = Guid.NewGuid();
            var eventDay = new Event() { Id = idEventOneDay, IsBlocked = false, DateFrom = DateTime.Now, DateTo = DateTime.Now, PhotoId = photo.Id, MaxParticipants = 8, IsPublic = true, EventLocationId = location.Id, Description = "Day event", Title = "This is Admin's Day Event" };
            dbContext.Add(eventDay);

            var eventOwnerDay = new EventOwner { EventId = idEventOneDay, UserId = idAdmin };
            dbContext.Add(eventOwnerDay);

            Guid idEventWeek = Guid.NewGuid();

            // var eventWeek = new Event() { Id = idEventWeek, IsBlocked = false, DateFrom = DateTime.Now.AddDays(1), DateTo = DateTime.Now.AddDays(8), PhotoId = photo.Id, MaxParticipants = 8, IsPublic = true, EventLocationId = location.Id, Description = "Week event", Title = "This is User's Week Event", Categories = new List<EventCategory> { new EventCategory { Category = new Category { Name = "Sea" } } } };
            var eventWeek = new Event() { Id = idEventWeek, IsBlocked = false, DateFrom = DateTime.Now.AddDays(1), DateTo = DateTime.Now.AddDays(8), PhotoId = photo.Id, MaxParticipants = 8, IsPublic = true, EventLocationId = location.Id, Description = "Week event", Title = "This is User's Week Event" };
            dbContext.Add(eventWeek);

            var eventOwnerWeek = new EventOwner { EventId = idEventWeek, UserId = idUser };
            dbContext.Add(eventOwnerWeek);

            Guid idEventMonth = Guid.NewGuid();

            // var eventMonth = new Event() { Id = idEventMonth, IsBlocked = false, DateFrom = NextMonth(DateTime.Now), DateTo = NextMonth(DateTime.Now.AddDays(7)), PhotoId = photo.Id, MaxParticipants = 8, IsPublic = true, EventLocationId = location.Id, Description = "Month event", Title = "This is Admin's Month Event", Categories = new List<EventCategory> { new EventCategory { Category = new Category { Name = "Sport" } } } };
            var eventMonth = new Event() { Id = idEventMonth, IsBlocked = false, DateFrom = NextMonth(DateTime.Now), DateTo = NextMonth(DateTime.Now.AddDays(7)), PhotoId = photo.Id, MaxParticipants = 8, IsPublic = true, EventLocationId = location.Id, Description = "Month event", Title = "This is Admin's Month Event" };
            dbContext.Add(eventMonth);

            var eventOwnerMonth = new EventOwner { EventId = idEventMonth, UserId = idAdmin };
            dbContext.Add(eventOwnerMonth);
            dbContext.SaveChanges();
            var result = dbContext.Events.SingleOrDefault(b => b.Description == "Day event");
            var golfCategory = dbContext.Categories.SingleOrDefault(b => b.Name == "Golf");
            if (result != null)
            {
                result.Categories = new List<EventCategory> { new EventCategory { Category = golfCategory } };
                dbContext.SaveChanges();
            }

            result = dbContext.Events.SingleOrDefault(b => b.Description == "Week event");
            var seaCategory = dbContext.Categories.SingleOrDefault(b => b.Name == "Sea");
            if (result != null)
            {
                result.Categories = new List<EventCategory> { new EventCategory { Category = seaCategory } };
                dbContext.SaveChanges();
            }

            result = dbContext.Events.SingleOrDefault(b => b.Description == "Month event");
            var monthCategory = dbContext.Categories.SingleOrDefault(b => b.Name == "Sport");
            if (result != null)
            {
                result.Categories = new List<EventCategory> { new EventCategory { Category = monthCategory } };
                dbContext.SaveChanges();
            }
        }

        public static DateTime NextMonth(DateTime date)
        {
            DateTime nextMonth = date.AddMonths(1);

            if (date.Day != DateTime.DaysInMonth(date.Year, date.Month))
            {
                return nextMonth;
            }
            else
            {
                return date.AddDays(DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month));
            }
        }
    }
}
