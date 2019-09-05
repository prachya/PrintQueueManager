CREATE TABLE [dbo].[Presets]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PresetName] NVARCHAR(150) NOT NULL, 
    [MediaType] NCHAR(10) NULL, 
    [PrintQuality] INT NULL, 
    [PaperSize] INT NULL, 
    [PaperSource] INT NULL, 
    [OtherSettings] NVARCHAR(1024) NULL
)
