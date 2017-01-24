-----------------------------------------------------------------------------------------------------------------------
-- Table:		[dbo].[RoleModuleComponentReport]
-- Author:		Scott Martin
-- Date:		04/25/2016
--
-- Purpose:		Store the reports specific roles may access
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/25/2016	Scott Martin	Initial creation.
-- 05/16/2016	Scott Martin	Renamed and refactored table
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[RoleModuleComponentReport](
	[RoleModuleComponentReportID] [int] IDENTITY(1,1) NOT NULL,
	[RoleModuleComponentID] [bigint] NOT NULL,
	[ReportID] [int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_RoleModuleComponentReport] PRIMARY KEY CLUSTERED 
(
	[RoleModuleComponentReportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Core].[RoleModuleComponentReport]  WITH CHECK ADD  CONSTRAINT [FK_RoleModuleComponentReport_RoleModuleComponentID] FOREIGN KEY([RoleModuleComponentID]) REFERENCES [Core].[RoleModuleComponent] ([RoleModuleComponentID])
GO
ALTER TABLE [Core].[RoleModuleComponentReport] CHECK CONSTRAINT [FK_RoleModuleComponentReport_RoleModuleComponentID]
GO
ALTER TABLE [Core].[RoleModuleComponentReport]  WITH CHECK ADD  CONSTRAINT [FK_RoleModuleComponentReport_ReportID] FOREIGN KEY([ReportID]) REFERENCES [Core].[Reports] ([ReportID])
GO
ALTER TABLE [Core].[RoleModuleComponentReport] CHECK CONSTRAINT [FK_RoleModuleComponentReport_ReportID]
GO
ALTER TABLE Core.RoleModuleComponentReport WITH CHECK ADD CONSTRAINT [FK_RoleModuleComponentReport_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.RoleModuleComponentReport CHECK CONSTRAINT [FK_RoleModuleComponentReport_UserModifedBy]
GO
ALTER TABLE Core.RoleModuleComponentReport WITH CHECK ADD CONSTRAINT [FK_RoleModuleComponentReport_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.RoleModuleComponentReport CHECK CONSTRAINT [FK_RoleModuleComponentReport_UserCreatedBy]
GO



