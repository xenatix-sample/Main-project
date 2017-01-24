-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Clinical].[[[HPISeverity]]]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		HPI Serverity Lookup table
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/19/2015	John Crossen	TFS# 3665 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Clinical].[HPISeverity](
	[HPISeverityID] [SMALLINT] IDENTITY(1,1) NOT NULL,
	[HPISeverity] [NVARCHAR](255) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_HPISeverity] PRIMARY KEY CLUSTERED 
(
	[HPISeverityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_HPISeverity_HPISeverityID] ON [Clinical].[HPISeverity]
(
	[HPISeverity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE Clinical.HPISeverity WITH CHECK ADD CONSTRAINT [FK_HPISeverity_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.HPISeverity CHECK CONSTRAINT [FK_HPISeverity_UserModifedBy]
GO
ALTER TABLE Clinical.HPISeverity WITH CHECK ADD CONSTRAINT [FK_HPISeverity_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Clinical.HPISeverity CHECK CONSTRAINT [FK_HPISeverity_UserCreatedBy]
GO
