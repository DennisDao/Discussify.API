using Application.Activites;
using Application.Followers;
using Application.Notifications;
using Application.Posts;
using CommonDataContract;
using Discussify.API.Service;
using Domain.AggegratesModel.UserAggegrate;
using Infrastructure;
using Infrastructure.BackroundServices;
using Infrastructure.Entities;
using Infrastructure.Interceptors;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ApplicationConnectionString");

// Interceptors
builder.Services.AddSingleton<DomainEventsInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>((sp,options) =>
{
    var interceptor = sp.GetService<DomainEventsInterceptor>();
    options.UseSqlServer(connectionString).AddInterceptors(interceptor);
});
       

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JWTSettings>(jwtSettings);

var secret = Encoding.ASCII.GetBytes(jwtSettings.Get<JWTSettings>().Secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;

}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, token =>
{
    token.RequireHttpsMetadata = false;
    token.SaveToken = true;
    token.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secret),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

//builder.Services.AddSingleton<TokenValidationParameters>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy("AppPolicy", builder =>
{
    builder.WithOrigins("*")
           .AllowAnyMethod()
    .AllowAnyHeader();
}));

// Repos
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IFollowerRepository, FolloweRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();

// Services
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ICommentCreatedProcessor, ActivityCommentCreatedProcessor>();
builder.Services.AddScoped<ICommentCreatedProcessor, NotificationProcessor>();

// Background Service
builder.Services.AddHostedService<OutBoxMessageProcessor>();

// Other services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AppPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Seed();

app.Run();
