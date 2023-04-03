using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _iwalkRepository;
        private readonly IMapper _imapper;

        public WalksController(IWalkRepository iwalkRepository, IMapper mapper)
        {
            this._iwalkRepository = iwalkRepository;
            this._imapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walksDomain = await _iwalkRepository.GetAllAsync();

            var walksDTO = _imapper.Map<List<Models.DTO.Walk>>(walksDomain);

            return Ok(walksDTO);

        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walkDomain = await _iwalkRepository.GetAsync(id);

            var walkDTO = _imapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);

        }


        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            var walkDomain = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,

                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
                RegionId = addWalkRequest.RegionId

            };

            var walkDomain2 = _iwalkRepository.AddAsync(walkDomain);

            var walkDTO = new Models.DTO.Walk()
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,

                WalkDifficultyId = walkDomain.WalkDifficultyId,
                RegionId = walkDomain.RegionId

            };

            return CreatedAtAction(nameof(GetWalkAsync), new {id = walkDTO.Id}, walkDTO);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            var WalkDomain = new Models.Domain.Walk()
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
                RegionId = updateWalkRequest.RegionId

            };

            var result = await _iwalkRepository.UpdateAsync(id, WalkDomain);

            if(result != null)
            {
                var updateDTO = new Models.DTO.Walk()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Length = result.Length,
                    WalkDifficultyId = result.WalkDifficultyId,
                    RegionId = result.RegionId

                };

                return Ok(updateDTO);

            }

            return NotFound();

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var deletedWalk = await _iwalkRepository.DeleteAsync(id);

            if(deletedWalk == null)
            {
                NotFound();

            }

            var deletedWalkDTO = _imapper.Map<Models.DTO.Walk>(deletedWalk);

            return Ok(deletedWalkDTO);

        }


    }
}
