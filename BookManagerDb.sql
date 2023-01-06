/* User */
CREATE TABLE [TypeUser]
(
    [TypeId] INT NOT NULL PRIMARY KEY, 
    [TypeName] NVARCHAR(50) NULL
)
CREATE TABLE [Userr]
(
    [UserId] INT NOT NULL PRIMARY KEY, 
    [Username] NVARCHAR(150) NULL, 
    [Password] NVARCHAR(150) NULL,
    [Name] NVARCHAR(150) NULL,
    [Dateofbirth] DATETIME NULL, 
    [TypeID] INT NULL,
    CONSTRAINT fk_idType
    FOREIGN KEY (TypeId)
    REFERENCES TypeUser (TypeId)
)
/* Category */
CREATE TABLE [dbo].[Category]
(
    [CategoryId] INT NOT NULL PRIMARY KEY, 
    [CategoryName] NVARCHAR(50) NULL,
)
/*  Book  */
CREATE TABLE [Book]
(
    [BookId] INT NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(150) NULL, 
    [AuthorName] NVARCHAR(150) NULL, 
    [Price] FLOAT NULL, 
    [Amount] INT NULL,
    [Year] INT NULL, 
    [CoverPage] VARCHAR(150) NULL,
    [CategoryId] int NULL,
    [PublisherName] NVARCHAR(150) NULL,
    CONSTRAINT fk_idCategory
    FOREIGN KEY (CategoryId)
    REFERENCES Category (CategoryId)
)
/*  Chapter  */
CREATE TABLE [Chapter]
(
    [ChapterId] INT NOT NULL PRIMARY KEY, 
    [ChapterName] NVARCHAR(50) NULL, 
    [ShortContent] NVARCHAR(150) NULL, 
    [BookId] INT NULL, 
    CONSTRAINT fk_idBook
    FOREIGN KEY (BookId)
    REFERENCES Book (BookId)
);
/*  Import  */
CREATE TABLE [Import]
(
    [ImportId] INT NOT NULL PRIMARY KEY, 
    [Date] DATETIME NULL,
    [ExportName] NVARCHAR(50) NULL,
    [Total] FLOAT NULL,
);
CREATE TABLE [ImportDetail]
(
    [ImportDetailId] INT NOT NULL PRIMARY KEY,  
    [ImportId] INT NOT NULL, 
    [BookId] INT NOT NULL, 
    [Quantity] INT NULL, 
    [Price] FLOAT NULL,
    CONSTRAINT fk_idBookIm
    FOREIGN KEY (BookId)
    REFERENCES Book (BookId),
    CONSTRAINT fk_idImport
    FOREIGN KEY (ImportId)
    REFERENCES Import (ImportId)
)
/*  Cart  */
CREATE TABLE [Cart]
(
    [CartId] INT NOT NULL PRIMARY KEY, 
    [Datebuy] DATETIME NULL, 
    [UserID] int NULL, 
    [Total] FLOAT NULL,
    CONSTRAINT fk_idUser
    FOREIGN KEY (UserID)
    REFERENCES Userr (UserID)
);
/*  CartDetail  */
CREATE TABLE [dbo].[CartDetail]
(
    [CartDetailId] INT NOT NULL PRIMARY KEY, 
    [CartId] INT NOT NULL , 
    [BookId] INT NOT NULL, 
    [Quantity] INT NULL, 
    [Price] FLOAT NULL, 
    CONSTRAINT fk_idCart
    FOREIGN KEY (CartID)
    REFERENCES Cart (CartID),
    CONSTRAINT fk_idBookCd
    FOREIGN KEY (BookID)
    REFERENCES Book (BookID)
)
/* Insert data
INSERT INTO table_name (column1, column2, column3, ...)
VALUES (value1, value2, value3, ...);
*/