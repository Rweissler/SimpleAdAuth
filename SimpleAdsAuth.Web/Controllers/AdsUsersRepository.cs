using SimpleAdAuth.Data;
using System;
using System.Collections.Generic;

namespace SimpleAdsAuth.Web.Controllers
{
    internal class AdsUsersRepository
    {
        private string connectionString;

        public AdsUsersRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        internal List<Ad> GetAdsByUser(string name)
        {
            throw new NotImplementedException();
        }

        internal void DeletAds(int id)
        {
            throw new NotImplementedException();
        }
    }
}