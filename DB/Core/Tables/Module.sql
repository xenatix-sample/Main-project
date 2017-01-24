-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Module]
-- Author:		Rajiv Ranjan
-- Date:		07/29/2015
--
-- Purpose:		Module Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Rajiv Ranjan	 Initial creation.
-- 07/30/2015   John Crossen     Change schema from dbo to Core
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn		
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 05/13/2016	Scott Martin	Added unique constraint for Name
-- 09/07/2016	Rahul Vats		Review the table
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[Module](
	[ModuleID] BIGINT IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Description] [nvarchar](1000) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()), 
    CONSTRAINT [PK_Module_ModuleID] PRIMARY KEY CLUSTERED 
	(
		[ModuleID] ASC
	) 
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Core.Module WITH CHECK ADD CONSTRAINT [FK_Module_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Module CHECK CONSTRAINT [FK_Module_UserModifedBy]
GO
ALTER TABLE Core.Module WITH CHECK ADD CONSTRAINT [FK_Module_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Module CHECK CONSTRAINT [FK_Module_UserCreatedBy]
GO
ALTER TABLE Core.Module WITH CHECK ADD CONSTRAINT [UC_Module_Name] UNIQUE ([Name])
GO
ALTER TABLE Core.Module CHECK CONSTRAINT [UC_Module_Name]
GO