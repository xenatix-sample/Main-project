-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[AccessToken]
-- Author:		Rajiv Ranjan
-- Date:		07/29/2015
--
-- Purpose:		Store the token generated for user
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/29/2015	Rajiv Ranjan	 Initial creation.
-- 07/30/2015	Suresh Pandey	TFS# - Add IsActive column, modified by not null, changed [Token] from max to 1000
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn							
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[AccessToken]
(
	[AccessTokenID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Token] [nvarchar](1000) NULL,
	[ClientIP] [nvarchar](200) NULL,
	[GeneratedOn] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_AccessToken_AccessTokenID] PRIMARY KEY CLUSTERED 
(
	[AccessTokenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO

CREATE NONCLUSTERED INDEX [IX_AccessToken_UserID_IsActive] ON [Core].[AccessToken]
(
	[UserId] ASC,
	[IsActive] ASC
)
INCLUDE ( 	[Token],
	[ClientIP]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO