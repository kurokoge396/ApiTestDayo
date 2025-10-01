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
            ValidateIssuer = true,  // ���s�҂�����
            ValidateAudience = true,  // ��M�҂�����
            ValidateLifetime = true,  // �g�[�N���̗L������������
            ValidateIssuerSigningKey = true,  // ���s�҂̏����L�[������
            ValidIssuer = builder.Configuration["Jwt:Issuer"],  // �ݒ肳�ꂽ���s��
            ValidAudience = builder.Configuration["Jwt:Audience"],  // �ݒ肳�ꂽ��M��
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))  // �閧��
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
