using Microsoft.EntityFrameworkCore;
using N5Backend.Data;
using N5Backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ElasticSearchConfig>(builder.Configuration.GetSection("Elasticsearch"));
builder.Services.AddCors();

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IPermissionTypeService, PermissionTypeService>();


builder.Services.Configure<ElasticSearchConfig>(builder.Configuration.GetSection("Elasticsearch"));
builder.Services.AddSingleton<IElasticService, ElasticService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseCors(x=> x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();