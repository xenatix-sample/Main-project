-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRoleById]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Get Role details by Role ID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 05/14/2016	Scott Martin	Added EffectiveDate and ExpirationDate
-----------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[usp_GetRoleById]    Script Date: 06/30/2015 14:52:57 ******/

CREATE PROCEDURE [Core].[usp_GetRoleById]
	@RoleID BIGINT,
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
			Core.Role r 
		WHERE 
			r.RoleID = @RoleID		
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO


