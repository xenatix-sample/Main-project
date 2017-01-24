-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[OrganizationDetailsModuleComponent]
-- Author:		Scott Martin
-- Date:		01/15/2017
--
-- Purpose:		Associates Organization Details with Module Components 
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2017	Scott Martin		Initial Creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[OrganizationDetailsModuleComponent](
	[OrganizationDetailsModuleComponentID] [bigint] IDENTITY(1,1) NOT NULL,
	[DetailID] [bigint] NOT NULL,
	[ModuleComponentID] [bigint] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_OrganizationDetailsModuleComponent] PRIMARY KEY CLUSTERED 
(
	[OrganizationDetailsModuleComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

ALTER TABLE [Core].[OrganizationDetailsModuleComponent] WITH CHECK ADD  CONSTRAINT [FK_OrganizationDetailsModuleComponent_DetailID] FOREIGN KEY([DetailID]) REFERENCES [Core].[OrganizationDetails] ([DetailID])
GO
ALTER TABLE [Core].[OrganizationDetailsModuleComponent] CHECK CONSTRAINT [FK_OrganizationDetailsModuleComponent_DetailID]
GO
ALTER TABLE [Core].[OrganizationDetailsModuleComponent] WITH CHECK ADD  CONSTRAINT [FK_OrganizationDetailsModuleComponent_ModuleComponentID] FOREIGN KEY([ModuleComponentID]) REFERENCES [Core].[ModuleComponent] ([ModuleComponentID])
GO
ALTER TABLE [Core].[OrganizationDetailsModuleComponent] CHECK CONSTRAINT [FK_OrganizationDetailsModuleComponent_ModuleComponentID]
GO
ALTER TABLE [Core].[OrganizationDetailsModuleComponent] WITH CHECK ADD CONSTRAINT [FK_OrganizationDetailsModuleComponent_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[OrganizationDetailsModuleComponent] CHECK CONSTRAINT [FK_OrganizationDetailsModuleComponent_UserModifedBy]
GO
ALTER TABLE [Core].[OrganizationDetailsModuleComponent] WITH CHECK ADD CONSTRAINT [FK_OrganizationDetailsModuleComponent_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[OrganizationDetailsModuleComponent] CHECK CONSTRAINT [FK_OrganizationDetailsModuleComponent_UserCreatedBy]
GO



