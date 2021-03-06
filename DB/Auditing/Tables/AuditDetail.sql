-----------------------------------------------------------------------------------------------------------------------
-- Table:	    Core.AuditDetail
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
-- 08/03/2015	John Crossen		TFS# 866 - Add XML Columns.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 09/06/2016	Rahul Vats		Reviewed the Table
-- 09/15/2016	Scott Martin	Added TableCatalogID, EntityID, and EntityTypeID
-- 09/16/2016	Scott Martin	Moved to Auditing Schema
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Auditing].[AuditDetail](
	[AuditDetailID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[AuditID] [BIGINT] NOT NULL,
	[AuditSource] NVARCHAR(255) NULL,
	[AuditPrimaryKeyValue] BIGINT NULL,
	[TableCatalogID] INT NULL,
	[EntityID] BIGINT NULL,
	[EntityTypeID] INT NULL,
	[AuditPre] [XML] NULL,
	[AuditPost] [XML] NULL,
	[AuditDiff] [XML] NULL,
 CONSTRAINT [PK_AuditDetail] PRIMARY KEY CLUSTERED 
(
	[AuditDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [Auditing].[AuditDetail]  WITH CHECK ADD  CONSTRAINT [FK_AuditDetail_Audit] FOREIGN KEY([AuditID]) REFERENCES [Auditing].[Audits] ([AuditId])
GO
ALTER TABLE [Auditing].[AuditDetail] CHECK CONSTRAINT [FK_AuditDetail_Audit]
GO
ALTER TABLE [Auditing].[AuditDetail]  WITH CHECK ADD  CONSTRAINT [FK_AuditDetail_TableCatalogID] FOREIGN KEY([TableCatalogID]) REFERENCES [Reference].[TableCatalog] ([TableCatalogID])
GO
ALTER TABLE [Auditing].[AuditDetail] CHECK CONSTRAINT [FK_AuditDetail_TableCatalogID]
GO
ALTER TABLE [Auditing].[AuditDetail]  WITH CHECK ADD  CONSTRAINT [FK_AuditDetail_EntityTypeID] FOREIGN KEY([EntityTypeID]) REFERENCES [Reference].[EntityType] ([EntityTypeID])
GO
ALTER TABLE [Auditing].[AuditDetail] CHECK CONSTRAINT [FK_AuditDetail_EntityTypeID]
GO