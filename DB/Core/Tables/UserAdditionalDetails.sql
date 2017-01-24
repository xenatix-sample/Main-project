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
-- 09/06/2016	Rahul Vats		Reviewed the Table 
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Core].[UserAdditionalDetails](
	[UserAdditionalDetailID] [bigint] IDENTITY (1,1) NOT NULL,
	[UserID] INT NOT NULL,
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
 CONSTRAINT [PK_UserAdditionalDetails_UserAdditionalDetailID] PRIMARY KEY CLUSTERED 
(
	[UserAdditionalDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Core].[UserAdditionalDetails] WITH CHECK ADD CONSTRAINT [FK_UserAdditionalDetails_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[UserAdditionalDetails] CHECK CONSTRAINT [FK_UserAdditionalDetails_UserModifedBy]
GO
ALTER TABLE [Core].[UserAdditionalDetails] WITH CHECK ADD CONSTRAINT [FK_UserAdditionalDetails_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[UserAdditionalDetails] CHECK CONSTRAINT [FK_UserAdditionalDetails_UserCreatedBy]
GO