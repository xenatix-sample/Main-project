-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[AuditClientViewReason]
-- Author:		Scott Martin
-- Date:		01/21/2016
--
-- Purpose:		Logs the reason for viewing the data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/21/2016	Scott Martin		Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[AuditClientViewReason](
	[AuditClientViewReasonID] [int] IDENTITY (1,1) NOT NULL,
	[ReasonText] NVARCHAR(500) NOT NULL,
	[AuditClientViewDetailID] BIGINT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AuditClientViewReason_AuditClientViewReasonID] PRIMARY KEY CLUSTERED 
(
	[AuditClientViewReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[AuditClientViewReason]  WITH CHECK ADD  CONSTRAINT [FK_AuditClientViewReason_AuditClientViewDetailID] FOREIGN KEY([AuditClientViewDetailID])
REFERENCES [Core].[AuditClientViewDetail] ([AuditClientViewDetailID])
GO

ALTER TABLE [Core].[AuditClientViewReason] CHECK CONSTRAINT [FK_AuditClientViewReason_AuditClientViewDetailID]
GO