# todolist

## ASP.NET Core + React + Unit Test Solution

This repository contains a solution with:
- ASP.NET Core Web API (Backend)
- React Frontend (in a separate project folder)
- xUnit Unit Tests

## Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/mhanifazmi/todolist.git
   cd todolist

## Backend Setup (.NET API)

1. **Navigate to the backend folder:**
    ```bash
    cd ToDoList.Server

2. **Restore Dependencies**
    ```bash
    dotnet restore

3. **Copy appsettings:**
    ```bash
    cp appsettings.example.json appsettings.json

4. **Apply Migrations**
    ```bash
    dotnet ef database update

5. **Run the Application and access the application**
    ```bash
    API: https://localhost:7289/swagger/index.html

## Frontend Setup (React)

1. **Navigate to the backend folder:**
    ```bash
    cd todolist.client

2. Copy environment
    ```bash
    cp .env.example .env

3. **Install Dependencies**
    ```bash
    npm install

4. **Start the development server:**
    ```bash 
    npm run dev

5. **Run the Application and access the application**
    ```bash
    API: https://localhost:5173/


