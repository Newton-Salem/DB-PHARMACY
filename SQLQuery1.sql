CREATE DATABASE [PROJECT 1];

CREATE TABLE [USER](
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    USERNAME VARCHAR(50),
    [PASSWORD] VARCHAR(100),
    [NAME] VARCHAR(100),
    [ROLE] VARCHAR(20),
    Email VARCHAR(100),
    Phone VARCHAR(20) UNIQUE,
    [Address] VARCHAR(200)
);

CREATE TABLE [ADMIN](
    UserID INT PRIMARY KEY,
    Salary DECIMAL(10,2),
    HireDate DATE,
    FOREIGN KEY (UserID) REFERENCES [USER](UserID)
);

CREATE TABLE Pharmacist(
    UserID INT PRIMARY KEY,
    Branch VARCHAR(100),
    License_NUMBER INT UNIQUE,
    HireDate DATE,
    FOREIGN KEY (UserID) REFERENCES [USER](UserID)
);

CREATE TABLE Supplier(
    UserID INT PRIMARY KEY,
    Company_Name VARCHAR(50),
    FOREIGN KEY (UserID) REFERENCES [USER](UserID)
);

CREATE TABLE Customer(
    UserID INT PRIMARY KEY,
    LoyaltyPoints INT,
    Gender VARCHAR(10),
    EmergencyPhone VARCHAR(20),
    FOREIGN KEY (UserID) REFERENCES [USER](UserID)
);

CREATE TABLE Category(
    Category_ID INT IDENTITY(1,1) PRIMARY KEY,
    Category_Name VARCHAR(100),
    [Description] VARCHAR(400)
);

CREATE TABLE Medicine(
    Medicine_ID INT IDENTITY(1,1) PRIMARY KEY,
    Stock_Quantity INT,
    [Expiry_Date] DATE,
    Price DECIMAL(10,2),
    [Name] VARCHAR(100)
);

CREATE TABLE MEDICINE_CATEGORY(
    Medicine_ID INT,
    Category_ID INT,
    PRIMARY KEY (Medicine_ID, Category_ID),
    FOREIGN KEY (Medicine_ID) REFERENCES Medicine(Medicine_ID),
    FOREIGN KEY (Category_ID) REFERENCES Category(Category_ID)
);

CREATE TABLE Supplier_Request(
    Request_id INT IDENTITY(1,1) PRIMARY KEY,
    Request_Date DATE,
    [Status] VARCHAR(30),
    Quantity INT,
    SupplierID INT,
    PharmacistID INT,
    FOREIGN KEY (SupplierID) REFERENCES Supplier(UserID),
    FOREIGN KEY (PharmacistID) REFERENCES Pharmacist(UserID)
);

CREATE TABLE SUPPLIER_REQUEST_MEDICINE(
    Request_ID INT,
    Medicine_ID INT,
    PRIMARY KEY (Request_ID, Medicine_ID),
    FOREIGN KEY (Request_ID) REFERENCES Supplier_Request(Request_id),
    FOREIGN KEY (Medicine_ID) REFERENCES Medicine(Medicine_ID)
);

CREATE TABLE supplied_by(
    SupplierID INT,
    Medicine_ID INT,
    PRIMARY KEY (SupplierID, Medicine_ID),
    FOREIGN KEY (SupplierID) REFERENCES Supplier(UserID),
    FOREIGN KEY (Medicine_ID) REFERENCES Medicine(Medicine_ID)
);

CREATE TABLE [Order](
    Order_ID INT IDENTITY(1,1) PRIMARY KEY,
    discount DECIMAL(5,2),
    [Status] VARCHAR(30),
    ORDER_Date DATE,
    Total_Amount DECIMAL(10,2),
    CustomerID INT,
    PharmacistID INT,

    FOREIGN KEY (CustomerID) REFERENCES Customer(UserID),
    FOREIGN KEY (PharmacistID) REFERENCES Pharmacist(UserID)
);

