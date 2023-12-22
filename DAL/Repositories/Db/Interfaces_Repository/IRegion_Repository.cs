﻿using Amazon.S3.Model;
using Domain.Entity.API;
using Domain.Entity.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Db.Interfaces_Repository
{
    public interface IRegion_Repository
    {

        public Task<BaseResponse<List<Region>>> GetRegions();


    }
}
