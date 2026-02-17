using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json;
using Data.DataBase;

var builder = WebApplication.CreateBuilder(args);

// ── Inicializar configuración de base de datos ─────────────────────────────
Conection.Initialize(builder.Configuration);

// ── Firebase Admin SDK ─────────────────────────────────────────────────────
var credentialPath = builder.Configuration["Firebase:CredentialPath"] ?? "firebase-credentials.json";
string? projectId = null;

if (File.Exists(credentialPath))
{
    var credentialJson = File.ReadAllText(credentialPath);
    FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.FromJson(credentialJson)
    });
    
    // Extraer project_id del archivo de credenciales
    var credentialDoc = JsonDocument.Parse(credentialJson);
    projectId = credentialDoc.RootElement.GetProperty("project_id").GetString();
}
else if (builder.Environment.IsProduction())
{
    var firebaseJson = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS");
    if (!string.IsNullOrEmpty(firebaseJson))
    {
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromJson(firebaseJson)
        });
        
        // Extraer project_id de la variable de entorno
        var credentialDoc = JsonDocument.Parse(firebaseJson);
        projectId = credentialDoc.RootElement.GetProperty("project_id").GetString();
    }
    else
    {
        throw new InvalidOperationException(
            "Firebase credentials not found. Set FIREBASE_CREDENTIALS environment variable in Azure.");
    }
}
else
{
    throw new FileNotFoundException($"Firebase credentials file not found: {credentialPath}");
}

// ── Autenticación con Firebase ─────────────────────────────────────────────
// Fallback a appsettings.json si no se pudo extraer del archivo de credenciales
if (string.IsNullOrEmpty(projectId))
{
    projectId = builder.Configuration["Firebase:ProjectId"];
}

if (string.IsNullOrEmpty(projectId))
{
    throw new InvalidOperationException("Firebase ProjectId could not be determined from credentials file or appsettings.json");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{projectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{projectId}",
            ValidateAudience = true,
            ValidAudience = projectId,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

// ── CORS para permitir requests desde frontend ─────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// ── HttpClient para comunicación con Firebase REST API ─────────────────────
builder.Services.AddHttpClient();
builder.Services.AddHttpClient();

// ── Swagger/OpenAPI ────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ERP Grupo 2 - API",
        Description = "API para gestión de pedidos a proveedores con Firebase Authentication",
        Contact = new OpenApiContact
        {
            Name = "Equipo Grupo 2",
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando Bearer. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// ── Inyección de dependencias centralizada ─────────────────────────────────
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API v1"));
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API v1");
        c.RoutePrefix = string.Empty;
    });
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ── CORS debe ir antes de Authentication/Authorization ────────────────────
app.UseCors("AllowAll");

// ── Middleware de autenticación (orden importante) ─────────────────────────
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

