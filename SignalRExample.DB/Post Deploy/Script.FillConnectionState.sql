/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
			   SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS(SELECT * FROM ConnectionState WHERE id = 0)
BEGIN 
	INSERT INTO ConnectionState
	VALUES (0, 'Disconnected')
END


IF NOT EXISTS(SELECT * FROM ConnectionState WHERE id = 1)
BEGIN 
	INSERT INTO ConnectionState
	VALUES (1, 'Connected')
END