using Microsoft.AspNetCore.Mvc;
using StepMaster.Services.ForDb.Interfaces;
using Domain.Entity.API;
using Domain.Entity.Main;
using StepMaster.Auth.ResponseLogic;

namespace StepMaster.Controllers.api;
[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly IRegion_Service _regions;
    public RegionsController(IRegion_Service regions)
    {
        _regions = regions;
    }
    
    [HttpGet]
    [Route("GetRegions")]
    public async Task<ResponseList<Region>> GetRegions()
    {
        
        var bodyResponse = await _regions.GetAllRegionsAsync();
        return ResponseLogic<ResponseList<Region>>.Response(Response, bodyResponse.Status, new ResponseList<Region>(bodyResponse.Data));
    }
}