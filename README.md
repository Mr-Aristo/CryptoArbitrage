# CryptoArbitrage
# Arbitrage Microservices Project

## Overview
This project is a microservices-based application designed to perform synthetic arbitrage calculations using Binance futures price data. It consists of two main services that communicate via RabbitMQ and store their data in PostgreSQL. The system follows the principles of Clean Architecture and utilizes various modern technologies for performance, scalability, and maintainability.

## Technologies Used
### Core Technologies
- **.NET 9** - The latest version of .NET for building modern web APIs.
- **C#** - The primary language used for development.
- **Minimal API with Carter** - Simplified API endpoints using the ICarterModule.
- **Microservices Architecture** - The application is split into multiple services for better scalability and maintainability.
- **RabbitMQ** - Used as a message broker for communication between services.
- **PostgreSQL** - Relational database used for storing arbitrage calculations and price data.
- **FluentValidation** - Used for request validation in services.
- **Serilog** - Logging framework for structured logging.
- **Quartz.NET** - Used for background job processing.
- **Docker & Docker Compose** - Containerized deployment and service orchestration.
- **Traefik** - Reverse proxy and API Gateway for routing requests.
- **Grafana** - Monitoring and visualization of system metrics.

### Patterns & Principles
- **Clean Architecture** - Separation of concerns using application, domain, and infrastructure layers.
- **SOLID Principles** - Ensures maintainability and scalability.
- **Repository Pattern** - Abstracts database operations.
- **Dependency Injection** - For better testability and modularity.

## Project Structure
```
📦 ArbitrageMicroservices
 ┣ 📂 ArbitrageService
 ┃ ┣ 📂 Application
 ┃ ┣ 📂 Domain
 ┃ ┣ 📂 Infrastructure
 ┃ ┣ 📂 API
 ┣ 📂 PriceDataService
 ┃ ┣ 📂 Application
 ┃ ┣ 📂 Domain
 ┃ ┣ 📂 Infrastructure
 ┃ ┣ 📂 API
 ┣ 📂 Shared
 ┃ ┣ 📂 Messaging
 ┃ ┣ 📂 Exceptions
 ┣ 📂 Monitoring (Traefik, Grafana, Prometheus)
 ┣ 📜 docker-compose.yml
 ┣ 📜 README.md
```

## Services
### 1. **PriceDataService**
- Fetches futures price data from Binance API.
- Stores price data in PostgreSQL.
- Publishes price data to RabbitMQ for other services.

### 2. **ArbitrageService**
- Listens to price updates from RabbitMQ.
- Performs arbitrage calculations based on received prices.
- Stores arbitrage calculation results in PostgreSQL.

## Message Broker Layer
A separate **RabbitMQ Service** is introduced to manage the messaging logic, ensuring that PriceDataService and ArbitrageService do not have direct dependencies on RabbitMQ.

## Configurations
- The services read their configurations from `appsettings.json`, including RabbitMQ and database credentials.
- RabbitMQ configurations are stored separately in each microservice’s `appsettings.json` file.

## Running the Project
### Prerequisites
- **Docker & Docker Compose** installed.
- **.NET 9 SDK** installed.

### Steps to Run
1. Clone the repository.
2. Navigate to the project folder.
3. Run the following command to start all services:
   ```sh
   docker-compose up --build
   ```

## Future Enhancements
- Implement caching with Redis.
- Add unit and integration tests.
- Introduce API rate limiting.
- Deploy to cloud services with Kubernetes.