CREATE TABLE Order_Medicine (
    Order_ID INT,
    Medicine_ID INT,
    Quantity INT,

    PRIMARY KEY (Order_ID, Medicine_ID),
    FOREIGN KEY (Order_ID) REFERENCES [Order](Order_ID),
    FOREIGN KEY (Medicine_ID) REFERENCES Medicine(Medicine_ID)
);

CREATE TABLE [Payment](
    PaymentID INT IDENTITY(1,1) PRIMARY KEY,
    Payment_Method VARCHAR(50),
    Payment_Date DATE,
    Amount DECIMAL(10,2),
    [Status] VARCHAR(20),
    Order_ID INT,

    FOREIGN KEY (Order_ID) REFERENCES [Order](Order_ID)
);



CREATE TABLE [NOTIFICATION](
    Notification_ID INT IDENTITY(1,1) PRIMARY KEY,
    [Status] VARCHAR(20),
    [Date] DATE,
    [Message] VARCHAR(250),
    [Type] VARCHAR(50),
    UserID INT,
    FOREIGN KEY (UserID) REFERENCES [USER](UserID)
);


CREATE TABLE Feedback(
    Feedback_ID INT IDENTITY(1,1) PRIMARY KEY,
    [Date] DATE,
    Rating INT,
    [Message] VARCHAR(400),

    CustomerID INT,
    Order_ID INT,

    FOREIGN KEY (CustomerID) REFERENCES Customer(UserID),
    FOREIGN KEY (Order_ID) REFERENCES [Order](Order_ID)
);

INSERT INTO [USER] (USERNAME, PASSWORD, NAME, ROLE, Email, Phone, Address)
VALUES
('admin1','pass123','Ahmed Ali','Admin','admin1@gmail.com','0100000001','Cairo'),
('admin2','pass123','Mona Said','Admin','admin2@gmail.com','0100000002','Giza'),
('pharm1','pass123','Omar Tarek','Pharmacist','omar@gmail.com','0100000003','Alex'),
('pharm2','pass123','Sara Adel','Pharmacist','sara@gmail.com','0100000004','Cairo'),
('sup1','pass123','Medico Supply','Supplier','medico@gmail.com','0100000005','Nasr City'),
('sup2','pass123','Pharma Plus','Supplier','plus@gmail.com','0100000006','Heliopolis'),
('cust1','pass123','Hala Mohamed','Customer','hala@gmail.com','0100000007','Alex'),
('cust2','pass123','Youssef Samy','Customer','youssef@gmail.com','0100000008','Cairo'),
('cust3','pass123','Nour Ashraf','Customer','nour@gmail.com','0100000009','Giza'),
('cust4','pass123','Mai Ibrahim','Customer','mai@gmail.com','0100000010','Cairo'),
('cust5','pass123','Salma Taha','Customer','salma@gmail.com','0100000011','Alex'),
('cust6','pass123','Khaled Sami','Customer','khaled@gmail.com','0100000012','Giza'),
('cust7','pass123','Mona Fathy','Customer','monaf@gmail.com','0100000013','Cairo'),
('cust8','pass123','Omar Said','Customer','omars@gmail.com','0100000014','Alex'),
('cust9','pass123','Laila Hossam','Customer','laila@gmail.com','0100000015','Giza'),
('cust10','pass123','Karim Adel','Customer','karim@gmail.com','0100000016','Cairo'),
('cust11','pass123','Sara Tamer','Customer','sarat@gmail.com','0100000017','Alex'),
('cust12','pass123','Hassan Ali','Customer','hassan@gmail.com','0100000018','Giza'),
('cust13','pass123','Nada Samy','Customer','nadas@gmail.com','0100000019','Cairo'),
('cust14','pass123','Tamer Fathy','Customer','tamer@gmail.com','0100000020','Alex'),
('cust15','pass123','Rania Adel','Customer','rania@gmail.com','0100000021','Giza'),
('cust16','pass123','Heba Mohamed','Customer','heba@gmail.com','0100000022','Cairo'),
('cust17','pass123','Ayman Samy','Customer','ayman@gmail.com','0100000023','Alex'),
('cust18','pass123','Maha Khaled','Customer','maha@gmail.com','0100000024','Giza'),
('cust19','pass123','Yara Tamer','Customer','yara@gmail.com','0100000025','Cairo'),
('cust20','pass123','Adham Hossam','Customer','adham@gmail.com','0100000026','Alex'),
('cust21','pass123','Dina Ali','Customer','dina@gmail.com','0100000027','Giza'),
('cust22','pass123','Fady Samy','Customer','fady@gmail.com','0100000028','Cairo'),
('cust23','pass123','Ramy Khaled','Customer','ramy@gmail.com','0100000029','Alex'),
('cust24','pass123','Reem Tamer','Customer','reem@gmail.com','0100000030','Giza');

