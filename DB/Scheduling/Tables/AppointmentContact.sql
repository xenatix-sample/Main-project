-----------------------------------------------------------------------------------------------------------------------
-- Table:		[AppointmentContact]
-- Author:		John Crossen
-- Date:		10/15/2015
--
-- Purpose:		Appointment Contact mapping
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

CREATE TABLE [Scheduling].[AppointmentContact](
	[AppointmentContactID] [bigint] IDENTITY(1,1) NOT NULL,
	[AppointmentID] [bigint] NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AppointmentContact] PRIMARY KEY CLUSTERED 
(
	[AppointmentContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Scheduling].[AppointmentContact]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentDetail_Appointment] FOREIGN KEY([AppointmentID])
REFERENCES [Scheduling].[Appointment] ([AppointmentID])
GO

ALTER TABLE [Scheduling].[AppointmentContact] CHECK CONSTRAINT [FK_AppointmentDetail_Appointment]
GO

ALTER TABLE [Scheduling].[AppointmentContact]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentDetail_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Scheduling].[AppointmentContact] CHECK CONSTRAINT [FK_AppointmentDetail_Contact]
GO

ALTER TABLE Scheduling.AppointmentContact WITH CHECK ADD CONSTRAINT [FK_AppointmentContact_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.AppointmentContact CHECK CONSTRAINT [FK_AppointmentContact_UserModifedBy]
GO
ALTER TABLE Scheduling.AppointmentContact WITH CHECK ADD CONSTRAINT [FK_AppointmentContact_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.AppointmentContact CHECK CONSTRAINT [FK_AppointmentContact_UserCreatedBy]
GO
