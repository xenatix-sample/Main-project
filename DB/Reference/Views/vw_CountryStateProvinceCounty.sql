
-----------------------------------------------------------------------------------------------------------------------
-- View:		vw_CountryStateProvinceCounty
-- Author:		Sumana Sangapu
-- Date:		07/29/2015
--
-- Purpose:		Return dataset with Country,StateProvince and County details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Sumana Sangapu	TFS# 674  - Initial creation.
-- 07/31/2015   John Crossen    TFS# 1016 -- move from dbo to Reference
-----------------------------------------------------------------------------------------------------------------------


CREATE VIEW [Reference].[vw_CountryStateProvinceCounty]
AS 


SELECT		c.CountryID as CountryID ,c.CountryCode as CountryCode ,c.CountryName as CountryName,
			sp.StateProvinceID as StateProvinceID, sp.StateProvinceCode as StateProvinceCode, sp.StateProvinceName as StateProvinceName,
			cty.CountyID as CountyID, cty.CountyName as CountyName, cty.CountyLatitude as CountyLatitude, cty.CountyLongitude as CountyLongitude
FROM		[Reference].Country c
LEFT JOIN	[Reference].StateProvince sp
ON			c.CountryID = sp.CountryID
AND			c.IsActive = sp.IsActive
LEFT JOIN	[Reference].County cty
ON			cty.StateProvinceID = sp.StateProvinceID
AND			cty.IsActive = sp.IsActive
WHERE		c.IsActive = 1 
GO


