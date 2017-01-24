----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetProviderByid]
-- Author:		Rajiv Ranjan
-- Date:		10/07/2016
--
-- Purpose:		Gets the users corresponding to ID 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/07/2016		Rajiv Ranjan		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetProviderById]
	@ProviderID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
		SELECT DISTINCT
		    U.UserID AS ID,
			CONCAT(U.FirstName, ' ', U.LastName) AS Name,
			pn.Number as Number,
			pn.PhoneID as ContactNumberID,
			up.UserPhoneID as PhoneID
		FROM 
			Core.Users U
				LEFT JOIN Core.UserPhone up
					ON up.UserID= u.UserID
						AND up.IsPrimary= 1
						AND up.IsActive= 1
				LEFT JOIN Core.Phone pn
					ON pn.PhoneID= up.PhoneID 
		WHERE 
			U.USERID=@ProviderID
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO