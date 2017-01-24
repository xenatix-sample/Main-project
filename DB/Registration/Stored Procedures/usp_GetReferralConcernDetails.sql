-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetReferralConcernDetails]
-- Author:		Sumana Sangapu
-- Date:		12/15/2015
--
-- Purpose:		Get Referral Concern details for Referral Client screen
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015   Sumana Sangapu  Initial Creation
-- 12/15/2015	Sumana Sangapu	Removed the parameters
-- 12/31/2015   Vishal Joshi    Added ReferralConcernDetailID,ReferralAdditionalDetailID 
-- 01/07/2015   Lokesh Singhal  Left join with referral concern table for referralconcern and select IsActive Column
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Registration].[usp_GetReferralConcernDetails]
		@ReferralAdditionalDetailID				BIGINT,
		@ResultCode								INT OUTPUT,
		@ResultMessage							NVARCHAR(500) OUTPUT
AS
BEGIN
		
		SELECT 	@ResultCode = 0,
			    @ResultMessage = 'executed successfully'

		BEGIN TRY

			SELECT  
					RCD.ReferralConcernDetailID,
					RCD.ReferralAdditionalDetailID,
					RCD.ReferralConcernID , 
					RCD.Diagnosis,
					RCD.ReferralPriorityID,
					RC.ReferralConcern,
					RCD.IsActive ,
					RCD.ModifiedBy , 
					RCD.ModifiedOn,
					RCD.IsActive 
			FROM	Registration.ReferralConcernDetails RCD
			LEFT JOIN Reference.ReferralConcern RC ON RCD.ReferralConcernID = RC.ReferralConcernID
			WHERE	ReferralAdditionalDetailID = @ReferralAdditionalDetailID
			AND		RCD.IsActive = 1
			 
		END TRY
		BEGIN CATCH
			SELECT 
				   @ResultCode = ERROR_SEVERITY(),
				   @ResultMessage = ERROR_MESSAGE()
		END CATCH
END
GO