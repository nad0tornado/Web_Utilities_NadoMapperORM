/* Create the DB first*/
create database TestDB;

/* Select TestDB->New Query and create the tables */
create table Tests (
	Id int primary key identity not null,
	Name varchar(64) not null,
	LastModified timestamp not null,
	DateAdded datetime not null constraint TestsDateAddedDefaultConstraint default(getutcdate())
)
go
create procedure AddTest
	@name varchar(64)
as
begin
	set nocount on;
	insert into Tests([Name]) values (@name)

	select cast(SCOPE_IDENTITY() as int) as Id
end
go
create procedure DeleteTest
	@id int,
	@lastmodified binary(8)
as
begin
	set nocount off;
	delete from Tests where Id=@id and LastModified=@lastmodified
end
go
create procedure GetTests
as
	select * from Tests
go
create procedure GetTestById
	@id int
as
begin
	set nocount on;
	select
		Id,
		[Name],
		LastModified,
		DateAdded
	from Tests where Id=@id
end
go
/* create procedure GetTestByName
	@name varchar(64)
as
begin
	set nocount on;
	select
		Id,
		[Name],
		LastModified,
		DateAdded
	from Tests where [Name]=@name
end 
go*/
create procedure UpdateTest
	@id int,
	@name varchar(64),
	@lastModified binary(8)
as
begin
	set nocount off;
	update Tests SET
		[Name] = @name
	where Id=@id and LastModified=@lastModified
end
/* ROLLBACK 
drop database TestDB
*/