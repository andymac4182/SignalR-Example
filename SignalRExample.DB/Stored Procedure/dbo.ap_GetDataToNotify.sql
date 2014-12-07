CREATE PROCEDURE [dbo].[ap_GetDataToNotify]
	@RowToProcess int = 10
AS

	SELECT TOP (@RowToProcess) dtp.id DataId, dtp.Result, us.ConnectionServer, us.ConnectionId, us.ConnectionState
	FROM DataToProcess dtp
	INNER JOIN UserSession us ON us.SessionIdentifier = dtp.SessionIdentifier
	WHERE dtp.Processed = 1
	AND dtp.Notified = 0
	ORDER BY dtp.ProccessedDTS
