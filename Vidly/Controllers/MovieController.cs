using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Parameter(int id, int anotherId)
        {
            return Content("Id: " + id + ", AnotherId: " + anotherId);
        }

        // Route: Convention-base Routing
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
        }

        // Route: Attribute Routing
        [Route("movie/issued/{year}/{month:range(1,12)}")]
        public ActionResult ByIssuedDate(int year, int month)
        {
            return Content(year + "/" + month);
        }

        // View: Passing Data to Views
        public ActionResult Ramdon()
        {
            var movie = new Movie() { Name = "Shrek!" };
            var customers = new List<Customer>
            {
                new Customer { Name = "Customer1" },
                new Customer { Name = "Customer2" },
                new Customer { Name = "Customer3" }
            };
            var viewModel = new RamdonMovieViewModel
            {
                Movie = movie,
                Customers = customers
            }; 

            return View(viewModel);
        }
    }
}