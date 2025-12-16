var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Enable Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// Global Authorization Middleware
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value.ToLower();

    if (!path.StartsWith("/account") &&
        !path.StartsWith("/css") &&
        !path.StartsWith("/js") &&
        !path.StartsWith("/lib") &&
        context.Session.GetString("Role") == null)
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next();
});

app.UseAuthorization();

app.MapRazorPages();

app.Run();
