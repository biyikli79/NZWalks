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
        private readonly IRegionRepository _iregionRepository;
        private readonly IWalkDifficultyRepository _iwalkDifficultyRepository;

        public WalksController(IWalkRepository iwalkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this._iwalkRepository = iwalkRepository;
            this._imapper = mapper;
            this._iregionRepository = regionRepository;
            this._iwalkDifficultyRepository = walkDifficultyRepository;
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
            if (! (await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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
            if(! (await ValidateUpdateWalkAsync(updateWalkRequest))) 
            {
                return BadRequest(ModelState);
            }


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

        #region Private methods
        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if(addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"{nameof(addWalkRequest)} can not be empty.");
                return false;
            }

            if(string.IsNullOrEmpty(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} is required.");

            }

            if (addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} should be greater than zero.");
                
            }

            var region = await _iregionRepository.GetAsync(addWalkRequest.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is invalid");
                
            }

            var walkDifficulty = await _iwalkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid");
                
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;

            }

            return true;

        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if(updateWalkRequest == null)
            {
                BadRequest(ModelState);
                return false;
            }

            if(string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} can nor be empty");

            }

            if (string.IsNullOrEmpty(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} is required.");
            }

            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} should be greater than zero.");
            }

            var region = await _iregionRepository.GetAsync(updateWalkRequest.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is invalid");
            }

            var walkDifficulty = await _iwalkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;


        }

        #endregion


    }
}
