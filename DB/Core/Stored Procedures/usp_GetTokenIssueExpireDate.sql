
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetTokenIssueExpireDate]
-- Author:		Suresh Pandey
-- Date:		07/30/2015
--
-- Purpose:		Get token issue data and expire date from database base on logic
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/30/2015	Suresh Pandey		TFS#  - Initial creation.
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Core].[usp_GetTokenIssueExpireDate] (
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
)
AS
BEGIN


set nocount on 

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Data load executed successfully'

	BEGIN TRY	
		SELECT GETDATE() AS GeneratedOn, DATEADD(DAY,10,GETDATE()) AS ExpirationDate
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH


END
GO

