using SimpleAdAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAdsAuth.Web.Models
{
    public class AdViewModel
    {
        public Ad ad { get; set; }
        public Ad Ad { get; internal set; }
        public bool CanDelete { get; set; }
    }
}
