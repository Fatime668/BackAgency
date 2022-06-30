using Agency.DAL;
using Agency.Extensions;
using Agency.Models;
using Agency.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agency.Areas.AgencyAdmin.Controllers
{
    [Area("AgencyAdmin")]
    public class PortfolioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PortfolioController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Portfolio> portfolios = await _context.Portfolios.ToListAsync();
            return View(portfolios);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Portfolio portfolio)
        {
            if (!ModelState.IsValid) return View();
            if (portfolio.Photo != null)
            {
                if (!portfolio.Photo.IsOkay(1))
                {
                    portfolio.Image = await portfolio.Photo.FileCreate(_env.WebRootPath, @"assets\images");
                    await _context.AddAsync(portfolio);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Zehmet olmasa 1 mb-n altinda sekil daxil edin");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Zehmet olmasa sekil secin");
                return View();
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            Portfolio existedPortfolio = await _context.Portfolios.FirstOrDefaultAsync(e => e.Id == id);
            return View(existedPortfolio);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id,Portfolio portfolio)
        {
            if (!ModelState.IsValid) return View();
            Portfolio existedPortfolio = await _context.Portfolios.FirstOrDefaultAsync(e => e.Id == id);
            if (portfolio != null)
            {
                if (!portfolio.Photo.IsOkay(1))
                {
                    string path = _env.WebRootPath + @"\assets\images\" + portfolio.Image;
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    existedPortfolio.Image = await portfolio.Photo.FileCreate(_env.WebRootPath, @"assets\images");
                    existedPortfolio.Title = portfolio.Title;
                    existedPortfolio.Subtitle = portfolio.Subtitle;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Zehmet olmasa 1 mb-n altinda sekil daxil edin");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Zehmet olmasa sekil secin");
                return View();
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            Portfolio portfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.Id == id);
            return View(portfolio);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePortfolio(int id)
        {
            Portfolio portfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.Id == id);
            string path = _env.WebRootPath + @"\assets\images\" + portfolio.Image;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
