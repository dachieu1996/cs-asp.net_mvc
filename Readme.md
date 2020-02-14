# Fundamentals
## Route: Get Parameters From Url

```js
public class MovieController : Controller
{
    public ActionResult Parameter(int id, int anotherId)
    {
        return Content("Id: " + id + ", AnotherId: " + anotherId);
    }
}
```
Link: /movie/parameter/**1?anotherId=3**
```
Id: 1, AnotherId: 3
```

## Route: Convention-base Routing
```js
public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.MapRoute(
            name: "MoviesByReleaseDate", 
            url: "movie/released/{year}/{month}", 
            defaults: new { controller = "Movie", action = "ByReleaseDate",
            constraints: new { year = @"\d{4}", month = @"\d{2}" }); // Optional
    }
}
```
```js
public class MovieController : Controller
{
    public ActionResult ByReleaseDate(int year, int month)
    {
        return Content(year + "/" + month);
    }
}
```

Link: /movie/released/2020/02
```
2012/2
```
Link: /movie/released/2020/2
```
ERROR 404
```

## Route: Attribute Routing
```js
public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.MapMvcAttributeRoutes();
    }
}
```
```js
public class MovieController : Controller
{
    // Route: Attribute Routing
    [Route("movie/issued/{year}/{month:range(1,12)}")]
    public ActionResult ByIssuedDate(int year, int month)
    {
        return Content(year + "/" + month);
    }
}
```
Link: /movie/issued/2020/2
```
2012/2
```

## View: Passing Data to View
```js
public class MovieController : Controller
{
    public ActionResult Ramdon()
    {
        var movie = new Movie() { Name = "Shrek!" };
        return View(movie);
    }
}
```
```html
@model Vidly.Models.Movie

<h2>@Model.Name</h2>
```
```
Shrek!
```
#### ViewData - Cons: Magic string inside
```js
public ActionResult Ramdon()
{
    var movie = new Movie() { Name = "Shrek!" };
    ViewData["Movie"] = movie;
    return View();
}
```
```html
@using Vidly.Models

<h2>@(((Movie)ViewData["Movie"]).Name)</h2>
```

#### ViewBag
```js
public ActionResult Ramdon()
{
    var movie = new Movie() { Name = "Shrek!" };
    ViewBag.Movie = movie;
    return View();
}
```
```html
@using Vidly.Models

<h2>@ViewBag.Movie.Name</h2>
```

## View: ViewModel
```js
public class RamdonMovieViewModel
{
    public Movie Movie { get; set; }
    public List<Customer> Customers { get; set; }
}
```
```js
public class MovieController : Controller
{
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
```
```html
@model Vidly.ViewModels.RamdonMovieViewModel

<h2>@Model.Movie.Name</h2>
<ul>
    @foreach (var customer in Model.Customers)
    {
        <li>@customer.Name</li>
    }
</ul>
```