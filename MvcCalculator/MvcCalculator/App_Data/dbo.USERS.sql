CREATE TABLE [dbo].[USERS]
(
	[id] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY, 
    [name] NCHAR(20) NOT NULL UNIQUE, 
    [password] NCHAR(20) NOT NULL, 
    [email] NCHAR(20) NOT NULL, 
    [birthDate] DATE NOT NULL
)
