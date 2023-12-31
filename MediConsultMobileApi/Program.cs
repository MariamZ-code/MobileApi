
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MediConsultMobileApi.Helper;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository;
using MediConsultMobileApi.Repository.Interfaces;

using MediConsultMobileApi.Validations;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

//var txt = "_myAllowSpecificOrigins";

builder.Services.AddScoped<IAuthRepository, AuthRepository>(); 
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IServiceRepository, SeviceRepository>();
builder.Services.AddScoped<IProviderDataRepository , ProviderDataRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IMedicalNetworkRepository , MedicalNetworkRepository>();
builder.Services.AddScoped<ITokenRepository , TokenRepository>();
builder.Services.AddScoped<ICategoryRepository , CategoryRepository>();
builder.Services.AddScoped<IRefundRepository, RefundRepository>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IRefundTypeRepository, RefundTypeRepository>();
builder.Services.AddScoped<IValidation, Validation>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string jsonKeyFilePath = "Keys/mediconsult_app_firebase_key.json";
string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, jsonKeyFilePath);
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(fullPath),
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
