using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

using NZWalks.API.Repositories;
using System.Linq.Expressions;

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
        public async Task<IActionResult> GetAllRegionsAsync()
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


        [HttpGet]
        [Route("{Id:Guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid Id)
        {
            var region = await _iregionRepository.GetAsync(Id);

            if (region == null)
            {
                return NotFound();

            }

            var regionDTO = _mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);

        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {

            if (!ValidateAddRegionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            }

            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,


            };

            region = await _iregionRepository.AddAsync(region);

            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);

        }


        [HttpDelete]
        [Route("{id::Guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            
            var region = await _iregionRepository.DeleteAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,

            };
            
            return Ok(regionDTO);

        }


        [HttpPut]
        [Route("{id::Guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id, [FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (!ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);

            }

            var regionRequest = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population,
            };

            var updatedRegion = await _iregionRepository.UpdateAsync(id, regionRequest);

            if (updatedRegion == null)
            {
                return NotFound();
            }

            var regionDTO = new Models.DTO.Region()
            {
                Code = updatedRegion.Code,
                Name = updatedRegion.Name,
                Area = updatedRegion.Area,
                Lat = updatedRegion.Lat,
                Long = updatedRegion.Long,
                Population= updatedRegion.Population,

            };

            return Ok(regionDTO);

        }

        #region Private methods

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if(addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), $"Add Region Data is required.");
                return false;

            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), $"{nameof(addRegionRequest.Code)} can not be null or white space.");    

            };

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} can not be null or white space.");

            };

            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} can not be less than or equal to zero.");

            };

            if (addRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat), $"{nameof(addRegionRequest.Lat)} can not be less than or equal to zero.");

            };

            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long), $"{nameof(addRegionRequest.Long)} can not be less than or equal to zero.");

            };

            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} can not be less than zero.");

            };

            if(ModelState.ErrorCount > 0)
            {
                return false;

            };

            return true;

        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if(updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest), $"update region is required.");
                return false;

            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{nameof(updateRegionRequest.Code)} can not be null or white space.");
            };

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{nameof(updateRegionRequest.Name)} can not be null or white space.");
            };

            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{nameof(updateRegionRequest.Area)} can not be less than or equal to zero.");
            };

            

            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{nameof(updateRegionRequest.Population)} can not be less than zero.");
            };
            if (ModelState.ErrorCount > 0)
            {
                return false;
            };
            return true;


        }


        #endregion

    }
}
