-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[Template]
-- Author:		Rajiv Ranjan
-- Date:		08/05/2015
--
-- Purpose:		Type of Template
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/05/2015	Rajiv Ranjan	 Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[Template] 
(
    [TemplateID]     BIGINT IDENTITY(1,1) NOT NULL,
	[TemplateName]  NVARCHAR (250)  NOT NULL,
	[TemplateDescription]  NVARCHAR (250)  NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_Template_TemplateID] PRIMARY KEY CLUSTERED ([TemplateID] ASC)
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
	
) ON [PRIMARY]
GO

ALTER TABLE Core.Template WITH CHECK ADD CONSTRAINT [FK_Template_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Template CHECK CONSTRAINT [FK_Template_UserModifedBy]
GO
ALTER TABLE Core.Template WITH CHECK ADD CONSTRAINT [FK_Template_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.Template CHECK CONSTRAINT [FK_Template_UserCreatedBy]
GO



