-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetUserOrgDetailsMapping]
-- Author:		Sumana Sangapu
-- Date:		03/29/2016
--
-- Purpose:		Get the user organization details.  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/29/2016	Sumana Sangapu    Initial creation. 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserOrgDetailsMapping]
	@UserID INT,
    @ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	


		DECLARE @OrgData TABLE(	
							RowID int identity(1,1) NOT NULL,
							UserID INT,
							PUDetailID INT,  -- Program Unit's DetailID
							PUParentID int, -- ProgramUnit's ParentID
							PUMappingID int, -- ProgrmUnit's Mapping 
							PU nvarchar(100), -- ProgramUnits's Name

							ProgramDetailID INT, 
							ProgramParentID INT,
							ProgramMappingID int,
							Program nvarchar(100),
							
							DivisionDetailID INT, 
							DivisonParentID int,
							DivisionMappingID INT,
							Division nvarchar(100),
							
							CompanyDetailID INT, 
							CompanyParentID int,
							CompanyMappingID INT,
							Company nvarchar(100))

		-- Insert the ProgramUnit details into temp table							
		INSERT	INTO @OrgData
		(UserID, PUDetailID, PUParentID, PUMappingID,PU )
		SELECT @UserID, DetailID as PUDetailID,ParentID as PUParentID, u.MappingID as PUMappingID, Name as PU
		FROM [Core].[vw_GetOrganizationStructureDetails] v
		INNER JOIN core.UserOrganizationDetailsMapping u
		ON v.mappingid = u.mappingid
		WHERE u.UserID = @UserID
		AND V.DataKey = 'ProgramUnit'
		AND u.IsActive = 1 

		-- Update the Program details by mapping the ProgramUnit's ParentID to its corresponding MappingID from view
		UPDATE o
		SET ProgramDetailID = DetailID ,  
			ProgramParentID = v.ParentID ,
			ProgramMappingID = v.MappingID,
			Program = Name
		FROM @OrgData o
		INNER JOIN [Core].[vw_GetOrganizationStructureDetails] v
		ON v.MappingID = o.PUParentID
		WHERE V.DataKey = 'Program'

		-- Update the Division details by mapping the Program's ParentID to its corresponding MappingID from view
		UPDATE o
		SET DivisionDetailID = DetailID ,  
			DivisonParentID = v.ParentID ,
			DivisionMappingID = v.MappingID,
			Division = Name
		FROM @OrgData o
		INNER JOIN [Core].[vw_GetOrganizationStructureDetails] v
		ON v.MappingID = o.ProgramParentID
		WHERE V.DataKey = 'Division'

		-- Update the Company's details by mapping the Division's ParentID to its corresponding MappingID from view
		UPDATE o
		SET CompanyDetailID = DetailID ,  
			CompanyParentID = v.ParentID ,
			CompanyMappingId = v.MappingID,
			Company = Name
		FROM @OrgData o
		INNER JOIN [Core].[vw_GetOrganizationStructureDetails] v
		ON v.MappingID = o.DivisonParentID
		WHERE V.DataKey = 'Company'

		--Output the resultset
		SELECT * FROM @OrgData


	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END