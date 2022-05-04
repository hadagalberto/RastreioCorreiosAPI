using Newtonsoft.Json;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/rastreio/{codigo}", async (string codigo) =>
{
    codigo = codigo.ToUpper();
    var client = new RestClient("https://proxyapp.correios.com.br/v1/sro-rastro/");
    var request = new RestRequest(codigo);
    var response = await client.GetAsync(request);
    if (response.Content != null)
    {
        var obj = JsonConvert.DeserializeObject(response.Content);
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        return json;
    } else
    {
        return "Objeto não encontrado";
    }

}).WithName("Rastreio de objetos");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}