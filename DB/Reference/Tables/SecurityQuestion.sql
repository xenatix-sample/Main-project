-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[SecurityQuestion]
-- Author:		Rajiv Ranjan
-- Date:		08/05/2015
--
-- Purpose:		Security Question details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/05/2015	Rajiv Ranjan	 Initial creation.
-- 08/25/2015	Rajiv Ranjan	 Moved table from Core to Reference
-- 08/26/2015	Rajiv Ranjan	 Changed SecurityQuestionID's datatype to INT
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[SecurityQuestion] 
(
    [SecurityQuestionID]     INT IDENTITY(1,1) NOT NULL,
    [Question]       NVARCHAR (500) NOT NULL,
    [QuestionDescription]          NVARCHAR (500)  NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_SecurityQuestion_SecurityQuestionID] PRIMARY KEY CLUSTERED ([SecurityQuestionID] ASC)
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
	
) ON [PRIMARY]

GO

ALTER TABLE Reference.SecurityQuestion WITH CHECK ADD CONSTRAINT [FK_SecurityQuestion_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.SecurityQuestion CHECK CONSTRAINT [FK_SecurityQuestion_UserModifedBy]
GO
ALTER TABLE Reference.SecurityQuestion WITH CHECK ADD CONSTRAINT [FK_SecurityQuestion_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.SecurityQuestion CHECK CONSTRAINT [FK_SecurityQuestion_UserCreatedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Security Questions', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SecurityQuestion;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Questions the system uses as hints for password retrieval', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SecurityQuestion;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SecurityQuestion;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = SecurityQuestion;
GO;

