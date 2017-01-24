-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[EducationStatus]
-- Author:		Sumana Sangapu
-- Date:		07/21/2015
--
-- Purpose:		Lookup for EducationStatus  details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015	Sumana Sangapu	TFS# 675 - Initial creation.
-- 07/24/2015   John Crossen    Change IsActive to NOT NULL and add default value
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[EducationStatus](
	[EducationStatusID] [int] IDENTITY (1,1) NOT NULL,
	[EducationStatus] [nvarchar](50) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_EducationStatus_EducationStatusID] PRIMARY KEY CLUSTERED 
(
	[EducationStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[EducationStatus] ADD CONSTRAINT IX_EducationStatus UNIQUE(EducationStatus)
GO
ALTER TABLE Reference.EducationStatus WITH CHECK ADD CONSTRAINT [FK_EducationStatus_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.EducationStatus CHECK CONSTRAINT [FK_EducationStatus_UserModifedBy]
GO
ALTER TABLE Reference.EducationStatus WITH CHECK ADD CONSTRAINT [FK_EducationStatus_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.EducationStatus CHECK CONSTRAINT [FK_EducationStatus_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Education Status', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = EducationStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating the highest level of education completed', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = EducationStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = EducationStatus;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = EducationStatus;
GO;
