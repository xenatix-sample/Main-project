-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[ResetPassword]
-- Author:		Rajiv Ranjan
-- Date:		08/05/2015
--
-- Purpose:		Reset Password details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/05/2015	Rajiv Ranjan	 Initial creation.
-- 08/25/2015	Rajiv Ranjan	 Added reset link field & changed phoneId to nullable.
-- 08/26/2015	Rajiv Ranjan	 Changed restLink to resetIdentifier beacuse it is a identifier for reset process.
-- 08/27/2015	Rajiv Ranjan	 Changed requestorIPAddress's size to nvarchar(30) from 20
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[ResetPassword] 
(
    [ResetPasswordID]     BIGINT IDENTITY(1,1) NOT NULL,
    [UserID]       INT NOT NULL,
	[ResetIdentifier] UNIQUEIDENTIFIER NOT NULL,
    [PhoneID]         BIGINT NULL,
	[OTPCode]  NVARCHAR (20)  NULL,
	[RequestorIPAddress] NVARCHAR(30) NOT NULL,
	[ExpiresOn] [datetime] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	CONSTRAINT [FK_ResetPassword_UserID] FOREIGN KEY ([UserID]) REFERENCES [Core].[Users] ([UserID]) ON DELETE CASCADE,
    CONSTRAINT [PK_ResetPassword_ResetPasswordID] PRIMARY KEY CLUSTERED ([ResetPasswordID] ASC)
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
	
) ON [PRIMARY]