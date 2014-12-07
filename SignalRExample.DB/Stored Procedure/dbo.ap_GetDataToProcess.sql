CREATE PROCEDURE [dbo].[ap_GetDataToProcess]
	@NumOfRowsToProcess int = 10
AS

DECLARE @ProcessedRows TABLE 
(
	Id INT,
	SessionIdentifier VARCHAR(MAX)
)

;;
WITH RowsToProcess AS
(
	SELECT TOP (@NumOfRowsToProcess) Id
	FROM DataToProcess
	WHERE Processing = 0
	AND Processed = 0
	ORDER BY Id
)

UPDATE dtp
SET Processing = 1, ProcessingDTS = GETUTCDATE()
OUTPUT inserted.Id, inserted.SessionIdentifier INTO @ProcessedRows
FROM DataToProcess dtp
INNER JOIN RowsToProcess rtp ON rtp.Id = dtp.Id


SELECT Id, SessionIdentifier
FROM @ProcessedRows
