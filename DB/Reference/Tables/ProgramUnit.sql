----------------------------------------------------------------------------------------------------------------
-- Table:	    [Reference].[ProgramUnit]
-- Author:		Gurpreet Singh
-- Date:		02/01/2016
--
-- Purpose:		Program Unit Table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY --------------------------------------------------------------------------------------------
-- 02/01/2016	Gurpreet Singh	Initial creation.
----------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ProgramUnit] (
	[ProgramUnitID] [INT] IDENTITY(1,1) NOT NULL,
	[ProgramUnit] [NVARCHAR](50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_ProgramUnit] PRIMARY KEY CLUSTERED 
(
	[ProgramUnitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.ProgramUnit WITH CHECK ADD CONSTRAINT [FK_ProgramUnit_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ProgramUnit CHECK CONSTRAINT [FK_ProgramUnit_UserModifedBy]
GO
ALTER TABLE Reference.ProgramUnit WITH CHECK ADD CONSTRAINT [FK_ProgramUnit_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ProgramUnit CHECK CONSTRAINT [FK_ProgramUnit_UserCreatedBy]
GO

