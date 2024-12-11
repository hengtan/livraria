# **Livraria - Gerenciador de Livros**

Bem-vindo ao repositório **Livraria**, um sistema desenvolvido para gerenciar livros utilizando práticas modernas de desenvolvimento, como **CQRS**, **SOLID**, e **Clean Code**. Este projeto foi construído para demonstrar uma arquitetura robusta, escalável e de fácil manutenção.

---

## **Visão Geral**

O sistema **Livraria** foi projetado utilizando **.NET 6**, com suporte a múltiplos bancos de dados e integração com mensageria através do **Kafka**. A aplicação está estruturada seguindo o padrão **Clean Architecture**, permitindo segregação clara de responsabilidades.

- **Arquitetura**: CQRS (Command Query Responsibility Segregation)
- **Princípios**: SOLID, Clean Code
- **Banco de Dados**:
    - SQL Server (para gravações)
    - MongoDB (para consultas)
- **Mensageria**: Apache Kafka
- **Tecnologias**:
    - .NET 6
    - Entity Framework Core
    - Dapper
    - MongoDB.Driver
    - Confluent.Kafka
- **Testes**:
    - xUnit

---

## **Estrutura do Projeto**

A solução está organizada em múltiplos projetos para manter a modularidade e a clareza.

| Projeto                | Descrição                                                                                           |
|------------------------|---------------------------------------------------------------------------------------------------|
| **Livraria.API**       | Camada responsável pela exposição dos endpoints RESTful.                                          |
| **Livraria.Application** | Contém Handlers, Commands, Queries e casos de uso.                                               |
| **Livraria.Service**   | Contém a lógica de negócios orquestrada e serviços reutilizáveis.                                 |
| **Livraria.Domain**    | Define as entidades e regras de negócios.                                                         |
| **Livraria.Infrastructure** | Implementa acesso a banco de dados, integração com Kafka e outras dependências externas.     |
| **Livraria.Shared**    | Contém interfaces e contratos compartilhados entre camadas.                                       |
| **Livraria.Tests**     | Contém testes unitários e de integração para validar o comportamento da aplicação.                |

---

## **Como Executar a Aplicação**

### **Pré-requisitos**

Certifique-se de que você possui as seguintes ferramentas instaladas no ambiente:

- **.NET SDK 6.0** ou superior
- **Docker** e **Docker Compose**
- **MongoDB**
- **SQL Server**
- **Apache Kafka**

### **Configuração do Ambiente**

1. **Configurar os Bancos de Dados**
    - **MongoDB**: Certifique-se de que o MongoDB esteja rodando na porta **27017**.
    - **SQL Server**:
        - Utilize a imagem oficial do SQL Server no Docker:
          ```bash
          docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
          ```

2. **Configurar o Kafka**
    - Utilize o **Docker Compose** para rodar o Kafka:
      ```yaml
      version: '3.7'
      services:
        zookeeper:
          image: confluentinc/cp-zookeeper
          ports:
            - "2181:2181"
          environment:
            ZOOKEEPER_CLIENT_PORT: 2181
 
        kafka:
          image: confluentinc/cp-kafka
          ports:
            - "9092:9092"
          environment:
            KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
            KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://localhost:9092
            KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1
      ```

    - Suba os serviços:
      ```bash
      docker-compose up -d
      ```

3. **Configurar a Aplicação**
    - Atualize os parâmetros no arquivo **`appsettings.json`**:
      ```json
      {
        "ConnectionStrings": {
          "SqlServer": "Server=localhost,1433;Database=Livraria;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True;",
          "MongoDb": "mongodb://localhost:27017"
        },
        "Kafka": {
          "BootstrapServers": "localhost:9092"
        }
      }
      ```

---

### **Executando a Aplicação**

1. **Build do Projeto**
    - No diretório raiz, execute:
      ```bash
      dotnet build
      ```

2. **Rodar a Aplicação**
    - Inicie o projeto **Livraria.API**:
      ```bash
      dotnet run --project Livraria/Livraria.API/Livraria.API.csproj
      ```

3. **Endpoints**
    - Acesse a documentação Swagger em:
      ```
      http://localhost:5000/swagger
      ```

---

## **Endpoints Disponíveis**

| Método | Endpoint          | Descrição                                      |
|--------|-------------------|-----------------------------------------------|
| GET    | `/api/livro`      | Lista todos os livros.                        |
| GET    | `/api/livro/{id}` | Busca um livro pelo ID.                       |
| POST   | `/api/livro`      | Cria um novo livro.                           |
| PUT    | `/api/livro/{id}` | Atualiza um livro existente.                  |
| DELETE | `/api/livro/{id}` | Exclui um livro.                              |

---

## **Conceitos Técnicos**

### **CQRS**
- A aplicação utiliza **CQRS (Command Query Responsibility Segregation)** para separar operações de escrita e leitura.
    - **Escrita**: Utiliza SQL Server para persistir dados transacionais.
    - **Leitura**: Utiliza MongoDB para consultas rápidas e escaláveis.

### **Clean Code e SOLID**
- Seguindo os princípios **SOLID**, o código foi estruturado para ser extensível, testável e de fácil manutenção.

### **Mensageria com Kafka**
- A aplicação utiliza o **Apache Kafka** para comunicação assíncrona entre serviços, permitindo maior resiliência e escalabilidade.

---

## **Testes**

1. **Executando Testes**
    - Para rodar os testes, execute:
      ```bash
      dotnet test
      ```

2. **Estratégia de Testes**
    - **Testes Unitários**: Validam funcionalidades isoladas.
    - **Testes de Integração**: Garantem que os módulos funcionam juntos corretamente.

---

## **Contribuição**

1. Crie um branch para sua feature:
   ```bash
   git checkout -b minha-feature