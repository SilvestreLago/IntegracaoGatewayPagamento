using IntegracaoGatewayPagamento.Repositories;
using IntegracaoGatewayPagamento.Repositories.Interfaces;
using IntegracaoGatewayPagamento.Services;
using IntegracaoGatewayPagamento.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<ICobrancaRepository, CobrancaRepository>();
builder.Services.AddScoped<ICobrancaService, CobrancaService>();
builder.Services.AddHttpClient<IClienteService, ClienteService>(client =>
{
    var baseUrl = Environment.GetEnvironmentVariable("ASAAS_BASEURL") 
                  ?? throw new InvalidOperationException("ASAAS_BASEURL não definida no .env");
    
    client.BaseAddress = new Uri(baseUrl);
    
    client.DefaultRequestHeaders.Add("access_token", Environment.GetEnvironmentVariable("ASAAS_APIKEY"));
    
    client.DefaultRequestHeaders.UserAgent.ParseAdd("Integracao Gateway");
});
builder.Services.AddHttpClient<ICobrancaService, CobrancaService>(client =>
{
    var baseUrl = Environment.GetEnvironmentVariable("ASAAS_BASEURL") 
                  ?? throw new InvalidOperationException("ASAAS_BASEURL não definida no .env");
    
    client.BaseAddress = new Uri(baseUrl);
    
    client.DefaultRequestHeaders.Add("access_token", Environment.GetEnvironmentVariable("ASAAS_APIKEY"));
    
    client.DefaultRequestHeaders.UserAgent.ParseAdd("Integracao Gateway");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CARREGAR O .ENV
DotNetEnv.Env.Load();

//LÊ VARIAVEL DE AMBIENTE DO .ENV DO BANCO DE DADOS
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

//VERIFICA QUE A STRING NÃO É NULA
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("A string de conexão DB_CONNECTION_STRING não foi encontrada no ambiente ou no arquivo .env.");
}

//REGISTRAR O DBCONTEXT USANDO A STRING DO .ENV
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

//HABILITAR O SWAGGER
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("FrontendPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();