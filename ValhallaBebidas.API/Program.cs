using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using ValhallaBebidas.Application.Services;
using ValhallaBebidas.Domain.Interfaces;
using ValhallaBebidas.Infrastructure.Data;
using ValhallaBebidas.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Consumindo o banco de dados, acesso ao banco
builder.Services.AddDbContext<ValhallaBebidasDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ValhallaBebidasConnection")));

//Registo dos repositórios (infrastructure implementa domain)
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

// Adicionar os repositórios faltantes:
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();


//registro dos serviços 
builder.Services.AddScoped<FuncionarioService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<PedidoService>();



//Passo 1: Configuração de CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});//configuração de API para permitir requisições de qualquer origem, método e cabeçalho.
   //Isso é útil para desenvolvimento, mas em produção,
   //é recomendado restringir as origens permitidas por questões de segurança.


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

//Passo 2: Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "ValhallaBebidas",
        Version = "v1",
        Description = "API do projeto ValhallaBebidas - Projeto Integrador"
    });
});


//verificar essa posição caso não de certo, no do luan não aparece 
builder.Services.AddOpenApi();

var app = builder.Build();
//Passo 3: Habilitar CORS no pipeline de requisições
app.UseCors("PermitirTudo");

//Passo 4: Habilitar Swagger/OpenAPI no ambiente de desenvolvimento
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ValhallaBebidas v1");//responsável por rodar a pagina
        c.RoutePrefix = string.Empty; // Define a raiz para acessar o Swagger UI// faz rodar com a porta configurada da aplicação
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
