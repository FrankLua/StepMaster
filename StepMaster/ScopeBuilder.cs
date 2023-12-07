﻿
using API.Services.ForS3.Configure;
using API.Services.ForS3.Int;
using API.Services.ForS3.Rep;
using Microsoft.AspNetCore.RateLimiting;
using StepMaster.Services.ForDb.Interfaces;
using StepMaster.Services.ForDb.Repositories;
using System.Threading.RateLimiting;

namespace API
{
    public static class ScopeBuilder 
    {
        public static void InitializerServices(this IServiceCollection service)
        {
            service.AddScoped<IUser_Service, UserRep>();
            service.AddScoped<IAppConfiguration, AppConfiguration>();
            //service.AddScoped<IAws3Services, Aws3Services>();
            service.AddScoped<IDays_Service, DaysRep>();
            service.AddScoped<IRegion_Service, RegionRep>();
            service.AddScoped<IPost_Service, PostRep>();
        }
        public static void InitializerRateLimiter(this IServiceCollection service)
        {
            service.AddRateLimiter(opt => opt.AddConcurrencyLimiter("ForFile", parametrs =>
            {
                parametrs.PermitLimit = 5;
                parametrs.QueueLimit = 5;
                parametrs.QueueProcessingOrder = QueueProcessingOrder.NewestFirst;
            }).RejectionStatusCode = 423);
            service.AddRateLimiter(opt => opt.AddConcurrencyLimiter("ForOther", parametrs =>
            {
                parametrs.PermitLimit = 10;
                parametrs.QueueLimit = 25;
                parametrs.QueueProcessingOrder = QueueProcessingOrder.NewestFirst;
            }).RejectionStatusCode = 423);

        }
    }
    
}
