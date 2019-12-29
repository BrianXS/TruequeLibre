using System.Collections.Generic;
using System.IO;
using API.Entities;
using API.Repositories.Implementations;
using API.Services.Database;

namespace API.Repositories.Interfaces
{
    public class PictureRepository : IPictureRepository
    {
        private readonly TruequeLibreDbContext _dbContext;

        public PictureRepository(TruequeLibreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void DeletePictures(List<PictureInfo> pictures)
        {
            pictures.ForEach(x => File.Delete($"{Constants.General.InternalImagesFolder}/{x.FileName}"));
            _dbContext.Pictures.RemoveRange(pictures);
            _dbContext.SaveChanges();
        }

        public void SavePictures(List<PictureInfo> pictures)
        {
            _dbContext.Pictures.AddRange(pictures);
            _dbContext.SaveChanges();
        }
    }
}