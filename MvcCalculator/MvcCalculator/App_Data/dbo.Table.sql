CREATE TABLE [dbo].History
(
	[Id] INT NOT NULL IDENTITY(1, 1), 
    [v1] FLOAT NULL, 
    [v2] FLOAT NULL, 
    [operation] NCHAR(10) NULL, 
    [ip] NCHAR(10) NULL
)
