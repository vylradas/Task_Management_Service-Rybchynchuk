# Task Management Service

Невеликий REST API-сервіс для управління завданнями з можливістю змінювати статуси за певними правилами.

## Опис

Проєкт реалізовано на **C# / .NET 8** з використанням **ASP.NET Core Web API**.  
Дані зберігаються в пам’яті (`InMemory`), що дозволяє легко розширювати сховище в майбутньому.  

**Допустимі статуси завдань:**  
- `Backlog` → `InWork` → `Testing` → `Done`  
- Інші переходи заборонені.

Проєкт містить **Unit-тести (xUnit)** для перевірки:
- створення завдання,  
- успішної зміни статусу,  
- недопустимого переходу.

---

## Як запустити

1. Клонувати репозиторій:
```bash
git clone https://github.com/vylradas/Task-Management-Service.git
```

---
## Приклад запиту до API

### 1. Створити нове завдання (POST)

```bash
curl -X POST "https://localhost:7143/api/tasks" \
-H "Content-Type: application/json" \
-d '{
  "title": "Навчити C#",
  "description": "Пройти базовий курс з ASP.NET Core"
}'
```
### 2. Отримати всі завдання (GET)
```bash
curl "https://localhost:7143/api/tasks"
```
### 3. Змінити статус завдання (PATCH)
```bash
curl -X PATCH "https://localhost:7143/api/tasks/{id}/status?newStatus=InWork"
```

