using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



namespace PatientApi.Tests;

public class PatientApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public PatientApiTests(WebApplicationFactory<Program> factory)
    {

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor!=null)
                {
                    services.Remove(descriptor);
                    
                }
                services.AddDbContext<AppDbContext>(Options =>
                {
                    Options.UseInMemoryDatabase($"TestPatientDb_{Guid.NewGuid()}");
                });
            });
        }).CreateClient();
    }



    [Fact]
    public async Task Post_Deve_Retornar_Created()
    {
        
        var newPatient = new { Name = "Carlos", Cpf = "123" };

        var response = await _client.PostAsJsonAsync("/api/patients", newPatient);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdPatient = await response.Content.ReadFromJsonAsync<PatientTestModel>();

        Assert.NotNull(createdPatient);
        Assert.Equal("Carlos", createdPatient.Name);
        Assert.Equal("123", createdPatient.Cpf);
    }









}

public class PatientTestModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}