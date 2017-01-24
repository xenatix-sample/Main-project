-----------------------------------------------------------------------------------------------------------------------
-- Table:	    Core.AuditClientViewDetail
-- Author:		Scott Martin
-- Date:		01/21/2016
--
-- Purpose:		Auditing identifying what data a user is viewing in the application
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/21/2016	Scott Martin		Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[AuditClientViewDetail](
	[AuditClientViewDetailID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[AuditID] [BIGINT] NOT NULL,
	[AuditClientViewCodeID] INT NOT NULL,
	[EntityID] BIGINT NULL,
	[EntityTypeID] BIGINT NULL,
	[ViewedValue] NVARCHAR(MAX) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AuditClientViewDetail_AuditClientViewDetailID] PRIMARY KEY CLUSTERED 
(
	[AuditClientViewDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [Core].[AuditClientViewDetail]  WITH CHECK ADD  CONSTRAINT [FK_AuditClientViewDetail_AuditID] FOREIGN KEY([AuditID])
REFERENCES [Core].[Audits] ([AuditID])
GO

ALTER TABLE [Core].[AuditClientViewDetail] CHECK CONSTRAINT [FK_AuditClientViewDetail_AuditID]
GO

ALTER TABLE [Core].[AuditClientViewDetail]  WITH CHECK ADD  CONSTRAINT [FK_AuditClientViewDetail_AuditClientViewCodeID] FOREIGN KEY([AuditClientViewCodeID])
REFERENCES [Core].[AuditClientViewCode] ([AuditClientViewCodeID])
GO

ALTER TABLE [Core].[AuditClientViewDetail] CHECK CONSTRAINT [FK_AuditClientViewDetail_AuditClientViewCodeID]
GO