-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAppointmentTypeCredential]
-- Author:		John Crossen
-- Date:		10/02/2015
--
-- Purpose:		Gets the list of Appointment Type Credential lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/11/2015	John Crossen	TFS# 2703 - Initial creation.
-- 10/28-2015    John Crossen    TFS#3105 -- Add check for AppointmentType Isactive
-- 12/10/2015    Rajiv Ranjan    Added CredentialAbbreviation
-- 01/15/2016    Satish Singh    @AppointmentTypeID optional for where clause
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Scheduling.[usp_GetAppointmentTypeCredential]
	@AppointmentTypeID INT=NULL,
    @ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT	
			AppointmentTypeCredentialID, 
			AC.CredentialID, 
			CR.CredentialName,
			CR.CredentialAbbreviation,
			AC.AppointmentTypeID
		FROM Scheduling.AppointmentTypeCredential  AC JOIN Reference.[Credentials] CR ON CR.CredentialID = AC.CredentialID
		JOIN Scheduling.AppointmentType AT on AC.AppointmentTypeID=AT.AppointmentTypeID
		WHERE  AC.IsActive = 1 
		AND (ISNULL(@AppointmentTypeID,0)=0 OR AC.AppointmentTypeID =@AppointmentTypeID)
		AND AT.IsActive=1
		ORDER BY CR.CredentialName ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


