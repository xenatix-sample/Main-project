-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ServiceRecordingVoid]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		User/Photo mapping data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Any changes will need to be reflected in merge procs and history table
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Scott Martin		Initial Creation
-- 03/31/2016   Lokesh Singhal      Set IncorrectEndDate to allow null as this is extra column added.
----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Core.ServiceRecordingVoid
(
	ServiceRecordingVoidID BIGINT IDENTITY(1,1) NOT NULL,
	ServiceRecordingID BIGINT NOT NULL,
	ServiceRecordingVoidReasonID SMALLINT NOT NULL,
	IncorrectOrganization BIT NOT NULL DEFAULT(0),
	IncorrectServiceType BIT NOT NULL DEFAULT(0),
	IncorrectServiceItem BIT NOT NULL DEFAULT(0),
	IncorrectServiceStatus BIT NOT NULL DEFAULT(0),
	IncorrectSupervisor BIT NOT NULL DEFAULT(0),
	IncorrectAdditionalUser BIT NOT NULL DEFAULT(0),
	IncorrectAttendanceStatus BIT NOT NULL DEFAULT(0),
	IncorrectStartDate BIT NOT NULL DEFAULT(0),
	IncorrectStartTime BIT NOT NULL DEFAULT(0),
	IncorrectEndDate BIT NULL DEFAULT(0),
	IncorrectEndTime BIT NOT NULL DEFAULT(0),
	IncorrectDeliveryMethod BIT NOT NULL DEFAULT(0),
	IncorrectServiceLocation BIT NOT NULL DEFAULT(0),
	IncorrectRecipientCode BIT NOT NULL DEFAULT(0),
	IncorrectTrackingField BIT NOT NULL DEFAULT(0),
	Comments NVARCHAR(1000) NULL,
	NoteHeaderID BIGINT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ServiceRecordingVoid_ServiceRecordingVoidID] PRIMARY KEY CLUSTERED 
(
	[ServiceRecordingVoidID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[ServiceRecordingVoid]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecordingVoid_ServiceRecordingID] FOREIGN KEY([ServiceRecordingID])
REFERENCES [Core].[ServiceRecording] ([ServiceRecordingID])
GO

ALTER TABLE Core.ServiceRecordingVoid CHECK CONSTRAINT [FK_ServiceRecordingVoid_ServiceRecordingID]
GO

ALTER TABLE [Core].[ServiceRecordingVoid]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecordingVoid_ServiceRecordingVoidReasonID] FOREIGN KEY([ServiceRecordingVoidReasonID])
REFERENCES [Reference].[ServiceRecordingVoidReason] ([ServiceRecordingVoidReasonID])
GO

ALTER TABLE Core.ServiceRecordingVoid CHECK CONSTRAINT [FK_ServiceRecordingVoid_ServiceRecordingID]
GO

ALTER TABLE [Core].[ServiceRecordingVoid]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecordingVoid_NoteHeaderID] FOREIGN KEY([NoteHeaderID])
REFERENCES [Registration].[NoteHeader] ([NoteHeaderID])
GO

ALTER TABLE Core.ServiceRecordingVoid CHECK CONSTRAINT [FK_ServiceRecordingVoid_NoteHeaderID]
GO

ALTER TABLE Core.ServiceRecordingVoid WITH CHECK ADD CONSTRAINT [FK_ServiceRecordingVoid_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ServiceRecordingVoid CHECK CONSTRAINT [FK_ServiceRecordingVoid_UserModifedBy]
GO
ALTER TABLE Core.ServiceRecordingVoid WITH CHECK ADD CONSTRAINT [FK_ServiceRecordingVoid_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ServiceRecordingVoid CHECK CONSTRAINT [FK_ServiceRecordingVoid_UserCreatedBy]
GO
