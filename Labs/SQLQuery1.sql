create database MyStore

go

use MyStore

create table AccountMember(
MemerID nvarchar(20) not null PRIMARY KEY,
MemberPassword nvarchar(80) not null,
FullName nvarchar(80) not null,
EmailAddress nvarchar(100) not null,
MemberRole int not null
);

create table Category(
CategoryID int not null PRIMARY KEY,
CategoryName nvarchar(15) not null
);

create table Product(
ProductID int not null PRIMARY KEY,
ProductName nvarchar(40) not null,
CategoryID int not null FOREIGN KEY REFERENCES Category(CategoryId),
UnitsInStock smallint,
UnitPrice money,
);