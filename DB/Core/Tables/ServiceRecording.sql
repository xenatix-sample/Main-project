-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[ServiceRecording]
-- Author:		Sumana Sangapu
-- Date:		01/28/2016
--
-- Purpose:		Holds the data for ServiceRecording. ServiceRecordingSourceID is pupulated from ServiceRecordingSource. And based on the 
--				Source the corresponding HeaderID from the source needs to be populated. For Ex: If Source is Call Center then pupuate the CallCenterHeaderID.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Any changes will need to be reflected in merge procs and history table
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/28/2016	Sumana Sangapu	 Initial creation.
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 03/23/2016	Scott Martin	Added new fields OrganizationID, ServiceTypeID, Service Start/End Date/Time, SupervisorID, TransmittalStatus
-- 04/08/2016   Lokesh Singhal  change ServiceStatusID and RecipientCodeID to allow null as on UI screen these fields are not mandatory
-- 06/02/2016   Gaurav Gupta    Added new filed RecipientCode
-- 06/16/2016	Scott Martin	Added ParentServiceRecordingID and ServiceRecordingHeaderID
-- 08/26/2016	Scott Martin	Added index
-- 10/15/2016	Sumana Sangapu	SenttoCMHCDate - new field to track the date/time on which the record was sent to CMHC
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[ServiceRecording](
	[ServiceRecordingID] [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceRecordingHeaderID] [BIGINT] NULL,
	[ParentServiceRecordingID] [BIGINT] NULL,
	[ServiceRecordingSourceID] [int] NOT NULL,
	[SourceHeaderID] [bigint] NOT NULL,
	[OrganizationID] [bigint] NULL,
	[ServiceTypeID] [smallint] NULL,
	[ServiceItemID] [int] NULL,
	[AttendanceStatusID] [smallint] NULL,
	[ServiceStatusID] [smallint] NULL,
	[RecipientCodeID] [smallint] NULL,
    [RecipientCode] [smallint] NULL,
	[DeliveryMethodID] [smallint] NULL,
	[ServiceLocationID] [smallint] NULL,
	[ServiceStartDate] [datetime] NULL,
	[ServiceEndDate] [datetime] NULL,
	[TransmittalStatus] NVARCHAR(255) NULL, --Placeholder for CMHC sent Status
	[NumberOfRecipients] [smallint] NULL, --Need to remove
	[TrackingFieldID] [int] NULL,
	[SupervisorUserID] [int] NULL,
	[SentToCMHCDate] [datetime] NULL,
	[UserID] [int] NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate())
 CONSTRAINT [PK_ServiceRecordingID] PRIMARY KEY CLUSTERED 
(
	[ServiceRecordingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IX_ServiceRecording] ON [Core].[ServiceRecording]
(
	[UserID] ASC,
	[OrganizationID] ASC,
	[AttendanceStatusID] ASC,
	[ServiceLocationID] ASC,
	[DeliveryMethodID] ASC,
	[SystemCreatedOn] ASC,
	[SystemModifiedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_ServiceRecording_ItemID_StatusID] ON [Core].[ServiceRecording]
(
	[ServiceItemID] ASC,
	[ServiceStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE  [Core].[ServiceRecording]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecording_ServiceRecordingHeaderID] FOREIGN KEY([ServiceRecordingHeaderID]) REFERENCES [Core].[ServiceRecordingHeader] ([ServiceRecordingHeaderID])
GO
ALTER TABLE  [Core].[ServiceRecording] CHECK CONSTRAINT [FK_ServiceRecording_ServiceRecordingHeaderID]
GO
ALTER TABLE  [Core].[ServiceRecording]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecording_ServiceRecordingSourceID] FOREIGN KEY([ServiceRecordingSourceID]) REFERENCES [Reference].[ServiceRecordingSource] ([ServiceRecordingSourceID])
GO
ALTER TABLE  [Core].[ServiceRecording] CHECK CONSTRAINT [FK_ServiceRecording_ServiceRecordingSourceID]
GO
ALTER TABLE  [Core].[ServiceRecording]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecording_OrganizationID] FOREIGN KEY([OrganizationID]) REFERENCES [Core].[OrganizationDetailsMapping] ([MappingID])
GO
ALTER TABLE  [Core].[ServiceRecording] CHECK CONSTRAINT [FK_ServiceRecording_OrganizationID]
GO

ALTER TABLE  [Core].[ServiceRecording]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecording_ServiceTypeID] FOREIGN KEY([ServiceTypeID]) REFERENCES [Reference].[ServiceType] ([ServiceTypeID])
GO

