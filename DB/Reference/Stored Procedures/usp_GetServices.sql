
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServices]
-- Author:		John Crossen
-- Date:		10/02/2015
--
-- Purpose:		Gets the list of Appointment Type lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/11/2015	John Crossen	TFS# 2583 - Initial creation.
-- 03/10/2016   Balu Som -  ProgramID ---> optional. TFSID: 7269.
-- 03/14/2016   Balu Som - include ProgramId - for filtering  
-- 04/01/2016	Sumana Sangapu	Refactored to use the new mapping
-- 12/27/2016	Scott Martin	Refactored to use a new mapping table
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServices]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT
			s.ServicesID as ServiceID,
			ServiceName,
			osd.MappingID as ProgramID
		FROM
			Reference.ServicesOrganizationDetails so
			INNER JOIN Core.vw_GetOrganizationStructureDetails osd
				ON so.DetailID = osd.DetailID
			INNER JOIN Reference.[Services]  s 
				ON so.ServicesID = s.ServicesID
		WHERE
			so.IsActive = 1
			AND	s.IsInternal = 0 -- Return the Services that are not defined by Xenatix
		ORDER BY ServiceName ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

