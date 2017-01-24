-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateVital]
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
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateVital]
	@VitalID BIGINT,
	@ContactID BIGINT,
	@EncounterID BIGINT,
	@HeightFeet SMALLINT,
	@HeightInches SMALLINT,
	@WeightLbs SMALLINT,
	@WeightOz SMALLINT,
	@BMI DECIMAL(8,1),
	@LMP DATE,
	@VitalTakenTime DATETIME,
	@VitalTakenBy INT,
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
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'Vitals', @VitalID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.[Vitals]
	SET ContactID = @ContactID,
		EncounterID = @EncounterID,
		HeightFeet = @HeightFeet,
		HeightInches = @HeightInches,
		WeightLbs = @WeightLbs,
		WeightOz = @WeightOz,
		BMI = @BMI,
		LMP = @LMP,
		TakenTime = @VitalTakenTime,
		TakenBy = @VitalTakenBy,
		BPMethodID = @BPMethodID,
		Systolic = @Systolic,
		Diastolic = @Diastolic,
		OxygenSaturation = @OxygenSaturation,
		[RespiratoryRate] = @RespiratoryRate,
		[Pulse] = @Pulse,
		[Temperature] = @Temperature,
		[Glucose] = @Glucose,
		WaistCircumference = @WaistCircumference,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		VitalID = @VitalID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'Vitals', @AuditDetailID, @VitalID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO