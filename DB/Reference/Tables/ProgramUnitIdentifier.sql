-----------------------------------------------------------------------------------------------------------------------
-- TAble:		[Reference].[ProgramUnitIdentifier]
-- Author:		Sumana Sangapu
-- Date:		06/15/2016
--
-- Purpose:		Script for [Reference].[ProgramUnitIdentifier]
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/15/2016	Sumana Sangapu	TFS #11185	Initial Creation
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Reference].[ProgramUnitIdentifier](
	[ProgramUnitIdentifierID] [int] IDENTITY(1,1) NOT NULL,
	[ProgramUnitIdentifier] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_ProgramUnitIdentifier] PRIMARY KEY CLUSTERED 
(
	[ProgramUnitIdentifierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ProgramUnitIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ProgramUnitIdentifier_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[ProgramUnitIdentifier] CHECK CONSTRAINT [FK_ProgramUnitIdentifier_UserCreatedBy]
GO

ALTER TABLE [Reference].[ProgramUnitIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ProgramUnitIdentifier_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[ProgramUnitIdentifier] CHECK CONSTRAINT [FK_ProgramUnitIdentifier_UserModifedBy]
GO


