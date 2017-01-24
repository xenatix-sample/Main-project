

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataUserRole_Staging_ErrorDetails]
-- Author:		Sumana Sangapu
-- Date:		05/19/2016
--
-- Purpose:		Validate lookup data in the staging files used for Data Conversion
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_ValidateDataUserRole_Staging_ErrorDetails]
AS 
BEGIN

		/******************************************** [Synch].[Userrole_staging]    ***********************************/
		--[RoleID]
		INSERT INTO [Synch].[Userrole_Staging_ErrorDetails]
		SELECT *,'RoleID' FROM  [Synch].[Userrole_staging]  c
		WHERE c.RoleID  NOT IN ( SELECT  NAme  FROM  Core.[Role] ) 
		AND c.RoleID  <> ''




END
 