using System;
using System.Linq;
using AgendamentoAPI.Interfaces;
using AgendamentoAPI.Models;
using AgendamentoAPI.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AgendamentoAPI.Tests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private readonly string _dbName = Guid.NewGuid().ToString();

        public Guid ClienteId { get; } = Guid.NewGuid();
        public Guid ProfissionalId { get; } = Guid.NewGuid();
        public Guid ServicoId { get; } = Guid.NewGuid();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove todos os registros de DbContext, DbContextOptions e o próprio ServiceProvider do EF
                var descriptors = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<AgendamentoDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType == typeof(AgendamentoDbContext)).ToList();

                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                // Cria um Provedor de Serviços Interno isolado para o EF InMemory
                var internalServiceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Configura o DbContext utilizando o banco InMemory isolado
                services.AddDbContext<AgendamentoDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName)
                           .UseInternalServiceProvider(internalServiceProvider);
                });

                // Re-registra os Repositórios para o container de testes
                services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
                services.AddScoped<IClienteRepository, ClienteRepository>();
                services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
                services.AddScoped<IServicoRepository, ServicoRepository>();

                // Alimenta os dados iniciais de teste (Seeding)
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AgendamentoDbContext>();

                db.Database.EnsureCreated();

                if (!db.Clientes.Any(c => c.IdCliente == ClienteId))
                {
                    db.Clientes.Add(new Cliente
                    {
                        IdCliente = ClienteId,
                        Nome = "Cliente Teste",
                        Email = "cliente@teste.com",
                        Senha = "123",
                        Telefone = "11999999999"
                    });
                }

                if (!db.Profissionais.Any(p => p.IdProfissional == ProfissionalId))
                {
                    db.Profissionais.Add(new Profissional
                    {
                        IdProfissional = ProfissionalId,
                        Nome = "Profissional Teste",
                        Email = "pro@teste.com",
                        Senha = "123",
                        Telefone = "11988888888"
                    });
                }

                if (!db.Servicos.Any(s => s.IdServico == ServicoId))
                {
                    db.Servicos.Add(new Servico
                    {
                        IdServico = ServicoId,
                        Nome = "Corte de Cabelo",
                        Preco = 50.00m
                    });
                }

                db.SaveChanges();
            });
        }
    }
}