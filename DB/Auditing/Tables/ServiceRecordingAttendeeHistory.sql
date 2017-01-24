-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Auditing].[ServiceRecordingAttendee]
-- Author:		Scott Martin
-- Date:		12/12/2016
--
-- Purpose:		Stores history of service recording attendees
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/12/2016	Scott Martin		Initial Creation
----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Auditing.ServiceRecordingAttendeeHistory
(
	TransactionLogID BIGINT NOT NULL,
	ServiceRecordingAttendeeID BIGINT NOT NULL,
	ServiceRecordingID BIGINT NOT NULL,
	Name NVARCHAR(255) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE())
)