# 🚀 Bike Rental API

Sistema de aluguel de motos desenvolvido em .NET 8 com MongoDB e RabbitMQ, como parte de um desafio técnico.

## 📚 Descrição

Esta API gerencia o cadastro de motos, usuários entregadores e aluguéis. Ela também implementa um sistema de mensageria com RabbitMQ para registrar logs da criação de motos fabricadas em 2024.

## 🛠️ Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/en-us/)
- [MongoDB](https://www.mongodb.com/)
- [RabbitMQ](https://www.rabbitmq.com/)
- [Swagger](https://swagger.io/)
- [Docker](https://www.docker.com/)

## 📦 Funcionalidades

- Cadastro de motos
- Cadastro de usuários entregadores
- Aluguel de motos por entregadores
- Publicação de eventos no RabbitMQ ao cadastrar uma nova moto
- Criação automática de logs em um banco de dados MongoDB para motos fabricadas em 2024

## 📁 Estrutura do Projeto
```
challange_bikeRental/
│
├── Models/                # Modelos do domínio
├── Services/              # Camadas de serviço (regras de negócio)
├── Repositories/          # Repositórios de acesso ao MongoDB
├── Utils/                 # Utilitários como RabbitMQ
├── Config/                # Configurações de MongoDB e RabbitMQ
├── Program.cs             # Entrada principal da aplicação
└── Dockerfile / docker-compose.yml
```


## 🔁 Fluxo com RabbitMQ

1. Ao cadastrar uma nova moto, um evento é publicado na fila `bike-queue`.
2. Um serviço hospedado (`BackgroundService`) consome esta fila.
3. Se o ano de fabricação for 2024, um log é salvo na coleção `logsMotocycleCreated` do MongoDB.

## ▶️ Como Rodar Localmente

### Pré-requisitos

- Docker e Docker Compose
- .NET 8 SDK

### 1. Clonar o repositório

```bash
git clone https://github.com/Habynner/MotocycleRental.git
cd challenge-bike-rental
```
### 2. Criar container no docker para MongoDB e Rabbit
```bash
docker-compose up -d
```
### 3. Rodar o projeto
```bash
dotnet run
```

## Acessar a documentação Swagger
- [Bike Reantal Swagger](https://localhost:5026/swagger)
  
## 🧪 Testes
Atualmente não há testes implementados, mas o projeto está estruturado para fácil adição.

---

## 👤 Autor

**Habynner Silva**  
Desenvolvedor Fullstack com expertise em .NET, NestJS, MongoDB, RabbitMQ e microsserviços.  

[LinkedIn](linkedin.com/in/habynner-silva-developer)


