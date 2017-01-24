-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ServiceRecordingVoid]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		User/Photo mapping data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/12/2016	Scott Martin		Initial Creation
----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Auditing.ServiceRecordingVoidHistory
(
	TransactionLogID BIGINT NOT NULL,
	ServiceRecordingVoidID BIGINT NOT NULL,
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
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE())
)