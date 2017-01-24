-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[[OrganizationCounty]
-- Author:		John Crossen
-- Date:		01/21/2016
--
-- Purpose:		Map Org to a county
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/21/2016	John Crossen	TFS#5409 - Initial creation.
-- 02/26/2016	Kyle Campbell	Added Foreign Keys for ModifiedBy/CreatedBy columns




CREATE TABLE [Core].OrganizationCounty(
	[OrganizationCountyID] [INT] IDENTITY(1,1) NOT NULL,
	[CountyID] [INT] NOT NULL,
	[DetailID] [BIGINT] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
	[ModifiedBy] INT NOT NULL,
	[ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL,
	[CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_OrganizationCounty] PRIMARY KEY CLUSTERED 
(
	[OrganizationCountyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[OrganizationCounty]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationCounty_County] FOREIGN KEY([CountyID])
REFERENCES [Reference].[County] ([CountyID])
GO

ALTER TABLE [Core].[OrganizationCounty] CHECK CONSTRAINT [FK_OrganizationCounty_County]
GO

ALTER TABLE [Core].[OrganizationCounty]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationCounty_OrganizationDetails] FOREIGN KEY([DetailID])
REFERENCES [Core].[OrganizationDetails] ([DetailID])
GO

ALTER TABLE [Core].[OrganizationCounty] CHECK CONSTRAINT [FK_OrganizationCounty_OrganizationDetails]
GO

ALTER TABLE Core.OrganizationCounty WITH CHECK ADD CONSTRAINT [FK_OrganizationCounty_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.OrganizationCounty CHECK CONSTRAINT [FK_OrganizationCounty_UserModifedBy]
GO
ALTER TABLE Core.OrganizationCounty WITH CHECK ADD CONSTRAINT [FK_OrganizationCounty_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.OrganizationCounty CHECK CONSTRAINT [FK_OrganizationCounty_UserCreatedBy]
GO

