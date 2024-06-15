using ApiBlog.Data;
using ApiBlog.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//Cors
var MyOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyOrigins, policy => {
        policy.WithOrigins("*");
        policy.AllowAnyHeader();
        policy.WithMethods("GET", "POST", "PUT", "DELETE");
        });
});

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DBcontext
builder.Services.AddSqlServer<DwiApiBlogContext>(builder.Configuration.GetConnectionString("BDConection"));

//Services
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserProfileService>();

//Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>{
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"])),
            ValidateIssuer = false ,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//cords
app.UseCors(MyOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();