

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataUserUIds_Staging_ErrorDetails]
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
-- 08/29/2016	Rahul Vats The [Synch].[UserUIds_Staging] table file has not been checked into the project.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_ValidateDataUserUIds_Staging_ErrorDetails]
AS 
BEGIN

		/********************************************  [Synch].[UserUIds_Staging]    ***********************************/
		--UserIdentifierTypeID
		INSERT INTO  [Synch].[UserUIds_Staging_ErrorDetails]
		SELECT *,'UserIdentifierTypeID' FROM [Synch].[UserUIds_Staging] cs
		WHERE cs.UserIdentifierTypeID NOT IN ( SELECT  UserIdentifierType FROM  Reference.UserIdentifierType  ) 
		AND cs.UserIdentifierTypeID <> '' 



END
 