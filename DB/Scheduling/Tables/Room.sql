-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Room]
-- Author:		John Crossen
-- Date:		09/10/2015
--
-- Purpose:		Room data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/10/2015	John Crossen	TFS# 2229 Initital Creation .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[Room](
	[RoomID] [INT] IDENTITY(1,1) NOT NULL,
	[FacilityID] INT NOT NULL,
	[RoomName] [NCHAR](255) NOT NULL,
	[RoomCapacity] INT NULL,
	IsSchedulable BIT NOT NULL DEFAULT(1),
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED 
(
	[RoomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Scheduling.Room WITH CHECK ADD CONSTRAINT [FK_Room_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.Room CHECK CONSTRAINT [FK_Room_UserModifedBy]
GO
ALTER TABLE Scheduling.Room WITH CHECK ADD CONSTRAINT [FK_Room_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.Room CHECK CONSTRAINT [FK_Room_UserCreatedBy]
GO
