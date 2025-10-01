using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,  // 発行者を検証
            ValidateAudience = true,  // 受信者を検証
            ValidateLifetime = true,  // トークンの有効期限を検証
            ValidateIssuerSigningKey = true,  // 発行者の署名キーを検証
            ValidIssuer = builder.Configuration["Jwt:Issuer"],  // 設定された発行者
            ValidAudience = builder.Configuration["Jwt:Audience"],  // 設定された受信者
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))  // 秘密鍵
        };
    });

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<WebApplication2.Interface.ITodoTest, WebApplication2.Interface.TodoTestImpl>();
builder.Services.AddDbContext<WebApplication2.Data.TestEFContext>();
builder.Services.AddControllersWithViews();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(option => option.SwaggerEndpoint("/openapi/v1.json", "v1"));
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
