using FrotiXApi.Data;
using FrotiXApi.Repository;
using FrotiXApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.WindowsServices;

var builder = WebApplication.CreateBuilder(args);

// Executar como Windows Service
builder.Host.UseWindowsService(options =>
{
    options.ServiceName = "FrotiXApiService";
});

// EF Core + SQL Server (com resiliência)
builder.Services.AddDbContext<FrotiXDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql =>
            sql.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            )
    )
);

// DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (apenas para dev; no MAUI Hybrid, CORS não é necessário em produção)
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "DevelopmentPolicy",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

// Kestrel ouvindo em HTTP:5000
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000);
});

var app = builder.Build();

// Swagger & CORS somente em Desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FrotiX API V1");
        c.RoutePrefix = "swagger";
    });

    app.UseCors("DevelopmentPolicy");
}

// Mantenha HTTPS desabilitado se você expõe só HTTP:5000
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
