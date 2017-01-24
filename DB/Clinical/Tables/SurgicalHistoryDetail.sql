-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[SurgicalHistoryDetail]
-- Author:		John Crossen
-- Date:		11/30/2015
--
-- Purpose:		Surgical History Detail Table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/30/2015	John Crossen	TFS# 3899 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[SurgicalHistoryDetail](
	[SurgicalHistoryDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[SurgicalHistoryID] [bigint] NOT NULL,
	[Surgery] [nvarchar](255) NOT NULL,
	[Date] [date] NOT NULL,
	[Comments] [nvarchar](2000) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_SurgicalHistoryDetail] PRIMARY KEY CLUSTERED 
(
	[SurgicalHistoryDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[SurgicalHistoryDetail]  WITH CHECK ADD  CONSTRAINT [FK_SurgicalHistoryDetail_SurgicalHistory] FOREIGN KEY([SurgicalHistoryID])
REFERENCES [Clinical].[SurgicalHistory] ([SurgicalHistoryID])
GO

ALTER TABLE [Clinical].[SurgicalHistoryDetail] CHECK CONSTRAINT [FK_SurgicalHistoryDetail_SurgicalHistory]
GO

ALTER TABLE Clinical.SurgicalHistoryDetail WITH CHECK ADD CONSTRAINT [FK_SurgicalHistoryDetail_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.SurgicalHistoryDetail CHECK CONSTRAINT [FK_SurgicalHistoryDetail_UserModifedBy]
GO
ALTER TABLE Clinical.SurgicalHistoryDetail WITH CHECK ADD CONSTRAINT [FK_SurgicalHistoryDetail_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.SurgicalHistoryDetail CHECK CONSTRAINT [FK_SurgicalHistoryDetail_UserCreatedBy]
GO
