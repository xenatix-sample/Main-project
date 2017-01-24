-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAppointmentTypes]
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
-- 10/13/2015   John Crossen    TFS# 2699 - Add ProgramID
-- 12/05/2015	D. Christopher	TFS# 1739 - Pass 0 as ProgramID to get entire list
-- 01/18/2016   Satish Singh    TFS# 5412   Added ProgramID in return
-- 03/03/2016	Scott Martin	Added parameter to filter out IsBlocked appointment types
-- 04/01/2016	Sumana Sangapu	Refactored to use the new mapping
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetAppointmentTypes]
    @ProgramID INT,
    @ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@IncludeBlocked BIT = 1
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT	a.AppointmentTypeID as AppointmentTypeID, AppointmentType,ao.OrganizationDetailsMappingID as  ProgramID
		FROM	[Scheduling].AppointmentTypeOrgDetailsMapping ao
		INNER JOIN [Scheduling].[AppointmentType]  a
		ON		ao.AppointmentTypeID = a.AppointmentTypeID 
		WHERE	ao.IsActive = 1 
		AND		a.IsActive = 1 
		AND		ao.OrganizationDetailsMappingID = CASE WHEN @ProgramID = 0 THEN OrganizationDetailsMappingID ELSE @ProgramID END 
		AND		(IsBlocked = 0 or IsBlocked = @IncludeBlocked)
		ORDER BY AppointmentType ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END