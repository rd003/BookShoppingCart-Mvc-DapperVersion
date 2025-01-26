USE BookShoppingCartMvc_Dapper;
GO

-- Genre data
SET IDENTITY_INSERT [dbo].[Genre] ON
GO
INSERT [dbo].[Genre] ([Id], [GenreName]) VALUES (1, N'Romance')
GO
INSERT [dbo].[Genre] ([Id], [GenreName]) VALUES (2, N'Action')
GO
INSERT [dbo].[Genre] ([Id], [GenreName]) VALUES (3, N'Thriller')
GO
INSERT [dbo].[Genre] ([Id], [GenreName]) VALUES (4, N'Crime')
GO
INSERT [dbo].[Genre] ([Id], [GenreName]) VALUES (5, N'SelfHelp')
GO
INSERT [dbo].[Genre] ([Id], [GenreName]) VALUES (6, N'Programming')
GO
SET IDENTITY_INSERT [dbo].[Genre] OFF
GO

-- Book Data

INSERT INTO book (BookName, AuthorName, Price, GenreId)
VALUES
('Pride and Prejudice', 'Jane Austen', 12.99, 1),
('The Notebook', 'Nicholas Sparks', 11.99, 1),
('Outlander', 'Diana Gabaldon', 14.99, 1),
('Me Before You', 'Jojo Moyes', 10.99, 1),
('The Fault in Our Stars', 'John Green', 9.99, 1);

-- Inserting rows for Action genre
INSERT INTO book (BookName, AuthorName, Price, GenreId)
VALUES
('The Bourne Identity', 'Robert Ludlum', 14.99, 2),
('Die Hard', 'Roderick Thorp', 13.99, 2),
('Jurassic Park', 'Michael Crichton', 15.99, 2),
('The Da Vinci Code', 'Dan Brown', 12.99, 2),
('The Hunger Games', 'Suzanne Collins', 11.99, 2);

-- Inserting rows for Thriller genre
INSERT INTO book (BookName, AuthorName, Price, GenreId)
VALUES
('Gone Girl', 'Gillian Flynn', 11.99, 3),
('The Girl with the Dragon Tattoo', 'Stieg Larsson', 10.99, 3),
('The Silence of the Lambs', 'Thomas Harris', 12.99, 3),
('Before I Go to Sleep', 'S.J. Watson', 9.99, 3),
('The Girl on the Train', 'Paula Hawkins', 13.99, 3);

-- Inserting rows for Crime genre
INSERT INTO book (BookName, AuthorName, Price, GenreId)
VALUES
('The Godfather', 'Mario Puzo', 13.99, 4),
('The Girl with the Dragon Tattoo', 'Stieg Larsson', 12.99, 4),
('The Cuckoo''s Calling', 'Robert Galbraith', 14.99, 4),
('In Cold Blood', 'Truman Capote', 11.99, 4),
('The Silence of the Lambs', 'Thomas Harris', 15.99, 4);

-- Inserting rows for SelfHelp genre
INSERT INTO book (BookName, AuthorName, Price, GenreId)
VALUES
('The 7 Habits of Highly Effective People', 'Stephen R. Covey', 9.99, 5),
('How to Win Friends and Influence People', 'Dale Carnegie', 8.99, 5),
('Atomic Habits', 'James Clear', 10.99, 5),
('The Subtle Art of Not Giving a F\*ck', 'Mark Manson', 7.99, 5),
('You Are a Badass', 'Jen Sincero', 11.99, 5);

-- Inserting rows for Programming genre
INSERT INTO book (BookName, AuthorName, Price, GenreId)
VALUES
('Clean Code', 'Robert C. Martin', 19.99, 6),
('Design Patterns', 'Erich Gamma', 17.99, 6),
('Code Complete', 'Steve McConnell', 21.99, 6),
('The Pragmatic Programmer', 'Andrew Hunt', 18.99, 6),
('Head First Design Patterns', 'Eric Freeman', 20.99, 6);

-- update stock
INSERT INTO Stock
(BookId,Quantity)
SELECT
b.Id,
10
FROM Book b
WHERE NOT EXISTS (
SELECT * FROM [Stock]
);

-- Order status

