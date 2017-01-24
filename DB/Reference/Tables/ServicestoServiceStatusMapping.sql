-----------------------------------------------------------------------------------------------------------------------
-- Table:	[Reference].[ServicestoServiceStatusMapping]
-- Author:		Sumana Sangapu
-- Date:		03/29/2016
--
-- Purpose:		Holds the ServiceStatus for Services.  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/31/2016	Sumana Sangapu    Initial creation. 
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ServicestoServiceStatusMapping](
	[ServicestoServiceStatusMappingID] [int] IDENTITY(1,1) NOT NULL,
	[ServicesID] [int] NULL,
	[ServiceStatusID] [smallint] NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_ServiceStatus_IsActive]  DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL CONSTRAINT [DF_ServiceStatus_ModifiedOn]  DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_ServiceStatus_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_ServicesO_CreatedOn]  DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL CONSTRAINT [DF_ServiceStatus_SystemCreatedOn]  DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL CONSTRAINT [DF_ServiceStatus_SystemModifiedOn]  DEFAULT (getutcdate()),
 CONSTRAINT [PK_ServicestoServiceStatusMappingID] PRIMARY KEY CLUSTERED 
(
	[ServicestoServiceStatusMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ServicestoServiceStatusMapping] UNIQUE NONCLUSTERED 
(
	[ServicesID] ASC,
	[ServiceStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ServicestoServiceStatusMapping]  WITH CHECK ADD  CONSTRAINT [FK_ServicestoServiceStatusMapping_ServiceStatusID] FOREIGN KEY([ServiceStatusID])
REFERENCES [Reference].[ServiceStatus] ([ServiceStatusID])
GO

ALTER TABLE [Reference].[ServicestoServiceStatusMapping] CHECK CONSTRAINT [FK_ServicestoServiceStatusMapping_ServiceStatusID]
GO

ALTER TABLE [Reference].[ServicestoServiceStatusMapping]  WITH CHECK ADD  CONSTRAINT [FK_ServicestoServiceStatusMapping_Services] FOREIGN KEY([ServicesID])
REFERENCES [Reference].[Services] ([ServicesID])
GO

ALTER TABLE [Reference].[ServicestoServiceStatusMapping] CHECK CONSTRAINT [FK_ServicestoServiceStatusMapping_Services]
GO

ALTER TABLE [Reference].[ServicestoServiceStatusMapping]  WITH CHECK ADD  CONSTRAINT [FK_ServicestoServiceStatusMapping_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[ServicestoServiceStatusMapping] CHECK CONSTRAINT [FK_ServicestoServiceStatusMapping_UserCreatedBy]
GO

ALTER TABLE [Reference].[ServicestoServiceStatusMapping]  WITH CHECK ADD  CONSTRAINT [FK_ServicestoServiceStatusMapping_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[ServicestoServiceStatusMapping] CHECK CONSTRAINT [FK_ServicestoServiceStatusMapping_UserModifedBy]
GO


