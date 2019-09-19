CREATE TABLE [dbo].[Filters]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(50) NULL, 
    [StartsWith] NVARCHAR(10) NULL, 
    [EndsWith] NVARCHAR(10) NULL, 
    [Contains] NVARCHAR(10) NULL, 
    [DoesNotContain] NVARCHAR(10) NULL, 
    [ProfileId] INT NULL,
	CONSTRAINT [FK_Profile_ToTable] FOREIGN KEY ([ProfileId]) REFERENCES [PrintProfiles]([Id]) ON DELETE CASCADE 
)
