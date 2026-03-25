namespace SalesSystem.Shared.Database.Database.Dtos;

//public record SalesLogDto(int employeeiD, 
//    string employeeName, 
//    int productId, 
//    string productName, 
//    int quantity, 
//    double totalPrice
//    );

public record SalesLogDto(int employeeId, 
    int productId, 
    int quantity
    );