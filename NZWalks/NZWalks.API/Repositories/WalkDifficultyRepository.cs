using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this._nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();

            await _nZWalksDbContext.WalkDifficulties.AddAsync(walkDifficulty);
            await _nZWalksDbContext.SaveChangesAsync();

            return walkDifficulty;

        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDifficulty = await _nZWalksDbContext.WalkDifficulties.FindAsync(id);

            if(existingWalkDifficulty != null)
            {
                _nZWalksDbContext.WalkDifficulties.Remove(existingWalkDifficulty);

                await _nZWalksDbContext.SaveChangesAsync();

                return existingWalkDifficulty;
             }

            return null;

        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await _nZWalksDbContext.WalkDifficulties.ToListAsync();
            
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await _nZWalksDbContext.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await _nZWalksDbContext.WalkDifficulties.FindAsync(id);

            if(existingWalkDifficulty == null)
            {
                return null;

            }

            existingWalkDifficulty.Code = walkDifficulty.Code;

            await _nZWalksDbContext.SaveChangesAsync();

            return existingWalkDifficulty;


        }
    }
}
