using System.Text;
using System.Text.Json.Serialization;
using DentistBooking.Application.Interfaces;
using DentistBooking.Application.Services;
using DentistBooking.Infrastructure;
using DentistBooking.Infrastructure.Repositories;
using DentistBooking.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

//Configure Json
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;    
});


// Configure services here
builder.Services.AddDbContext<DentistBookingContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add service here
builder.Services.AddScoped<PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddScoped<StaffRepository>();
builder.Services.AddScoped<IStaffService, StaffService>();

builder.Services.AddScoped<ProposeAppointmentRepository>();
builder.Services.AddScoped<IProposeAppointmentService, ProposeAppointmentService>();

builder.Services.AddScoped<TreatmentRepository>();
builder.Services.AddScoped<ITreatmentService, TreatmentService>();

builder.Services.AddScoped<MedicalRecordRepository>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();

builder.Services.AddScoped<AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddScoped<AppointmentDetailRepository>();
builder.Services.AddScoped<IAppointmentDetailService, AppointmentDetailService>();

builder.Services.AddScoped<DentistAvailabilityRepository>();
builder.Services.AddScoped<IDentistAvailabilityService, DentistAvailabilityService>();

builder.Services.AddScoped<IllnessRepository>();
builder.Services.AddScoped<IIllnessService, IllnessService>();


builder.Services.AddScoped<DentistRepository>();
builder.Services.AddScoped<IDentistService, DentistService>();



// Inject IConfiguration
builder.Services.AddSingleton(builder.Configuration);

//Add authentication
builder.Services.AddAuthentication("Bearer")
  .AddJwtBearer("Bearer", options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = false, // Disable issuer validation
          ValidateAudience = false, // Disable audience validation
          ValidateIssuerSigningKey = true, // Enable issuer signing key validation

          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("QZAAIVO8Jm5v01EVn7VQkkNaxWhrrfPbysLOvCP2iJk="))
      };
  });

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StaffOnly", policy =>
    {
        policy.RequireRole("staff");
    });
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PatientOnly", policy =>
    {
        policy.RequireRole("patient");
    });
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DentistOnly", policy =>
    {
        policy.RequireRole("dentist");
    });
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DentistOrStaff", policy =>
    {
        policy.RequireAssertion(context =>
        {
            return context.User.IsInRole("dentist") || context.User.IsInRole("staff");
        });
    });
});
builder.Services.AddAuthorization(options =>
{
   options.AddPolicy("PatientOrStaff", policy =>
   {
       policy.RequireAssertion(context =>
       {
           return context.User.IsInRole("patient") || context.User.IsInRole("staff");
       });
   });
});


// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
  
app.UseSwagger();
app.UseSwaggerUI();


app.UseMiddleware<AuthMiddleware>();

//app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Run the application
app.Run();
