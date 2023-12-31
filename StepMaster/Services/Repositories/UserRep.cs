﻿using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using StepMaster.Models.Entity;
using StepMaster.Services.Interfaces;

using StepMaster.Models.HashSup;
using MongoDB.Bson;
using Application.Services.ForDb.APIDatebaseSet;

namespace StepMaster.Services.Repositories
{
    public class UserRep : IUser_Service
    {
        IMemoryCache _cache;
        private readonly IMongoCollection<User> _users;        
        
        public UserRep(IAPIDatabaseSettings settings, IMongoClient mongoClient ,IMemoryCache cache)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>("User");
            _cache = cache;

        }
        public async  Task<List<User>> GetAllUser()
        {
            var list = new List<User>();
            list = await _users.FindAsync(_ => true).Result.ToListAsync();
            return list;
        }     


        public async Task<User> GetByLoginAsync(string email)
        {
            try
            {
                _cache.TryGetValue(email, out User? user);
                if (user == null)
                {
                    user = await _users.FindAsync(user => user.email == email).Result.FirstAsync();
                    _cache.Set(email, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                    return user;
                }
                else
                {
                    return user;
                }
                
            }
            catch
            {
                return null;
            }
        }

        public async Task<User> RegUserAsync(User newUser)
        {
            try
            {
                
                
                newUser.password = HashCoder.GetHash(newUser.password);
                newUser.role = "user";
                

                await _users.InsertOneAsync(newUser);
                return newUser;
            }
            catch
            {
                return null;
            }
        }
        public async Task<User> RecoveryPasswordAsync(User userWithNewPassword)
        {
            try
            {
                userWithNewPassword.password = HashCoder.GetHash(userWithNewPassword.password);
                var filter = Builders<User>.Filter.Eq("email",userWithNewPassword.email);
                var update = Builders<User>.Update.Set("password", userWithNewPassword.password);    
                
                await _users.UpdateOneAsync(filter,update);
                return userWithNewPassword;
            }
            catch
            {
                return null;
            }
        }
    }
}
