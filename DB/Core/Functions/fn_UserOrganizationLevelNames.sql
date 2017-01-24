-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[fn_UserOrganizationLevelNames]
-- Author:		Scott Martin
-- Date:		02/29/2016
--
-- Purpose:		Gets a list of users and a comma delimited list of the Organization Level they are assigned to
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/29/2016	Scott Martin		Initial Creation
----------------------------------------------------------------------------------------------------------------------

CREATE FUNCTION Core.fn_UserOrganizationLevelNames
(
	@DataKey NVARCHAR(50)
)
RETURNS @TempTable TABLE
(
	UserID INT,
	OrganizationLevelNames NVARCHAR(MAX)
)
AS
BEGIN
WITH Org (UserID, DetailID, ParentID, OrganizationLevelName, DataKey)
AS
(
	SELECT
		UODM.UserID,
		ODM.DetailID,
		ODM.ParentID,
		OD.Name,
		OA.DataKey
	FROM
		Core.UserOrganizationDetailsMapping UODM
		INNER JOIN Core.OrganizationDetailsMapping ODM
			ON UODM.MappingID = ODM.MappingID
		INNER JOIN Core.OrganizationAttributesMapping OAM
			ON ODM.DetailID = OAM.DetailID
		INNER JOIN Core.OrganizationAttributes OA
			ON OAM.AttributeID = OA.AttributeID
		INNER JOIN Core.OrganizationDetails OD
			ON ODM.DetailID = OD.DetailID
	UNION ALL
	SELECT
		O.UserID,
		ODM.DetailID,
		ODM.ParentID,
		OD.Name,
		OA.DataKey
	FROM
		Core.OrganizationDetailsMapping ODM
		INNER JOIN Core.OrganizationAttributesMapping OAM
			ON ODM.DetailID = OAM.DetailID
		INNER JOIN Core.OrganizationAttributes OA
			ON OAM.AttributeID = OA.AttributeID
		INNER JOIN Core.OrganizationDetails OD
			ON ODM.DetailID = OD.DetailID
		INNER JOIN Org O
			ON ODM.DetailID = O.ParentID
)
INSERT INTO @TempTable
SELECT DISTINCT
	O.UserID,
	LTRIM(STUFF((SELECT DISTINCT ', ' + OrganizationLevelName FROM Org WHERE UserID = O.UserID AND DataKey = @DataKey FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, '')) AS OrganizationLevelNames
FROM
	Org O

RETURN
END