INSERT INTO [ADMIN] (UserID, Salary, HireDate)
VALUES
(1,15000,'2021-05-10'),
(2,18000,'2020-11-22');

INSERT INTO Pharmacist (UserID, Branch, License_NUMBER, HireDate)
VALUES
(3,'Cairo',1001,'2022-02-14'),
(4,'Alex',1002,'2023-01-05');

INSERT INTO Supplier (UserID, Company_Name)
VALUES
(5,'Medico Supply Co'),
(6,'Pharma Plus Group');

INSERT INTO Customer (UserID, LoyaltyPoints, Gender, EmergencyPhone)
VALUES
(7,120,'Female','0111111111'),
(8,60,'Male','0111111112'),
(9,200,'Female','0111111113'),
(10,40,'Female','0111111114'),
(11,90,'Female','0111111115'),
(12,50,'Male','0111111116'),
(13,75,'Female','0111111117'),
(14,30,'Male','0111111118'),
(15,60,'Female','0111111119'),
(16,80,'Male','0111111120'),
(17,25,'Female','0111111121'),
(18,95,'Male','0111111122'),
(19,100,'Female','0111111123'),
(20,45,'Male','0111111124'),
(21,70,'Female','0111111125'),
(22,55,'Male','0111111126'),
(23,35,'Female','0111111127'),
(24,65,'Male','0111111128'),
(25,85,'Female','0111111129'),
(26,60,'Male','0111111130'),
(27,90,'Female','0111111131'),
(28,50,'Male','0111111132'),
(29,70,'Female','0111111133'),
(30,80,'Male','0111111134');;

INSERT INTO Category (Category_Name, Description)
VALUES
('Painkillers','For pain relief'),
('Antibiotics','Used to treat infections'),
('Vitamins','Dietary supplements'),
('Cold & Flu','Relief from cold and flu symptoms'),
('Diabetes','Diabetes management'),
('Cardiac','Heart-related medications'),
('Allergy','For allergies'),
('Gastro','Digestive system support'),
('Skin','Skin care medications'),
('Eye care','Eye drops and related');

INSERT INTO Medicine (Stock_Quantity, Expiry_Date, Price, Name)
VALUES
(200,'2026-12-01',50,'Panadol'),
(150,'2025-06-10',75,'Augmentin'),
(300,'2027-04-18',30,'Vitamin C'),
(250,'2026-10-05',20,'Cough Syrup'),
(180,'2025-12-01',60,'Insulin'),
(400,'2026-08-15',15,'Antacid'),
(220,'2027-01-10',55,'Aspirin'),
(300,'2026-05-12',35,'Cetirizine'),
(150,'2025-11-20',40,'Omeprazole'),
(280,'2027-03-25',25,'Hydrocortisone'),
(320,'2026-09-30',45,'Amoxicillin'),
(210,'2027-06-15',50,'Metformin'),
(180,'2026-12-12',30,'Acyclovir'),
(200,'2027-05-20',70,'Losartan'),
(150,'2026-07-18',25,'Paracetamol'),
(250,'2025-10-10',55,'Doxycycline'),
(300,'2026-03-15',40,'Levocetirizine'),
(220,'2027-08-05',35,'Simvastatin'),
(260,'2026-11-20',45,'Prednisone'),
(270,'2027-01-30',60,'Atorvastatin'),
(300,'2025-12-25',50,'Ranitidine'),
(310,'2026-04-10',30,'Fluconazole'),
(200,'2026-07-15',20,'Multivitamin'),
(180,'2027-02-18',55,'Clarithromycin'),
(220,'2026-08-25',35,'Levothyroxine'),
(250,'2026-09-30',45,'Metoprolol'),
(300,'2026-10-05',40,'Loratadine'),
(280,'2027-01-12',25,'Omeprazole DR'),
(260,'2026-12-18',50,'Cefixime'),
(300,'2027-03-22',30,'Zinc Tablets');

