-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_AddVital]
-- Author:		John Crossen
-- Date:		11/05/2015
--
-- Purpose:		Update list of Vitals by VitalID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/05/2015	John Crossen	TFS# 2894 - Initial creation.
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging
-- 11/24/2015	Scott Martin	TFS 3703	Added Waist Circumference
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_AddVital]
	@ContactID BIGINT,
	@EncounterID BIGINT NULL,
	@HeightFeet SMALLINT,
	@HeightInches SMALLINT,
	@WeightLbs SMALLINT,
	@WeightOz SMALLINT,
	@BMI DECIMAL(8,1),
	@LMP DATE,
	@TakenTime DATETIME,
	@TakenBy INT,
	@BPMethodID SMALLINT,
	@Systolic SMALLINT,
	@Diastolic SMALLINT,
	@OxygenSaturation SMALLINT,
	@RespiratoryRate SMALLINT,
	@Pulse SMALLINT,
	@Temperature DECIMAL(4,1),
	@Glucose SMALLINT,
	@WaistCircumference SMALLINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	INSERT INTO Clinical.[Vitals]
	(
		[ContactID],
		[HeightFeet],
		[HeightInches],
		[WeightLbs],
		[WeightOz],
		[BMI],
		[LMP],
		[TakenTime],
		[TakenBy],
		[BPMethodID],
		[Systolic],
		[Diastolic],
		[OxygenSaturation],
		[RespiratoryRate],
		[Pulse],
		[Temperature],
		[Glucose],
		[WaistCircumference],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn]
	)
	VALUES
	(
		@ContactID,
		@HeightFeet,
		@HeightInches,
		@WeightLbs,
		@WeightOz,
		@BMI,
		@LMP,
		@TakenTime,
		@TakenBy,
		@BPMethodID,
		@Systolic,
		@Diastolic,
		@OxygenSaturation,
		@RespiratoryRate,
		@Pulse,
		@Temperature,
		@Glucose,
		@WaistCircumference,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'Vitals', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'Vitals', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO