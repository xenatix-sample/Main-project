-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ServiceRecordingAdditionalUser]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		Stores additional users associated with Service Recording
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Any changes will need to be reflected in merge procs and history table
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Scott Martin		Initial Creation
----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Core.ServiceRecordingAdditionalUser
(
	ServiceRecordingAdditionalUserID BIGINT IDENTITY(1,1) NOT NULL,
	ServiceRecordingID BIGINT NOT NULL,
	UserID INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ServiceRecordingAdditionalUser_ServiceRecordingAdditionalUserID] PRIMARY KEY CLUSTERED 
(
	[ServiceRecordingAdditionalUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[ServiceRecordingAdditionalUser]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecordingAdditionalUser_ServiceRecordingID] FOREIGN KEY([ServiceRecordingID])
REFERENCES [Core].[ServiceRecording] ([ServiceRecordingID])
GO

ALTER TABLE [Core].[ServiceRecordingAdditionalUser] CHECK CONSTRAINT [FK_ServiceRecordingAdditionalUser_ServiceRecordingID]
GO

ALTER TABLE [Core].[ServiceRecordingAdditionalUser]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecordingAdditionalUser_UserID] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [Core].[ServiceRecordingAdditionalUser] CHECK CONSTRAINT [FK_ServiceRecordingAdditionalUser_UserID]
GO

ALTER TABLE Core.ServiceRecordingAdditionalUser WITH CHECK ADD CONSTRAINT [FK_ServiceRecordingAdditionalUser_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ServiceRecordingAdditionalUser CHECK CONSTRAINT [FK_ServiceRecordingAdditionalUser_UserModifedBy]
GO
ALTER TABLE Core.ServiceRecordingAdditionalUser WITH CHECK ADD CONSTRAINT [FK_ServiceRecordingAdditionalUser_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ServiceRecordingAdditionalUser CHECK CONSTRAINT [FK_ServiceRecordingAdditionalUser_UserCreatedBy]
GO
