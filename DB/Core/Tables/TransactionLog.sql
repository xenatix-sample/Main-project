-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[TransactionLog]
-- Author:		Scott Martin
-- Date:		09/16/2016
--
-- Purpose:		Stores unique IDs to be used for groups of transactions (insert, update, delete, etc)
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/16/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[TransactionLog](
	[TransactionLogID] BIGINT NOT NULL,
	[TransactionID] INT NOT NULL,
    [TransactionDate] DATE NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
	[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_TransactionLog_TransactionLogID] PRIMARY KEY CLUSTERED
	(
		[TransactionLogID] ASC
	) 
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Core.TransactionLog WITH CHECK ADD CONSTRAINT [FK_TransactionLog_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.TransactionLog CHECK CONSTRAINT [FK_TransactionLog_UserCreatedBy]
GO
ALTER TABLE Core.TransactionLog ADD CONSTRAINT [IX_TransactionID_TransactionDate] UNIQUE (TransactionID, TransactionDate)
GO

