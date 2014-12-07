CREATE PROCEDURE [dbo].[ap_SessionDisconnect]
	@SessionIdentifier VARCHAR(MAX)
	, @CustomerId VARCHAR(MAX)
	, @ConnectionServer VARCHAR(50)
	, @ConnectionId VARCHAR(MAX)
AS
	IF EXISTS (SELECT * FROM UserSession WHERE SessionIdentifier = @SessionIdentifier)
	BEGIN
		UPDATE UserSession
		SET ConnectionId = @ConnectionId, ConnectionServer = @ConnectionServer, CustomerId = @CustomerId, ConnectionState = 0
		WHERE SessionIdentifier = @SessionIdentifier
	END
	ELSE
	BEGIN
		-- TODO: Raise Error
		SELECT 'ERROR'
	END
