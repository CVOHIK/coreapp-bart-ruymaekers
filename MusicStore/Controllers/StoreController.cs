using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStore.Controllers
{
    public class StoreController : Controller
    {

        private readonly StoreContext _context;
        public StoreController(StoreContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult ListGenres()
        {
            var vGenres = _context.Genres.OrderBy(g => g.Name);
            return View(vGenres);
        }

        public IActionResult ListAlbums(int id)
        {
            var vAlbums = _context.Albums.Where(x => x.GenreID == id).ToList();
            var vGenre = _context.Genres.FirstOrDefault(y => y.GenreID == id);

            ViewData["Genre"] = vGenre.Name.ToString();

            return View(vAlbums);
        }

        public IActionResult Details(int id)
        {
            var vAlbums = _context.Albums
                .Include(x => x.Artist)
                .Include(x => x.Genre)
                .FirstOrDefault(y => y.AlbumID == id);

            return View(vAlbums);
        }
    }
}
