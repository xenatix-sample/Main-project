-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ServiceStatusModuleComponent]
-- Author:		Kyle Campbell
-- Date:		01/09/2017
--
-- Purpose:		Mapping table for Service Recording and ServiceStatus Module Components
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/09/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ServiceStatusModuleComponent](
	[ServiceStatusModuleComponentID] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleComponentID] BIGINT NOT NULL,
	[ServiceStatusID] SMALLINT NOT NULL,
	[ServicesID] [int] NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate())
 CONSTRAINT [PK_ServiceStatusModuleComponent_ServiceStatusModuleComponentID] PRIMARY KEY CLUSTERED 
(
	[ServiceStatusModuleComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]

GO
ALTER TABLE  [Reference].[ServiceStatusModuleComponent]  WITH CHECK ADD  CONSTRAINT [FK_ServiceStatusModuleComponent_ModuleComponentID] FOREIGN KEY([ModuleComponentID]) REFERENCES [Core].[ModuleComponent] ([ModuleComponentID])
GO

ALTER TABLE  [Reference].[ServiceStatusModuleComponent] CHECK CONSTRAINT [FK_ServiceStatusModuleComponent_ModuleComponentID]
GO

ALTER TABLE  [Reference].[ServiceStatusModuleComponent]  WITH CHECK ADD  CONSTRAINT [FK_ServiceStatusModuleComponent_ServiceStatusID] FOREIGN KEY([ServiceStatusID]) REFERENCES [Reference].[ServiceStatus] ([ServiceStatusID])
GO

ALTER TABLE  [Reference].[ServiceStatusModuleComponent] CHECK CONSTRAINT [FK_ServiceStatusModuleComponent_ServiceStatusID]
GO

ALTER TABLE  [Reference].[ServiceStatusModuleComponent]  WITH CHECK ADD  CONSTRAINT [FK_ServiceStatusModuleComponent_ServicesID] FOREIGN KEY([ServicesID]) REFERENCES [Reference].[Services] ([ServicesID])
GO

ALTER TABLE  [Reference].[ServiceStatusModuleComponent] CHECK CONSTRAINT [FK_ServiceStatusModuleComponent_ServicesID]
GO

ALTER TABLE [Reference].[ServiceStatusModuleComponent] WITH CHECK ADD CONSTRAINT [FK_ServiceStatusModuleComponent_UserModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[ServiceStatusModuleComponent] CHECK CONSTRAINT [FK_ServiceStatusModuleComponent_UserModifiedBy]
GO
ALTER TABLE [Reference].[ServiceStatusModuleComponent] WITH CHECK ADD CONSTRAINT [FK_ServiceStatusModuleComponent_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[ServiceStatusModuleComponent] CHECK CONSTRAINT [FK_ServiceStatusModuleComponent_UserCreatedBy]
GO
