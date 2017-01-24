
 
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[CallCenter].[usp_GetBehavioralCategory]
-- Author:		John Crossen
-- Date:		01/19/2016
--
-- Purpose:		Get list of BehavioralCategory
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 2/12/2016	John Crossen -   TFS # 6219     Initial creation
-- 2/29/2016    Lokesh Singhal   TFS # 6699     Change OrderBy for ordering of Items.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE CallCenter.usp_GetBehavioralCategory
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'


SELECT BehavioralCategoryID
      ,BehavioralCategory
      ,[IsActive]
      ,[ModifiedBy]
      ,[ModifiedOn]
      ,[CreatedBy]
      ,[CreatedOn]
      ,[SystemCreatedOn]
      ,[SystemModifiedOn]
  FROM [CallCenter].[BehavioralCategory]
  WHERE IsActive=1


  ORDER BY BehavioralCategory

  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO