CREATE TABLE [dbo].PrintProfiles
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProfileName] NVARCHAR(150) NOT NULL, 
    [CreatedDate] DATETIME NULL, 
    [Status] INT NULL, 
    [FtpServer] NVARCHAR(250) NULL, 
    [FtpDirectory] NVARCHAR(250) NULL, 
    [FtpUsername] NVARCHAR(50) NULL, 
    [FtpPassword] NVARCHAR(50) NULL, 
    [StartTime] DATETIME NULL, 
    [TotalCompletes] INT NOT NULL, 
    [TotalPendings] INT NOT NULL, 
    [TotalPauses] INT NOT NULL, 
    [EndTime] DATETIME NULL, 
    [ETA] DATETIME NULL, 
    [FtpCheckInterval] INT NOT NULL, 
    [FilterType] INT NULL
)
