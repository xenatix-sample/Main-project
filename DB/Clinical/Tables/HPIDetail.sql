-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[[[HPIDetail]]]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		HPI HeaderDetail table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/19/2015	John Crossen	TFS# 3665 - Initial creation.
-- 11/20/2015	Scott Martin	ModifiedBy column was spelled incorrectly
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[HPIDetail](
	[HPIDetailID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[HPIID] [BIGINT] NOT NULL,
	[Comment] [NVARCHAR](2000) NULL,
	[Location] [NVARCHAR](255) NULL,
	[HPISeverityID] [SMALLINT] NULL,
	[Quality] [NVARCHAR](255) NULL,
	[Duration] [NVARCHAR](255) NULL,
	[Timing] [NVARCHAR](255) NULL,
	[ModifyingFactors] [NVARCHAR](255) NULL,
	[Conditions] [NVARCHAR](255) NULL,
	[Symptoms] NVARCHAR(255) NULL, 
    [Context] NVARCHAR(255) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_HPIDetail] PRIMARY KEY CLUSTERED 
(
	[HPIDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Clinical].[HPIDetail]  WITH CHECK ADD  CONSTRAINT [FK_HPIDetail_HPI] FOREIGN KEY([HPIID])
REFERENCES [Clinical].[HPI] ([HPIID])
GO

ALTER TABLE [Clinical].[HPIDetail] CHECK CONSTRAINT [FK_HPIDetail_HPI]
GO

ALTER TABLE [Clinical].[HPIDetail]  WITH CHECK ADD  CONSTRAINT [FK_HPIDetail_HPISeverity] FOREIGN KEY([HPISeverityID])
REFERENCES [Clinical].[HPISeverity] ([HPISeverityID])
GO

ALTER TABLE [Clinical].[HPIDetail] CHECK CONSTRAINT [FK_HPIDetail_HPISeverity]
GO

ALTER TABLE Clinical.HPIDetail WITH CHECK ADD CONSTRAINT [FK_HPIDetail_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.HPIDetail CHECK CONSTRAINT [FK_HPIDetail_UserModifedBy]
GO
ALTER TABLE Clinical.HPIDetail WITH CHECK ADD CONSTRAINT [FK_HPIDetail_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.HPIDetail CHECK CONSTRAINT [FK_HPIDetail_UserCreatedBy]
GO

