var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ProductDbContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:AppDataConnection"]);
});

builder.Services.AddHttpsRedirection(opts => {
    opts.HttpsPort = 44350;
});

builder.Services.AddDbContext<IdentityDbContext>(opts => {
    opts.UseSqlServer(
        builder.Configuration["ConnectionStrings:IdentityConnection"],
        opts => opts.MigrationsAssembly("IdentityApp")
    );
});

builder.Services.AddScoped<IEmailSender, ConsoleEmailSender>();

builder.Services.AddDefaultIdentity<IdentityUser>(opts => {
        opts.Password.RequiredLength = 8;
        opts.Password.RequireDigit = false;
        opts.Password.RequireLowercase = false;
        opts.Password.RequireUppercase = false;
        opts.Password.RequireNonAlphanumeric = false;
        opts.SignIn.RequireConfirmedAccount = true;
    }).AddEntityFrameworkStores<IdentityDbContext>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});
//app.MapGet("/", () => "Hello World!");

app.Run();
