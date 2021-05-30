using System;
using System.Linq;
using src.Resources;

namespace src.Subsystems.User.Data
{
    public class UserQueries//THIS IS AN EXAMPLE CLASS TO GENERALISE CERTAIN QUERIES
    {
        private readonly HighFiveContext context;

        public void InsertUser(string firstName, string lastName, string username, string password, string email)
        {
            User newUser = new User()
            {
                FirstName = (firstName),
                LastName = lastName,
                Username = username,
                Password = password,
                Email = email
            };
            context.Add(newUser);
            context.SaveChanges();
        }

        public User[] SearchUser(Guid userId)
        {
            var users = context.Users.Where(user => user.UserId.Equals(userId));
            int count = 0;
            foreach (User u in users)
            {
                Console.WriteLine("Id: " + u.UserId);
                count++;
            }

            User[] userList = new User[count];
            count = 0;
            foreach (User u in users)
            {
                userList[count++] = u;
            }

            return userList;

        }

        public void DeleteUser(Guid userId)
        {
            User[] users = SearchUser(userId);
            if (users == null) 
                return;
            foreach (User u in users)
            {
                context.Remove(u);
            }
        }

        public void UpdateUser(Guid userId, string firstName)
        {
            User[] users = SearchUser(userId);
            if (users == null)
                return;
            foreach (User u in users)
            {
                u.FirstName = firstName;
                context.Update(u);
            }
        }
    }
}