using Agency.DAL;
using Agency.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agency.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<HomeVM> GetDatas()
        {
            HomeVM model = new HomeVM
            {
                Settings = await _context.Settings.ToListAsync(),
                SocialMedias = await _context.SocialMedias.ToListAsync()
            };
            return (model);
        }
    }
}
