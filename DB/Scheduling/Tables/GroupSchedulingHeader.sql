
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Scheduling].[GroupSchedulingHeader]
-- Author:		Sumana Sangapu
-- Date:		02/09/2016
--
-- Purpose:		Header for Scheduling Group Sessions
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/09/2016	Sumana Sangapu	Initital Creation . 
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 03/11/2016	Sumana Sangapu	Refactored schema based on latest USerstory
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[GroupSchedulingHeader](
	[GroupHeaderID] [bigint] IDENTITY(1,1) NOT NULL,
	[GroupDetailID] BIGINT NOT  NULL,
	[Comments] [nvarchar](1000) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_GroupHeaderID] PRIMARY KEY CLUSTERED 
(
	[GroupHeaderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



ALTER TABLE Scheduling.GroupSchedulingHeader WITH CHECK ADD CONSTRAINT [FK_GroupSchedulingHeader_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.GroupSchedulingHeader CHECK CONSTRAINT [FK_GroupSchedulingHeader_UserModifedBy]
GO
ALTER TABLE Scheduling.GroupSchedulingHeader WITH CHECK ADD CONSTRAINT [FK_GroupSchedulingHeader_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.GroupSchedulingHeader CHECK CONSTRAINT [FK_GroupSchedulingHeader_UserCreatedBy]
GO
ALTER TABLE Scheduling.GroupSchedulingHeader WITH CHECK ADD CONSTRAINT [FK_GroupSchedulingHeader_GroupDetailID] FOREIGN KEY ([GroupDetailID]) REFERENCES [Scheduling].[GroupDetails] ([GroupDetailID])
GO
ALTER TABLE Scheduling.GroupSchedulingHeader CHECK CONSTRAINT [FK_GroupSchedulingHeader_GroupDetailID]
GO
