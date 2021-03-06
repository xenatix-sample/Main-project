
 
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_GetContactTypeHeirarchy]
-- Author:		John Crossen
-- Date:		01/19/2016
--
-- Purpose:		Get list of Contact Types based on heirarchy 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/19/2016	John Crossen -   TFS # 5514     Initial creation
--01/29/2016	Gurpreet Singh	Removed ContactTypeID 
--02/02/2016	Gurpreet Singh	Updated ChildContactTypeID to ContactTypeID in output
--03/20/2016    Gaurav Gupta    Order by ContactType

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_GetContactTypeHeirarchy]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

		SELECT 
			CC.ContactTypeID,
			CC.ContactType
		FROM [Reference].[ContactType] CT
		LEFT OUTER JOIN Reference.ContactTypeHeirarchy CH ON CH.ParentContactTypeID = CT.ContactTypeID
		LEFT OUTER JOIN (SELECT ContactTypeID, ContactType FROM Reference.ContactType WHERE IsActive = 1) CC ON CC.ContactTypeID = CH.ChildContactTypeID
		WHERE CH.IsActive = 1 AND CT.IsActive = 1
		AND CT.ContactTypeID = 9
		ORDER BY CC.ContactType

  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END