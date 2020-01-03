using System.Collections.Generic;
using System.Linq;
using API.Entities;
using API.Repositories.Interfaces;
using API.Services.Database;

namespace API.Repositories.Implementations
{
    public class OfferRepository : IOfferRepository
    {
        private readonly TruequeLibreDbContext _dbContext;

        public OfferRepository(TruequeLibreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddOffer(Offer offer)
        {
            _dbContext.Offers.Add(offer);
            _dbContext.SaveChanges();
        }

        public Offer FindOfferByIdCombination(int product, int offer)
        {
            return _dbContext.Offers.FirstOrDefault(x => x.OfferedProductId == offer && x.ReceiverProductId == product);
        }
    }
}