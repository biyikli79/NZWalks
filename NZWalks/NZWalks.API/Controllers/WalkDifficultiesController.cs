using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace NZWalks.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository _iwalkDifficultyRepository;
        private readonly IMapper _imapper;

        public WalkDifficultiesController(IWalkDifficultyRepository iwalkDifficultyRepository, IMapper imapper)
        {
            this._iwalkDifficultyRepository = iwalkDifficultyRepository;
            this._imapper = imapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var WalkDifficultiesDomain = await _iwalkDifficultyRepository.GetAllAsync();

            if (WalkDifficultiesDomain == null)
            {
                return NotFound();

            };

            
            var WalkDifficultiesDTO = _imapper.Map<List<Models.DTO.WalkDifficulty>>(WalkDifficultiesDomain);
            return Ok(WalkDifficultiesDTO);
            

            /*
            var WalkDifficultiuesDTOList = new List<Models.DTO.WalkDifficulty>();

            WalkDifficultiesDomain.ToList().ForEach(x =>
            {
                var WalkDifficultiuesDTO = new Models.DTO.WalkDifficulty()
                {
                    Id = x.Id,
                    Code = x.Code

                };

            WalkDifficultiuesDTOList.Add(WalkDifficultiuesDTO);

            });    
            

            return Ok(WalkDifficultiuesDTOList);

            */
            
        }


        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetWalkDifficultyById")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var WalkDifficultyDomain = await _iwalkDifficultyRepository.GetAsync(id);

            if(WalkDifficultyDomain == null)
            {
                return NotFound();

            }

            /*
            var WalkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                Id = WalkDifficultyDomain.Id,
                Code = WalkDifficultyDomain.Code

            };
            */

            var WalkDifficultyDTO = _imapper.Map<Models.DTO.WalkDifficulty>(WalkDifficultyDomain);

            return Ok(WalkDifficultyDTO);

        }


        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {

            if (! ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
            {
                return BadRequest(ModelState);     

            }

            var WalkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code,
            };

            var walk = await _iwalkDifficultyRepository.AddAsync(WalkDifficultyDomain);


            /*
            var WalkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walk.Id,
                Code = walk.Code,
            };
            */

            var WalkDifficultyDTO = _imapper.Map<Models.DTO.WalkDifficulty>(walk);

            return CreatedAtAction(nameof(GetWalkDifficultyById), new {Id = WalkDifficultyDTO.Id}, WalkDifficultyDTO);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWalkdifficultiesAsync(Guid id, Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (!ValidateUpdateWalkdifficultiesAsync(updateWalkDifficultyRequest))
            {
                return BadRequest(ModelState);

            }

            var domainWalkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code,

            };

            var existingWalkDifficulty = await _iwalkDifficultyRepository.UpdateAsync(id,domainWalkDifficulty);

            if(existingWalkDifficulty == null)
            {
                return NotFound();

            }

            /*
            var WalkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code,

            };
            */

            var WalkDifficultyDTO = _imapper.Map<Models.DTO.WalkDifficulty>(existingWalkDifficulty);

            return Ok(WalkDifficultyDTO);

        }

        [HttpDelete]
        [Route("id:Guid")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var DomainWalkDifficulty = await _iwalkDifficultyRepository.DeleteAsync(id);

            if(DomainWalkDifficulty == null)
            {
                return NotFound();

            }

            var WalkDifficultyDTO = _imapper.Map<Models.DTO.WalkDifficulty>(DomainWalkDifficulty);

            //return CreatedAtAction(nameof(GetWalkDifficultyById), new { id = DomainWalkDifficulty.Id }, WalkDifficultyDTO);

            return Ok(WalkDifficultyDTO);

        }

        #region Private methods

        private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), $"{nameof(addWalkDifficultyRequest)} is required");
                return false;

            }

            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), $"{nameof(addWalkDifficultyRequest.Code)} is not valid");

            }

            if(ModelState.ErrorCount > 0)
            {
                return false;

            }

            return true;

        }

        private bool ValidateUpdateWalkdifficultiesAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $"{nameof(updateWalkDifficultyRequest)} is required");
                return false;

            }

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} is not valid");

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
