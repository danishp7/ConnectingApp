﻿using ConnectingApp.API.Helpers;
using ConnectingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Data
{
    public class UserRepository : IUserRepository
    {
        // to implement methods we need the context so we inject it
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            // if user hasnt already like other user then it'll return null 
            // other wise like object which contain likeeid and likerid
            return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
        }

        public async Task<User> GetUser(int id)
        {
            // eager loading i.e include to get all the photos associated with user
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            
            // we don't need to check if the user is null or not cz 'firstordefault' method returns null
            // if no user exists
            return user;

        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            // we'll not convert the users into list now
            var users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();


            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.Gender == userParams.Gender);

            // to get the list of likers
            // all the users who have liked the currently logged in user
            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }

            // to get the list of likees
            // all the users which currently logged in user liked
            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }


            // we filter the age here
            // if user doesnt specify about age in query string
            // then min and max will be set to default value i.e 7 and 99
            // otherwise we'll calculate min and max age
            if (userParams.MinAge != 7 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                // now we put where clause to filter
                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            // to sort list according to create at
            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            // we've created 'createasync' extension method will be take care of skip and take and then return tolist
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        // to get the list of int based on likee and likers 
        // id = currently logged in user id
        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            // we'll return either list of liked users or
            // the list of user who liked currently logged in user

            // get the user
            var user = await _context.Users.
                Include(x => x.Likers).
                Include(x => x.Likees).
                FirstOrDefaultAsync(u => u.Id == id);

            // now check if likers is true
            // it means we'll return the list of users who like currently logged in user
            if (likers)
                return user.Likers.
                    Where(u => u.LikeeId == id /*it means like hone wala loggedin user hai*/).
                    Select(i => i.LikerId /*jinhon ne like kiya un ki id chahiye int me*/);

            return user.Likees.
                Where(u => u.LikerId == id /*like krne wala loggedin user hai*/).
                Select(i => i.LikeeId /*jo users liked hue hn un ki ids int me*/);    
        }

        public async Task<bool> SaveAll()
        {
            // if it equals to 0 it means nothing has been saved
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
