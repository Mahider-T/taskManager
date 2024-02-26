# SimpleTaskManagerDotNet

This repository hosts a basic task manager application implemented in .NET Core and C#. The application is structured as a microservices architecture with two distinct services: Task Manager and Notification Service.

## Features
- **Task Manager Service**:
  - Performs CRUD (Create, Read, Update, Delete) operations for tasks.
  - Utilizes MongoDB for persistent data storage.

- **Notification Service**:
  - Sends email notifications for specific task events.
  - Integrated with RabbitMQ for inter-service communication.

## Technologies Used
- **.NET Core**: Framework for building the microservices.
- **C#**: Primary language for backend development.
- **RabbitMQ**: Message broker facilitating communication between microservices.
- **MongoDB**: NoSQL database for storing task-related information.

## Setup
1. **Prerequisites**:
   - Install .NET Core SDK.
   - Ensure RabbitMQ and MongoDB are installed and running locally.

2. **Clone the Repository**:
   git clone https://github.com/Mahider-T/taskManager
