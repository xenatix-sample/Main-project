-----------------------------------------------------------------------------------------------------------------------
-- Table:	    Core.MergedContactResult
-- Author:		Scott Martin
-- Date:		08/09/2015
--
-- Purpose:		Audit functionality
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/09/201	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[MergedContactResult](
	[MergedContactResultID] BIGINT IDENTITY(1,1) NOT NULL,
	[MergedContactsMappingID] BIGINT NOT NULL,
	[IsSuccessful] BIT NOT NULL DEFAULT(0),
	[ModuleComponentID] BIGINT,
	[TotalRecords] INT DEFAULT(0),
	[TotalRecordsMerged] INT DEFAULT(0),
	[ResultMessage] NVARCHAR(MAX)
 CONSTRAINT [PK_MergedContactResult_MergedContactResultID] PRIMARY KEY CLUSTERED 
(
	[MergedContactResultID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[MergedContactResult]  WITH CHECK ADD  CONSTRAINT [FK_MergedContactResult_MergedContactsMappingID] FOREIGN KEY([MergedContactsMappingID]) REFERENCES [Core].[MergedContactsMapping] ([MergedContactsMappingID])
GO
ALTER TABLE [Core].[MergedContactResult] CHECK CONSTRAINT [FK_MergedContactResult_MergedContactsMappingID]
GO
ALTER TABLE [Core].[MergedContactResult]  WITH CHECK ADD  CONSTRAINT [FK_MergedContactResult_ModuleComponentID] FOREIGN KEY([ModuleComponentID]) REFERENCES [Core].[ModuleComponent] ([ModuleComponentID])
GO
ALTER TABLE [Core].[MergedContactResult] CHECK CONSTRAINT [FK_MergedContactResult_ModuleComponentID]
GO