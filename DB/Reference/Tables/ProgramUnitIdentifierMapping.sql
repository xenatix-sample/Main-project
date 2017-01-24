-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ProgramUnitIdentifier]
-- Author:		Sumana Sangapu
-- Date:		06/15/2016
--
-- Purpose:		Script for [Reference].[ProgramUnitIdentifier]
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/15/2016	Sumana Sangapu	TFS #11185	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[ProgramUnitIdentifierMapping](
	[ProgramUnitIdentifierMappingID] [int] IDENTITY(1,1) NOT NULL,
	[MappingID] [bigint] NOT NULL,
	[ProgramUnitIdentifierID] [int] NOT NULL,
	[ProgramUnitIdentifierValue] nvarchar(10) NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_ProgramUnitIdentifierMapping] PRIMARY KEY CLUSTERED 
(
	[ProgramUnitIdentifierMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[ProgramUnitIdentifierMapping]  WITH CHECK ADD  CONSTRAINT [FK_ProgramUnitIdentifierMapping_MappingID] FOREIGN KEY([MappingID])
REFERENCES [Core].[OrganizationDetailsMapping] ([MappingID])
GO

ALTER TABLE [Core].[ProgramUnitIdentifierMapping] CHECK CONSTRAINT [FK_ProgramUnitIdentifierMapping_MappingID]
GO

ALTER TABLE [Core].[ProgramUnitIdentifierMapping]  WITH CHECK ADD  CONSTRAINT [FK_ProgramUnitIdentifierMapping_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Core].[ProgramUnitIdentifierMapping] CHECK CONSTRAINT [FK_ProgramUnitIdentifierMapping_UserCreatedBy]
GO

ALTER TABLE [Core].[ProgramUnitIdentifierMapping]  WITH CHECK ADD  CONSTRAINT [FK_ProgramUnitIdentifierMapping_ProgramUnitIdentifierID] FOREIGN KEY([ProgramUnitIdentifierID])
REFERENCES [Reference].[ProgramUnitIdentifier] ([ProgramUnitIdentifierID])
GO

ALTER TABLE [Core].[ProgramUnitIdentifierMapping] CHECK CONSTRAINT [FK_ProgramUnitIdentifierMapping_ProgramUnitIdentifierID]
GO

ALTER TABLE [Core].[ProgramUnitIdentifierMapping]  WITH CHECK ADD  CONSTRAINT [FK_ProgramUnitIdentifierMapping_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Core].[ProgramUnitIdentifierMapping] CHECK CONSTRAINT [FK_ProgramUnitIdentifierMapping_UserModifedBy]
GO


