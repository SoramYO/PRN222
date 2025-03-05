
create table AccountMember(
MemberID nvarchar(20) not null PRIMARY KEY,
MemberPassword nvarchar(80) not null,
FullName nvarchar(80) not null,
EmailAddress nvarchar(100) not null,
MemberRole int not null
);

