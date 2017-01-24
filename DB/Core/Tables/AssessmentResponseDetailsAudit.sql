-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[AssessmentResponseDetailsAudit]
-- Author:		Scott Martin
-- Date:		04/25/2016
--
-- Purpose:		Holds a snapshot of Assessment and Response data 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/25/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[AssessmentResponseDetailsAudit](
	[AssessmentResponseDetailsAuditID] [bigint] NOT NULL IDENTITY(1,1),
	[ResponseID] BIGINT NOT NULL,
	[AuditXML] XML NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AssessmentResponseDetailsAudit_AssessmentResponseDetailsAuditID] PRIMARY KEY CLUSTERED 
(
	[AssessmentResponseDetailsAuditID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[AssessmentResponseDetailsAudit]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentResponseDetailsAudit_ResponseID] FOREIGN KEY([ResponseID]) REFERENCES [Core].[AssessmentResponses] ([ResponseID])
GO
ALTER TABLE [Core].[AssessmentResponseDetailsAudit] CHECK CONSTRAINT [FK_AssessmentResponseDetailsAudit_ResponseID]
GO

ALTER TABLE Core.AssessmentResponseDetailsAudit WITH CHECK ADD CONSTRAINT [FK_AssessmentResponseDetailsAudit_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentResponseDetailsAudit CHECK CONSTRAINT [FK_AssessmentResponseDetailsAudit_UserModifedBy]
GO
ALTER TABLE Core.AssessmentResponseDetailsAudit WITH CHECK ADD CONSTRAINT [FK_AssessmentResponseDetailsAudit_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.AssessmentResponseDetailsAudit CHECK CONSTRAINT [FK_AssessmentResponseDetailsAudit_UserCreatedBy]
GO
