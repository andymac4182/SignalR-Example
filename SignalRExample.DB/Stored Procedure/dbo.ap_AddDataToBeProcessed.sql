CREATE PROCEDURE [dbo].[ap_AddDataToBeProcessed]
	@SessionIdentifier VARCHAR(8000)
AS
	INSERT INTO DataToProcess (SessionIdentifier, SubmittedDTS)
	VALUES (@SessionIdentifier, GETUTCDATE())

	SELECT SCOPE_IDENTITY()
