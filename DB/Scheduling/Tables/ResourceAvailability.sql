-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ResourceAvailability]
-- Author:		John Crossen
-- Date:		09/10/2015
--
-- Purpose:		ResourceAvailability
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/11/2015	John Crossen	TFS# 2229 Initital Creation .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Scheduling].[ResourceAvailability](
	[ResourceAvailabilityID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ResourceID] [INT] NOT NULL,
	[ResourceTypeID] [SMALLINT] NOT NULL,
	[FacilityID] INT NOT NULL,
	[DefaultFacilityID] INT NOT NULL,
	[ScheduleTypeID] SMALLINT NULL,
	[DayOfWeekID] INT NOT NULL,
	[AvailabilityStartTime] [NVARCHAR](10) NOT NULL,
	[AvailabilityEndTime] [NVARCHAR](10) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ResourceAvailability] PRIMARY KEY CLUSTERED 
(
	[ResourceAvailabilityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Scheduling].[ResourceAvailability]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAvailability_ResourceType] FOREIGN KEY([ResourceTypeID])
REFERENCES [Scheduling].[ResourceType] ([ResourceTypeID])
GO

ALTER TABLE [Scheduling].[ResourceAvailability] CHECK CONSTRAINT [FK_ResourceAvailability_ResourceType]
GO

ALTER TABLE [Scheduling].[ResourceAvailability]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAvailability_Facility] FOREIGN KEY([FacilityID])
REFERENCES [Reference].[Facility] ([FacilityID])
GO

ALTER TABLE [Scheduling].[ResourceAvailability] CHECK CONSTRAINT [FK_ResourceAvailability_Facility]
GO

ALTER TABLE [Scheduling].[ResourceAvailability]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAvailability_DefaultFacility] FOREIGN KEY([DefaultFacilityID])
REFERENCES [Reference].[Facility] ([FacilityID])
GO

ALTER TABLE [Scheduling].[ResourceAvailability] CHECK CONSTRAINT [FK_ResourceAvailability_DefaultFacility]
GO

ALTER TABLE [Scheduling].[ResourceAvailability]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAvailability_ScheduleType] FOREIGN KEY([ScheduleTypeID])
REFERENCES [Scheduling].[ScheduleType] ([ScheduleTypeID])
GO

ALTER TABLE [Scheduling].[ResourceAvailability] CHECK CONSTRAINT [FK_ResourceAvailability_ScheduleType]
GO

ALTER TABLE [Scheduling].[ResourceAvailability]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAvailability_DayOfWeek] FOREIGN KEY([DayOfWeekID])
REFERENCES [Reference].[DayOfWeek] ([DayOfWeekID])
GO

ALTER TABLE [Scheduling].[ResourceAvailability] CHECK CONSTRAINT [FK_ResourceAvailability_DayOfWeek]
GO

ALTER TABLE Scheduling.ResourceAvailability WITH CHECK ADD CONSTRAINT [FK_ResourceAvailability_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.ResourceAvailability CHECK CONSTRAINT [FK_ResourceAvailability_UserModifedBy]
GO
ALTER TABLE Scheduling.ResourceAvailability WITH CHECK ADD CONSTRAINT [FK_ResourceAvailability_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.ResourceAvailability CHECK CONSTRAINT [FK_ResourceAvailability_UserCreatedBy]
GO
