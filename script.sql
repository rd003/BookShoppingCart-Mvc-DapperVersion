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
