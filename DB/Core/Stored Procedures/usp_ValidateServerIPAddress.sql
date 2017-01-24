
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_ValidateServerIPAddress
-- Author:		Suresh Pandey
-- Date:		07/29/2015
-- Purpose:		To check that given IP Address as parameter is exist in ServerResource table or not.
-- Notes:		n/a
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Suresh Pandey		TFS#  - Initial creation.
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Core].[usp_ValidateServerIPAddress] (
	@IPAddress NVARCHAR(200),
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,  
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
	--@ServerResourceType int,  --for future use
)
AS
BEGIN
set nocount on
	
	SELECT  @ID=0, @ResultCode = 0,  
		    @ResultMessage = OBJECT_NAME(@@PROCID) + ' executed successfully'    

	BEGIN TRY  
		SELECT	@ID=COUNT(1) 
		FROM	[Core].[ServerResource]
		WHERE	ServerIP=@IPAddress and 
				IsActive = 1  
				--ServerResourceTypeID =@ServerResourceType  and --for future use
	END TRY  
	BEGIN CATCH  
			SELECT @ID=0,  @ResultCode = ERROR_SEVERITY(),  
			@ResultMessage = ERROR_MESSAGE()  
	END CATCH 
END  
GO
