CREATE TABLE [dbo].[UserSession]
(
	[SessionIdentifier] VARCHAR(8000) NOT NULL PRIMARY KEY
	, CustomerId VARCHAR(MAX)
	, SubmittedTime DATETIME
	, ConnectionServer VARCHAR(50)
	, ConnectionState SMALLINT
	, ConnectionId VARCHAR(MAX)
)
