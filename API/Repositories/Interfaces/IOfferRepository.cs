using System.Collections.Generic;
using API.Entities;

namespace API.Repositories.Interfaces
{
    public interface IOfferRepository
    {
        void AddOffer(Offer offer);
        Offer FindOfferByIdCombination(int product, int offer);
    }
}