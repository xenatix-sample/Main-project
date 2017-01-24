-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[MergedAssessmentResponse]
-- Author:		Scott Martin
-- Date:		12/12/2016
--
-- Purpose:		Staging table for merging Assessments to a new contact
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/12/2016	Scott Martin	Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[MergedAssessmentResponse]
(
	[TransactionLogID] bigint NOT NULL,
	[ResponseID] bigint,
	[NewResponseID] bigint
);
GO

ALTER TABLE [Core].[MergedAssessmentResponse]  WITH CHECK ADD  CONSTRAINT [FK_MergedAssessmentResponse_TransasctionLogID] FOREIGN KEY([TransactionLogID]) REFERENCES [Core].[TransactionLog] ([TransactionLogID])
GO
ALTER TABLE [Core].[MergedAssessmentResponse] CHECK CONSTRAINT [FK_MergedAssessmentResponse_TransasctionLogID]
GO