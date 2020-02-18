# Fundamentals
## Route: Get Parameters From Url

```cs
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
```cs
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
```cs
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
```cs
public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.MapMvcAttributeRoutes();
    }
}
```
```cs
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
```cs
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
```cs
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
```cs
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
```cs
public class RamdonMovieViewModel
{
    public Movie Movie { get; set; }
    public List<Customer> Customers { get; set; }
}
```
```cs
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
```cs
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
```cs
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
```cs
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
```cs
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
```cs
public class Customer
{
    [Required(ErrorMessage = "Field Name is required")]
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
Add style for validation Content/Site.css
```css
.field-validation-error {
    color: red;
}

.input-validation-error {
    border: 2px solid red;
}
```

## Controller - Model - View: Custom Validation
```cs
public class Min18YearsIfAMember : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var customer = (Customer) validationContext.ObjectInstance;

        if(customer.MembershipTypeId == MembershipType.Unknown || customer.MembershipTypeId == MembershipType.PayAsYouGo)
            return ValidationResult.Success;

        if(customer.BirthDate == null)
            return new ValidationResult("Birthdate is required");

        var age = DateTime.Today.Year - customer.BirthDate.Value.Year;

        return (age >= 18) 
            ? ValidationResult.Success 
            : new ValidationResult("Customer should be at least 18 year");
    }
}
```
```cs
// Customer.cs
[Display(Name = "Date of Birth")] 
[Min18YearsIfAMember]
public DateTime? BirthDate { get; set; }
```

## Controller - Model - View: Client-side Validation
```cs
// CustomerForm.chtml
@section scripts
{
    @Scripts.Render("~/bundles/jqueryval")
}
```

## Controller - View: CSRF
```cs
@using (Html.BeginForm("Save", "Customer"))
{
    ...
    @Html.AntiForgeryToken()
    <button type="submit" class="btn btn-primary">Save</button>
}
```
```cs
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Save(Customer customer) {}
```

## Controller: Api
```cs
// App_Start Folder_
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
    }
}
```
```cs
public class MvcApplication : System.Web.HttpApplication
{
    protected void Application_Start()
    {
        GlobalConfiguration.Configure(WebApiConfig.Register);
        ...
    }
}
```
## Controller: Api - DTO -Data Transfer Object
```cs
// Dtos/CustomerDto.cs
public class CustomerDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Field Name is required")]
    [MaxLength(255)]
    public string Name { get; set; }

    [Min18YearsIfAMember]
    public DateTime? BirthDate { get; set; }

    public bool IsSubcribedToNewsletter { get; set; }

    public byte MembershipTypeId { get; set; }
}
```

#### AutoMapper
```
install-package automapper -version:4.1
```
```cs
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        Mapper.CreateMap<Customer, CustomerDto>();
        Mapper.CreateMap<CustomerDto, Customer>();
    }
}
```
```cs
public class MvcApplication : System.Web.HttpApplication
{
    protected void Application_Start()
    {
        Mapper.Initialize(c => c.AddProfile<MappingProfile>());
        ...
    }
}
```
```cs
public class CustomerController : ApiController
{
    private ApplicationDbContext _context = new ApplicationDbContext();

    // GET: api/Customer
    public IEnumerable<CustomerDto> GetCustomers()
    {
        return _context.Customers.ToList().Select(Mapper.Map<Customer, CustomerDto>);
    }
}
```