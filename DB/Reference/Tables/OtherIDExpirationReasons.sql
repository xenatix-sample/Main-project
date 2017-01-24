-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[OtherIDExpirationReasons]
-- Author:		Deepak Kumar
-- Date:		07/27/2016
--
-- Purpose:		Lookup for OtherIDExpirationReason  details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/27/2016	Deepak Kumar	TFS# 11337 - Initial creation.
-- 08/17/2016	Scott Martin	Added extended properties to describe the use of the table. Needed for option set management
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[OtherIDExpirationReasons](
	[ExpirationReasonID] [int] IDENTITY(1,1) NOT NULL,
	[ExpirationReason] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_OtherIDExpirationReasons_ExpirationReasonID] PRIMARY KEY CLUSTERED 
(
	[ExpirationReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_OtherIDExpirationReasons] UNIQUE NONCLUSTERED 
(
	[ExpirationReason] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[OtherIDExpirationReasons]  WITH CHECK ADD  CONSTRAINT [FK_OtherIDExpirationReasons_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[OtherIDExpirationReasons] CHECK CONSTRAINT [FK_OtherIDExpirationReasons_UserCreatedBy]
GO
ALTER TABLE [Reference].[OtherIDExpirationReasons]  WITH CHECK ADD  CONSTRAINT [FK_OtherIDExpirationReasons_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[OtherIDExpirationReasons] CHECK CONSTRAINT [FK_OtherIDExpirationReasons_UserModifedBy]
GO

--------------------------Extended Properties-----------------------------
EXEC sys.sp_addextendedproperty 
@name = N'Caption', 
@value = N'Other ID Expiration Reasons', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = OtherIDExpirationReasons;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Values indicating why other ID expired', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = OtherIDExpirationReasons;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsOptionSet',
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = OtherIDExpirationReasons;
GO;

EXEC sys.sp_addextendedproperty 
@name = N'IsUserOptionSet', 
@value = N'1', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = OtherIDExpirationReasons;
GO;