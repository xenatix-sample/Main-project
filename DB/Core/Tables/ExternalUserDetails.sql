
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[UserAdditionalDetails]
-- Author:		Sumana Sangapu
-- Date:		04/06/2016
--
-- Purpose:		Details to store the additional details of User. For ex: Contracting details.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/06/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[UserAdditionalDetails](
	[ExternalUserDetailID] [bigint] IDENTITY (1,1) NOT NULL,
	[UserID] BIGINT NOT NULL,
	[ContractingEntity] NVARCHAR(100) NULL,
	[IDNumber] NVARCHAR(100) NULL,
	[EffectiveDate] DATETIME NULL,
	[ExpirationDate] DATETIME NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_UserIdentifierType_UserIdentifierTypeID] PRIMARY KEY CLUSTERED 
(
	[UserIdentifierTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Core].[ExternalUserDetails] WITH CHECK ADD CONSTRAINT [FK_ExternalUserDetails_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[ExternalUserDetails] CHECK CONSTRAINT [FK_ExternalUserDetails_UserModifedBy]
GO
ALTER TABLE [Core].[ExternalUserDetails] WITH CHECK ADD CONSTRAINT [FK_ExternalUserDetails_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[ExternalUserDetails] CHECK CONSTRAINT [FK_ExternalUserDetails_UserCreatedBy]
GO