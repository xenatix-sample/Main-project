-----------------------------------------------------------------------------------------------------------------------
-- Table:	    Core.AuditReason
-- Author:		John Crossen
-- Date:		08/03/2015
--
-- Purpose:		Audit functionality
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/03/2015	John Crossen		TFS# 866 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-- 09/16/2016	Scott Martin	Moved to Audit Schema
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Auditing].[AuditReason](
	[AuditReasonId] [bigint] IDENTITY(1,1) NOT NULL,
	[AuditId] [bigint] NOT NULL,
	[ReasonText] [nvarchar](max) NULL,
 CONSTRAINT [PK_AuditReason] PRIMARY KEY CLUSTERED 
(
	[AuditReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO
ALTER TABLE [Auditing].[AuditReason]  WITH CHECK ADD  CONSTRAINT [FK_AuditReason_Audit] FOREIGN KEY([AuditId]) REFERENCES [Auditing].[Audits] ([AuditId])
GO
ALTER TABLE [Auditing].[AuditReason] CHECK CONSTRAINT [FK_AuditReason_Audit]
GO
