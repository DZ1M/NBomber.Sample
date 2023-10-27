using NBomber.CSharp;
using static System.Net.WebRequestMethods;

using var httpClient = new HttpClient();

var scenario = Scenario.Create("Teste de Carga no Login", async context =>
{
    httpClient.DefaultRequestHeaders.Add("accept", "*/*");
    var requestData = new
    {
        email = "teste@teste.com",
        senha = "123456",
        idEmpresa = Guid.Empty.ToString()
    };

    string jsonContent = System.Text.Json.JsonSerializer.Serialize(requestData);
    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

    var response = await httpClient.PostAsync("https://localhost:7138/api/Auth/autenticar", content);

    return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
})
    .WithoutWarmUp()
    .WithLoadSimulations(
        Simulation.Inject(rate: 30,
        interval: TimeSpan.FromSeconds(1),
        during: TimeSpan.FromSeconds(30))
    );

NBomberRunner
    .RegisterScenarios(scenario)
    .Run();