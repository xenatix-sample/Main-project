-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[GroupType]
-- Author:		Sumana Sangapu
-- Date:		02/09/2016
--
-- Purpose:		To hold the group types for scheduling group Sessions
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/09/2016	Sumana Sangapu	  Initital Creation . 
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Reference].[GroupType](
	[GroupTypeID] [int] IDENTITY (1,1) NOT NULL,
	[GroupType] [nvarchar](50) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_GroupTypeID] PRIMARY KEY CLUSTERED 
(
	[GroupTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

ALTER TABLE Reference.GroupType WITH CHECK ADD CONSTRAINT [FK_GroupType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.GroupType CHECK CONSTRAINT [FK_GroupType_UserModifedBy]
GO
ALTER TABLE Reference.GroupType WITH CHECK ADD CONSTRAINT [FK_GroupType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.GroupType CHECK CONSTRAINT [FK_GroupType_UserCreatedBy]
GO
