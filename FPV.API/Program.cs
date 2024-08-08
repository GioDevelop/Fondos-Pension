using FPV.API.Authentication;
using FPV.API.Authentication.Contract;
using FPV.API.IoCRegister;
using FPV.Common.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


IoCRegister.AddDbContext(builder.Services, builder.Configuration.GetConnectionString("DefaultConnection"));
IoCRegister.AddRepository(builder.Services);
IoCRegister.AddServices(builder.Services);


#region Configure BasicAuthentication Swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FPV", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });

    c.CustomSchemaIds(type => type.ToString());
});

builder.Services.AddScoped<IValidateCredentials, ValidateCredentials>();
builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

#endregion
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });

builder.Services.AddControllers();


var app = builder.Build();

#region Carga los setting del appsettings.json a la clase AppSettingsApi
AppSettingsApi.Settings = builder.Configuration.GetSection("AppSettings").Get<AppSettingsApi>();
//AppSettingsWeb.Settings = builder.Configuration.GetSection("AppSettings").Get<AppSettingsWeb>();
#endregion 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();
