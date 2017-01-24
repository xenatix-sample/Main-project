-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRole]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Get Role details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetRole]
	@RoleName NVARCHAR(500),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT 
			RoleID,
			Name,
			[Description],
			EffectiveDate,
			ExpirationDate
		FROM 
			Core.[Role] r
	    WHERE 
			Name LIKE '%'+@RoleName+'%'	
			AND ISNULL(IsActive,1) = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


