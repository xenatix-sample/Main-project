-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[OrganizationDetails]
-- Author:		Sumana Sangapu
-- Date:		01/15/2016
--
-- Purpose:		Lookup Details/Data for an Organization
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2016	Sumana Sangapu		Initial Creation.
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns
-- 12/12/2016	Kyle Campbell	TFS #17998	Add Acronym, Code, EffectiveDate, ExpirationDate columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[OrganizationDetails](
	[DetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Acronym] [nvarchar](20) NULL,
	[Code] [nvarchar](20) NULL,
	[EffectiveDate] DATE NULL,
	[ExpirationDate] DATE NULL,
	[IsExternal] [bit] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_OrganizationDetails] PRIMARY KEY CLUSTERED 
(
	[DetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

ALTER TABLE Core.OrganizationDetails WITH CHECK ADD CONSTRAINT [FK_OrganizationDetails_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.OrganizationDetails CHECK CONSTRAINT [FK_OrganizationDetails_UserModifedBy]
GO
ALTER TABLE Core.OrganizationDetails WITH CHECK ADD CONSTRAINT [FK_OrganizationDetails_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.OrganizationDetails CHECK CONSTRAINT [FK_OrganizationDetails_UserCreatedBy]
GO


