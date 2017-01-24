


-----------------------------------------------------------------------------------------------------------------------
-- Table:		[CallCenter].[BehavioralCategory]
-- Author:		John Crossen
-- Date:		01/15/2016
--
-- Purpose:		Lookup Table for BehavioralCategory
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/08/2016	John Crossen	TFS#6217 - Initial creation.
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns

CREATE TABLE [CallCenter].[BehavioralCategory](
[BehavioralCategoryID] [SMALLINT] IDENTITY(1,1) NOT NULL,
[BehavioralCategory] [NVARCHAR](75) NOT NULL,
[IsActive] BIT NOT NULL DEFAULT(1),
[ModifiedBy] INT NOT NULL,
[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[CreatedBy] INT NOT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_BehavioralCategory] PRIMARY KEY CLUSTERED 
(
	[BehavioralCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE CallCenter.BehavioralCategory WITH CHECK ADD CONSTRAINT [FK_BehavioralCategory_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.BehavioralCategory CHECK CONSTRAINT [FK_BehavioralCategory_UserModifedBy]
GO
ALTER TABLE CallCenter.BehavioralCategory WITH CHECK ADD CONSTRAINT [FK_BehavioralCategory_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE CallCenter.BehavioralCategory CHECK CONSTRAINT [FK_BehavioralCategory_UserCreatedBy]
GO



