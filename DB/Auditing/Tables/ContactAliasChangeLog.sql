-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Auditing].[ContactAliasChangeLog]
-- Author:		Kyle Campbell
-- Date:		09/16/2016
--
-- Purpose:		Holds the change log details for contact aliases
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/16/2016	Kyle Campbell	TFS #14753	Initial creation
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Auditing].[ContactAliasChangeLog]
(
	[ContactAliasChangeLogID] bigint identity(1,1) NOT NULL,
	[TransactionLogID] bigint NOT NULL,
	[UserID] int NOT NULL,
	[ChangedDate] datetime NOT NULL,
	[UserFirstName] nvarchar(50),
	[UserLastName] nvarchar(50),
	[ContactAliasID] bigint NOT NULL,	
	[ContactID] bigint NOT NULL,
	[AliasFirstName] NVARCHAR(200),
	[AliasMiddle] NVARCHAR(200),
	[AliasLastName] NVARCHAR(200),
	[Suffix] [nvarchar](50),
	[IsActive] bit,
	CONSTRAINT [PK_ContactAliasChangeLog_ContactAliasChangeLogID] PRIMARY KEY CLUSTERED ([ContactAliasChangeLogID] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_ContactAliasChangeLog_TransactionLogID] ON [Auditing].[ContactAliasChangeLog]
(
	[TransactionLogID] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_ContactAliasChangeLog_ContactID] ON [Auditing].[ContactAliasChangeLog]
(
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE [Auditing].[ContactAliasChangeLog]  WITH CHECK ADD  CONSTRAINT [FK_ContactAliasChangeLog_TransasctionLogId] FOREIGN KEY([TransactionLogId]) REFERENCES [Core].[TransactionLog] ([TransactionLogId])
GO
ALTER TABLE [Auditing].[ContactAliasChangeLog] CHECK CONSTRAINT [FK_ContactAliasChangeLog_TransasctionLogId]
GO