 -----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[BatchStatus]
-- Author:		
-- Date:		
--
-- Purpose:		
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 09/03/2016	Rahul Vats		Review the table
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Synch].[BatchStatus] (
    [BatchStatusID]    INT            IDENTITY (1, 1) NOT NULL,
    [BatchStatus]      NVARCHAR (100) NOT NULL,
    [IsActive]         BIT            DEFAULT ((1)) NOT NULL,
    [ModifiedBy]       INT            NOT NULL,
    [ModifiedOn]       DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT            DEFAULT ((1)) NOT NULL,
    [CreatedOn]        DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [SystemCreatedOn]  DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [SystemModifiedOn] DATETIME       DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_BatchStatus] PRIMARY KEY CLUSTERED ([BatchStatusID] ASC)
)
GO

ALTER TABLE Synch.BatchStatus WITH CHECK ADD CONSTRAINT [FK_BatchStatus_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Synch.BatchStatus CHECK CONSTRAINT [FK_BatchStatus_UserModifedBy]
GO
ALTER TABLE Synch.BatchStatus WITH CHECK ADD CONSTRAINT [FK_BatchStatus_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Synch.BatchStatus CHECK CONSTRAINT [FK_BatchStatus_UserCreatedBy]
GO
