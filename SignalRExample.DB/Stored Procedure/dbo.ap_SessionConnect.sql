CREATE PROCEDURE [dbo].[ap_SessionConnect]
	@SessionIdentifier VARCHAR(MAX)
	, @CustomerId VARCHAR(MAX)
	, @ConnectionServer VARCHAR(50)
	, @ConnectionId VARCHAR(MAX)
AS
	IF EXISTS (SELECT * FROM UserSession WHERE SessionIdentifier = @SessionIdentifier)
	BEGIN
		UPDATE UserSession
		SET ConnectionId = @ConnectionId, ConnectionServer = @ConnectionServer, CustomerId = @CustomerId, ConnectionState = 1, SubmittedTime = GETUTCDATE()
		WHERE SessionIdentifier = @SessionIdentifier
	END
	ELSE
	BEGIN
		INSERT INTO UserSession (SessionIdentifier, ConnectionId, CustomerId, ConnectionServer, ConnectionState, SubmittedTime)
		VALUES (@SessionIdentifier, @ConnectionId, @CustomerId, @ConnectionServer, 1, GETUTCDATE())
	END
