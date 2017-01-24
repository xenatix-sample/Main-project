-----------------------------------------------------------------------------------------------------------------------
-- Procedure:  [usp_AddOrganizationDetails]
-- Author:		Kyle Campbell
-- Date:		12/12/2016
--
-- Purpose:		Add Organization Detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/12/2016	Kyle Campbell	TFS #17998	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddOrganizationDetails]
	@DataKey NVARCHAR(50),
	@Name NVARCHAR(100),
	@Acronym NVARCHAR(20) NULL,
	@Code NVARCHAR(20) NULL,
	@EffectiveDate DATE,
	@ExpirationDate DATE NULL,
	@IsExternal BIT,
	@IsActive BIT NULL,
	@ModifiedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN

SET NOCOUNT ON;

DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditDetailID BIGINT,
		@AttributeID INT,
		@AttributeMappingID BIGINT;
		

	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully';

		SELECT @AttributeID = AttributeID FROM Core.OrganizationAttributes WHERE DataKey = @DataKey;

		IF ISNULL(@AttributeID,0) = 0
			RAISERROR('Invalid DataKey', 16, 0)

		INSERT INTO Core.OrganizationDetails
		(
			Name,
			Acronym,
			Code,
			IsExternal,
			EffectiveDate,
			ExpirationDate,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@Name,
			@Acronym,
			@Code,
			@IsExternal,
			@EffectiveDate,
			@ExpirationDate,
			@IsActive,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		)

		SELECT @ID = SCOPE_IDENTITY();
		
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'OrganizationDetails', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'OrganizationDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;


		INSERT INTO Core.OrganizationAttributesMapping
		(
			AttributeID,
			DetailID,
			IsActive,
			ModifiedBy,
			ModifiedOn
		)
		VALUES
		(
			@AttributeID,
			@ID,
			1,
			@ModifiedBy,
			@ModifiedOn
		)

		SELECT @AttributeMappingID = SCOPE_IDENTITY();

		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'OrganizationAttributesMapping', @AttributeMappingID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'OrganizationAttributesMapping', @AuditDetailID, @AttributeMappingID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE(),
			   @ID=0;
	END CATCH
END