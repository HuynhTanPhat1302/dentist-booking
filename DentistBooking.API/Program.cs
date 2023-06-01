using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using DentistBooking.Infrastructure.Repositories;
using DentistBooking.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();


// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configure your services here
// For example, you can register your DbContext and other services
builder.Services.AddDbContext<DentistBookingContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add the PatientRepository as a service
builder.Services.AddScoped<PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<DentistRepository>();
builder.Services.AddScoped<StaffRepository>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<AppointmentRepository>();
builder.Services.AddScoped<MedicalRepository>();
builder.Services.AddScoped<DentistAvailabilityRepository>();
builder.Services.AddScoped<TreatmentRepository>();
builder.Services.AddScoped<IllnessRepository>();   

// Inject IConfiguration
builder.Services.AddSingleton(builder.Configuration);

//Add authentication
/*builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Disable issuer validation
            ValidateAudience = false, // Disable audience validation
            ValidateIssuerSigningKey = true, // Enable issuer signing key validation
            
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("QZAAIVO8Jm5v01EVn7VQkkNaxWhrrfPbysLOvCP2iJk="))
        };
    });*/

// Add authorization policies
/*builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StaffOnly", policy =>
    {
        policy.RequireRole("staff");
        


    });
});*/

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*app.UseMiddleware<AuthMiddleware>();*/

app.UseHttpsRedirection();

app.UseRouting();

/*app.UseAuthentication();

app.UseAuthorization();*/

app.MapControllers();

app.Run();

// Run the application
await app.RunAsync();
