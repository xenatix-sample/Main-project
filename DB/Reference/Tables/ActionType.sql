
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ActionType]
-- Author:		Sumana Sangapu
-- Date:		12/12/2016
--
-- Purpose:		Indicates the action that is performed by the user. Ex: Add/Edit/PrintView/Delete/View
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/12/2016	Sumana Sangapu  - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ActionType](
	[ActionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ActionType] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_ActionType] PRIMARY KEY CLUSTERED 
(
	[ActionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ActionType_ActionType] UNIQUE NONCLUSTERED 
(
	[ActionType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ActionType]  WITH CHECK ADD  CONSTRAINT [FK_ActionType_UserCreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[ActionType] CHECK CONSTRAINT [FK_ActionType_UserCreatedBy]
GO

ALTER TABLE [Reference].[ActionType]  WITH CHECK ADD  CONSTRAINT [FK_ActionType_UserModifedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Reference].[ActionType] CHECK CONSTRAINT [FK_ActionType_UserModifedBy]
GO

EXEC sys.sp_addextendedproperty @name=N'Caption', @value=N'Action Type' , @level0type=N'SCHEMA',@level0name=N'Reference', @level1type=N'TABLE',@level1name=N'ActionType'
GO

EXEC sys.sp_addextendedproperty @name=N'IsOptionSet', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'Reference', @level1type=N'TABLE',@level1name=N'ActionType'
GO

EXEC sys.sp_addextendedproperty @name=N'IsUserOptionSet', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'Reference', @level1type=N'TABLE',@level1name=N'ActionType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Values indicating what was the user''s action in the application' , @level0type=N'SCHEMA',@level0name=N'Reference', @level1type=N'TABLE',@level1name=N'ActionType'
GO


