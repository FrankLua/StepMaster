﻿using Microsoft.AspNetCore.Mvc;
using StepMaster.Models.Entity;
using System.IO;
using StepMaster.Services.ForDb.Interfaces;
using API.Auth.AuthCookie;
using Domain.Entity.API;
using Domain.Entity.Main;
using StepMaster.Auth.ResponseLogic;
using StepMaster.Models.API.Day;
using StepMaster.Auth.AuthRequest;


namespace StepMaster.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DaysController : ControllerBase
    {
        private readonly IDays_Service _days;
        public DaysController(IDays_Service days)
        {
            _days = days;
        }
        [HttpGet]
        [CustomAuthorizeUser("user")]
        [Route("GetAllDayUser")]
        public async Task<ResponseList<Day>> GetAllDayUser()
        {
            var email = User.Identity.Name;            
            var responseBody = await _days.GetDaysUserByEmail(email);
            return ResponseLogic<ResponseList<Day>>.Response(Response, responseBody.Status, new ResponseList<Day>(responseBody.Data));
           

        }
        [HttpPost]
        [CustomAuthorizeUser("user")]        
        [Route("SetNewDay")]
        public async Task<Day> SetNewDay([FromForm] DayCreate daySet)
        {
            var email = User.Identity.Name;            
            var response = await _days.SetDayAsync(daySet.ConvertToBase(), email);

            return ResponseLogic<Day>.Response(Response, response.Status, response.Data);

        }
        [HttpPut]
        [CustomAuthorizeUser("user")]
        [ModelValidateFilterAttribute]
        [Route("UploadDay")]
        public async Task<Day> UploadDay([FromForm] DayResponse dayUpd)
        {
                var response = await _days.UploadDayAsync(DayResponse.ConvertToBase(dayUpd));
                return ResponseLogic<Day>.Response(Response, response.Status, response.Data);
        }
       
    }
}