GO
SET IDENTITY_INSERT [dbo].[OrderStatus] ON
GO
INSERT [dbo].[OrderStatus] ([Id], [StatusId], [StatusName]) VALUES (1, 1, N'Pending')
GO
INSERT [dbo].[OrderStatus] ([Id], [StatusId], [StatusName]) VALUES (2, 2, N'Shipped')
GO
INSERT [dbo].[OrderStatus] ([Id], [StatusId], [StatusName]) VALUES (3, 3, N'Delivered')
GO
INSERT [dbo].[OrderStatus] ([Id], [StatusId], [StatusName]) VALUES (4, 4, N'Cancelled')
GO
INSERT [dbo].[OrderStatus] ([Id], [StatusId], [StatusName]) VALUES (5, 5, N'Returned')
GO
INSERT [dbo].[OrderStatus] ([Id], [StatusId], [StatusName]) VALUES (6, 6, N'Refund')
GO
SET IDENTITY_INSERT [dbo].[OrderStatus] OFF
GO

-- Stored procedures

USE [BookShoppingCartMvc_Dapper]
GO


CREATE OR ALTER  PROCEDURE [dbo].[AddItemToCart]
 @BookId INT,
 @Quantity INT,
 @UserId NVARCHAR(450)
AS
BEGIN
  BEGIN TRANSACTION;

  BEGIN TRY
    IF @UserId is null or @UserId = ''
	BEGIN
	  RAISERROR('User is not logged in', 16, 1);
      RETURN -1;
	END

   DECLARE @CartId INT;

   SELECT @CartId = Id
   FROM ShoppingCart
   WHERE UserId = @UserId;

   IF @CartId IS NULL
   begin
      INSERT INTO ShoppingCart(UserId,IsDeleted)
	  VALUES (@UserId,0);

	  SET @CartId = SCOPE_IDENTITY();
   end

   -- Check if item already exists in cart
   DECLARE @ExistingCartDetailId INT;
   DECLARE @ExistingQuantity INT;
   
   SELECT 
     @ExistingCartDetailId = Id,
	 @ExistingQuantity = Quantity
   FROM CartDetail
   where ShoppingCartId=@CartId and BookId=@BookId

   -- Get book price for new cart items
   DECLARE @UnitPrice DECIMAL(18,2);

   SELECT @UnitPrice = Price 
   FROM Book 
   WHERE Id = @BookId;

   -- Update existing cart item or insert new

   if @ExistingCartDetailId is not null
   begin
     UPDATE CartDetail
	 set Quantity = Quantity+1
	 Where Id= @ExistingCartDetailId;
   end
   else
   begin
     INSERT INTO CartDetail (ShoppingCartId, BookId, Quantity, UnitPrice)
	 VALUES (@CartId, @BookId, @Quantity, @UnitPrice);
   end

    -- Get total cart item count for return
    DECLARE @CartItemCount INT;

	SELECT
	 @CartItemCount=Count(1)
	FROM CartDetail
	Where ShoppingCartId=@CartId

	COMMIT TRANSACTION;

	RETURN @CartItemCount;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT>0
	   ROLLBACK TRANSACTION;
    THROW; 
  END CATCH

end
GO

create or alter procedure RemoveCartItem
  @UserId nvarchar(100),
  @BookId int
as
begin
 -- Ensure @UserId is not null  
 if (@UserId is null or @UserId = '')
 begin
  raiserror('@UserId is null',16,1); 
 end

 -- check whether a user has cart or not

 declare @CartId INT;

 select 
  @CartId=s.Id
 from ShoppingCart s
 where s.UserId = @UserId;

 if @CartId is null
 begin
   raiserror('Invalid cart',16,1);
 end

 declare @CartItemQuantity int;
 declare @CartDetailId int;

 select 
  @CartDetailId = cd.Id,
  @CartItemQuantity = cd.Quantity
 from CartDetail cd
 where cd.ShoppingCartId=@CartId and cd.BookId=@BookId;

 if @CartItemQuantity is null
 begin
  raiserror('No items in cart',16,1);
 end

 else if(@CartItemQuantity = 1)
 begin
   delete from CartDetail 
   where Id=@CartDetailId;
 end

 else
 begin
   update CartDetail
   set Quantity  = Quantity - 1
   where Id=@CartDetailId;
 end

 -- return the cart items

 declare @CartItemCount int;

 select
  @CartItemCount = Count(1)
 from CartDetail cd
 where cd.ShoppingCartId= @CartId;

 return @CartItemCount;

end
go

