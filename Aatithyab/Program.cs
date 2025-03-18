using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using AatithyaB_BLL.Middleware;
using AatithyaB_Core.Common;
using AatithyaB_DAL.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using AatithyaB_DAL.Entities;
using Microsoft.Extensions.FileProviders;
using AatithyaB.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Authorize please:",
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

builder.Services.AddRepository();
///=============Added Middleware By Krupa on 06-01-2025 for Dependency Injection From MIDAS_BusinessLogicLayer.Middleware
builder.Services.AddServices();

///=============Added AutoMapper By Krupa on 06-01-2025 Registers AutoMapper to map objects between layers.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<AESEncryption>();
builder.Services.AddSingleton<GetLoggedInUserId>();

builder.Services.AddHttpContextAccessor();

var aesEncryption = builder.Services.BuildServiceProvider().GetService<AESEncryption>();
if (aesEncryption != null)
{
    string? encryptedConnectionString = builder.Configuration.GetConnectionString("RestaurantConnectionString");

    if (!string.IsNullOrEmpty(encryptedConnectionString))
    {
        string? decryptedConnectionString = aesEncryption.Decrypt(encryptedConnectionString, "Aathithya@rudyyy");

        if (!string.IsNullOrEmpty(decryptedConnectionString))
        {
            builder.Services.AddDbContext<AatithyaDbContext>(options => options.UseSqlServer(decryptedConnectionString));

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.WriteTo.Console()
				.WriteTo.MSSqlServer(
                    connectionString: decryptedConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "WebApiLogs",
                        AutoCreateSqlTable = true
					},
                    columnOptions: new ColumnOptions(),
					restrictedToMinimumLevel: LogEventLevel.Information)
				.CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}



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

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
//    RequestPath = "/Uploads"
//});

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Gallery_img")),
//    RequestPath = "/Gallery_img"
//});

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "TypesofFood_img")),
//    RequestPath = "/TypesofFood_img"
//});

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Banquet_img")),
//    RequestPath = "/Banquet_img"
//});

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Restaurant_img")),
//    RequestPath = "/Restaurant_img"
//});

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Menu_img")),
//    RequestPath = "/Menu_img"
//});

app.UseCors("MyCorsPolicy");

app.UseAuthorization();

app.UseMiddleware<TokenValidationMiddleware>();


app.MapControllers();

app.Run();