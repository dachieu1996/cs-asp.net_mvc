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
## View: Link
```css
@Html.ActionLink(customer.Name, "Detail", "Customer", new { id = customer.Id }, null)
```

## Model: Migrations
```
enable-migrations
add-migration InitialModel
update-database
```
```js
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Movie> Movies { get; set; }

    public ApplicationDbContext()
        : base("DefaultConnection", throwIfV1Schema: false)
    {
    }

    public static ApplicationDbContext Create()
    {
        return new ApplicationDbContext();
    }
}
```
## Model: Seeding - Raw Query
```js
public partial class SeedMembershipType : DbMigration
{
    public override void Up()
    {
        Sql("INSERT INTO MembershipTypes (SignUpFee, DurationInMonths, DiscountRate) VALUES (0, 0, 0)");
        Sql("INSERT INTO MembershipTypes (SignUpFee, DurationInMonths, DiscountRate) VALUES (30, 1, 10)");
        Sql("INSERT INTO MembershipTypes (SignUpFee, DurationInMonths, DiscountRate) VALUES (90, 3, 15)");
        Sql("INSERT INTO MembershipTypes (SignUpFee, DurationInMonths, DiscountRate) VALUES (300, 12, 20)");
    }
        
    public override void Down()
    {
    }
}
```

## View: Form (TextBox, Date, Checkbox, DropdownList, Hidden)
```html
<h2>New Customer</h2>

@using (Html.BeginForm("Save", "Customer"))
{
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.Name)
        @Html.TextBoxFor(m => m.Customer.Name, new { @class = "form-control" })
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.BirthDate)
        @Html.TextBoxFor(m => m.Customer.BirthDate,"{0:yyyy-MM-dd}", new { @class = "form-control datepicker", type = "date" })
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.MembershipTypeId)
        @Html.DropDownListFor(m => m.Customer.MembershipTypeId, new SelectList(Model.MembershipTypes,"Id","DurationInMonths"),"Select Membership Type", new { @class = "form-control" })
    </div>
    <div class="checkbox">
        <label>
            @Html.CheckBoxFor(m => m.Customer.IsSubcribedToNewsletter) Subcribed to Newsletter
        </label>
    </div>
    @Html.HiddenFor(m => m.Customer.Id)
    <button type="submit" class="btn btn-primary">Save</button>
}
```

## Controller - Model: Model Binding
```js
[HttpPost]
public ActionResult Save(Customer customer)
{
    if (customer.Id == 0)
        _context.Customers.Add(customer);
    else
    {
        var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == customer.Id);
        customerInDb.Name = customer.Name;
        customerInDb.BirthDate = customer.BirthDate;
        customerInDb.IsSubcribedToNewsletter = customer.IsSubcribedToNewsletter;
        customerInDb.MembershipTypeId = customer.MembershipTypeId;
    }
            
    _context.SaveChanges();
    return RedirectToAction("Index");
}
```
## Controller - Model - View: Validation
```js
[HttpPost]
public ActionResult Save(Customer customer)// CustomerController
{
    if (!ModelState.IsValid)
    {
        var viewModel = new CustomerFormViewModel
        {
            Customer = customer,
            MembershipTypes = _context.MembershipTypes.ToList()
        };
        return View("CustomerForm", viewModel);
    }
    ...
}
```
```js
public class Customer
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
}
```
```html
<div class="form-group">
    @Html.LabelFor(m => m.Customer.Name)
    @Html.TextBoxFor(m => m.Customer.Name, new { @class = "form-control" })
    @Html.ValidationMessageFor( m => m.Customer.Name)
</div>
```