INSERT INTO MEDICINE_CATEGORY (Medicine_ID, Category_ID)
VALUES
(1, 1),(2, 2),(3, 3),(4, 4),(5, 5),(6, 8),(7, 1),(8, 7),(9, 8),(10, 9),
(11, 2),(12, 5),(13, 2),(14, 6),(15, 1),(16, 2),(17, 7),(18, 6),(19, 6),(20, 6),
(21, 8),(22, 2),(23, 3),(24, 2),(25, 6),(26, 6),(27, 7),(28, 8),(29, 2),(30, 3);

INSERT INTO Supplier_Request (Request_Date, Status, Quantity, SupplierID, PharmacistID)
VALUES
('2025-02-10', 'Pending', 50, 5, 3),
('2025-02-12', 'Approved', 100, 6, 4),
('2025-02-15', 'Pending', 70, 5, 3),
('2025-02-17', 'Approved', 60, 6, 4),
('2025-02-19', 'Pending', 80, 5, 3),
('2025-02-21', 'Approved', 90, 6, 4),
('2025-02-23', 'Pending', 50, 5, 3),
('2025-02-25', 'Approved', 100, 6, 4),
('2025-02-27', 'Pending', 70, 5, 3),
('2025-03-01', 'Approved', 60, 6, 4);


INSERT INTO SUPPLIER_REQUEST_MEDICINE (Request_ID, Medicine_ID)
VALUES
(1,1),(2,2),(3,3),(4,4),(5,5),(6,6),(7,7),(8,8),(9,9),(10,10);

INSERT INTO supplied_by (supplierID, Medicine_ID)
VALUES
(5,1),(6,2),(5,3),(6,4),(5,5),(6,6),(5,7),(6,8),(5,9),(6,10),
(5,11),(6,12),(5,13),(6,14),(5,15),(6,16),(5,17),(6,18),(5,19),(6,20);

INSERT INTO [Order] (discount, Status, ORDER_Date, Total_Amount, CustomerID, PharmacistID)
VALUES
(10.00, 'Completed', '2025-02-14', 100.00, 7, 3),
(5.00, 'Pending', '2025-02-15', 75.00, 8, 4),
(0.00, 'Completed', '2025-02-16', 50.00, 9, 3),
(15.00, 'Cancelled', '2025-02-17', 120.00, 10, 4),
(5.00, 'Completed', '2025-02-18', 90.00, 7, 3),
(0.00, 'Pending', '2025-02-19', 60.00, 8, 4),
(10.00, 'Completed', '2025-02-20', 110.00, 9, 3),
(5.00, 'Pending', '2025-02-21', 80.00, 10, 4),
(0.00, 'Completed', '2025-02-22', 55.00, 7, 3),
(5.00, 'Cancelled', '2025-02-23', 95.00, 8, 4),
(0.00, 'Completed', '2025-02-24', 65.00, 9, 3),
(10.00, 'Pending', '2025-02-25', 100.00, 10, 4),
(5.00, 'Completed', '2025-02-26', 85.00, 7, 3),
(0.00, 'Completed', '2025-02-27', 70.00, 8, 4),
(5.00, 'Cancelled', '2025-02-28', 90.00, 9, 3),
(10.00, 'Completed', '2025-03-01', 120.00, 10, 4),
(0.00, 'Pending', '2025-03-02', 60.00, 7, 3),
(5.00, 'Completed', '2025-03-03', 95.00, 8, 4),
(0.00, 'Completed', '2025-03-04', 50.00, 9, 3),
(10.00, 'Pending', '2025-03-05', 105.00, 10, 4),
(5.00, 'Completed', '2025-03-06', 80.00, 7, 3),
(0.00, 'Completed', '2025-03-07', 70.00, 8, 4),
(5.00, 'Cancelled', '2025-03-08', 90.00, 9, 3),
(10.00, 'Completed', '2025-03-09', 115.00, 10, 4),
(0.00, 'Pending', '2025-03-10', 60.00, 7, 3),
(5.00, 'Completed', '2025-03-11', 85.00, 8, 4),
(0.00, 'Completed', '2025-03-12', 55.00, 9, 3),
(10.00, 'Pending', '2025-03-13', 100.00, 10, 4),
(5.00, 'Completed', '2025-03-14', 90.00, 7, 3),
(0.00, 'Completed', '2025-03-15', 70.00, 8, 4);

