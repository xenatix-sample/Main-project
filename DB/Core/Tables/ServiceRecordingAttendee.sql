-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ServiceRecordingAttendee]
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
----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Core.ServiceRecordingAttendee
(
	ServiceRecordingAttendeeID BIGINT IDENTITY(1,1) NOT NULL,
	ServiceRecordingID BIGINT NOT NULL,
	Name NVARCHAR(255) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ServiceRecordingAttendee_ServiceRecordingAttendeeID] PRIMARY KEY CLUSTERED 
(
	[ServiceRecordingAttendeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[ServiceRecordingAttendee]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecordingAttendee_ServiceRecordingID] FOREIGN KEY([ServiceRecordingID])
REFERENCES [Core].[ServiceRecording] ([ServiceRecordingID])
GO

ALTER TABLE Core.ServiceRecordingAttendee WITH CHECK ADD CONSTRAINT [FK_ServiceRecordingAttendee_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ServiceRecordingAttendee CHECK CONSTRAINT [FK_ServiceRecordingAttendee_UserModifedBy]
GO
ALTER TABLE Core.ServiceRecordingAttendee WITH CHECK ADD CONSTRAINT [FK_ServiceRecordingAttendee_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ServiceRecordingAttendee CHECK CONSTRAINT [FK_ServiceRecordingAttendee_UserCreatedBy]
GO
