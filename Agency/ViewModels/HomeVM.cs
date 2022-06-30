using Agency.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agency.ViewModels
{
    public class HomeVM
    {
        public List<Portfolio> Portfolios { get; set; }
        public List<Setting> Settings { get; set; }
        public List<SocialMedia> SocialMedias { get; set; }
    }
}