ALTER TABLE  [Core].[ServiceRecording] CHECK CONSTRAINT [FK_ServiceRecording_ServiceTypeID]
GO

ALTER TABLE  [Core].[ServiceRecording]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecording_ServiceItemID] FOREIGN KEY([ServiceItemID]) REFERENCES [Reference].[Services] ([ServicesID])
GO

ALTER TABLE  [Core].[ServiceRecording] CHECK CONSTRAINT [FK_ServiceRecording_ServiceItemID]
GO

ALTER TABLE  [Core].[ServiceRecording]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecording_AttendanceStatusID] FOREIGN KEY([AttendanceStatusID]) REFERENCES [Reference].[AttendanceStatus] ([AttendanceStatusID])
GO

ALTER TABLE  [Core].[ServiceRecording] CHECK CONSTRAINT [FK_ServiceRecording_AttendanceStatusID]
GO

ALTER TABLE  [Core].[ServiceRecording]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecording_DeliveryMethodID] FOREIGN KEY([DeliveryMethodID]) REFERENCES [Reference].[DeliveryMethod] ([DeliveryMethodID])
GO

ALTER TABLE  [Core].[ServiceRecording] CHECK CONSTRAINT [FK_ServiceRecording_DeliveryMethodID]
GO

ALTER TABLE  [Core].[ServiceRecording]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecording_ServiceStatusID] FOREIGN KEY([ServiceStatusID]) REFERENCES [Reference].[ServiceStatus] ([ServiceStatusID])
GO

ALTER TABLE  [Core].[ServiceRecording] CHECK CONSTRAINT [FK_ServiceRecording_ServiceStatusID]
GO

ALTER TABLE  [Core].[ServiceRecording]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecording_ServiceLocationID] FOREIGN KEY([ServiceLocationID]) REFERENCES [Reference].[ServiceLocation] ([ServiceLocationID])
GO

ALTER TABLE  [Core].[ServiceRecording] CHECK CONSTRAINT [FK_ServiceRecording_ServiceLocationID]
GO

ALTER TABLE  [Core].[ServiceRecording]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecording_RecipientCodeID] FOREIGN KEY([RecipientCodeID]) REFERENCES [Reference].[RecipientCode] ([CodeID])
GO

ALTER TABLE  [Core].[ServiceRecording] CHECK CONSTRAINT [FK_ServiceRecording_RecipientCodeID]
GO

ALTER TABLE  [Core].[ServiceRecording]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecording_TrackingFieldID] FOREIGN KEY([TrackingFieldID]) REFERENCES [Reference].[TrackingField] ([TrackingFieldID])
GO

ALTER TABLE  [Core].[ServiceRecording] CHECK CONSTRAINT [FK_ServiceRecording_TrackingFieldID]
GO

ALTER TABLE  [Core].[ServiceRecording]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRecording_UserID] FOREIGN KEY([UserID]) REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE  [Core].[ServiceRecording] CHECK CONSTRAINT [FK_ServiceRecording_UserID]
GO

ALTER TABLE Core.ServiceRecording WITH CHECK ADD CONSTRAINT [FK_ServiceRecording_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ServiceRecording CHECK CONSTRAINT [FK_ServiceRecording_UserModifedBy]
GO
ALTER TABLE Core.ServiceRecording WITH CHECK ADD CONSTRAINT [FK_ServiceRecording_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ServiceRecording CHECK CONSTRAINT [FK_ServiceRecording_UserCreatedBy]
GO
