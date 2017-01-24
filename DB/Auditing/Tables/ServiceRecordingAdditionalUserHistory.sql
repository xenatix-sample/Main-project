-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Auditing].[ServiceRecordingAdditionalUser]
-- Author:		Scott Martin
-- Date:		12/12/2016
--
-- Purpose:		Stores history of additional users associated with Service Recording
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/12/02016	Scott Martin		Initial Creation
----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Auditing.ServiceRecordingAdditionalUserHistory
(
	TransactionLogID BIGINT NOT NULL,
	ServiceRecordingAdditionalUserID BIGINT NOT NULL,
	ServiceRecordingID BIGINT NOT NULL,
	UserID INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE())
)