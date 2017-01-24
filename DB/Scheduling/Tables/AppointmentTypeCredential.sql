-----------------------------------------------------------------------------------------------------------------------
-- Table:		[AppointmentTypeCredential]
-- Author:		John Crossen
-- Date:		10/02/2015
--
-- Purpose:		Mapping Table between Appointment Type and Credential
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/11/2015	John Crossen	TFS# 2565 Initital Creation .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Scheduling].[AppointmentTypeCredential](
	[AppointmentTypeCredentialID] [INT] IDENTITY(1,1) NOT NULL,
	[AppointmentTypeID] [INT] NOT NULL,
	[CredentialID] [BIGINT] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AppointmentTypeCredential] PRIMARY KEY CLUSTERED 
(
	[AppointmentTypeCredentialID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Scheduling].[AppointmentTypeCredential]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentTypeCredential_AppointmentType] FOREIGN KEY([AppointmentTypeID])
REFERENCES [Scheduling].[AppointmentType] ([AppointmentTypeID])
GO

ALTER TABLE [Scheduling].[AppointmentTypeCredential] CHECK CONSTRAINT [FK_AppointmentTypeCredential_AppointmentType]
GO

ALTER TABLE [Scheduling].[AppointmentTypeCredential]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentTypeCredential_Credentials] FOREIGN KEY([CredentialID])
REFERENCES [Reference].[Credentials] ([CredentialID])
GO

ALTER TABLE [Scheduling].[AppointmentTypeCredential] CHECK CONSTRAINT [FK_AppointmentTypeCredential_Credentials]
GO

ALTER TABLE Scheduling.AppointmentTypeCredential WITH CHECK ADD CONSTRAINT [FK_AppointmentTypeCredential_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.AppointmentTypeCredential CHECK CONSTRAINT [FK_AppointmentTypeCredential_UserModifedBy]
GO
ALTER TABLE Scheduling.AppointmentTypeCredential WITH CHECK ADD CONSTRAINT [FK_AppointmentTypeCredential_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Scheduling.AppointmentTypeCredential CHECK CONSTRAINT [FK_AppointmentTypeCredential_UserCreatedBy]
GO

