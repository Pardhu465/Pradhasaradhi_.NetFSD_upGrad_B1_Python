
using AutoMapper;
using ELearningPlatform.Data;
using ELearningPlatform.Mapping;
using ELearningPlatform.Repositories;
using ELearningPlatform.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ── Database ────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── AutoMapper ──────────────────────────────────────────────────────────────
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ── Repositories ────────────────────────────────────────────────────────────
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IQuizRepository,  QuizRepository>();
builder.Services.AddScoped<IUserRepository,  UserRepository>();

// ── Services ────────────────────────────────────────────────────────────────
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IQuizService,   QuizService>();
builder.Services.AddScoped<IUserService,   UserService>();

// ── MVC + API ───────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ── Swagger ─────────────────────────────────────────────────────────────────
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "E-Learning Platform API",
        Version     = "v1",
        Description = "REST API for the E-Learning Platform — " +
                      "Courses, Lessons, Quizzes, Users",
        Contact = new OpenApiContact
        {
            Name  = "E-Learning Dev Team",
            Email = "dev@elearning.com"
        }
    });
});

// ── CORS (dev) ───────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// ── Pipeline ─────────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Learning API v1");
        c.RoutePrefix = "swagger";
    });
}

var defaultFileOptions = new DefaultFilesOptions();
defaultFileOptions.DefaultFileNames.Clear();
defaultFileOptions.DefaultFileNames.Add("Dashboard.html");
defaultFileOptions.DefaultFileNames.Add("index.html");
app.UseDefaultFiles(defaultFileOptions);    // serves Dashboard.html as the default landing page
app.UseStaticFiles();                       // serves wwwroot/

app.UseCors();
app.UseAuthorization();
app.MapControllers();

// Auto-migrate on startup (dev convenience)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();

