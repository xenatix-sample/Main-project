-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[PresentingProblemType]
-- Author:		Scott Martin
-- Date:		03/31/2016
--
-- Purpose:		Lookup Table for Presenting Problem Items
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/31/2016	Scott Martin	Initial creation.
-- 06/27/2016	Sumana Sangapu	LegacyCode and LegacyCodeDescription
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[PresentingProblemType](
	[PresentingProblemTypeID] [SMALLINT] IDENTITY(1,1) NOT NULL,
	[PresentingProblemType] [NVARCHAR](255) NOT NULL,
	[LegacyCode] nvarchar(10) NULL,
	[LegacyCodeDescription] nvarchar(100) NULL,
	[SortOrder] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
	[ModifiedBy] INT NOT NULL,
	[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL,
	[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	 CONSTRAINT [PK_PresentingProblemType] PRIMARY KEY CLUSTERED 
(
	[PresentingProblemTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.PresentingProblemType WITH CHECK ADD CONSTRAINT [FK_PresentingProblemType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.PresentingProblemType CHECK CONSTRAINT [FK_PresentingProblemType_UserModifedBy]
GO
ALTER TABLE Reference.PresentingProblemType WITH CHECK ADD CONSTRAINT [FK_PresentingProblemType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.PresentingProblemType CHECK CONSTRAINT [FK_PresentingProblemType_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Presenting Problem Type', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = PresentingProblemType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating kind of problem present', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = PresentingProblemType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = PresentingProblemType;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = PresentingProblemType;
GO;
