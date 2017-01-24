-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Scheduling].[GroupSchedulingModerators]
-- Author:		Sumana Sangapu
-- Date:		02/09/2016
--
-- Purpose:		To hold the moderators while scheduling group Sessions
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/09/2016	Sumana Sangapu	  Initital Creation . 
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[GroupSchedulingModerators](
	[GroupModeratorID] [bigint] IDENTITY (1,1) NOT NULL,
	[GroupHeaderID] [bigint] NULL,
	[UserID] [int] NULL,
	[IsPrimary] [bit] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_GroupModeratorID] PRIMARY KEY CLUSTERED 
(
	[GroupModeratorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Scheduling].[GroupSchedulingModerators]  WITH CHECK ADD  CONSTRAINT [FK_GroupSchedulingModerators] FOREIGN KEY([GroupHeaderID])
REFERENCES [Scheduling].[GroupSchedulingHeader]  ([GroupHeaderID])
GO

ALTER TABLE [Scheduling].[GroupSchedulingModerators] CHECK CONSTRAINT [FK_GroupSchedulingModerators] 
GO

