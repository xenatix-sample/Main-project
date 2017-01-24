-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAddress]
-- Author:		Demetrios C. Christopher
-- Date:		08/06/2015
--
-- Purpose:		Get Address Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/06/2015 - Demetrios Christopher - Initial draft
-- 09/22/2015   Gurpreet Singh      Changed AptComplexName to ComplexName
----------------------------------------------------------------------------------------------------------------------- 

CREATE PROCEDURE [Core].[usp_GetAddress]
	@AddressID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

		SELECT 
			a.AddressID, 
			at.AddressTypeID, 
			a.Line1, 
			a.Line2, 
			a.City, 
			a.StateProvince, 
			a.County, 
			a.Zip, 
			a.ComplexName, 
			a.GateCode,
			a.IsActive,
			a.IsVerified
		FROM 
			[Core].[Addresses] a
			LEFT OUTER JOIN Reference.AddressType at ON a.AddressTypeID = at.AddressTypeID
		WHERE 
			a.AddressID = @AddressID

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END