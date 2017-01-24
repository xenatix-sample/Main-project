-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetClinicalIntakeStatus]
-- Author:		Rajiv Ranjan
-- Date:		12/07/2015
--
-- Purpose:		Get clinical intake Status
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/07/2015	Rajiv Ranjan	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_GetClinicalIntakeStatus]
@ContactID BIGINT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT 
		'patientprofile.chart.intake.chiefcomplaint' AS [Key], 
		CASE WHEN COUNT(1) > 0 THEN 'valid' ELSE 'warning' END AS Value
	FROM 
		Clinical.ChiefComplaint cc 
	WHERE 
		cc.ContactID=@ContactID 
		AND cc.IsActive=1

	UNION 

	SELECT 
		'patientprofile.chart.intake.vitals', 
		CASE WHEN COUNT(1) > 0 THEN 'valid' ELSE 'warning' END 
	FROM 
		Clinical.Vitals v 
	WHERE 
		v.ContactID=@ContactID 
		AND v.IsActive=1

	UNION 

	SELECT 
		'patientprofile.chart.intake.allergy', 
		CASE WHEN COUNT(1) > 0 THEN 'valid' ELSE 'warning' END
	FROM 
		Clinical.ContactAllergy ca
		LEFT JOIN Clinical.ContactAllergyDetail ad on ca.ContactAllergyID=ad.ContactAllergyID AND ad.IsActive=1
	WHERE 
		ca.ContactID=@ContactID 
		AND ca.IsActive = 1		
		AND ca.AllergyTypeID = 1

	UNION 

	SELECT 
		'patientprofile.chart.intake.drugallergy', 
		CASE WHEN COUNT(1) > 0 THEN 'valid' ELSE 'warning' END
	FROM 
		Clinical.ContactAllergy ca
		LEFT JOIN Clinical.ContactAllergyDetail ad on ca.ContactAllergyID=ad.ContactAllergyID AND ad.IsActive=1
	WHERE 
		ca.ContactID=@ContactID 
		AND ca.IsActive=1
		AND ca.AllergyTypeID=2

	UNION 

	SELECT 
		'patientprofile.chart.intake.drugintolerance', 
		CASE WHEN COUNT(1) > 0 THEN 'valid' ELSE 'warning' END
	FROM 
		Clinical.ContactAllergy ca
		LEFT JOIN Clinical.ContactAllergyDetail ad on ca.ContactAllergyID=ad.ContactAllergyID AND ad.IsActive=1
	WHERE 
		ca.ContactID=@ContactID 
		AND ca.IsActive=1
		AND ca.AllergyTypeID=3

	UNION 

	SELECT 
		'patientprofile.chart.intake.presentillness', 
		CASE WHEN COUNT(1) > 0 THEN 'valid' ELSE 'warning' END
	FROM 
		Clinical.HPI h
		INNER JOIN Clinical.HPIDetail hd on h.HPIID=hd.HPIID AND hd.IsActive=1
	WHERE 
		h.ContactID=@ContactID 
		AND h.IsActive=1

	UNION 

	SELECT 
		'patientprofile.chart.intake.reviewOfSystems', 
		CASE WHEN COUNT(1) > 0 THEN 'valid' ELSE 'warning' END 
	FROM 
		Clinical.ReviewOfSystems ros
	WHERE 
		ros.ContactID=@ContactID 
		AND ros.IsActive=1

	UNION 

	SELECT 
		'patientprofile.chart.intake.medicalhistory', 
		CASE WHEN COUNT(1) > 0 THEN 'valid' ELSE 'warning' END 
	FROM 
		Clinical.MedicalHistory mh
	WHERE 
		mh.ContactID=@ContactID 
		AND mh.IsActive=1

	UNION 

	SELECT 
		'patientprofile.chart.intake.socialrelationship', 
		CASE WHEN COUNT(1) > 0 THEN 'valid' ELSE 'warning' END
	FROM 
		Clinical.SocialRelationship s
	WHERE 
		s.ContactID=@ContactID 
		AND s.IsActive=1

	UNION 

	SELECT 
		'patientprofile.chart.intake.clinicalAssessment', 
		CASE WHEN COUNT(1) > 0 THEN 'valid' ELSE 'warning' END
	FROM 
		Clinical.ClinicalAssessments ca
	WHERE 
		ca.ContactID=@ContactID 
		AND ca.IsActive=1

	UNION 

	SELECT 
		'patientprofile.chart.intake.note', 
		CASE WHEN COUNT(1) > 0 THEN 'valid' ELSE 'warning' END
	FROM 
		Clinical.Notes n
	WHERE 
		n.ContactID=@ContactID 
		AND n.IsActive=1

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END