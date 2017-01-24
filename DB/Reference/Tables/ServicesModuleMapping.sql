-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ServicesModuleMapping]
-- Author:		Sumana Sangapu
-- Date:		04/16/2016
--
-- Purpose:		Holds the Services Module Mapping
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/16/2016 - Sumana Sangapu Initial Creation 
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ServicesModuleMapping](
	[ServicesModuleMappingID] [int] IDENTITY(1,1) NOT NULL,
	[ServicesID] INT NOT NULL,
	[ModuleID] BIGINT NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_ServicesModuleMapping] PRIMARY KEY CLUSTERED 
(
	[ServicesModuleMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ServicesModuleMapping]  WITH CHECK ADD  CONSTRAINT [FK_ServicesModuleMapping_ModuleID] FOREIGN KEY([ModuleID])
REFERENCES [Core].[Module] ([ModuleID])
GO

ALTER TABLE [Reference].[ServicesModuleMapping] CHECK CONSTRAINT [FK_ServicesModuleMapping_ModuleID]
GO

ALTER TABLE [Reference].[ServicesModuleMapping]  WITH CHECK ADD  CONSTRAINT [FK_ServicesModuleMapping_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[ServicesModuleMapping] CHECK CONSTRAINT [FK_ServicesModuleMapping_UserCreatedBy]
GO

ALTER TABLE [Reference].[ServicesModuleMapping]  WITH CHECK ADD  CONSTRAINT [FK_ServicesModuleMapping_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[ServicesModuleMapping] CHECK CONSTRAINT [FK_ServicesModuleMapping_UserModifedBy]
GO


