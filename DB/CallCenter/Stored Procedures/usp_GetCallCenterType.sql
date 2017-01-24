
 
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[CallCenter].[usp_GetCallCenterType]
-- Author:		John Crossen
-- Date:		01/19/2016
--
-- Purpose:		Get list of Call Center Type 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/19/2016	John Crossen -   TFS # 5505     Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE CallCenter.[usp_GetCallCenterType]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'


SELECT [CallCenterTypeID]
      ,[CallCenterType]
      ,[IsActive]
      ,[ModifiedBy]
      ,[ModifiedOn]
      ,[CreatedBy]
      ,[CreatedOn]
      ,[SystemCreatedOn]
      ,[SystemModifiedOn]
  FROM [CallCenter].[CallCenterType]


  WHERE Isactive=1
  ORDER BY [CallCenterTypeID]

  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO