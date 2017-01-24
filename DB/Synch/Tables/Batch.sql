-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Synch].[Batch]
-- Author:		Sumana Sangapu
-- Date:		03/01/2016
--
-- Purpose:		Holds the Batch data for the Batch processes
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/01/2016	Chad Roberts - Initial creation.
-- 03/01/201	Sumana Sangapu	Added BatchTypeID column
-- 09/07/2016	Rahul Vats		Review the table
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Synch].[Batch] (
    [BatchID]          BIGINT   IDENTITY (1, 1) NOT NULL,
    [BatchStatusID]    INT      NOT NULL,
	[BatchTypeID]	   INT		NOT NULL,
    [ConfigID]         INT      NOT NULL,
    [USN]              BIGINT   CONSTRAINT [DF_Batch_usn] DEFAULT ((0)) NOT NULL,
    [IsActive]         BIT      NOT NULL,
    [ModifiedBy]       INT      NOT NULL,
    [ModifiedOn]       DATETIME CONSTRAINT [DF_Batch_ModifiedOn] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT      CONSTRAINT [DF_Batch_CreatedBy] DEFAULT ((1)) NOT NULL,
    [CreatedOn]        DATETIME CONSTRAINT [DF_Batch_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [SystemCreatedOn]  DATETIME CONSTRAINT [DF_Batch_SystemCreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [SystemModifiedOn] DATETIME CONSTRAINT [DF_Batch_SystemModifiedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Batch] PRIMARY KEY CLUSTERED ([BatchID] ASC)
)
GO

ALTER TABLE [Synch].[Batch]  WITH CHECK ADD  CONSTRAINT [FK_Batch_BatchStatus] FOREIGN KEY([BatchStatusID])
REFERENCES [Synch].[BatchStatus] ([BatchStatusID])
GO

GO

ALTER TABLE Synch.Batch WITH CHECK ADD CONSTRAINT [FK_Batch_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Synch.Batch CHECK CONSTRAINT [FK_Batch_UserModifedBy]
GO
ALTER TABLE Synch.Batch WITH CHECK ADD CONSTRAINT [FK_Batch_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Synch.Batch CHECK CONSTRAINT [FK_Batch_UserCreatedBy]
GO

ALTER TABLE [Synch].[Batch]  WITH CHECK ADD  CONSTRAINT [FK_Batch_Config] FOREIGN KEY([ConfigID])
REFERENCES [Synch].[Config] ([ConfigID])
GO

ALTER TABLE [Synch].[Batch] CHECK CONSTRAINT [FK_Batch_Config]
GO

ALTER TABLE [Synch].[Batch]  WITH CHECK ADD  CONSTRAINT [FK_Batch_BatchType] FOREIGN KEY([BatchTypeID])
REFERENCES [Synch].[BatchType] ([BatchTypeID])
GO

ALTER TABLE [Synch].[Batch] CHECK CONSTRAINT [FK_Batch_BatchType]
GO