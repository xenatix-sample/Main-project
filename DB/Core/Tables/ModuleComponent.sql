-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[ModuleComponent]
-- Author:		Scott Martin
-- Date:		05/13/2016
--
-- Purpose:		Module Component Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/13/2016	Scott Martin	Initial Creation
-- 09/05/2016	Scott Martin	Added unique constraint for DataKey
-- 01/04/2017	Kyle Campbell	TFS #14007	Add column AllowServiceMapping for Service configuration screen Service Workflow dropdown
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[ModuleComponent](
	[ModuleComponentID] BIGINT IDENTITY(1,1) NOT NULL,
	[ModuleID] BIGINT NOT NULL,
	[FeatureID] BIGINT NOT NULL,
	[ComponentID] BIGINT NOT NULL,
	[DataKey] NVARCHAR(250) NOT NULL,
	[AllowServiceMapping] BIT DEFAULT(0) NOT NULL ,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_ModuleComponent_ModuleComponentID] PRIMARY KEY CLUSTERED 
	( 
		[ModuleComponentID] ASC
    )
	WITH 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON,
		 ALLOW_PAGE_LOCKS = ON
	 ) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE Core.ModuleComponent WITH CHECK ADD CONSTRAINT [FK_ModuleComponent_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ModuleComponent CHECK CONSTRAINT [FK_ModuleComponent_UserModifedBy]
GO
ALTER TABLE Core.ModuleComponent WITH CHECK ADD CONSTRAINT [FK_ModuleComponent_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ModuleComponent CHECK CONSTRAINT [FK_ModuleComponent_UserCreatedBy]
GO
ALTER TABLE Core.ModuleComponent WITH CHECK ADD CONSTRAINT [FK_ModuleComponent_ModuleID] FOREIGN KEY ([ModuleID]) REFERENCES [Core].[Module] ([ModuleID])
GO
ALTER TABLE Core.ModuleComponent CHECK CONSTRAINT [FK_ModuleComponent_ModuleID]
GO
ALTER TABLE Core.ModuleComponent WITH CHECK ADD CONSTRAINT [FK_ModuleComponent_FeatureID] FOREIGN KEY ([FeatureID]) REFERENCES [Core].[Feature] ([FeatureID])
GO
ALTER TABLE Core.ModuleComponent CHECK CONSTRAINT [FK_ModuleComponent_FeatureID]
GO
ALTER TABLE Core.ModuleComponent WITH CHECK ADD CONSTRAINT [FK_ModuleComponent_ComponentID] FOREIGN KEY ([ComponentID]) REFERENCES [Core].[Component] ([ComponentID])
GO
ALTER TABLE Core.ModuleComponent CHECK CONSTRAINT [FK_ModuleComponent_ComponentID]
GO
ALTER TABLE Core.ModuleComponent ADD CONSTRAINT IX_ModuleComponent_DataKey UNIQUE([DataKey])
GO