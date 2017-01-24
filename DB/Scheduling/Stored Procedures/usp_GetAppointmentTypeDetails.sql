-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAppointmentTypeDetails]
-- Author:		John Crossen
-- Date:		09/11/2015
--
-- Purpose:		Gets the list of Appointment Type lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/11/2015	John Crossen	TFS# 2275 - Initial creation.
-- 03/03/2016	Scott Martin	Added parameter to filter out IsBlocked appointment types
-- 04/01/2016	Sumana Sangapu	Refactored to use the new mapping
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetAppointmentTypeDetails]
    @ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ProgramID INT,
	@IncludeBlocked BIT = 0
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT	a.AppointmentTypeID, AppointmentType
		FROM	[Scheduling].AppointmentTypeOrgDetailsMapping ao
		INNER JOIN [Scheduling].[AppointmentType]  a
		ON		ao.AppointmentTypeID = a.AppointmentTypeID
		WHERE	ao.IsActive = 1 
		AND		ao.OrganizationDetailsMappingID = @ProgramID 
		AND		(IsBlocked = 0 or IsBlocked = @IncludeBlocked)
		ORDER BY AppointmentType ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


