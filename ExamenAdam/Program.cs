using ExamenAdam.Data;
using ExamenAdam.Entities;
using ExamenAdam.Identity;
using ExamenAdam.Identity.Entities;
using ExamenAdam.Identity.Requirements;
using ExamenAdam.Identity.Requirements.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    .AddMvcLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddRequestLocalization(options =>
{
    options.AddSupportedUICultures("en-US", "nl");
    options.AddSupportedUICultures("en-US", "nl");
});


builder.Services.AddDbContext<ExamenAdamContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<CommentRepository>();

builder.Services.AddAuthorization(options =>
{  
    
    options.AddPolicy(Policies.Approved, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Administrator", "Manager", "User");
        policy.Requirements.Add(new MustBeApproved());
    });

    //options.DefaultPolicy = options.GetPolicy("Approved")!;
});

builder.Services.AddScoped<IAuthorizationHandler, MustBeApprovedHandler>();
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<ExamenAdamContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

    var users = new User[]
    {
        new User
        {
            Birthday = new DateTime(2000, 01, 01),
            Sex = "male",
            Address = new Address()
            {
                StreetName = "street",
                StreetNumber = "01",
                PostalBus = "/",
                PostalCode = "1000",
                City = "city",
                Country = "country"
            },
            Email = "email@address1.com",
            UserName = "username1",
            PhoneNumber = "+32111111111",
            FirstName = "firstname1",
            LastName = "lastname1",
            PasswordHash = "Password1!",
            Approved = true
        },
        new User
        {
            Birthday = new DateTime(2000, 02, 02),
            Sex = "female",
            Address = new Address()
            {
                StreetName = "street",
                StreetNumber = "02",
                PostalBus = "/",
                PostalCode = "2000",
                City = "city",
                Country = "country"
            },
            Email = "email@address2.com",
            UserName = "username2",
            PhoneNumber = "+32222222222",
            FirstName = "firstname2",
            LastName = "lastname2",
            PasswordHash = "Password2!",
            Approved = true
        },
        new User
        {
            Birthday = new DateTime(2000, 03, 03),
            Sex = "female",
            Address = new Address()
            {
                StreetName = "street",
                StreetNumber = "03",
                PostalBus = "/",
                PostalCode = "3000",
                City = "city",
                Country = "country"
            },
            Email = "email@address3.com",
            UserName = "username3",
            PhoneNumber = "+32333333333",
            FirstName = "firstname3",
            LastName = "lastname3",
            PasswordHash = "Password3!",
            Approved = true
        },
        new User
        {
            Birthday = new DateTime(2000, 04, 04),
            Sex = "female",
            Address = new Address()
            {
                StreetName = "street",
                StreetNumber = "04",
                PostalBus = "/",
                PostalCode = "4000",
                City = "city",
                Country = "country"
            },
            Email = "email@address4.com",
            UserName = "username4",
            PhoneNumber = "+32444444444",
            FirstName = "firstname4",
            LastName = "lastname4",
            PasswordHash = "Password4!",
            Approved = false
        }
    };

    var roles = new Role[]
    {
        new Role
        {
            Name = "Administrator",
            NormalizedName = "ADMINISTRATOR"
        },
        new Role
        {
            Name = "Manager",
            NormalizedName = "MANAGER"
        },
        new Role
        {
            Name = "User",
            NormalizedName = "USER"
        },
        new Role
        {
            Name = "Pending",
            NormalizedName = "PENDING"
        }
    };


    foreach (var user in users)
    {
        if (await userManager.FindByNameAsync(user.UserName) is null)
        {
            var result = await userManager.CreateAsync(user, user.PasswordHash);

            if (result.Succeeded is false)
            {
                throw new Exception($"Failed to add user {user.UserName}");
            }

        }
    }

    foreach (var role in roles)
    {
        if (await roleManager.FindByNameAsync(role.Name) is null)
        {
            var result = await roleManager.CreateAsync(role);

            if (result.Succeeded is false)
            {
                throw new Exception($"Failed to add role {role.Name}");
            }
        }
    }

    await userManager.AddToRoleAsync(await userManager.FindByNameAsync("username1"), "Administrator");
    await userManager.AddToRoleAsync(await userManager.FindByNameAsync("username2"), "Manager");
    await userManager.AddToRoleAsync(await userManager.FindByNameAsync("username3"), "User");
    await userManager.AddToRoleAsync(await userManager.FindByNameAsync("username4"), "Pending");

}

app.Run();
