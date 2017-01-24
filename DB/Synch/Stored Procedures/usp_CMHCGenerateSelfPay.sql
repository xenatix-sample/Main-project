-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Synch.usp_CMHCGenerateSelfPay
-- Author:		Sumana Sangapu
-- Date:		07/09/2016
--
-- Purpose:		Generate Client Selfpay data for CMHC
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/09/2016	Sumana Sangapu		Initial creation
-- 08/08/2016	Sumana Sangapu		Filter only for contacttypeid=1 and MRN is not NULL
-- 09/14/2016	Sumana Sangapu		Insert NULL as ErrorMessage
-- 10/11/2016	Sumana Sangapu		Return the latest unexpired Financial assessment
-- 11/14/2016	Sumana Sangapu		Isactive = 1 
-- 11/29/2016	Sumana Sangapu		Return records that fall whose SelfPay EffectiveDate falls between  FinancialAssesment AssessmentDate and ExpirationDate
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Synch.usp_CMHCGenerateSelfPay
@BatchID BIGINT,
@LastRunDate DATETIME
AS
BEGIN
		TRUNCATE TABLE Synch.CMHCSelfPay

		INSERT INTO Synch.CMHCSelfPay
		SELECT  
		SP.SelfPayID,
		SP.ContactID,
		STUFF(MRN, 1, 0, REPLICATE('0', 9 - LEN(MRN))) AS MRN,-- Padded to 9,
		CONVERT(varchar,SP.EffectiveDate,101),
		FA.TotalIncome,
		FA.FamilySize,
		SP.SelfPayAmount,
		CASE SP.IsPercent WHEN 1 THEN 'TRUE' WHEN 0 THEN 'FALSE' END AS IsPercent, 
		OS.Name AS Division,
		CONVERT(varchar,sp.ExpirationDate,101),
		@BatchID,
		NULL as ErrorMessage
		FROM Registration.SelfPay SP  
		LEFT JOIN Registration.FinancialAssessment FA 
		ON		  FA.ContactID = SP.ContactID  
		AND		  FA.AssessmentDate <= SP.[EffectiveDate]
        AND       FA.ExpirationDate >= SP.[EffectiveDate] 
		LEFT JOIN Registration.Contact C 
		ON		  C.ContactID = SP.ContactID
		LEFT OUTER JOIN Core.vw_GetOrganizationStructureDetails OS 
		ON		  SP.OrganizationDetailID = OS.MappingID
		WHERE	  C.ContactTypeID = 1 
		AND		  C.MRN IS NOT NULL
		AND		  SP.IsActive = 1
		AND		 ((SP.[SystemCreatedOn] >= ISNULL(@LastRunDate,'') OR SP.[SystemModifiedOn] >= ISNULL(@LastRunDate,'')) OR 
				 (FA.[SystemCreatedOn] >=ISNULL(@LastRunDate,'') OR FA.[SystemModifiedOn] >= ISNULL(@LastRunDate,''))) 

  END
