-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[AssessmentExpirationReason]
-- Author:		Scott Martin
-- Date:		04/08/2016
--
-- Purpose:		Lookup for Assessment Expiration Reasons
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/08/2016	Scott Martin	Initial creation.
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[AssessmentExpirationReason](
	[AssessmentExpirationReasonID] [int] IDENTITY (1,1) NOT NULL,
	[AssessmentExpirationReason] [nvarchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsSystem] [bit] NOT NULL DEFAULT(0),
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AssessmentExpirationReason_AssessmentExpirationReasonID] PRIMARY KEY CLUSTERED 
(
	[AssessmentExpirationReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[AssessmentExpirationReason] ADD CONSTRAINT IX_AssessmentExpirationReason UNIQUE(AssessmentExpirationReason)
GO

ALTER TABLE Reference.AssessmentExpirationReason WITH CHECK ADD CONSTRAINT [FK_AssessmentExpirationReason_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AssessmentExpirationReason CHECK CONSTRAINT [FK_AssessmentExpirationReason_UserModifedBy]
GO
ALTER TABLE Reference.AssessmentExpirationReason WITH CHECK ADD CONSTRAINT [FK_AssessmentExpirationReason_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AssessmentExpirationReason CHECK CONSTRAINT [FK_AssessmentExpirationReason_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Assessment Expiration Reason', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentExpirationReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating the reason for expiring an assessment', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentExpirationReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentExpirationReason;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentExpirationReason;
GO;