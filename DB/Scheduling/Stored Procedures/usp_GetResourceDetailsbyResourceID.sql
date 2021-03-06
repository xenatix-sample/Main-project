-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetResourceDetailsbyResourceID]
-- Author:		Sumana Sangapu
-- Date:		03/16/2016
--
-- Purpose:		Gets the resource details from Appointment Resource for that ResourceID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/16/2016	Sumana Sangapu		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetResourceDetailsbyResourceID]
	@AppointmentID bigint,
    @ResourceID SMALLINT,
	@ResourceTypeID smallint,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		
		-- ResourceType IN (User,Co-Provider) then fetch details from Core.Users
		IF @ResourceTypeID IN (2,3) 
		BEGIN

		      SELECT U.UserID AS ResourceID, @ResourceTypeID AS ResourceTypeID, 
					 U.FirstName + ' ' +  U.LastName  AS ResourceName
              FROM   Scheduling.AppointmentResource ar
			  JOIN	 Core.Users U 
			  ON	 ar.ResourceID= U.UserID   
			  AND	 ar.IsActive =  U.IsActive         
              WHERE	 ar.AppointmentID = @AppointmentID	
			  AND	 ar.ResourceTypeID = @ResourceTypeID
			  AND	 ar.ResourceID = @ResourceID
			  AND	 ar.IsActive=1 -- this condition should be checked.
		END
		
		-- ResourceType IN (Contact) then fetch details from Registration.Contact
		IF @ResourceTypeID = 7
		BEGIN
		      SELECT c.ContactID AS ResourceID, @ResourceTypeID AS ResourceTypeID, 
					 c.FirstName + ' ' +  c.LastName  AS ResourceName
              FROM   Scheduling.AppointmentResource ar
			  JOIN	 Registration.Contact c 
			  ON	 ar.ResourceID= c.ContactID   
			  AND	 ar.IsActive =  c.IsActive         
              WHERE	 ar.AppointmentID = @AppointmentID	
			  AND	 ar.ResourceTypeID = @ResourceTypeID
			  AND	 ar.ResourceID = @ResourceID
			  AND	 ar.IsActive=1
		END
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END