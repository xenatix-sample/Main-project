-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[MessageTemplate]
-- Author:		Rajiv Ranjan
-- Date:		08/05/2015
--
-- Purpose:		Message Template details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/05/2015	Rajiv Ranjan	 Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[MessageTemplate] 
(
    [MessageTemplateID]     BIGINT IDENTITY(1,1) NOT NULL,
	[TemplateID] BIGINT NOT NULL,
	[IsHtmlBody] BIT NOT NULL,
	[EmailSubject]  NVARCHAR (500)  NULL,
	[MessageBody]  NVARCHAR (2500)  NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [FK_MessageTemplate_TemplateID] FOREIGN KEY ([TemplateID]) REFERENCES [Core].[Template] ([TemplateID]) ON DELETE CASCADE,
    CONSTRAINT [PK_MessageTemplate_MessageTemplateID] PRIMARY KEY CLUSTERED ([MessageTemplateID] ASC)
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
	
) ON [PRIMARY]


