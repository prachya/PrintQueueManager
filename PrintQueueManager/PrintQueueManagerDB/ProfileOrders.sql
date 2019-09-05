CREATE TABLE [dbo].[ProfileOrders]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProfileId] INT NOT NULL, 
    [OrderId] INT NOT NULL, 
    [Reserved] INT NULL, 
    [Reserved1] NVARCHAR(50) NULL, 
    [PrintingOrder] INT NOT NULL, 
    [PrintingStatus] INT NOT NULL, 
    [StaredPrintingTime] DATETIME NULL, 
    [CompletedPrintingTime] DATETIME NULL, 
    [DateTimeAdded] DATETIME NULL, 
    [ItemStatus] INT NULL, 
    CONSTRAINT [FK_ProfileOrders_ToTable] FOREIGN KEY ([ProfileId]) REFERENCES [PrintProfiles]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_ProfileOrders_ToTable_1] FOREIGN KEY ([OrderId]) REFERENCES [Orders]([Id])
)
