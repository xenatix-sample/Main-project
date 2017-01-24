-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Synch].[BatchDetails]
-- Author:		Sumana Sangapu
-- Date:		03/01/2016
--
-- Purpose:		Holds the BatchDetails for the Batch processes
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/01/2016	Sumana Sangapu	 - Initial creation.
-- 09/03/2016	Rahul Vats		Review the table
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Synch].[BatchDetails](
	[BatchDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchID] [bigint] NULL,
	[BatchStatusID] [int] NOT NULL,
	[PackageName] [nvarchar] (100) NULL,
	[PackageTask] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[SystemCreatedOn] [datetime] NOT NULL,
	[SystemModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_BatchDetail] PRIMARY KEY CLUSTERED 
(
	[BatchDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Synch].[BatchDetails] ADD  CONSTRAINT [DF__BatchDetails__ModifiedO__440BE8B8]  DEFAULT (getutcdate()) FOR [ModifiedOn]
GO

ALTER TABLE [Synch].[BatchDetails] ADD  CONSTRAINT [DF__BatchDetails__CreatedBy__45000CF1]  DEFAULT ((1)) FOR [CreatedBy]
GO

ALTER TABLE [Synch].[BatchDetails] ADD  CONSTRAINT [DF__BatchDetails__CreatedOn__45F4312A]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO

ALTER TABLE [Synch].[BatchDetails] ADD  CONSTRAINT [DF__BatchDetails__SystemCre__46E85563]  DEFAULT (getutcdate()) FOR [SystemCreatedOn]
GO

ALTER TABLE [Synch].[BatchDetails] ADD  CONSTRAINT [DF__BatchDetails__SystemMod__47DC799C]  DEFAULT (getutcdate()) FOR [SystemModifiedOn]
GO