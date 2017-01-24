-----------------------------------------------------------------------------------------------------------------------
-- Table:		[AppointmentResource]
-- Author:		John Crossen
-- Date:		10/01/2015
--
-- Purpose:		AppointmentResource table to store appointment links
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/15/2015	John Crossen	TFS# 2731 Initital Creation .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[AppointmentResource](
	[AppointmentResourceID] [bigint] IDENTITY(1,1) NOT NULL,
	[AppointmentID] [bigint] NOT NULL,
	[ResourceID] INT NOT NULL,
	[ResourceTypeID] SMALLINT NOT NULL,
	[ParentID] BIGINT NULL,
	[GroupHeaderID] BIGINT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AppointmentResource] PRIMARY KEY CLUSTERED 
(
	[AppointmentResourceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Scheduling].[AppointmentResource]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentResource_Appointment] FOREIGN KEY([AppointmentID])
REFERENCES [Scheduling].[Appointment] ([AppointmentID])
GO

ALTER TABLE Scheduling.AppointmentResource CHECK CONSTRAINT [FK_AppointmentResource_Appointment] 
GO

ALTER TABLE [Scheduling].[AppointmentResource]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentResource_GroupHeaderID] FOREIGN KEY([GroupHeaderID])
REFERENCES [Scheduling].[GroupSchedulingHeader] ([GroupHeaderID])
GO

ALTER TABLE Scheduling.AppointmentResource CHECK CONSTRAINT [FK_AppointmentResource_GroupHeaderID]
GO

ALTER TABLE [Scheduling].[AppointmentResource]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentResource_ResourceTypeID] FOREIGN KEY([ResourceTypeID])
REFERENCES [Scheduling].[ResourceType] ([ResourceTypeID])
GO

ALTER TABLE Scheduling.AppointmentResource CHECK CONSTRAINT [FK_AppointmentResource_ResourceTypeID]
GO

ALTER TABLE Scheduling.AppointmentResource WITH CHECK ADD CONSTRAINT [FK_AppointmentResource_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.AppointmentResource CHECK CONSTRAINT [FK_AppointmentResource_UserModifedBy]
GO
ALTER TABLE Scheduling.AppointmentResource WITH CHECK ADD CONSTRAINT [FK_AppointmentResource_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.AppointmentResource CHECK CONSTRAINT [FK_AppointmentResource_UserCreatedBy]
GO

