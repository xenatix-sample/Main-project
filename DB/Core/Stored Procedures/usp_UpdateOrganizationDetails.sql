-----------------------------------------------------------------------------------------------------------------------
-- Procedure:  [usp_UpdateOrganizationDetails]
-- Author:		Kyle Campbell
-- Date:		12/20/2016
--
-- Purpose:		Update Organization Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/20/2016	Kyle Campbell	TFS #17998	Initial Creation
-- 01/18/2017	Scott Martin	Added code that will update the expiration date on all Organization Detail Mappings linked to Detail
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateOrganizationDetails]
	@DetailID BIGINT,
	@Name NVARCHAR(100),
	@Acronym NVARCHAR(20) NULL,
	@Code NVARCHAR(20) NULL,
	@EffectiveDate DATE,
	@ExpirationDate DATE NULL,
	@IsExternal BIT NULL,
	@IsActive BIT NULL,
	@ModifiedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

SET NOCOUNT ON;

DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditDetailID BIGINT;		

	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully';

		IF @ExpirationDate IS NOT NULL
			BEGIN
			EXEC Core.usp_UpdateOrganizationDetailsMappingExpirationDate @DetailID, @ExpirationDate, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
			END

		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'OrganizationDetails', @DetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.OrganizationDetails 
		SET
			Name = @Name,
			Acronym = @Acronym,
			Code = @Code,
			EffectiveDate = @EffectiveDate,
			ExpirationDate = @ExpirationDate,
			IsExternal = @IsExternal,
			IsActive = @IsActive,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn
		WHERE DetailID = @DetailID;		

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'OrganizationDetails', @AuditDetailID, @DetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()			   
	END CATCH
END