INSERT INTO Order_Medicine (Order_ID, Medicine_ID, Quantity)
VALUES
(1, 1, 2),
(1, 3, 1),
(2, 2, 1),
(3, 4, 2),
(4, 5, 1),
(5, 7, 1),
(6, 8, 2),
(7, 10, 1),
(8, 12, 1),
(9, 15, 2),
(10, 18, 1),
(11, 20, 1),
(12, 22, 2),
(13, 25, 1),
(14, 30, 1);

INSERT INTO Payment 
(Payment_Method, Payment_Date, Amount, Status, Order_ID)
VALUES
('Credit Card', '2025-02-14', 100.00, 'Paid', 1),
('Cash',        '2025-02-15', 75.00,  'Pending', 2),
('Credit Card', '2025-02-16', 50.00,  'Paid', 3),
('Cash',        '2025-02-17', 120.00, 'Cancelled', 4),
('Credit Card', '2025-02-18', 90.00,  'Paid', 5),
('Cash',        '2025-02-19', 60.00,  'Pending', 6),
('Credit Card', '2025-02-20', 110.00, 'Paid', 7),
('Cash',        '2025-02-21', 80.00,  'Pending', 8),
('Credit Card', '2025-02-22', 55.00,  'Paid', 9),
('Cash',        '2025-02-23', 95.00,  'Cancelled', 10),
('Credit Card', '2025-02-24', 65.00,  'Paid', 11),
('Cash',        '2025-02-25', 100.00, 'Pending', 12),
('Credit Card', '2025-02-26', 85.00,  'Paid', 13),
('Cash',        '2025-02-27', 70.00,  'Paid', 14),
('Credit Card', '2025-02-28', 90.00,  'Cancelled', 15),
('Cash',        '2025-03-01', 120.00, 'Paid', 16),
('Credit Card', '2025-03-02', 60.00,  'Pending', 17),
('Cash',        '2025-03-03', 95.00,  'Paid', 18),
('Credit Card', '2025-03-04', 50.00,  'Paid', 19),
('Cash',        '2025-03-05', 105.00, 'Pending', 20),
('Credit Card', '2025-03-06', 80.00,  'Paid', 21),
('Cash',        '2025-03-07', 70.00,  'Paid', 22),
('Credit Card', '2025-03-08', 90.00,  'Cancelled', 23),
('Cash',        '2025-03-09', 115.00, 'Paid', 24),
('Credit Card', '2025-03-10', 60.00,  'Pending', 25),
('Cash',        '2025-03-11', 85.00,  'Paid', 26),
('Credit Card', '2025-03-12', 55.00,  'Paid', 27),
('Cash',        '2025-03-13', 100.00, 'Pending', 28),
('Credit Card', '2025-03-14', 90.00,  'Paid', 29),
('Cash',        '2025-03-15', 70.00,  'Paid', 30);





