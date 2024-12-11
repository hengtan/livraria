using System.Data;
using Livraria.Application.Handlers;
using Livraria.Application.Interfaces;
using Livraria.Application.Queries;
using Livraria.Domain.Entities;
using Livraria.Domain.Event;
using Livraria.Infrastructure.Consumers;
using Livraria.Infrastructure.Data;
using Livraria.Infrastructure.Messaging;
using Livraria.Infrastructure.Repositories;
using Livraria.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Microsoft.OpenApi.Models;

// Método principal
var builder = WebApplication.CreateBuilder(args);

// Configurar serviços
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configurar middlewares e rotas
ConfigureMiddleware(app);

app.Run();

// Métodos auxiliares

// Método para configurar serviços
static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Configuração do Swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Livraria API", Version = "v1" });
    });

    // Configuração do Entity Framework Core
    services.AddDbContext<LivrariaDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")));

    // Configuração para GUIDs no MongoDB
    BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

    // Configuração do SQL Server com IDbConnection
    services.AddSingleton<IDbConnection>(_ =>
        new SqlConnection(configuration.GetConnectionString("SqlServerConnection")));

    // Configuração do MongoDB
    services.AddSingleton<IMongoClient>(_ =>
        new MongoClient(configuration.GetConnectionString("MongoDbConnection")));
    services.AddSingleton<IMongoDatabase>(provider =>
    {
        var client = provider.GetRequiredService<IMongoClient>();
        return client.GetDatabase(configuration["MongoDbDatabaseName"]);
    });

    // Configuração do Kafka
    services.AddSingleton<IMessageBus>(_ => new KafkaMessageBus(configuration["Kafka:Broker"]));

    // Configuração de repositórios
    services.AddScoped<IRepository<Livro>, LivroRepository>();
    services.AddScoped<LivroLeituraRepository>();
    // services.AddScoped<ILivroCommandHandler, LivroCommandHandler>();
    services.AddScoped<ILivroQueryHandler, LivroQueryHandler>();
    
    // Configuração de consumidores e handlers
    services.AddScoped<IConsumer<LivroCriadoEvent>, LivroCriadoConsumer>();
    services.AddScoped<ObterLivrosQueryHandler>();
    services.AddScoped<CriarLivroHandler>();
    services.AddScoped<AtualizarLivroHandler>();
    services.AddScoped<ExcluirLivroHandler>();
    services.AddScoped<ConsultarLivrosQueryHandler>();
    
    // Configuração de controllers
    services.AddControllers();
}

// Método para configurar middlewares e rotas
static void ConfigureMiddleware(WebApplication app)
{
    // Configuração do Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Livraria API v1");
    });

    // Redirecionar raiz para Swagger
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
            return;
        }
        await next();
    });

    // Mapeamento automático de controllers
    app.MapControllers();

    // Configuração de consumidor Kafka
    ConfigureLivroCriadoConsumer(app);
    ConfigureLivroAtualizadoConsumer(app);
    ConfigureLivroExcluidoConsumer(app);
}

static void ConfigureLivroCriadoConsumer(WebApplication app)
{
    var kafkaConsumer = new KafkaConsumer<LivroCriadoEvent>("livros-criado", async evento =>
    {
        using var scope = app.Services.CreateScope();
        var mongoDatabase = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
        var collection = mongoDatabase.GetCollection<Livro>("Livros");

        try
        {
            var livro = new Livro(evento.Titulo, evento.Autor, evento.Preco, evento.DataPublicacao)
            {
                Id = evento.Id
            };

            await collection.InsertOneAsync(livro);
            Console.WriteLine($"[KafkaCriadoConsumer] Livro inserido no MongoDB: {evento.Titulo}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[KafkaCriadoConsumer] Erro ao salvar no MongoDB: {ex.Message}");
        }
    });

    kafkaConsumer.Start();
}

static void ConfigureLivroAtualizadoConsumer(WebApplication app)
{
    var kafkaConsumer = new KafkaConsumer<LivroAtualizadoEvent>("livros-atualizado", async evento =>
    {
        using var scope = app.Services.CreateScope();
        var mongoDatabase = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
        var collection = mongoDatabase.GetCollection<Livro>("Livros");

        try
        {
            var updateDefinition = Builders<Livro>.Update
                .Set(x => x.Titulo, evento.Titulo)
                .Set(x => x.Autor, evento.Autor)
                .Set(x => x.Preco, evento.Preco)
                .Set(x => x.DataPublicacao, evento.DataPublicacao);

            var task = await collection.UpdateOneAsync(
                filter: Builders<Livro>.Filter.Eq(x => x.Id, evento.Id),
                update: updateDefinition
            );

            Console.WriteLine($"[KafkaAtualizadoConsumer] Livro atualizado no MongoDB: {evento.Titulo}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[KafkaAtualizadoConsumer] Erro ao atualizar no MongoDB: {ex.Message}");
        }
    });

    kafkaConsumer.Start();
}

static void ConfigureLivroExcluidoConsumer(WebApplication app)
{
    var kafkaConsumer = new KafkaConsumer<LivroExcluidoEvent>("livros-excluido", async evento =>
    {
        using var scope = app.Services.CreateScope();
        var mongoDatabase = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
        var collection = mongoDatabase.GetCollection<Livro>("Livros");

        try
        {
            var result = await collection.DeleteOneAsync(
                Builders<Livro>.Filter.Eq(x => x.Id, evento.Id)
            );

            Console.WriteLine($"[KafkaExcluidoConsumer] Livro removido do MongoDB: {evento.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[KafkaExcluidoConsumer] Erro ao remover no MongoDB: {ex.Message}");
        }
    });

    kafkaConsumer.Start();
}