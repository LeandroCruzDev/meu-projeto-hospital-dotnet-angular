


using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if(!builder.Environment.IsEnvironment("Testing")){
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));
}
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();


app.UseCors("AllowAll");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var patientsApi = app.MapGroup("/api/patients");

patientsApi.MapGet("/", async (AppDbContext db) =>
    await db.Patients.ToListAsync());

patientsApi.MapGet("/{id}", async (int id, AppDbContext db) =>
    await db.Patients.FindAsync(id) is Patient patient
        ? Results.Ok(patient)
        : Results.NotFound());

patientsApi.MapPost("/", async (Patient patient, AppDbContext db) =>
{
    db.Patients.Add(patient);
    await db.SaveChangesAsync();
    return Results.Created($"/api/patients/{patient.Id}", patient);
});

patientsApi.MapPut("/{id}", async (int id, Patient updatedPatient, AppDbContext db) =>
{
    var patient = await db.Patients.FindAsync(id);
    if (patient is null) return Results.NotFound();

    patient.Name = updatedPatient.Name;
    patient.Cpf = updatedPatient.Cpf;
    
    await db.SaveChangesAsync();
    return Results.NoContent();
});

patientsApi.MapDelete("/{id}", async (int id, AppDbContext db) =>
{
    var patient = await db.Patients.FindAsync(id);
    if (patient is null) return Results.NotFound();

    db.Patients.Remove(patient);
    await db.SaveChangesAsync();
    return Results.NoContent();
});




app.Run();

public partial class Program { }