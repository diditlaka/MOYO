CREATE TABLE Client (
    ClientId     INT IDENTITY(1,1) PRIMARY KEY,
    Name         NVARCHAR(100) NOT NULL,
    Email        NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedAt    DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE Product (
    ProductId   INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500),
    Category    NVARCHAR(100),
    Price       DECIMAL(10,2) NOT NULL,
    IsAvailable BIT DEFAULT 1
);

CREATE TABLE [Order] (
    OrderId   INT IDENTITY(1,1) PRIMARY KEY,
    ClientId  INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity  INT NOT NULL DEFAULT 1,
    Status    NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    OrderDate DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Order_Client  FOREIGN KEY (ClientId)  REFERENCES Client(ClientId),
    CONSTRAINT FK_Order_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);
