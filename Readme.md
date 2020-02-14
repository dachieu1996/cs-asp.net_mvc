# Fundamentals
## Get Parameters From Url

```js
public class MovieController : Controller
{
    public ActionResult Edit(int id, int anotherId)
    {
        return Content("Id: " + id + ", AnotherId: " + anotherId);
    }
}
```
Link: /movie/edit/**1?anotherId=3**
```
Id: 1, AnotherId: 3
```

