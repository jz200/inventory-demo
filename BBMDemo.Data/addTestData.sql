use BBMDemo
--create admin role if not existing already
Declare @roleId nvarchar(450)
select @roleId = id from dbo.AspNetRoles where name='Admin'
if @roleId is null
begin 
	insert into dbo.AspNetRoles
	(Id, Name, NormalizedName)
	values
	(NEWID(), 'Admin', 'ADMIN');
	select @roleId = id from dbo.AspNetRoles where name='Admin'
end

--set the test user as admin so that he can manage other users
declare @userId nvarchar(450)
select @userId = Id from dbo.AspNetUsers where email = 'test1@test.com'
--if user test1@test.com does not exist, please register him first
if @userId is not null
insert into dbo.AspNetUserRoles
(UserId, RoleId)
values
(@userId, @roleId)

go

--insert test data to Category table if no data exist
insert into Production.Category
(Name, EnteredDate)
values 
('Base Cabinet', getdate()),
('Tall Cabinet', getdate() ),
('Vanity', getdate() )

--insert data to Style if no data exists
insert into Production.Style
(Name, EnteredDate)
values
('White Shake', getdate()),
('Victorial Red', getdate()),
('Fulton Offwhite', getdate())

--insert test data to Location
Insert into Production.Location
(name, EnteredDate)
values
('Durham', getdate()),
('Raleigh', getdate())

--insert test data to Product
declare @base int
select @base=categoryId from production.Category where name='Base Cabinet'
insert into Production.Product
(CategoryId, ItemNumber, EnteredDate)
values
(@base,	'B09FD', getdate()),
(@base,	'B12',	getdate()),
(@base,	'B15',	getdate()),
(@base,	'B18',	getdate()),
(@base,	'B21',	getdate()),
(@base,	'B24',	getdate()),
(@base,	'B27',	getdate()),
(@base,	'B30',	getdate()),
(@base,	'B33',	getdate()),
(@base,	'B36',	getdate())

select @base=categoryId from production.Category where name='Tall Cabinet'
insert into Production.Product
(CategoryId, ItemNumber, EnteredDate)
values
(@base,	'TP1884'	,getdate()),
(@base,	'TP1890'	,getdate()),
(@base,	'TP1896'	,getdate()),
(@base,	'TP3084'	,getdate()),
(@base,	'TP3096'	,getdate())

select @base=categoryId from production.Category where name='Vanity'
insert into Production.Product
(CategoryId, ItemNumber, EnteredDate)
values
(@base,	'VSD30'	,getdate()),
(@base,	'VSD36'	,getdate()),
(@base,	'VSD48'	,getdate()),
(@base,	'VSD60'	,getdate())

--Poplulate inventory table with some test data
insert into Production.ProductInventory
(ProductId, StyleId, LocationId, Quantity, BodyQuantity, FrameQuantity, EnteredDate)
select ProductId, StyleId, locationId, 100, 100, 100, getdate() from production.Product
cross join production.style cross join production.Location order by ProductId, StyleId, LocationId