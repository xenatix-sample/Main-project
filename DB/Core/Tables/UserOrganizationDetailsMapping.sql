-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Core].[UserOrganizationDetailsMapping]
-- Author:		Scott Martin
-- Date:		02/25/2016
--
-- Purpose:		Users Organization Mapping 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/25/2016	Scott Martin		Initial creation.
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[UserOrganizationDetailsMapping](
	[UserOrganizationDetailMappingID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [INT] NOT NULL,
	[MappingID] [BIGINT] NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_UserOrganizationDetailsMapping] PRIMARY KEY CLUSTERED 
(
	[UserOrganizationDetailMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[UserOrganizationDetailsMapping] WITH CHECK ADD  CONSTRAINT [FK_UserOrganizationDetailsMapping_UserId] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
ON DELETE CASCADE
GO

ALTER TABLE [Core].[UserOrganizationDetailsMapping] CHECK CONSTRAINT [FK_UserOrganizationDetailsMapping_UserId] 
GO

ALTER TABLE [Core].[UserOrganizationDetailsMapping] WITH CHECK ADD  CONSTRAINT [FK_UserOrganizationDetailsMapping_MappingID] FOREIGN KEY([MappingID]) REFERENCES [Core].[OrganizationDetailsMapping] ([MappingID])
ON DELETE CASCADE
GO

ALTER TABLE [Core].[UserOrganizationDetailsMapping] CHECK CONSTRAINT [FK_UserOrganizationDetailsMapping_MappingID]
GO 

ALTER TABLE [Core].[UserOrganizationDetailsMapping] WITH CHECK ADD CONSTRAINT [FK_UserOrganizationDetailsMapping_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[UserOrganizationDetailsMapping] CHECK CONSTRAINT [FK_UserOrganizationDetailsMapping_UserModifedBy]
GO
ALTER TABLE [Core].[UserOrganizationDetailsMapping] WITH CHECK ADD CONSTRAINT [FK_UserOrganizationDetailsMapping_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[UserOrganizationDetailsMapping] CHECK CONSTRAINT [FK_UserOrganizationDetailsMapping_UserCreatedBy]
GO

CREATE NONCLUSTERED INDEX [IX_UserOrganizationDetailsMapping_MappingID] ON [Core].[UserOrganizationDetailsMapping]
(
	[MappingID] ASC
)
INCLUDE ( 	[UserOrganizationDetailMappingID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_UserOrganizationDetailsMapping_UserID] ON [Core].[UserOrganizationDetailsMapping]
(
	[UserID] ASC
)
INCLUDE ( 	[MappingID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO