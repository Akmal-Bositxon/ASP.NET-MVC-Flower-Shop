CREATE TABLE [dbo].[Country] (
    [CountryName] VARCHAR (255) NOT NULL
);
CREATE TABLE [dbo].[Customer] (
    [Customer_ID]  INT             IDENTITY (1, 1) NOT NULL,
    [FirstName]    NVARCHAR (255)  NOT NULL,
    [LastName]     NVARCHAR (255)  NOT NULL,
    [DateOfBirth]  DATE            NOT NULL,
    [City]         NVARCHAR (255)  NOT NULL,
    [Email]        NVARCHAR (255)  NOT NULL,
    [password]     VARCHAR (100)   NOT NULL,
    [username]     VARCHAR (255)   NULL,
    [Phone Number] VARCHAR (255)   NULL,
    [Photo]        VARBINARY (MAX) NULL,
    [Title]        VARCHAR (255)   NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Customer_ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IFK_Customer_ID]
    ON [dbo].[Customer]([Customer_ID] ASC);


GO
CREATE NONCLUSTERED INDEX [IFK_PhoneCustomerID]
    ON [dbo].[Customer]([Customer_ID] ASC);


GO

CREATE TABLE [dbo].[Employee] (
    [Emp_ID]      INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]   NVARCHAR (255) NOT NULL,
    [LastName]    NVARCHAR (255) NOT NULL,
    [DateOfBirth] DATE           NULL,
    [Position]    NVARCHAR (255) NOT NULL,
    [WorkedHours] INT            NOT NULL,
    [HiringDate]  DATE           NOT NULL,
    CONSTRAINT [PK_Emp_ID] PRIMARY KEY CLUSTERED ([Emp_ID] ASC),
    CHECK ([DateOfBirth]<'2001-01-01'),
    CHECK ([WorkedHours]<=(40))
);


GO
CREATE NONCLUSTERED INDEX [IFK_Emp_ID]
    ON [dbo].[Employee]([Emp_ID] ASC);


GO
CREATE NONCLUSTERED INDEX [IFK_PhoneEmpID]
    ON [dbo].[Employee]([Emp_ID] ASC);

CREATE TABLE [dbo].[EmployeeSkills] (
    [Emp_ID]       INT             NOT NULL,
    [Skills]       NVARCHAR (255)  NOT NULL,
    [SalaryAmount] NUMERIC (10, 2) NOT NULL,
    CONSTRAINT [PK_EmployeeSkills] PRIMARY KEY NONCLUSTERED ([Emp_ID] ASC),
    CONSTRAINT [FK_EmployeeSkillsID] FOREIGN KEY ([Emp_ID]) REFERENCES [dbo].[Employee] ([Emp_ID]),
    CONSTRAINT [CH_Empoyee] CHECK ([SalaryAmount]>=(300000) AND [SalaryAmount]<=(1500000))
);

CREATE TABLE [dbo].[Flower] (
    [FlowerId]      INT           IDENTITY (1, 1) NOT NULL,
    [FlowerName]    VARCHAR (255) NOT NULL,
    [DeliveredDate] DATETIME      NULL,
    [Color]         VARCHAR (255) NULL,
    [Price]         INT           NULL,
    CONSTRAINT [PK_Flower] PRIMARY KEY CLUSTERED ([FlowerId] ASC)
);

CREATE TABLE [dbo].[WishList] (
    [WishId]          INT            IDENTITY (1, 1) NOT NULL,
    [Customer_ID]     INT            NOT NULL,
    [FlowerId]        INT            NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [NumberOfFlowers] INT            NOT NULL,
    [comment]         VARCHAR (1600) NULL,
    CONSTRAINT [PK_WishId] PRIMARY KEY CLUSTERED ([WishId] ASC),
    CONSTRAINT [FK_Client_ID] FOREIGN KEY ([Customer_ID]) REFERENCES [dbo].[Customer] ([Customer_ID]),
    CONSTRAINT [FK_FlowerId] FOREIGN KEY ([FlowerId]) REFERENCES [dbo].[Flower] ([FlowerId])
);


GO
CREATE NONCLUSTERED INDEX [IFK_Res_ID]
    ON [dbo].[WishList]([WishId] ASC);

CREATE PROCEDURE udpCreateClient 
		 @FirstName nvarchar(255)
		, @LastName nvarchar(255)		
		, @DateOfBirth datetime
		, @City  nvarchar(255)		
		, @Email nvarchar(255)
		,@password nvarchar(100),
		@UserName nvarchar(255),
		@PhoneNumber nvarchar(255),
		@Photo varbinary(max)
		, @Title nvarchar(255)
AS
BEGIN
INSERT INTO Customer ([FirstName], [LastName], [DateOfBirth],
[City],[Email], [password],[username], [Phone Number],[Photo],[Title] ) 
 VALUES ( @FirstName, @LastName,
 @DateOfBirth, @City, @Email, @password, @UserName, @PhoneNumber,@Photo,@Title)
END
CREATE PROCEDURE udpCreateWishList
	@Customer_ID int, @FlowerId int, @CreatedDate datetime, @NumberOfFlowers int, @comment nvarchar(255)
