using ConnectingApp.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Data.SeedData
{
    public static class Seed
    {
        public static void SeedUser(DataContext ctx)
        {
            // if no user exist
            if (!ctx.Users.Any())
            {
                var userData = File.ReadAllText("Data/SeedData/UserSeedData.json");

                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    byte[] hashPassword, saltPassword;
                    CreateHashPassword("password", out hashPassword, out saltPassword);

                    user.PasswordHash = hashPassword;
                    user.PasswordSalt = saltPassword;
                    user.UserName = user.UserName.ToLower();
                    ctx.Users.Add(user);
                }
                ctx.SaveChanges();
            }
        }

        private static void CreateHashPassword(string password, out byte[] hashPassword, out byte[] key)
        {
            using (var hashedPassword = new System.Security.Cryptography.HMACSHA512())
            {
                key = hashedPassword.Key;
                hashPassword = hashedPassword.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
