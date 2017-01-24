-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[DeliveryMethodModuleComponent]
-- Author:		Scott Martin
-- Date:		06/22/2016
--
-- Purpose:		Mapping table for Service Recording and Delivery Method Module Components
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/22/2016	Scott Martin	 Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[DeliveryMethodModuleComponent](
	[DeliveryMethodModuleComponentID] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleComponentID] BIGINT NOT NULL,
	[DeliveryMethodID] [smallint] NOT NULL,
	[ServicesID] [int] NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate())
 CONSTRAINT [PK_DeliveryMethodModuleComponentID] PRIMARY KEY CLUSTERED 
(
	[DeliveryMethodModuleComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]

GO
ALTER TABLE  [Reference].[DeliveryMethodModuleComponent]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryMethodModuleComponent_ModuleComponentID] FOREIGN KEY([ModuleComponentID]) REFERENCES [Core].[ModuleComponent] ([ModuleComponentID])
GO

ALTER TABLE  [Reference].[DeliveryMethodModuleComponent] CHECK CONSTRAINT [FK_DeliveryMethodModuleComponent_ModuleComponentID]
GO

ALTER TABLE  [Reference].[DeliveryMethodModuleComponent]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryMethodModuleComponent_DeliveryMethodID] FOREIGN KEY([DeliveryMethodID]) REFERENCES [Reference].[DeliveryMethod] ([DeliveryMethodID])
GO

ALTER TABLE  [Reference].[DeliveryMethodModuleComponent] CHECK CONSTRAINT [FK_DeliveryMethodModuleComponent_DeliveryMethodID]
GO

ALTER TABLE  [Reference].[DeliveryMethodModuleComponent]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryMethodModuleComponent_ServicesID] FOREIGN KEY([ServicesID]) REFERENCES [Reference].[Services] ([ServicesID])
GO

ALTER TABLE  [Reference].[DeliveryMethodModuleComponent] CHECK CONSTRAINT [FK_DeliveryMethodModuleComponent_ServicesID]
GO

ALTER TABLE [Reference].[DeliveryMethodModuleComponent] WITH CHECK ADD CONSTRAINT [FK_DeliveryMethodModuleComponent_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[DeliveryMethodModuleComponent] CHECK CONSTRAINT [FK_DeliveryMethodModuleComponent_UserModifedBy]
GO
ALTER TABLE [Reference].[DeliveryMethodModuleComponent] WITH CHECK ADD CONSTRAINT [FK_DeliveryMethodModuleComponent_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[DeliveryMethodModuleComponent] CHECK CONSTRAINT [FK_DeliveryMethodModuleComponent_UserCreatedBy]
GO
