-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Appointment]
-- Author:		John Crossen
-- Date:		10/06/2015
--
-- Purpose:		AppointmentReoccuring
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/06/2015	John Crossen	TFS# 2565 Initital Creation .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[AppointmentReoccuring](
	[AppointmentReoccuringID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[SchedulingOccurID] [INT] NOT NULL,
	[SchedulingFrequencyID] [INT] NULL,
	[StartDate] [DATE] NOT NULL,
	[EndDate] [DATE] NULL,
	[DaysOfTheWeek] NVARCHAR(20),
	IsCancelled BIT DEFAULT (0),
	CancelReasonID INT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AppointmentReoccuring] PRIMARY KEY CLUSTERED 
(
	[AppointmentReoccuringID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



ALTER TABLE [Scheduling].[AppointmentReoccuring]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentReoccuring_SchedulingFrequency] FOREIGN KEY([SchedulingFrequencyID])
REFERENCES [Scheduling].[SchedulingFrequency] ([SchedulingFrequencyID])
GO

ALTER TABLE [Scheduling].[AppointmentReoccuring] CHECK CONSTRAINT [FK_AppointmentReoccuring_SchedulingFrequency]
GO

ALTER TABLE [Scheduling].[AppointmentReoccuring]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentReoccuring_SchedulingOccur] FOREIGN KEY([SchedulingOccurID])
REFERENCES [Scheduling].[SchedulingOccur] ([SchedulingOccurID])
GO

ALTER TABLE [Scheduling].[AppointmentReoccuring] CHECK CONSTRAINT [FK_AppointmentReoccuring_SchedulingOccur]
GO

ALTER TABLE [Scheduling].[AppointmentReoccuring]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentReoccuring_CancelReason] FOREIGN KEY([CancelReasonID])
REFERENCES [Scheduling].[CancelReason] ([ReasonID])
GO

ALTER TABLE [Scheduling].[AppointmentReoccuring] CHECK CONSTRAINT [FK_AppointmentReoccuring_CancelReason]
GO

ALTER TABLE Scheduling.AppointmentReoccuring WITH CHECK ADD CONSTRAINT [FK_AppointmentReoccuring_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.AppointmentReoccuring CHECK CONSTRAINT [FK_AppointmentReoccuring_UserModifedBy]
GO
ALTER TABLE Scheduling.AppointmentReoccuring WITH CHECK ADD CONSTRAINT [FK_AppointmentReoccuring_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.AppointmentReoccuring CHECK CONSTRAINT [FK_AppointmentReoccuring_UserCreatedBy]
GO
