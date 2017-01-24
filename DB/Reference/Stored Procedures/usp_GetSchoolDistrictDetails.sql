

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetSchoolDistrictDetails]
-- Author:		Sumana Sangapu
-- Date:		08/05/2015
--
-- Purpose:		Gets the list of SchoolDistrict lookup  details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/05/2015	Sumana Sangapu		TFS# 972 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Reference].[usp_GetSchoolDistrictDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		SchoolDistrictID, SchoolDistrictName, sd.CountyID as CountyID, CountyName, StateProvinceName
		FROM		[Reference].[SchoolDistrict]  sd
		INNER JOIN  [Reference].[vw_CountryStateProvinceCounty] c 
		ON			sd.[CountyID] = c.CountyID
		WHERE		IsActive = 1
		ORDER BY	SchoolDistrictName  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END



GO