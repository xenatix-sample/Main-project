-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[AssessmentLogicLocation]
-- Author:		Kyle Campbell
-- Date:		03/28/2016
--
-- Purpose:		Holds the values to define if assessment logic is performed on the assessment question or input
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/28/2016	Kyle Campbell	TFS #8273	Initial Creation
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[AssessmentLogicLocation](
	[LogicLocationID] [int] IDENTITY(1,1) NOT NULL,
	[LogicLocation] [nvarchar](50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AssessmentLogicLocation] PRIMARY KEY CLUSTERED 
(
	[LogicLocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE Reference.AssessmentLogicLocation WITH CHECK ADD CONSTRAINT [FK_AssessmentLogicLocation_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AssessmentLogicLocation CHECK CONSTRAINT [FK_AssessmentLogicLocation_UserModifedBy]
GO
ALTER TABLE Reference.AssessmentLogicLocation WITH CHECK ADD CONSTRAINT [FK_AssessmentLogicLocation_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.AssessmentLogicLocation CHECK CONSTRAINT [FK_AssessmentLogicLocation_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Assessment Logic Location', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentLogicLocation;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values that specify where logic code is supposed to execute in the assessments (the question or input field)', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentLogicLocation;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentLogicLocation;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'0', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentLogicLocation;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores the values that specify where logic code is supposed to execute in the assessments (the question or input field)', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = AssessmentLogicLocation,
@level2type = N'COLUMN', @level2name = LogicLocation;
GO