AS
BEGIN
INSERT INTO [dbo].[WishList] ([Customer_ID], [FlowerId], [CreatedDate], [NumberOfFlowers],[comment])  
                                      VALUES ( @Customer_ID, @FlowerId, @CreatedDate, @NumberOfFlowers, @comment );
END

CREATE PROCEDURE udpUpdateClient 
         @Customer_ID int
       , @FirstName nvarchar(255)
		, @LastName nvarchar(255)		
		, @DateOfBirth datetime
		, @City  nvarchar(255)		
		, @Email nvarchar(255)
		,@password varchar(100)
AS
BEGIN
	UPDATE [dbo].[Customer]
	SET 	[FirstName] = @FirstName,
	        [LastName] = @LastName, 
	        [DateOfBirth] = @DateOfBirth,
	        [City] = @City,
			[Email] = @Email,
			[password] = @password
	WHERE Customer_ID = @Customer_ID;
END

CREATE function udfLogin(@username varchar(100), @password varchar(100)) 
returns int as
begin
  if exists(select 1 from Customer where username = @username and password = @password)
    return 1

  return 0;
endcreate trigger tr_customer_AuditChanges
on Customer
for insert, update, delete
as
declare @operation varchar(20)
begin
  
  if exists(select 1 from inserted) and Not exists (select 1 from deleted)
  set @operation = 'Insert'
  else if exists(select 1 from inserted) and exists (select 1 from deleted)
    set @operation = 'Update'
  else if NOT exists(select 1 from inserted) and exists (select 1 from deleted)
    set @operation = 'Delete'
  
  set identity_insert c_Log_modifications on
  
go

select * into c_Log_modifications from Customer where 1 <>1
alter table c_Log_modifications add [User] nvarchar(100)
alter table c_Log_modifications add [Modification date] Datetime
alter table c_Log_modifications add Operation varchar(20)

  insert into c_Log_modifications (
      [Customer_ID],
      [username],
          [FirstName],
      [LastName],
      [DateOfBirth],
      [Email],
      [City],
      [password],
      [User],
      [Modification date],
      [Operation])
  select 
      [Customer_ID],
      [username],
          [FirstName],
      [LastName],
      [DateOfBirth],
      [Email],
      [City],
      [password],
        CURRENT_USER,
        GETDATE(),
        @operation
    from ((select * from inserted) union all (select * from deleted)) as m

end
go


Insert Into Customer(    [FirstName],
    [LastName]     ,
    [DateOfBirth] ,
    [City]       ,
    [Email]     ,
    [password]   ,
    [username] )Values('Aziz', 'Abdulla', '1997-01-01', 'Uzbekistan', 'aziz_', 'aziz19');
Insert Into Customer(    [FirstName],
    [LastName]     ,
    [DateOfBirth] ,
    [City]       ,
    [Email]     ,
    [password]   ,
    [username] )Values('Sarvar', 'Elbek', '1997-01-01', 'Uzbekistan', 'sarvar_', 'sarvar19');
Insert Into Customer(    [FirstName],
    [LastName]     ,
    [DateOfBirth] ,
    [City]       ,
    [Email]     ,
    [password]   ,
    [username] )Values('Husan', 'Ozod', '1997-01-01', 'Uzbekistan', 'husan_', 'husan21');
Insert Into Customer(    [FirstName],
    [LastName]     ,
    [DateOfBirth] ,
    [City]       ,
    [Email]     ,
    [password]   ,
    [username] )Values('Ibrohim', 'Akbarov', '1997-01-01', 'Uzbekistan', 'ibro_', 'ibrohim21');
Insert into Flower(
    [FlowerName]  ,
    [DeliveredDate],
    [Color] ,
    [Price])Values('Rose', '2019-01-02', 'red',2000);
Insert into Flower(
    [FlowerName]  ,
    [DeliveredDate],
    [Color] ,
    [Price])Values('Rose', '2019-01-02', 'red',2000);Insert into Flower(
    [FlowerName]  ,
    [DeliveredDate],
    [Color] ,
    [Price])Values('Lily', '2019-01-02', 'white',2000);Insert into Flower(
    [FlowerName]  ,
    [DeliveredDate],
    [Color] ,
    [Price])Values('Gardenia', '2019-01-02', 'pink',2000);Insert into Flower(
    [FlowerName]  ,
    [DeliveredDate],
    [Color] ,
    [Price])Values('Magnolia', '2019-01-02', 'yellow',2000);
Insert Into WishList(    [Customer_ID]  ,
    [FlowerId]     ,
    [CreatedDate]    ,
    [NumberOfFlowers] ,
    [comment] )Values(1,1,'2019-03-02',10,'NO Comment');
Insert Into WishList(    [Customer_ID]  ,
    [FlowerId]     ,
    [CreatedDate]    ,
    [NumberOfFlowers] ,
    [comment] )Values(2,1,'2019-03-02',10,'NO Comment');
Insert Into WishList(    [Customer_ID]  ,
    [FlowerId]     ,
    [CreatedDate]    ,
    [NumberOfFlowers] ,
    [comment] )Values(2,2,'2019-03-02',10,'NO Comment');
Insert Into WishList(    [Customer_ID]  ,
    [FlowerId]     ,
    [CreatedDate]    ,
    [NumberOfFlowers] ,
    [comment] )Values(3,1,'2019-03-02',10,'NO Comment');