-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ScheduleType]
-- Author:		Scott Martin
-- Date:		02/14/2016
--
-- Purpose:		ScheduleType
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/14/2016	Scott Martin		Initital Creation .
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[ScheduleType](
	[ScheduleTypeID] [SMALLINT] IDENTITY(1,1) NOT NULL,
	[ScheduleType] [NVARCHAR](255) NOT NULL,
	[IsSystem] BIT NOT NULL DEFAULT(0),
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ScheduleType] PRIMARY KEY CLUSTERED 
(
	[ScheduleTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_ScheduleType_ScheduleType] ON [Scheduling].[ScheduleType]
(
	[ScheduleType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Scheduling.ScheduleType WITH CHECK ADD CONSTRAINT [FK_ScheduleType_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.ScheduleType CHECK CONSTRAINT [FK_ScheduleType_UserModifedBy]
GO
ALTER TABLE Scheduling.ScheduleType WITH CHECK ADD CONSTRAINT [FK_ScheduleType_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.ScheduleType CHECK CONSTRAINT [FK_ScheduleType_UserCreatedBy]
GO
