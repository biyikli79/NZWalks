using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;


namespace NZWalks.API.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository _iregionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper) 
        {
            this._iregionRepository = regionRepository;
            this._mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await _iregionRepository.GetAllAsync();

            //return DTO regions

            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,

            //    };

            //    regionsDTO.Add(regionDTO);

            //});

            var regionsDTO = _mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }
    }
}