-- checkout script
CREATE OR ALTER PROCEDURE Checkout
  @Name NVARCHAR(30),
  @Email NVARCHAR(30),
  @MobileNumber NVARCHAR(15),
  @Address NVARCHAR(200),
  @PaymentMethod NVARCHAR(20),
  @UserId NVARCHAR(200)
AS
BEGIN
  SET NOCOUNT ON;

  -- Input validation (existing checks)
  IF (@Name IS NULL OR @Name = '')
    THROW 50001, 'Name cannot be null', 1;
  
  IF (@Email IS NULL OR @Email = '')
    THROW 50002, 'Email cannot be null', 1;
  
  IF (@MobileNumber IS NULL OR @MobileNumber = '')
    THROW 50003, 'Mobile Number cannot be null', 1;
  
  IF (@Address IS NULL OR @Address = '')
    THROW 50004, 'Address cannot be null', 1;
  
  IF (@PaymentMethod IS NULL OR @PaymentMethod = '')
    THROW 50005, 'Payment Method cannot be null', 1;
  
  IF (@UserId IS NULL OR @UserId = '')
    THROW 50006, 'User ID cannot be null', 1;

  BEGIN TRY
    BEGIN TRANSACTION;

    -- Get Cart Information
    DECLARE @CartId INT;
    SELECT @CartId = Id 
    FROM ShoppingCart 
    WHERE UserId = @UserId;

    IF @CartId IS NULL
      THROW 50007, 'Invalid cart', 1;

    IF NOT EXISTS (SELECT 1 FROM CartDetail WHERE ShoppingCartId = @CartId)
      THROW 50008, 'Cart is empty', 1;

    -- Check Stock Availability
    IF EXISTS (
      SELECT 1 
      FROM CartDetail cd
      INNER JOIN Stock s ON cd.BookId = s.BookId
      WHERE cd.ShoppingCartId = @CartId AND cd.Quantity > s.Quantity
    )
      THROW 50009, 'Insufficient stock for one or more items', 1;

    -- Get Pending Order Status
    DECLARE @PendingOrderStatusId INT;
    SELECT @PendingOrderStatusId = Id 
    FROM OrderStatus 
    WHERE StatusName = 'Pending';

    IF @PendingOrderStatusId IS NULL
      THROW 50010, 'No ''Pending'' order status found', 1;

    -- Create Order
    INSERT INTO [Order]
    (CreateDate, [Address], Email, IsDeleted, IsPaid, MobileNumber, [Name], OrderStatusId, PaymentMethod, UserId)
    VALUES 
    (CURRENT_TIMESTAMP, @Address, @Email, 0, 0, @Name, @MobileNumber, @PendingOrderStatusId, @PaymentMethod, @UserId);

    DECLARE @CreatedOrderId INT = SCOPE_IDENTITY();

    -- Create Order Details
    INSERT INTO OrderDetail
    (BookId, OrderId, Quantity, UnitPrice)
    SELECT 
      cd.BookId,
      @CreatedOrderId,
      cd.Quantity,
      cd.UnitPrice
    FROM CartDetail cd
    WHERE cd.ShoppingCartId = @CartId;

    -- Update Stock 
    UPDATE Stock
    SET Quantity = Stock.Quantity - OrderDetail.Quantity
    FROM Stock
    INNER JOIN OrderDetail ON Stock.BookId = OrderDetail.BookId
    WHERE OrderDetail.OrderId = @CreatedOrderId;

    -- Remove Cart Items
    DELETE FROM CartDetail 
    WHERE ShoppingCartId = @CartId;

    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
      ROLLBACK TRANSACTION;
    THROW;
  END CATCH
END

-- get books for home page

create or alter  procedure [dbo].[USP_GetBooksForHomePage]
  @search_term nvarchar(30)='',
  @genre_id int = 0
as
begin

 set @search_term = case when @search_term='' then 'show_all' else @search_term end;

 SELECT
  b.Id,
  b.[Image],
  b.AuthorName,
  b.BookName,
  b.GenreId,
  b.Price,
  g.GenreName,
  COALESCE(s.Quantity,0) as Quantity
 FROM Book b
 inner join Genre g
 on b.GenreId = g.Id
 left outer join Stock s
 on b.Id = s.BookId
 Where 
 (@search_term='show_all' OR CONTAINS(b.BookName,@search_term))
 AND (@genre_id = 0 OR b.GenreId = @genre_id) 
 ORDER BY b.BookName ASC;
end
GO


