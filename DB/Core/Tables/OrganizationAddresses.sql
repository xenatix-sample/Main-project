-----------------------------------------------------------------------------------------------------------------------
-- Table:		Core.OrganizationAddress
-- Author:		Kyle Campbell
-- Date:		12/12/2016	
--
-- Purpose:		Store Organization Address data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/12/2016	Kyle Campbell	TFS #17998	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[OrganizationAddress]
(
	[OrganizationAddressID] BIGINT NOT NULL IDENTITY(1,1),
	[AddressID] BIGINT NOT NULL,
	[DetailID] BIGINT NOT NULL,
	[IsPrimary] BIT NULL,
	[EffectiveDate] DATE NULL,
	[ExpirationDate] DATE NULL,
	[IsActive] BIT DEFAULT (1),
	[ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    CONSTRAINT [PK_OrganizationAddress] PRIMARY KEY CLUSTERED 
(
	[OrganizationAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

ALTER TABLE [Core].[OrganizationAddress] WITH CHECK ADD CONSTRAINT [FK_OrganizationAddress_AddressID] FOREIGN KEY (AddressID) REFERENCES Core.Addresses (AddressID)
GO
ALTER TABLE [Core].[OrganizationAddress] CHECK CONSTRAINT [FK_OrganizationAddress_AddressID]
GO

ALTER TABLE [Core].[OrganizationAddress] WITH CHECK ADD CONSTRAINT [FK_OrganizationAddress_DetailID] FOREIGN KEY (DetailID) REFERENCES Core.OrganizationDetails (DetailID)
GO
ALTER TABLE [Core].[OrganizationAddress] CHECK CONSTRAINT [FK_OrganizationAddress_DetailID]
GO

ALTER TABLE Core.[OrganizationAddress] WITH CHECK ADD CONSTRAINT [FK_OrganizationAddresses_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.[OrganizationAddress] CHECK CONSTRAINT [FK_OrganizationAddresses_ModifiedBy]
GO

ALTER TABLE Core.[OrganizationAddress] WITH CHECK ADD CONSTRAINT [FK_OrganizationAddresses_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.[OrganizationAddress] CHECK CONSTRAINT [FK_OrganizationAddresses_CreatedBy]
GO