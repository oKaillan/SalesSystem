# SalesSystem
SalesSystem simulates an employee’s sales point of view when a sale is made manually. It includes CRUD operations for Employees, Products, and Sales.

The first version was a Console App. Later, I created an API for it, and the final version with a Web interface is under development. You can check the previous versions in their respective branches.

# SalesSystem.API
This API is protected using Microsoft Identity, you can login as an Employee or Admin.

The employee’s password in the main database is protected using the **bcrypt** hashing method. When an employee is created through this API, the record is created simultaneously in both the main database and the Identity database, allowing the employee to log in to the API.

### Created with .NET EF Core 8
<img width="1339" height="811" alt="Screenshot_2" src="https://github.com/user-attachments/assets/00c16fac-e796-4af8-9778-047c4ea91847" />
