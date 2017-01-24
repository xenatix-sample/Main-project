-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetUserAddresses]
-- Author:		Justin Spalti
-- Date:		07/23/2015
--
-- Purpose:		Get User Address Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/30/2015	Justin Spalti - Initial procedure creation
-- 03/04/2016   Justin Spalti - Added UserAddressID to the results set
----------------------------------------------------------------------------------------------------------------------- 

CREATE PROCEDURE [Core].[usp_GetUserAddresses]
	@UserID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'

	BEGIN TRY
		SELECT u.UserID,
			   a.AddressID, a.Line1, a.Line2, a.City, a.StateProvince, a.County, a.Zip, a.ComplexName, a.GateCode, 
			   ua.MailPermissionID, ua.IsPrimary, ua.UserAddressID,
			   t.AddressTypeID,
			   a.IsActive, a.ModifiedBy, a.ModifiedOn
		FROM Core.Users u
		INNER JOIN Core.UserAddress ua
			ON ua.UserID = u.UserID
		INNER JOIN Core.Addresses a
			ON a.AddressID = ua.AddressID
		LEFT OUTER JOIN Reference.AddressType t
			ON t.AddressTypeID = a.AddressTypeID
		WHERE u.UserID = @UserID
			AND u.IsActive = 1
			AND ua.IsActive = 1
			AND a.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END