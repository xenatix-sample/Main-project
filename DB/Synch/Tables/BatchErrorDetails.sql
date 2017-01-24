 -----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[BatchErrorDetails]
-- Author:		Sumana Sangapu
-- Date:		08/16/2016
--
-- Purpose:		Holds the error details of the SSIS packages
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/16/2016	Sumana Sangapu	- Initial creation.
-- 09/03/2016	Rahul Vats		Review the table
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Synch].[BatchErrorDetails] (
	[BatchErrorDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchID] [bigint] NULL,
	[BatchStatusID] [int] NOT NULL,
	[PackageName] [nvarchar](100) NULL,
	[PackageTask] [nvarchar](50) NULL,
	[ErrorCode] [nvarchar](50) NULL,
	[ErrorDescription] [nvarchar](1000) NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[SystemCreatedOn] [datetime] NOT NULL,
	[SystemModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_BatchErrorDetail] PRIMARY KEY CLUSTERED 
(
	[BatchErrorDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO

ALTER TABLE [Synch].[BatchErrorDetails] ADD  CONSTRAINT [DF__BatchErrorDetailsModifiedOn]  DEFAULT (getutcdate()) FOR [ModifiedOn]
GO

ALTER TABLE [Synch].[BatchErrorDetails] ADD  CONSTRAINT [DF__BatchErrorDetailsCreatedBy]  DEFAULT ((1)) FOR [CreatedBy]
GO

ALTER TABLE [Synch].[BatchErrorDetails] ADD  CONSTRAINT [DF__BatchErrorDetailsCreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO

ALTER TABLE [Synch].[BatchErrorDetails] ADD  CONSTRAINT [DF__BatchErrorDetailsSystemCreatedOn]  DEFAULT (getutcdate()) FOR [SystemCreatedOn]
GO

ALTER TABLE [Synch].[BatchErrorDetails] ADD  CONSTRAINT [DF__BatchErrorDetailsSystemModifiedOn]  DEFAULT (getutcdate()) FOR [SystemModifiedOn]
GO

ALTER TABLE  [Synch].[BatchErrorDetails] WITH CHECK ADD CONSTRAINT [FK_BatchErrorDetails_BatchID] FOREIGN KEY (BatchID) REFERENCES Synch.Batch (BatchID)
GO

ALTER TABLE  [Synch].[BatchErrorDetails] WITH CHECK ADD CONSTRAINT [FK_BatchErrorDetails_BatchStatusID] FOREIGN KEY (BatchStatusID) REFERENCES Synch.BatchStatus (BatchStatusID)
GO
