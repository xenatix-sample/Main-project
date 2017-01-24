
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[CMHCFilesRecordCount]
-- Author:		Sumana Sangapu
-- Date:		08/16/2016
--
-- Purpose:		Holds the CMHC Files Record count from CMHC Packages
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/16/2016	Sumana Sangapu	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Synch].[CMHCFilesRecordCount](
	[RecordCountID] [int] IDENTITY(1,1) NOT NULL,
	[BatchID] [BIGINT] NOT NULL,
	[BatchTypeID] INT NOT NULL,
	[SourceRecordCount] [int] NULL,
 	[DestinationRecordCount] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_CMHCFilesRecordCount] PRIMARY KEY CLUSTERED 
(
	[RecordCountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Synch].[CMHCFilesRecordCount] WITH CHECK ADD CONSTRAINT [FK_CMHCFilesRecordCount_BatchID] FOREIGN KEY (BatchID) REFERENCES Synch.Batch (BatchID)
GO

ALTER TABLE [Synch].[CMHCFilesRecordCount] WITH CHECK ADD CONSTRAINT [FK_CMHCFilesRecordCount_BatchTypeID] FOREIGN KEY (BatchTypeID) REFERENCES Synch.BatchType (BatchTypeID)
GO