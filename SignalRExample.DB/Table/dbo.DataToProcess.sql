CREATE TABLE [dbo].[DataToProcess]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [SessionIdentifier] VARCHAR(MAX) NOT NULL,
    SubmittedDTS DATETIME NOT NULL,
    Processing BIT NOT NULL DEFAULT 0,
    ProcessingDTS DATETIME NULL,
    Processed BIT NOT NULL DEFAULT 0,
    ProccessedDTS DATETIME NULL, 
    [Result] INT NULL, 
    [Notified] BIT NOT NULL DEFAULT 0, 
    [NotifiedDTS] DATETIME NULL
)
