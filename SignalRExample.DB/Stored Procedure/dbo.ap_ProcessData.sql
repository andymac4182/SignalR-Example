CREATE PROCEDURE [dbo].[ap_ProcessData]
	@DataId INT,
	@Result BIT
AS
	UPDATE DataToProcess
	SET Processed = 1, ProccessedDTS = GETUTCDATE(), Result = @Result
	WHERE Id = @DataId
