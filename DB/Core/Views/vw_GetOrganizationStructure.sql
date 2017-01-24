-----------------------------------------------------------------------------------------------------------------------
-- View:		Core.vw_GetOrganizationStructure
-- Author:		Sumana Sangapu
-- Date:		02/17/2016
--
-- Purpose:		List the entire organization structure
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/17/2016	Sumana Sangapu	Initital Creation 
-----------------------------------------------------------------------------------------------------------------------

CREATE VIEW [Core].[vw_GetOrganizationStructure]
AS


	WITH GetCompaniesList AS (SELECT        dm.ParentID AS parentID, ld.Name AS company, ld1.DetailID AS Divisionid, ld1.Name AS Division
                                                              FROM            Core.OrganizationAttributes AS a INNER JOIN
                                                                                        Core.OrganizationAttributesMapping AS am ON a.AttributeID = am.AttributeID INNER JOIN
                                                                                        Core.OrganizationDetails AS ld ON am.DetailID = ld.DetailID INNER JOIN
                                                                                        Core.OrganizationDetailsMapping AS dm ON dm.ParentID = ld.DetailID INNER JOIN
                                                                                        Core.OrganizationDetails AS ld1 ON dm.DetailID = ld1.DetailID
                                                              WHERE        (a.AttributeID = 1))
    SELECT        gc.parentID AS CompanyID, gc.company AS Company, gc.Divisionid AS DivisionID, gc.Division, ld.DetailID AS ProgramID, ld.Name AS Program, ld1.DetailID AS ProgramUnitID, ld1.Name AS ProgramUnit
    FROM            GetCompaniesList AS gc INNER JOIN
                              Core.OrganizationDetailsMapping AS dm ON dm.ParentID = gc.Divisionid INNER JOIN
                              Core.OrganizationDetails AS ld ON dm.DetailID = ld.DetailID INNER JOIN
                              Core.OrganizationDetailsMapping AS dm1 ON dm1.ParentID = dm.DetailID INNER JOIN
                              Core.OrganizationDetails AS ld1 ON dm1.DetailID = ld1.DetailID

GO




