using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this._nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await _nZWalksDbContext.Walks.AddAsync(walk);
            await _nZWalksDbContext.SaveChangesAsync();

            return walk;

        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await _nZWalksDbContext.Walks.FindAsync(id);

            if (existingWalk != null)
            {
                _nZWalksDbContext.Walks.Remove(existingWalk);
                await _nZWalksDbContext.SaveChangesAsync();

                return existingWalk;

            }

            return null;

        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await _nZWalksDbContext.Walks.Include(X => X.Region).Include(Z => Z.WalkDifficulty).ToListAsync();

        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await _nZWalksDbContext.Walks.Include(x => x.Region).Include(x => x.WalkDifficulty).FirstOrDefaultAsync(x =>x.Id == id);


        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await _nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if(existingWalk != null)
            {
                existingWalk.Length = walk.Length;
                existingWalk.Name = walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;

                await _nZWalksDbContext.SaveChangesAsync();

                return existingWalk;

            }

            return null;


        }
    }
}
