-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Email]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Store Email data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification .

-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[Email] (
    [EmailID]    BIGINT				NOT NULL	IDENTITY (1, 1) ,
    [Email]      NVARCHAR (255)		NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_Email_EmailID]	PRIMARY KEY CLUSTERED ([EmailID] ASC)
)
GO

ALTER TABLE Core.Email WITH CHECK ADD CONSTRAINT [FK_Email_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Email CHECK CONSTRAINT [FK_Email_UserModifedBy]
GO
ALTER TABLE Core.Email WITH CHECK ADD CONSTRAINT [FK_Email_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Email CHECK CONSTRAINT [FK_Email_UserCreatedBy]
GO




