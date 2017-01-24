-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_MHMRAddPayor]
-- Author:		Kyle Campbell
-- Date:		11/14/2016
--
-- Purpose:		Temporary proc to allow MHMR Tarrant to add their own payors outside of aXis
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/14/2016	Kyle Campbell		TFS# 17715 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[usp_MHMRAddPayor]	
	@PayorCode int,
	@PayorName nvarchar(250),
	@EffectiveDate date,
	@ExpirationDate date,
	@PlanName nvarchar(250),
	@AddressLine1 nvarchar(200),
	@AddressLine2 nvarchar(200),
	@City nvarchar(200),
	@State nvarchar(2),
	@Zip nvarchar(10),
	@Address_Not_Applicable bit,
	@ContractID nvarchar(20),	
	@ElectronicPayorID nvarchar(20),
	@ModifiedOn datetime,
	@ModifiedBy int,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
SET NOCOUNT ON;
DECLARE @AuditDetailID bigint,
		@ProcName varchar(255) = OBJECT_NAME(@@PROCID),
		@PayorID int = NULL,
		@PayorPlanID int = NULL,
		@StateID int = NULL,
		@AddressID bigint = NULL,
		@PayorAddressID int = NULL

SELECT	@ResultCode = 0,
		@ResultMessage = 'executed successfully';

