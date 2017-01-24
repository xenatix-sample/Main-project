
 
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[CallCenter].[usp_GetClientStatus]
-- Author:		John Crossen
-- Date:		01/19/2016
--
-- Purpose:		Get list of Client Status 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/19/2016	John Crossen -   TFS # 5505     Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE CallCenter.usp_GetClientStatus
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'


SELECT ClientStatusID
      ,ClientStatus
      ,[IsActive]
      ,[ModifiedBy]
      ,[ModifiedOn]
      ,[CreatedBy]
      ,[CreatedOn]
      ,[SystemCreatedOn]
      ,[SystemModifiedOn]
  FROM [CallCenter].[ClientStatus]
  WHERE IsActive=1


  ORDER BY ClientStatusID

  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO