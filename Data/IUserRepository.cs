﻿using ConnectingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Data
{
    public interface IUserRepository
    {
        // T is for generic to add any entity we only need to specify its type in this method
        // we can use this method to add for both users and photos
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        // note both add and delete methods are not asynchronous
        // when we add or delete it will be in memory
        // when we call 'SaveAll' method it will be asynchronously changes



        // to save changes this method will only return boolean and also task type cz we want asynchronously save changes
        Task<bool> SaveAll();

        // to get individual users this also be asynchronous method and returning newly created user
        Task<User> GetUser(int id);

        // to get all users
        Task<IEnumerable<User>> GetUsers();
    }
}