INSERT INTO [NOTIFICATION] (Status, Date, Message, Type, UserID)
VALUES
('Unread', '2025-02-14', 'Your order #1 has been completed', 'Order', 7),
('Unread', '2025-02-15', 'Payment pending for order #2', 'Payment', 8),
('Unread', '2025-02-16', 'Your order #3 has been completed', 'Order', 9),
('Read', '2025-02-17', 'Order #4 has been cancelled', 'Order', 10),
('Unread', '2025-02-18', 'Your order #5 has been completed', 'Order', 7),
('Unread', '2025-02-19', 'Payment pending for order #6', 'Payment', 8),
('Unread', '2025-02-20', 'Your order #7 has been completed', 'Order', 9),
('Read', '2025-02-21', 'Order #8 is pending', 'Order', 10),
('Unread', '2025-02-22', 'Your order #9 has been completed', 'Order', 7),
('Unread', '2025-02-23', 'Payment pending for order #10', 'Payment', 8),
('Unread', '2025-02-24', 'Your order #11 has been completed', 'Order', 9),
('Read', '2025-02-25', 'Order #12 is pending', 'Order', 10),
('Unread', '2025-02-26', 'Your order #13 has been completed', 'Order', 7),
('Unread', '2025-02-27', 'Your order #14 has been completed', 'Order', 8),
('Read', '2025-02-28', 'Order #15 has been cancelled', 'Order', 9),
('Unread', '2025-03-01', 'Your order #16 has been completed', 'Order', 10),
('Unread', '2025-03-02', 'Payment pending for order #17', 'Payment', 7),
('Unread', '2025-03-03', 'Your order #18 has been completed', 'Order', 8),
('Unread', '2025-03-04', 'Your order #19 has been completed', 'Order', 9),
('Read', '2025-03-05', 'Payment pending for order #20', 'Payment', 10),
('Unread', '2025-03-06', 'Your order #21 has been completed', 'Order', 7),
('Unread', '2025-03-07', 'Your order #22 has been completed', 'Order', 8),
('Read', '2025-03-08', 'Order #23 has been cancelled', 'Order', 9),
('Unread', '2025-03-09', 'Your order #24 has been completed', 'Order', 10),
('Unread', '2025-03-10', 'Payment pending for order #25', 'Payment', 7),
('Unread', '2025-03-11', 'Your order #26 has been completed', 'Order', 8),
('Unread', '2025-03-12', 'Your order #27 has been completed', 'Order', 9),
('Read', '2025-03-13', 'Payment pending for order #28', 'Payment', 10),
('Unread', '2025-03-14', 'Your order #29 has been completed', 'Order', 7),
('Unread', '2025-03-15', 'Your order #30 has been completed', 'Order', 8);



INSERT INTO Feedback ([Date], Rating, [Message], CustomerID, Order_ID)
VALUES
('2025-02-10', 5, 'Great service!', 7, 1),
('2025-02-12', 4, 'Good but slow delivery', 8, 2),
('2025-02-14', 3, 'Average experience', 9, 3),
('2025-02-16', 5, 'Excellent!', 10, 4),
('2025-02-18', 4, 'Satisfied', 7, 5),
('2025-02-20', 2, 'Delayed delivery', 8, 6),
('2025-02-22', 5, 'Very good', 9, 7),
('2025-02-24', 3, 'Could be better', 10, 8),
('2025-02-26', 4, 'Happy with service', 7, 9),
('2025-02-28', 5, 'Fantastic', 8, 10),
('2025-03-01', 4, 'Good', 9, 11),
('2025-03-02', 3, 'Okay experience', 10, 12),
('2025-03-03', 5, 'Loved it', 7, 13),
('2025-03-04', 4, 'Pretty good', 8, 14),
('2025-03-05', 5, 'Highly recommend', 9, 15),
('2025-03-06', 3, 'Average', 10, 16),
('2025-03-07', 4, 'Good enough', 7, 17),
('2025-03-08', 5, 'Excellent service', 8, 18),
('2025-03-09', 4, 'Nice', 9, 19),
('2025-03-10', 5, 'Perfect', 10, 20),
('2025-03-11', 3, 'Okay', 7, 21),
('2025-03-12', 4, 'Good', 8, 22),
('2025-03-13', 5, 'Excellent', 9, 23),
('2025-03-14', 4, 'Happy', 10, 24),
('2025-03-15', 5, 'Fantastic', 7, 25),
('2025-03-16', 4, 'Good', 8, 26),
('2025-03-17', 5, 'Very satisfied', 9, 27),
('2025-03-18', 3, 'Average', 10, 28),
('2025-03-19', 4, 'Good', 7, 29),
('2025-03-20', 5, 'Excellent', 8, 30);


