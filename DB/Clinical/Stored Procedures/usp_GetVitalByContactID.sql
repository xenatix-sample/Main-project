-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetVitalByContactID]
-- Author:		John Crossen
-- Date:		11/05/2015
--
-- Purpose:		Get list of Vitals by ContactID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/05/2015	John Crossen	TFS# 2894 - Initial creation.
-- 11/24/2015	Arun Chaudhary	Added TakenByName.
-- 11/24/2015	Scott Martin	TFS 3703	Added Waist Circumference
-- 11/26/2015	D. Christopher	TFS 3811	Added IsActive filter to exclude deleted records
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE Clinical.[usp_GetVitalByContactID]

@ContactID BIGINT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'


SELECT [VitalID]
      ,[ContactID]
	  ,EncounterID
      ,[HeightFeet]
      ,[HeightInches]
      ,[WeightLbs]
      ,[WeightOz]
      ,[BMI]
      ,[LMP]
      ,[TakenTime]
	  ,[TakenBy]
      ,[BPMethodID]
      ,[Systolic]
      ,[Diastolic]
      ,[OxygenSaturation]
      ,[RespiratoryRate]
      ,[Pulse]
      ,[Temperature]
      ,[Glucose]
	  ,[WaistCircumference]
      ,[IsActive]
      ,[ModifiedBy]
      ,[ModifiedOn]
  FROM Clinical.[Vitals]
   WHERE ContactID=@ContactID
   AND IsActive = 1



 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END