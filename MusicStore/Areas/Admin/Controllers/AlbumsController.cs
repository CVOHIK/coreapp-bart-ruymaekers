using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Models;

namespace MusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AlbumsController : Controller
    {
        private readonly StoreContext _context;

        public AlbumsController(StoreContext context)
        {
            _context = context;
        }

        // GET: Admin/Albums
        public async Task<IActionResult> Index(int? GenreID, int? ArtistID, string Title)
        {
            var storeData = _context.Albums.Include(a => a.Artist).Include(a => a.Genre).ToList();

            if (GenreID.HasValue | GenreID != null)
            {
                storeData = storeData.Where(a => a.GenreID == GenreID).ToList();
            }

            if (ArtistID.HasValue | ArtistID!=null)
            {
                storeData = storeData.Where(a => a.ArtistID == ArtistID).ToList();
            }
 
            if (Title != null)
            {
                storeData = storeData.Where(a => a.Title.ToUpper().Contains(Title.ToUpper())).ToList();
            }

            ViewData["AllGenres"] = new SelectList(_context.Genres.ToList(), "GenreID", "Name");
            ViewData["AllArtists"] = new SelectList(_context.Artists.ToList(), "ArtistID", "Name");

            return View(storeData);

            //var albums = _context.Albums
            //   .Where(a => (GenreID == 0 || GenreID == null) || a.GenreID == GenreID)
            //   .Where(a => (ArtistID == 0 || ArtistID == null) || a.ArtistID == ArtistID)
            //   .Where(a => Title == null || a.Title.Contains(Title))
            //   .OrderBy(a => a.Title);

            //var listAlbumsViewModel = new ();
            //listAlbumsViewModel.Albums = await albums.ToListAsync();

            //listAlbumsViewModel.Genres = new SelectList(await _context.Genres.OrderBy(g => g.Name).ToListAsync(), "GenreID", "Name");

            //listAlbumsViewModel.Artists = new SelectList(await _context.Artists.OrderBy(ar => ar.Name).ToListAsync(), "ArtistID", "Name");

            //return View(listAlbumsViewModel);

        }

        // GET: Admin/Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.AlbumID == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Admin/Albums/Create
        public IActionResult Create()
        {
            //ViewData["ArtistID"] = new SelectList(_context.Artists, "ArtistID", "ArtistID");
            //ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "GenreID");
            ViewData["ArtistNames"] = new SelectList(_context.Artists, "ArtistID", "Name");
            ViewData["GenreNames"] = new SelectList(_context.Genres, "GenreID", "Name");
            return View();
        }

        // POST: Admin/Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlbumID,GenreID,ArtistID,Title,Price,AlbumArtUrl")] Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistID"] = new SelectList(_context.Artists, "ArtistID", "ArtistID", album.ArtistID);
            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "GenreID", album.GenreID);

            return View(album);
        }

        // GET: Admin/Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            //var albums = _context.Albums
            //    .Where(a => a.AlbumID == id)
            //    .Include(t => t.Genre)
            //    .Include(t => t.Artist)
            //    .ToList();
            //ViewData["ArtistID"] = new SelectList(_context.Artists, "ArtistID", "ArtistID", album.ArtistID);
            //ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "GenreID", album.GenreID);
            ViewData["ArtistNames"] = new SelectList(_context.Artists, "ArtistID", "Name");
            ViewData["GenreNames"] = new SelectList(_context.Genres, "GenreID", "Name");
            return View(album);
        }

        // POST: Admin/Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlbumID,GenreID,ArtistID,Title,Price,AlbumArtUrl")] Album album)
        {
            if (id != album.AlbumID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.AlbumID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistID"] = new SelectList(_context.Artists, "ArtistID", "ArtistID", album.ArtistID);
            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "GenreID", album.GenreID);
            return View(album);
        }

        // GET: Admin/Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.AlbumID == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Admin/Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.AlbumID == id);
        }
    }
}
