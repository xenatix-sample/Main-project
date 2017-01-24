-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Auditing].[ContactClientIdentifierChangeLog]
-- Author:		Kyle Campbell
-- Date:		09/16/2016
--
-- Purpose:		Holds the change log details for contact ClientIdentifier
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/16/2016	Kyle Campbell	TFS #14753	Initial creation
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Auditing].[ContactClientIdentifierChangeLog]
(
	[ContactClientIdentifierChangeLogID] bigint identity(1,1) NOT NULL,
	[TransactionLogID] bigint NOT NULL,
	[UserID] int NOT NULL,
	[ChangedDate] datetime NOT NULL,
	[UserFirstName] nvarchar(50),
	[UserLastName] nvarchar(50),
	[ContactClientIdentifierID] bigint,	
	[ContactID] bigint NOT NULL,
	[ClientIdentifierType] nvarchar(50),
	[AlternateID] nvarchar(50),
	[ExpirationReason] nvarchar(50),
	[EffectiveDate] date,
	[ExpirationDate] date,
	[IsActive] bit,
	CONSTRAINT [PK_ContactClientIdentifierChangeLog_ContactClientIdentifierChangeLogID] PRIMARY KEY CLUSTERED ([ContactClientIdentifierChangeLogID] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_ContactClientIdentifierChangeLog_TransactionLogID] ON [Auditing].[ContactClientIdentifierChangeLog]
(
	[TransactionLogID] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [IX_ContactClientIdentifierChangeLog_ContactID] ON [Auditing].[ContactClientIdentifierChangeLog]
(
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE [Auditing].[ContactClientIdentifierChangeLog]  WITH CHECK ADD  CONSTRAINT [FK_ContactClientIdentifierChangeLog_TransasctionLogId] FOREIGN KEY([TransactionLogId]) REFERENCES [Core].[TransactionLog] ([TransactionLogId])
GO
ALTER TABLE [Auditing].[ContactClientIdentifierChangeLog] CHECK CONSTRAINT [FK_ContactClientIdentifierChangeLog_TransasctionLogId]
GO