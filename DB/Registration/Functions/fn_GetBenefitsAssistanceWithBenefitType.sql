-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[fn_GetBenefitsAssistanceWithBenefitType]
-- Author:		Scott Martin
-- Date:		
--
-- Purpose:		Function to pull Benefits Assistance records that have a benefit type
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/10/2016	Scott Martin		Modified the function to query benefit types differently
-----------------------------------------------------------------------------------------------------------------------

CREATE FUNCTION Registration.fn_GetBenefitsAssistanceWithBenefitType (@StartDate DATETIME, @EndDate DATETIME)
	RETURNS @BA TABLE
	(
		BenefitsAssistanceID BIGINT
	)
AS
BEGIN
	INSERT INTO @BA
	SELECT
		BA.BenefitsAssistanceID
	FROM
		Registration.BenefitsAssistance BA WITH(NOLOCK)
		INNER JOIN Core.AssessmentResponseDetails ARD WITH(NOLOCK)
			ON BA.ResponseID = ARD.ResponseID
			AND ARD.IsActive = 1
		INNER JOIN Core.AssessmentOptions AO WITH(NOLOCK)
			ON ARD.OptionsID = AO.OptionsID
		INNER JOIN Registration.fn_GetBenefitsAssistanceBenefitTypes() BT
			ON ARD.QuestionID = BT.QuestionID
		LEFT OUTER JOIN Registration.vw_GetBenefitsAssistanceServiceRecordingDetails SRD WITH(NOLOCK)
			ON BA.BenefitsAssistanceID = SRD.SourceHeaderID
	WHERE
		SRD.ServiceStartDate >= @StartDate AND SRD.ServiceStartDate < DATEADD(DAY, 1, @EndDate)
		AND BA.IsActive = 1
		AND ISNULL(SRD.IsVoided, 0) = 0
	GROUP BY
		COALESCE(SRD.ServiceStartDate, BA.DateEntered),
		BA.BenefitsAssistanceID
	RETURN
END