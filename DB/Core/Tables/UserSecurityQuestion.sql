-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[UserSecurityQuestion]
-- Author:		Rajiv Ranjan
-- Date:		08/05/2015
--
-- Purpose:		User Security Question details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/05/2015	Rajiv Ranjan	 Initial creation.
-- 08/25/2015	Rajiv Ranjan	 Schema changed from core to reference for SecurityQuestion
-- 08/26/2015	Rajiv Ranjan	 Changed SecurityQuestionID's datatype to INT
-- 09/30/2015   Justin Spalti    Removed the space at the end of the table name
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[UserSecurityQuestion] 
(
    [UserSecurityQuestionID]     BIGINT IDENTITY(1,1) NOT NULL,
    [UserID]       INT NOT NULL,
    [SecurityQuestionID]         INT NOT NULL,
	[SecurityAnswer]  NVARCHAR (250)  NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [FK_UserSecurityQuestion_UserID] FOREIGN KEY ([UserID]) REFERENCES [Core].[Users] ([UserID]) ,
	CONSTRAINT [FK_UserSecurityQuestion_SecurityQuestionID] FOREIGN KEY ([SecurityQuestionID]) REFERENCES [Reference].[SecurityQuestion] ([SecurityQuestionID]),
    CONSTRAINT [PK_UserSecurityQuestion_UserSecurityQuestionID] PRIMARY KEY CLUSTERED ([UserSecurityQuestionID] ASC)
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
	
) ON [PRIMARY]
GO

ALTER TABLE Core.UserSecurityQuestion WITH CHECK ADD CONSTRAINT [FK_UserSecurityQuestion_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserSecurityQuestion CHECK CONSTRAINT [FK_UserSecurityQuestion_UserModifedBy]
GO
ALTER TABLE Core.UserSecurityQuestion WITH CHECK ADD CONSTRAINT [FK_UserSecurityQuestion_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.UserSecurityQuestion CHECK CONSTRAINT [FK_UserSecurityQuestion_UserCreatedBy]
GO


