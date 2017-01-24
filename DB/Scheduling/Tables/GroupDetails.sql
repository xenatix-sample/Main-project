
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Scheduling].[GroupDetails]
-- Author:		Sumana Sangapu
-- Date:		02/09/2016
--
-- Purpose:		Holds the details of groups for Group Sessions
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/12/2016	Sumana Sangapu	  Initital Creation . 
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[GroupDetails](
	[GroupDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](100) NULL,
	[GroupTypeID] [int] NULL,
	[GroupCapacity] [smallint] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_GroupDetailID] PRIMARY KEY CLUSTERED 
(
	[GroupDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE  [Scheduling].[GroupDetails]  WITH CHECK ADD  CONSTRAINT [FK_GroupTypeID] FOREIGN KEY([GroupTypeID])
REFERENCES [Reference].[GroupType]  ([GroupTypeID])
GO

ALTER TABLE  [Scheduling].[GroupDetails] CHECK CONSTRAINT [FK_GroupTypeID] 
GO

ALTER TABLE Scheduling.GroupDetails WITH CHECK ADD CONSTRAINT [FK_GroupDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.GroupDetails CHECK CONSTRAINT [FK_GroupDetails_UserModifedBy]
GO
ALTER TABLE Scheduling.GroupDetails WITH CHECK ADD CONSTRAINT [FK_GroupDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.GroupDetails CHECK CONSTRAINT [FK_GroupDetails_UserCreatedBy]
GO

ALTER TABLE [Scheduling].[GroupDetails] ADD CONSTRAINT UNQ_GroupName UNIQUE (GroupName)