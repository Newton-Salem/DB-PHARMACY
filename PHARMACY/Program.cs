var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSession();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.Use(async (context, next) =>
{
    var path = context.Request.Path.ToString().ToLower();

    if (!path.Contains("/account") &&
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