BEGIN TRY	
    BEGIN TRANSACTION
	
	SET @PayorCode = LTRIM(RTRIM(@PayorCode));
	SET @PayorName = LTRIM(RTRIM(@PayorName));
	SET @EffectiveDate = LTRIM(RTRIM(@EffectiveDate));
	SET @ExpirationDate = LTRIM(RTRIM(@ExpirationDate));
	SET @PlanName = LTRIM(RTRIM(@PlanName));
	SET @AddressLine1 = LTRIM(RTRIM(@AddressLine1));
	SET @AddressLine2 = LTRIM(RTRIM(@AddressLine2));
	SET @City = LTRIM(RTRIM(@City));
	SET @State = LTRIM(RTRIM(@State));
	SET @Zip = LTRIM(RTRIM(@Zip));
	
	SET @EffectiveDate = CASE @EffectiveDate WHEN '1900-01-01' THEN NULL ELSE @EffectiveDate END;
	SET @ExpirationDate = CASE @ExpirationDate WHEN '1900-01-01' THEN NULL ELSE @ExpirationDate END;

	IF ISNULL(LEN(@PayorCode), 0) = 0 
		OR @PayorCode = 0
		OR ISNULL(LEN(@PayorName), 0) = 0
		OR ISNULL(LEN(@PlanName), 0) = 0
		OR ISNULL(LEN(@ContractID), 0) = 0

		BEGIN
			RAISERROR(N'PayorCode, PayorName, PlanName and ContractID are required', 16, 1)
		END

	IF EXISTS (SELECT 'X' FROM Registration.PayorAddress WHERE ContactID = @ContractID)
		BEGIN
			RAISERROR(N'This ContractID already exists', 16, 1)
		END


	--ADD ADDRESS TO CORE.ADDRESSES IF NECESSARY--
	IF ISNULL(@Address_Not_Applicable, 0) = 0
		BEGIN
			IF (
				ISNULL(LEN(@AddressLine1), 0) = 0
				AND ISNULL(LEN(@AddressLine2), 0) = 0
				AND ISNULL(LEN(@City), 0) = 0
				AND ISNULL(LEN(@State), 0) = 0
				AND ISNULL(LEN(@Zip), 0) = 0			
				)
				BEGIN
					RAISERROR(N'No address information was entered', 16, 1)
				END

			--GET StateProvinceCode if State was specified
			IF ISNULL(LEN(@State), 0) > 0
				BEGIN
					SELECT @StateID = StateProvinceID FROM Reference.StateProvince WHERE StateProvinceCode = @State				
					IF ISNULL(@StateID, 0) = 0
						BEGIN
							RAISERROR(N'Invalid state entered', 16, 1)
						END
				END
			ELSE
				BEGIN
					SET @StateID = NULL;
				END

			SELECT @AddressID = AddressID 
			FROM Core.Addresses 
			WHERE 
				Line1 = @AddressLine1 
				AND Line2 = @AddressLine2
				AND City = @City
				AND StateProvince = @StateID
				AND Zip = @Zip

			IF ISNULL(@AddressID, 0) = 0
				BEGIN
					INSERT INTO Core.Addresses
					(
						AddressTypeID,
						Line1,
						Line2,
						City,
						StateProvince,
						Zip,
						Country,
						IsActive,
						ModifiedBy,
						ModifiedOn
					)
					VALUES
					(
						'2',
						@AddressLine1,
						@AddressLine2,
						@City,
						@StateID,
						@Zip,
						'235',
						'1',
						@ModifiedBy,
						@ModifiedOn
					);

					SELECT @AddressID = SCOPE_IDENTITY();

					EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'Addresses', @AddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

					EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'Addresses', @AuditDetailID, @AddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				END	
		END
	ELSE
		--SET AddressID to the Not Applicable Address in Core.Addresses
		BEGIN
			IF (
				ISNULL(LEN(@AddressLine1), 0) > 0
				OR ISNULL(LEN(@AddressLine2), 0) > 0
				OR ISNULL(LEN(@City), 0) > 0
				OR ISNULL(LEN(@State), 0) > 0
				OR ISNULL(LEN(@Zip), 0) > 0			
				)
				BEGIN
					RAISERROR(N'Address information has been entered with @Address_Not_Applicable set to true', 16, 1)
				END
			SELECT @AddressID = AddressID FROM Core.Addresses WHERE Line1 = 'Not Applicable' AND IsActive = 1;
		END
	
	--CHECK IF PAYOR ALREADY EXISTS
	SELECT @PayorID = PayorID FROM Reference.Payor WHERE PayorCode = @PayorCode AND PayorName = @PayorName;
	
	IF ISNULL(@PayorID, 0) = 0
		BEGIN
			--ADD PAYOR--
			INSERT INTO Reference.Payor
			(
				PayorCode,
				PayorName,
				EffectiveDate,
				ExpirationDate,
				IsActive,
				ModifiedBy,
				ModifiedOn
			)
			VALUES
			(
				@PayorCode,
				@PayorName,
				@EffectiveDate,
				@ExpirationDate,
				'1',
				@ModifiedBy,
				@ModifiedOn
			);
			SELECT @PayorID = SCOPE_IDENTITY();
			
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'Payor', @PayorID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'Payor', @AuditDetailID, @PayorID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		END	

	--CHECK IF PAYOR PLAN ALREADY EXISTS
	SELECT @PayorPlanID = PayorPlanID FROM Reference.PayorPlan WHERE PayorID = @PayorID AND PlanName = @PlanName;

	IF ISNULL(@PayorPlanID, 0) = 0
		BEGIN
			--ADD PAYOR PLAN--
			INSERT INTO Reference.PayorPlan
			(
				PayorID,
				PlanName,
				IsActive,
				ModifiedBy,
				ModifiedOn
			)
			VALUES
			(
				@PayorID,
				@PlanName,
				'1',
				@ModifiedBy,
				@ModifiedOn
			);
			SELECT @PayorPlanID = SCOPE_IDENTITY();
			
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'PayorPlan', @PayorPlanID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'PayorPlan', @AuditDetailID, @PayorPlanID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END	

	--ADD PAYOR ADDRESS--
	INSERT INTO Registration.PayorAddress
	(
		PayorPlanID,
		AddressID,
		ContactID,
		ElectronicPayorID,
		IsActive,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@PayorPlanID,
		@AddressID,
		@ContractID,
		@ElectronicPayorID,
		'1',
		@ModifiedBy,
		@ModifiedOn
	);
	SELECT @PayorAddressID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'PayorAddress', @PayorAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'PayorAddress', @AuditDetailID, @PayorAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH	
    SELECT 
        @ResultMessage = ERROR_MESSAGE(),
        @ResultCode = ERROR_SEVERITY();        
    ROLLBACK TRANSACTION
END CATCH

END