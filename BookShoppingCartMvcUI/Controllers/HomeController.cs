using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace BookShoppingCartMvcUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeRepository _homeRepository;
    private readonly IGenreRepository _genreRepo;

    private readonly IMemoryCache _memoryCache;

    public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository, IGenreRepository genreRepo, IMemoryCache memoryCache)
    {
        _homeRepository = homeRepository;
        _logger = logger;
        _genreRepo = genreRepo;
        _memoryCache = memoryCache;
    }

    public async Task<IActionResult> Index(string sterm = "", int genreId = 0)
    {
        string cacheKey = $"book-{sterm}-{genreId}";

        if (!_memoryCache.TryGetValue(cacheKey, out BookDisplayModel bookModel))
        {

            IEnumerable<Book> books = await _homeRepository.GetBooks(sterm, genreId);
            IEnumerable<Genre> genres = await _genreRepo.GetGenres();

            bookModel = new()
            {
                Books = books,
                Genres = genres,
                STerm = sterm,
                GenreId = genreId
            };

            var cacheOptions = new MemoryCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromMinutes(10));
            _memoryCache.Set(cacheKey, bookModel, cacheOptions);
        }
        return View(bookModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
