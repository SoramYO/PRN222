create database MilkTeaShop


CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(200),
	ImageUrl VARCHAR(MAX),
	Status BIT DEFAULT 1
);

-- Tạo bảng Products (Sản phẩm trà sữa có sẵn)
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    CategoryId INT FOREIGN KEY REFERENCES Categories(CategoryId),
    Description NVARCHAR(500),
    ImageURL VARCHAR(255),
    Status BIT DEFAULT 1  -- 1: còn hàng, 0: hết hàng
);

CREATE TABLE ProductVariants (
    VariantId INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId),
    Size NVARCHAR(20),  -- S, M, L
    Price DECIMAL(10,2),
    Status BIT DEFAULT 1
);

-- Đổi tên từ Toppings thành ExtraProducts
CREATE TABLE ExtraProducts (
    ExtraProductId INT PRIMARY KEY IDENTITY(1,1),
    ExtraProductName NVARCHAR(50) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    ImageURL VARCHAR(255),
    Status BIT DEFAULT 1  -- 1: còn hàng, 0: hết hàng
);

-- Tạo bảng Customers (Khách hàng)
-- Nên đổi thành
CREATE TABLE Accounts (
    AccountId INT PRIMARY KEY IDENTITY(1,1), 
    Username NVARCHAR(50) NOT NULL,           
    Password NVARCHAR(255) NOT NULL,         
    Name NVARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(15),
    Email VARCHAR(100),
    Role NVARCHAR(20) DEFAULT 'Customer',    
    JoinDate DATETIME DEFAULT GETDATE()
);

-- Tạo bảng PaymentMethods (Phương thức thanh toán)
CREATE TABLE PaymentMethods (
    PaymentMethodId INT PRIMARY KEY IDENTITY(1,1),
    MethodName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(200),
    Status BIT DEFAULT 1  -- 1: đang hoạt động, 0: ngưng hoạt động
);

-- Tạo bảng Orders (Đơn hàng)
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT FOREIGN KEY REFERENCES Accounts(AccountId),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(10,2),
    Status NVARCHAR(20) DEFAULT N'Đang xử lý',  -- Đang xử lý, Hoàn thành, Hủy
    Note NVARCHAR(500)
);

-- Tạo bảng Payments (Thanh toán)
CREATE TABLE Payments (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId),
    PaymentMethodId INT FOREIGN KEY REFERENCES PaymentMethods(PaymentMethodId),
    PaymentDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT N'Chờ thanh toán',
    Note NVARCHAR(500)
);

-- Tạo bảng OrderDetails (Chi tiết đơn hàng)
CREATE TABLE OrderDetails (
    OrderDetailId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId),
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
);

-- Đổi tên từ OrderToppings thành OrderExtraProducts
CREATE TABLE OrderExtraProducts (
    OrderDetailId INT FOREIGN KEY REFERENCES OrderDetails(OrderDetailId),
    ExtraProductId INT FOREIGN KEY REFERENCES ExtraProducts(ExtraProductId),
    Quantity INT NOT NULL,
    PRIMARY KEY (OrderDetailId, ExtraProductId)
);

CREATE TABLE ProductExtras (
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId),
    ExtraProductId INT FOREIGN KEY REFERENCES ExtraProducts(ExtraProductId),
    PRIMARY KEY (ProductId, ExtraProductId)
);


-- Chèn dữ liệu mẫu cho Categories
INSERT INTO Categories (CategoryName, Description) VALUES
(N'Trà sữa truyền thống', N'Các loại trà sữa cổ điển'),
(N'Trà sữa đặc biệt', N'Các loại trà sữa signature của quán'),
(N'Trà trái cây', N'Các loại trà kết hợp với trái cây tươi'),
(N'Sữa tươi', N'Các loại đồ uống từ sữa tươi'),
(N'Đá xay', N'Các loại đồ uống đá xay');

INSERT INTO Products (ProductName, CategoryId, Description, Status) VALUES
-- Trà sữa truyền thống (CategoryID = 1)
(N'Trà sữa trân châu đường đen', 1, N'Trà sữa truyền thống kết hợp với trân châu và đường đen', 1),
(N'Trà sữa Oolong', 1, N'Trà sữa với lá trà Oolong thơm ngon', 1),
(N'Trà sữa Earl Grey', 1, N'Trà sữa với hương bergamot đặc trưng', 1),
(N'Trà sữa Matcha', 1, N'Trà sữa từ bột trà xanh Nhật Bản', 1),

-- Trà sữa đặc biệt (CategoryID = 2)
(N'Brown Sugar Pearl Milk Tea', 2, 45000, N'Sữa tươi với trân châu đường nâu', 1),
(N'Oreo Milk Tea', 2, 42000, N'Trà sữa kết hợp với bánh Oreo', 1),
(N'Taro Milk Tea', 2, 40000, N'Trà sữa khoai môn đặc biệt', 1),
(N'Tiger Sugar Milk Tea', 2, 45000, N'Sữa tươi sọc hổ với đường nâu', 1),

-- Trà trái cây (CategoryID = 3)
(N'Trà đào', 3, 35000, N'Trà với đào tươi và syrup đào', 1),
(N'Trà vải', 3, 35000, N'Trà với vải tươi', 1),
(N'Trà dâu tây', 3, 38000, N'Trà với dâu tây tươi', 1),
(N'Trà chanh dây', 3, 35000, N'Trà với chanh dây tươi', 1),

-- Sữa tươi (CategoryID = 4)
(N'Sữa tươi trân châu đường đen', 4, 40000, N'Sữa tươi với trân châu và đường đen', 1),
(N'Sữa tươi matcha', 4, 38000, N'Sữa tươi kết hợp với bột trà xanh', 1),
(N'Sữa tươi chocolate', 4, 38000, N'Sữa tươi với socola nguyên chất', 1),

-- Đá xay (CategoryID = 5)
(N'Matcha đá xay', 5, 45000, N'Matcha đá xay với kem tươi', 1),
(N'Oreo đá xay', 5, 45000, N'Oreo đá xay với kem tươi', 1),
(N'Chocolate đá xay', 5, 45000, N'Chocolate đá xay với kem tươi', 1);




-- Chèn dữ liệu mẫu cho PaymentMethods
INSERT INTO PaymentMethods (MethodName, Description) VALUES
(N'Tiền mặt', N'Thanh toán bằng tiền mặt tại quầy'),
(N'Momo', N'Thanh toán qua ví điện tử Momo'),
(N'VNPay', N'Thanh toán qua VNPay'),
(N'Chuyển khoản ngân hàng', N'Thanh toán qua chuyển khoản ngân hàng');

-- Chèn dữ liệu mẫu cho ExtraProducts
INSERT INTO ExtraProducts (ExtraProductName, Price, Status) VALUES
(N'Trân châu đen', 5000,  1),
(N'Trân châu trắng', 5000, 1),
(N'Thạch rau câu', 7000,  1),
(N'Pudding', 8000,  1),
(N'Kem sữa', 10000, 1),
(N'Thạch nha đam', 7000, 1),
(N'Đậu đỏ', 8000, 1),
(N'Whipping Cream', 10000,  1),
(N'Trân châu đường đen', 10000, 1),
(N'Foam Cheese', 10000, 1);
