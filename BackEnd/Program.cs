using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SoftOS.API.Middlewares;
using SoftOS.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SoftOS.DAL.Context.AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLite"))
);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();


builder
    .Services.AddScoped<IAuthService, AuthService>()
    .AddScoped<IUsuarioService, EmpresaService>()
    .AddScoped<IClienteService, ClienteService>()
    .AddScoped<IOrdemServicoService, OrdemServicoService>()
    .AddScoped<IProfissionalService, ProfissionalService>()
    .AddScoped<ITicketService, TicketService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description = "Insira um token v<>lido, por favor",
            Type = SecuritySchemeType.Http,
            In = ParameterLocation.Header,
            Name = "Authorization",
            BearerFormat = "JWT",
            Scheme = "Bearer",
        }
    );
    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
