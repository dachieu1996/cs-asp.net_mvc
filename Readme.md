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
2012/02
```
Link: /movie/released/2020/2
```
ERROR 404
```
