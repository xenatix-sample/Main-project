
 
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[CallCenter].[usp_GetCallStatus]
-- Author:		John Crossen
-- Date:		01/19/2016
--
-- Purpose:		Get list of Call Status 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/19/2016	John Crossen -   TFS # 5505     Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE CallCenter.[usp_GetCallStatus]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'


SELECT [CallStatusID]
      ,[CallStatus]
      ,[IsActive]
      ,[ModifiedBy]
      ,[ModifiedOn]
      ,[CreatedBy]
      ,[CreatedOn]
      ,[SystemCreatedOn]
      ,[SystemModifiedOn]
  FROM [CallCenter].[CallStatus]


  WHERE Isactive=1
  ORDER BY [CallStatusID]

  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO