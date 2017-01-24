

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetFinancialAsessmentDetails]
-- Author:		Sumana Sangapu
-- Date:		08/07/2015
--
-- Purpose:		Get Financial Assessment Details List 
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/07/2015	Sumana Sangapu Task - 634	Initial creation
-- 08/18/2015	Suresh Pandey	Add FinancialAssessmentDetailsID column in select statement
-- 08/18/2015	Suresh Pandey	Removed column alise for FinanceFrequencyID from  select statement
-- 08/24/2015	Suresh Pandey	Add order by effective data desc
-- 08/25/2015	Suresh Pandey	change inner join to left join in [Reference].[Category] table. b'cos categoryId is null type.
-- 08/26/2015	Suresh Pandey	Added IsActive =1 check on Registration.FinancialAssessmentDetails table
-- 09/03/2015	Suresh Pandey	Remove the left join from category table	
-- 09/18/2015	Sumana Sangapu	2370	Remove and Add fields to FA screen

-- exec [Registration].[usp_GetFinancialAssessmentDetails] 16,'',''
-- 12/23/2015	Scott Martin	Added RelationshipTypeID and RelationshipType
----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetFinancialAssessmentDetails]
@FinancialAssessmentID BIGINT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

		SELECT	fad.FinancialAssessmentDetailsID,
				fa.FinancialAssessmentID AS FinancialAssessmentID, 
				fa.ContactID AS ContactID, 
				fad.CategoryTypeID, 
				ct.CategoryType AS FinancialCategoryType,
				Amount,
				f.FinanceFrequencyID, 
				f.FinanceFrequency as Frequency, 
				c.CategoryID, 
				c.Category,
				rt.RelationshipTypeID,
				rt.RelationshipType
		FROM    Registration.FinancialAssessment fa
		INNER	JOIN Registration.FinancialAssessmentDetails fad
		ON		fa.FinancialAssessmentID = fad.FinancialAssessmentID
		INNER   JOIN [Reference].[CategoryType] ct
		ON		fad.[CategoryTypeID] = ct.[CategoryTypeID]
		INNER   JOIN [Reference].[FinanceFrequency] f 
		ON		fad.FinanceFrequencyID = f.FinanceFrequencyID
		INNER	JOIN [Reference].[Category] c
		ON		fad.CategoryID = c.CategoryID
		INNER	JOIN [Reference].[RelationshipType] rt
		ON		fad.RelationshipTypeID = rt.RelationshipTypeID
		WHERE   fa.FinancialAssessmentID = @FinancialAssessmentID
		AND		fa.IsActive =1 
		AND		fad.IsActive =1 
		ORDER BY FinancialAssessmentDetailsID DESC

	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
