
-----------------------------------------------------------------------------------------------------------------------
-- View:	    [Core].[vw_GetOrganizationStructureDetails]
-- Author:		Sumana Sangapu
-- Date:		03/24/2016
--
-- Purpose:		View to get the Organization Structure Details
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/24/2016 - Sumana Sangau Initial Creation 
-- 12/15/2016	Kyle Campbell	TFS #17998	Include effective/expiration dates for organizationdetails record and organizationdetailsmapping record
-----------------------------------------------------------------------------------------------------------------------

CREATE VIEW [Core].[vw_GetOrganizationStructureDetails]
AS
SELECT
	dm.MappingID,
	dm.DetailID,
	d.Name,
	d.EffectiveDate,
	d.ExpirationDate,
	dm.ParentID,
	dm.EffectiveDate AS MappingEffectiveDate,
	dm.ExpirationDate AS MappingExpirationDate,
	am.AttributeID,
	a.Name AS AttributeName,
	a.DataKey,
	d.IsExternal
FROM
	Core.OrganizationDetails AS d
	INNER JOIN Core.OrganizationDetailsMapping AS dm
		ON dm.DetailID = d.DetailID
	INNER JOIN Core.OrganizationAttributesMapping AS am
		ON d.DetailID = am.DetailID
	INNER JOIN Core.OrganizationAttributes AS a
		ON am.AttributeID = a.AttributeID
WHERE
	dm.IsActive = 1
GO
