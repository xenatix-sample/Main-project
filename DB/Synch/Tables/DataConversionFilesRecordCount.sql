 -----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[DataConversionFilesRecordCount]
-- Author:		Sumana Sangapu
-- Date:		05/19/2016
--
-- Purpose:		Data Conversion record count between source and destination
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Synch].[DataConversionFilesRecordCount](
	[RecordCountID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](100) NULL,
	[SourceCount] [int] NULL,
	[DestinationCount] [int] NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_DataConversionFilesRecordCount] PRIMARY KEY CLUSTERED 
(
	[RecordCountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


