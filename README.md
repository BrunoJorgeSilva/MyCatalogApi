Projeto Catalog API
Requisitos para rodar o projeto
.NET 9.0: Certifique-se de ter a versão mais recente do .NET 9.0 instalada em sua máquina. Baixe o .NET 9.0 aqui.


Docker: Necessário para configurar o banco de dados PostgreSQL com container. Baixe e instale o Docker Desktop.


Configuração do Banco de Dados


Utilizando Docker


Caso tenha o Docker instalado, execute o seguinte comando para criar um container PostgreSQL:

bash
docker run --name PostgresContainerCatalog -e POSTGRES_PASSWORD=CatalogPassWord -p 5432:5432 -d postgres


Depois que o container estiver em execução, crie o banco de dados executando o seguinte script no seu cliente PostgreSQL favorito:

sql
-- Criar o banco de dados CatalogDbPostgres
CREATE DATABASE "CatalogDbPostgres"
WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.utf8'
    LC_CTYPE = 'en_US.utf8'
    LOCALE_PROVIDER = 'libc'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

Caso não utilize Docker


Se preferir utilizar sua própria instância local de PostgreSQL, altere as connection strings no arquivo de configuração da aplicação (appsettings.json) para apontar para seu banco local.


Exemplo de connection string padrão:


json

"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=CatalogDbPostgres;Username=postgres;Password=CatalogPassWord"
}


Criação das tabelas e inserção de dados


Depois de criar o banco de dados, execute o script SQL para criar as tabelas e os relacionamentos necessários:

sql
-- Script para criar as tabelas Pokemon, User, Filme, UserPokemon, UserFilme
CREATE TABLE IF NOT EXISTS Pokemon (
    Id SERIAL PRIMARY KEY,
    IdExternoPokemon INT UNIQUE NOT NULL,
    Name VARCHAR(400) NOT NULL,
    HP INT NOT NULL,
    Ataque INT NOT NULL,
    Defesa INT NOT NULL,
    AtaqueEspecial INT NOT NULL,
    DefesaEspecial INT NOT NULL,
    Velocidade INT NOT NULL
);



CREATE TABLE IF NOT EXISTS "User" (
    UserId SERIAL PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Apelido VARCHAR(100) NOT NULL,
    CONSTRAINT unique_nome_apelido UNIQUE (Nome, Apelido)
);



CREATE TABLE IF NOT EXISTS UserPokemon (
    PokemonUserId SERIAL PRIMARY KEY,
    IdExternoPokemon INT NOT NULL,
    UserId INT NOT NULL,
    CONSTRAINT fk_pokemon FOREIGN KEY (IdExternoPokemon) REFERENCES Pokemon(IdExternoPokemon) ON DELETE CASCADE,
    CONSTRAINT fk_user FOREIGN KEY (UserId) REFERENCES "User"(UserId) ON DELETE CASCADE,
    CONSTRAINT unique_pokemon_user UNIQUE (IdExternoPokemon, UserId)
);



CREATE TABLE IF NOT EXISTS Filme (
    FilmeId SERIAL PRIMARY KEY,
    Titulo VARCHAR(255) NOT NULL,
    Diretor VARCHAR(255),
    TempoDeFilme INT,
    Ano INT,
    AtoresPrincipais VARCHAR(700),
    Genero VARCHAR(400),
    ClassificacaoIndicativa VARCHAR(100),
    OrcamentoProducaoMilhao INT,
    BilheteriaMundialMilhao DECIMAL(10, 2),
    Sinopse VARCHAR(2000),
    PremiosEIndicacoes VARCHAR(1000),
    Roteiristas VARCHAR(500),
    CronologiaNoUniverso VARCHAR(10),
    CameosOuParticipacoes VARCHAR(255),
    RecepcaoDaCritica VARCHAR(255),
    CuriosidadeDeProducao VARCHAR(1000),
    TrilhaSonora VARCHAR(255),
    EstudioProducao VARCHAR(400),
    CONSTRAINT unique_titulo UNIQUE (Titulo)
);



CREATE TABLE IF NOT EXISTS UserFilme (
    UserFilmeId SERIAL PRIMARY KEY,
    UserId INT NOT NULL,
    FilmeId INT NOT NULL,
    CONSTRAINT fk_user FOREIGN KEY (UserId) REFERENCES "User"(UserId) ON DELETE CASCADE,
    CONSTRAINT fk_filme FOREIGN KEY (FilmeId) REFERENCES Filme(FilmeId) ON DELETE CASCADE,
    CONSTRAINT unique_user_filme UNIQUE (UserId, FilmeId)
);


Popular dados iniciais (Pokemons)


Depois de criar as tabelas, execute o script para inserir dados iniciais:

sql


INSERT INTO Pokemon (IdExternoPokemon, Name, HP, Ataque, Defesa, AtaqueEspecial, DefesaEspecial, Velocidade)
VALUES
    (1, 'Pikachu', 35, 55, 40, 50, 50, 90),
    (2, 'Bulbasaur', 45, 49, 49, 65, 65, 45),
    (3, 'Charmander', 39, 52, 43, 60, 50, 65),
    (4, 'Squirtle', 44, 48, 65, 50, 64, 43),
    (5, 'Jigglypuff', 115, 45, 20, 45, 25, 20),
    (6, 'Meowth', 40, 45, 35, 40, 40, 90);



Executando a aplicação


Com o banco de dados configurado e as tabelas criadas:


Compile e execute o projeto usando o Visual Studio ou via linha de comando:

bash

dotnet run

A aplicação estará disponível em http://localhost:5000.

Ou utilize o swagger, utilizando o Visual Studio 2022 selecionando a opção do ISS Express, sua aplicação estará disponível em: https://localhost:44378/swagger/index.html
