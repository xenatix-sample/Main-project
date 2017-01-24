
 
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[CallCenter].[usp_GetCallCenterPriority]
-- Author:		John Crossen
-- Date:		01/19/2016
--
-- Purpose:		Get list of Call Center Priority
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/19/2016	John Crossen -   TFS # 5505     Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE CallCenter.[usp_GetCallCenterPriority]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'


SELECT [CallCenterPriorityID]
      ,[CallCenterPriority]
      ,[IsActive]
      ,[ModifiedBy]
      ,[ModifiedOn]
      ,[CreatedBy]
      ,[CreatedOn]
      ,[SystemCreatedOn]
      ,[SystemModifiedOn]
  FROM [CallCenter].[CallCenterPriority]

  WHERE Isactive=1
  ORDER BY [CallCenterPriorityID]

  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO