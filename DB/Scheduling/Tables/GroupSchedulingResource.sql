-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Scheduling].[GroupSchedulingResources]
-- Author:		Sumana Sangapu
-- Date:		02/09/2016
--
-- Purpose:		To hold the resources for scheduling group sessions
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/09/2016	Sumana Sangapu	  Initital Creation . 
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[GroupSchedulingResource](
	[GroupResourceID] [bigint] IDENTITY (1,1) NOT NULL,
	[GroupHeaderID] [bigint] NULL,
	[ResourceID] [int] NOT NULL,
	[ResourceTypeID] [smallint] NOT NULL,
	[IsPrimary] [bit] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_GroupResourceID] PRIMARY KEY CLUSTERED 
(
	[GroupResourceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Scheduling].[GroupSchedulingResource]  WITH CHECK ADD  CONSTRAINT [FK_GroupSchedulingResource] FOREIGN KEY([GroupHeaderID])
REFERENCES [Scheduling].[GroupSchedulingHeader]  ([GroupHeaderID])
GO

ALTER TABLE [Scheduling].[GroupSchedulingResource] CHECK CONSTRAINT [FK_GroupSchedulingResource] 
GO

ALTER TABLE [Scheduling].[GroupSchedulingResource]  WITH CHECK ADD  CONSTRAINT [FK_GroupSchedulingResourceTypeID] FOREIGN KEY([ResourceTypeID])
REFERENCES [Scheduling].[ResourceType]  ([ResourceTypeID])
GO

ALTER TABLE [Scheduling].[GroupSchedulingResource] CHECK CONSTRAINT [FK_GroupSchedulingResourceTypeID]
GO

ALTER TABLE Scheduling.GroupSchedulingResource WITH CHECK ADD CONSTRAINT [FK_GroupSchedulingResource_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.GroupSchedulingResource CHECK CONSTRAINT [FK_GroupSchedulingResource_UserModifedBy]
GO
ALTER TABLE Scheduling.GroupSchedulingResource WITH CHECK ADD CONSTRAINT [FK_GroupSchedulingResource_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.GroupSchedulingResource CHECK CONSTRAINT [FK_GroupSchedulingResource_UserCreatedBy]
GO
