using Aatithya_DAL.Middleware;
using Aatithya_BLL.Middleware;
using Aatithya_DAL.Entities;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Aatithya_Core.Common;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Aatithya_BLL.Middleware;
using Aatithya_Core.Common;
using Aatithya_DAL.Middleware;
using Aatithya.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
// Lea
//
// rn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    // Define a CORS policy named "MyCorsPolicy"
    options.AddPolicy("MyCorsPolicy",
        builder =>
        {
            // Allow requests from any origin
            builder.AllowAnyOrigin()
                   // Allow any header in the requests
                   .AllowAnyHeader()
                   // Allow any HTTP method in the requests
                   .AllowAnyMethod();
        });
});

///=============Start: Added By Jayshree on 29-03-2024 for JWT Authorization
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });



    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var jwtConfig = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication("JwtScheme")
    .AddJwtBearer("JwtScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]))
        };
    });
///=============End: Added By Jayshree on 29-03-2024 for JWT Authorization
///
//builder.Services.AddSwaggerGen();

///=============Added Middleware By Krupa on 06-01-2025 for Dependency Injection From MIDAS_DataAccessLayer.Middleware 
builder.Services.AddRepository();
///=============Added Middleware By Krupa on 06-01-2025 for Dependency Injection From MIDAS_BusinessLogicLayer.Middleware
builder.Services.AddServices();

///=============Added AutoMapper By Krupa on 06-01-2025 Registers AutoMapper to map objects between layers.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<GetLoggedInUserId>();

builder.Services.AddHttpContextAccessor();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Products_img")),
    RequestPath = "/Products_img"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Services_img")),
    RequestPath = "/Services_img"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Solutions_img")),
    RequestPath = "/Solutions_img"
});
/// =============Added By Krupa on 10-02-2025 Specify the CORS policy to allow cross-origin requests
app.UseCors("MyCorsPolicy");

app.UseAuthorization();

// Add your token validation middleware here
app.UseMiddleware<TokenValidationMiddleware>();


app.MapControllers();

app.Run();








//using Aatithya_DAL.Middleware;
//using Aatithya_BLL.Middleware;
//using Aatithya_DAL.Entities;
//using System.Configuration;
//using Microsoft.EntityFrameworkCore;
//using Aatithya_Core.Common;
//using Serilog.Events;
//using Serilog.Sinks.MSSqlServer;
//using Serilog;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using System.Text;
//using Microsoft.Extensions.FileProviders;
//using Aatithya_BLL.Middleware;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();

//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
//});

//builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddCors(options =>
//{
//    // Define a CORS policy named "MyCorsPolicy"
//    options.AddPolicy("MyCorsPolicy",
//        builder =>
//        {
//            // Allow requests from any origin
//            builder.AllowAnyOrigin()
//                   // Allow any header in the requests
//                   .AllowAnyHeader()
//                   // Allow any HTTP method in the requests
//                   .AllowAnyMethod();
//        });
//});

//builder.Services.AddSwaggerGen(opt =>
//{
//    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
//    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Please enter token",
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        BearerFormat = "JWT",
//        Scheme = "bearer"
//    });



//    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type=ReferenceType.SecurityScheme,
//                    Id="Bearer"
//                }
//            },
//            new string[]{}
//        }
//    });
//});


//var app = builder.Build();



//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseSwagger();
//app.UseSwaggerUI();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseCors("MyCorsPolicy");

//app.UseAuthorization();

//app.MapControllers();


//app.Run();








