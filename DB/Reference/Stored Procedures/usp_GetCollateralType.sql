-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetCollateralType]
-- Author:		Kyle Campbell
-- Date:		03/14/2016
--
-- Purpose:		Gets the list of Collateral Types
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/14/2016	Kyle Campbell	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_GetCollateralType]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully';

	BEGIN TRY
		SELECT CollateralTypeID, CollateralType, RelationshipGroupID
		FROM Reference.CollateralType
		WHERE IsActive = 1
		ORDER BY SortOrder
	END TRY

	BEGIN CATCH

	END CATCH
END

