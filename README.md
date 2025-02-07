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



INSERT INTO Pokemon (IdExternoPokemon, name, HP, Ataque, Defesa, AtaqueEspecial, DefesaEspecial, Velocidade)
VALUES
    (1, 'Pikachu', 35, 55, 40, 50, 50, 90),
    (2, 'Bulbasaur', 45, 49, 49, 65, 65, 45),
    (3, 'Charmander', 39, 52, 43, 60, 50, 65),
    (4, 'Squirtle', 44, 48, 65, 50, 64, 43),
    (5, 'Jigglypuff', 115, 45, 20, 45, 25, 20),
    (6, 'Meowth', 40, 45, 35, 40, 40, 90),
    (7, 'Psyduck', 50, 52, 48, 65, 50, 55),
    (8, 'Machop', 70, 80, 50, 35, 35, 35),
    (9, 'Geodude', 40, 80, 100, 30, 30, 20),
    (10, 'Magnemite', 25, 35, 70, 95, 55, 45),
    (11, 'Gastly', 30, 35, 30, 100, 35, 80),
    (12, 'Onix', 35, 45, 160, 30, 45, 70),
    (13, 'Hitmonlee', 50, 120, 53, 35, 110, 87),
    (14, 'Hitmonchan', 50, 105, 79, 35, 110, 76),
    (15, 'Rhyhorn', 80, 85, 95, 30, 30, 25),
    (16, 'Chansey', 250, 5, 5, 35, 105, 50),
    (17, 'Tangela', 65, 55, 115, 100, 40, 60),
    (18, 'Kangaskhan', 105, 95, 80, 40, 80, 90),
    (19, 'Horsea', 30, 40, 70, 70, 25, 60),
    (20, 'Goldeen', 45, 67, 60, 35, 50, 63),
    (21, 'Mr. Mime', 40, 45, 65, 100, 120, 90),
    (22, 'Scyther', 70, 110, 80, 55, 80, 105),
    (23, 'Jynx', 65, 50, 35, 115, 95, 95),
    (24, 'Electabuzz', 65, 83, 57, 95, 85, 105),
    (25, 'Magmar', 65, 95, 57, 100, 85, 93),
    (26, 'Pinsir', 65, 125, 100, 55, 70, 85),
    (27, 'Tauros', 75, 100, 95, 40, 70, 110),
    (28, 'Magikarp', 20, 10, 55, 15, 20, 80),
    (29, 'Gyarados', 95, 125, 79, 60, 100, 81),
    (30, 'Lapras', 130, 85, 80, 85, 95, 60),
    (31, 'Ditto', 48, 48, 48, 48, 48, 48),
    (32, 'Eevee', 55, 55, 50, 45, 65, 55),
    (33, 'Vaporeon', 130, 65, 60, 110, 95, 65),
    (34, 'Jolteon', 65, 65, 60, 110, 95, 130),
    (35, 'Flareon', 65, 130, 60, 95, 110, 65),
    (36, 'Porygon', 65, 60, 70, 85, 75, 40),
    (37, 'Omanyte', 35, 40, 100, 90, 55, 35),
    (38, 'Omastar', 70, 60, 125, 115, 70, 55),
    (39, 'Kabuto', 30, 80, 90, 55, 45, 55),
    (40, 'Kabutops', 60, 115, 105, 65, 70, 80),
    (41, 'Aerodactyl', 80, 105, 65, 60, 75, 130),
    (42, 'Snorlax', 160, 110, 65, 65, 110, 30),
    (43, 'Articuno', 90, 85, 100, 95, 125, 85),
    (44, 'Zapdos', 90, 90, 85, 125, 90, 100),
    (45, 'Moltres', 90, 100, 90, 125, 85, 90),
    (46, 'Dratini', 41, 64, 45, 50, 50, 50),
    (47, 'Dragonair', 61, 84, 65, 70, 70, 70),
    (48, 'Dragonite', 91, 134, 95, 100, 100, 80),
    (49, 'Mewtwo', 106, 110, 90, 154, 90, 130),
    (50, 'Mew', 100, 100, 100, 100, 100, 100),
    (51, 'Chikorita', 45, 49, 65, 49, 65, 45),
    (52, 'Bayleef', 60, 62, 80, 63, 80, 60),
    (53, 'Meganium', 80, 82, 100, 83, 100, 80),
    (54, 'Cyndaquil', 39, 52, 43, 60, 50, 65),
    (55, 'Quilava', 58, 64, 58, 80, 65, 80),
    (56, 'Typhlosion', 78, 84, 78, 109, 85, 100),
    (57, 'Totodile', 50, 65, 64, 44, 48, 43),
    (58, 'Croconaw', 65, 80, 80, 59, 63, 58),
    (59, 'Feraligatr', 85, 105, 100, 79, 83, 78),
    (60, 'Sentret', 35, 46, 34, 35, 45, 20),
    (61, 'Furret', 85, 76, 64, 45, 55, 90),
    (62, 'Hoothoot', 60, 30, 30, 36, 56, 50),
    (63, 'Noctowl', 100, 50, 50, 76, 96, 70),
    (64, 'Ledyba', 40, 20, 30, 40, 80, 55),
    (65, 'Ledian', 55, 35, 50, 55, 110, 85),
    (66, 'Spinarak', 40, 60, 40, 40, 40, 30),
    (67, 'Ariados', 70, 90, 70, 60, 60, 40),
    (68, 'Crobat', 85, 90, 80, 70, 80, 130),
    (69, 'Chinchou', 75, 38, 38, 56, 56, 67),
    (70, 'Lanturn', 125, 58, 58, 76, 76, 67),
    (71, 'Pichu', 20, 40, 15, 35, 35, 60),
    (72, 'Cleffa', 50, 25, 28, 45, 55, 15),
    (73, 'Igglybuff', 90, 30, 15, 40, 20, 15),
    (74, 'Togepi', 35, 20, 65, 40, 65, 20),
    (75, 'Togetic', 55, 40, 85, 80, 105, 40),
    (76, 'Natu', 40, 50, 45, 70, 45, 70),
    (77, 'Xatu', 65, 75, 70, 95, 70, 95),
    (78, 'Mareep', 55, 40, 40, 65, 45, 35),
    (79, 'Flaaffy', 70, 55, 55, 80, 60, 45),
    (80, 'Ampharos', 90, 75, 85, 115, 90, 55),
    (81, 'Bellossom', 75, 60, 95, 90, 100, 50),
    (82, 'Sunkern', 30, 30, 30, 30, 30, 30),
    (83, 'Hoppip', 35, 35, 40, 35, 55, 50),
    (84, 'Skiploom', 55, 45, 50, 45, 65, 60),
    (85, 'Jumpluff', 75, 55, 70, 55, 85, 110),
    (86, 'Gulpin', 70, 43, 53, 43, 53, 40),
    (87, 'Swalot', 100, 73, 83, 73, 83, 55),
    (88, 'Zubat', 40, 45, 35, 30, 40, 60),
    (89, 'Golbat', 75, 80, 70, 65, 75, 90),
    (90, 'Croagunk', 48, 61, 40, 61, 40, 50),
    (91, 'Toxicroak', 83, 106, 65, 86, 65, 85),
    (92, 'Slakoth', 60, 60, 60, 35, 35, 30),
    (93, 'Vigoroth', 80, 80, 80, 55, 55, 90),
    (94, 'Slaking', 150, 160, 100, 95, 65, 100),
	(95, 'Beldum', 40, 55, 80, 35, 60, 30),
    (96, 'Metang', 60, 75, 100, 55, 80, 50),
    (97, 'Metagross', 80, 135, 130, 95, 90, 70),
    (98, 'Registeel', 80, 75, 150, 75, 150, 50),
    (99, 'Regice', 80, 50, 100, 100, 150, 50),
    (100, 'Regirock', 80, 100, 200, 50, 100, 50);





Executando a aplicação


Com o banco de dados configurado e as tabelas criadas:


Compile e execute o projeto usando o Visual Studio ou via linha de comando:

bash

dotnet run

A aplicação estará disponível em http://localhost:5000.

Ou utilize o swagger, utilizando o Visual Studio 2022 selecionando a opção do ISS Express, sua aplicação estará disponível em: https://localhost:44378/swagger/index.html
