using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Otel.Data;
using Otel.Models;

namespace Otel.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        _logger.LogInformation("vytvářím uživatele");
        var user = new User()
        {
            Age = 15,
            Name = "Franta",
            Surname = "Macek"
        };
        
        using (var db = new DataContext(_configuration))
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}