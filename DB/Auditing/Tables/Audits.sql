-----------------------------------------------------------------------------------------------------------------------
-- Table:	    Core.Audits
-- Author:		John Crossen
-- Date:		08/03/2015
--
-- Purpose:		Audit functionality
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/03/2015	John Crossen		TFS# 866 - Initial creation.
-- 08/10/2015   John Crossen                - Add default to Timestamp
-- 01/26/2016	Scott Martin		Removed SQLColumnID and made ModuleID nullable
-- 09/06/2016	Rahul Vats		Reviewed the Table
-- 09/15/2016	Scott Martin	Removed ModuleID and added TransactionID and ModuleComponentID
-- 09/16/2016	Scott Martin	Renamed TransactionID to TransactionLogID and added a foreign key reference: Moved to Auditing Schema	
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Auditing].[Audits](
	[AuditID] [bigint] IDENTITY(1,1) NOT NULL,
	[AuditTypeID] [int] NOT NULL,
	[TransactionLogID] [bigint] NULL,
	[ModuleComponentID] BIGINT NULL,
	[UserID] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[AuditTimeStamp] [datetime] NOT NULL DEFAULT (GETUTCDATE()),
	[IsArchivable] [bit] NOT NULL,
 CONSTRAINT [PK_Audits] PRIMARY KEY CLUSTERED 
(
	[AuditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Auditing.Audits WITH CHECK ADD CONSTRAINT [FK_Audits_ModuleComponentID] FOREIGN KEY ([ModuleComponentID]) REFERENCES [Core].[ModuleComponent] ([ModuleComponentID])
GO
ALTER TABLE Auditing.Audits CHECK CONSTRAINT [FK_Audits_ModuleComponentID]
GO
ALTER TABLE Auditing.Audits WITH CHECK ADD CONSTRAINT [FK_Audits_TransactionLogID] FOREIGN KEY ([TransactionLogID]) REFERENCES [Core].[TransactionLog] ([TransactionLogID])
GO
ALTER TABLE Auditing.Audits CHECK CONSTRAINT [FK_Audits_TransactionLogID]
GO