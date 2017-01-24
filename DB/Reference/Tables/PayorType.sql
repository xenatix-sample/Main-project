-----------------------------------------------------------------------------------------------------------------------
-- Table:		Reference.PayorType
-- Author:		Atul Chauhan
-- Date:		12/07/2016
--
-- Purpose:		Lookup for PayorType details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/07/2016	Atul Chauhan	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[PayorType](
	[PayorTypeID] [int] IDENTITY(1,1) NOT NULL,
	[PayorType] [nvarchar](20) NOT NULL,
	[SortOrder][int] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_PayorType_PayorTypeID] PRIMARY KEY CLUSTERED 
(
	[PayorTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]

GO

ALTER TABLE Reference.PayorType WITH CHECK ADD CONSTRAINT [FK_PayorType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.PayorType CHECK CONSTRAINT [FK_PayorType_UserModifedBy]
GO
ALTER TABLE Reference.PayorType WITH CHECK ADD CONSTRAINT [FK_PayorType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.PayorType CHECK CONSTRAINT [FK_PayorType_UserCreatedBy]
GO

EXEC sys.sp_addextendedproperty @name=N'Caption', @value=N'Payor Type' , @level0type=N'SCHEMA',@level0name=N'Reference', @level1type=N'TABLE',@level1name=N'PayorType'
GO

EXEC sys.sp_addextendedproperty @name=N'IsOptionSet', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'Reference', @level1type=N'TABLE',@level1name=N'PayorType'
GO

EXEC sys.sp_addextendedproperty @name=N'IsUserOptionSet', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'Reference', @level1type=N'TABLE',@level1name=N'PayorType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Values indicating PayorType' , @level0type=N'SCHEMA',@level0name=N'Reference', @level1type=N'TABLE',@level1name=N'PayorType'
GO
