-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Auditing].[ServiceRecordingHistory]
-- Author:		Scott Martin
-- Date:		12/12/2016
--
-- Purpose:		Holds a snapshot of a service recording
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/12/2016	Scott Martin	 Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Auditing].[ServiceRecordingHistory](
	[TransactionLogID] [bigint] NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[ServiceRecordingID] [bigint] NOT NULL,
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
)
