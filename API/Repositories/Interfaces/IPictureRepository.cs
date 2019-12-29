using System.Collections.Generic;
using API.Entities;

namespace API.Repositories.Interfaces
{
    public interface IPictureRepository
    {
        void DeletePictures(List<PictureInfo> pictures);
        void SavePictures(List<PictureInfo> pictures);
    }
}