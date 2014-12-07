CREATE PROCEDURE [dbo].[ap_ClientNotified]
	@DataId int 
AS
	UPDATE DataToProcess
	SET Notified = 1, NotifiedDTS = GETUTCDATE()
	WHERE id = @DataId