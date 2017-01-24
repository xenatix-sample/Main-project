
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[PayorExpirationReason]
-- Author:		Kyle Campbell
-- Date:		03/16/2016
--
-- Purpose:		Lookup for Payor Eligibility Expiration Reasons
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/16/2016	Kyle Campbell	TFS #7308	Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[PayorExpirationReason](
	[PayorExpirationReasonID] [int] IDENTITY (1,1) NOT NULL,
	[PayorExpirationReason] [nvarchar](50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
	[SortOrder] [int] NULL,
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_PayorExpirationReason_PayorExpirationReasonID] PRIMARY KEY CLUSTERED 
(
	[PayorExpirationReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[PayorExpirationReason] ADD CONSTRAINT IX_PayorExpirationReason UNIQUE(PayorExpirationReason)
GO

ALTER TABLE Reference.PayorExpirationReason WITH CHECK ADD CONSTRAINT [FK_PayorExpirationReason_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.PayorExpirationReason CHECK CONSTRAINT [FK_PayorExpirationReason_UserModifedBy]
GO
ALTER TABLE Reference.PayorExpirationReason WITH CHECK ADD CONSTRAINT [FK_PayorExpirationReason_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.PayorExpirationReason CHECK CONSTRAINT [FK_PayorExpirationReason_UserCreatedBy]
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Stores the Payor Eligibility Expiration Reasons', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = PayorExpirationReason,
@level2type = N'COLUMN', @level2name = PayorExpirationReason;
GO
