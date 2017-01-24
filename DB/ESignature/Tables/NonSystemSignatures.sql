-----------------------------------------------------------------------------------------------------------------------
-- Table:		[ESignature].[NonSystemSignatures]
-- Author:		John Crossen
-- Date:		03/15/2016
--
-- Purpose:		Holds Signatures for non system users.  Example being a cab driver that drops off a patient
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--				
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/15/2016	John Crossen	TFS: 7854  - Initial creation.

-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [ESignature].[NonSystemSignatures](
	[NonSystemSignatureID] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] NVARCHAR(200) NULL,
	[MiddleName] NVARCHAR(200) NULL,
	[LastName] NVARCHAR(200) NULL,
	[Description] NVARCHAR(400) NULL,
	[SignatureID] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_NonSystemSignatures_IsActive]  DEFAULT ((1)),
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_NonSystemSignature] PRIMARY KEY CLUSTERED 
(
	[NonSystemSignatureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [ESignature].[NonSystemSignatures]  WITH CHECK ADD  CONSTRAINT [FK_NonSystemSignatures_SignatureID] FOREIGN KEY([SignatureID])
REFERENCES [ESignature].[Signatures] ([SignatureID])
GO

ALTER TABLE [ESignature].[NonSystemSignatures]  CHECK CONSTRAINT [FK_NonSystemSignatures_SignatureID]
GO

ALTER TABLE [ESignature].[NonSystemSignatures]  WITH CHECK ADD CONSTRAINT [FK_NonSystemSignatures_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [ESignature].[NonSystemSignatures]  CHECK CONSTRAINT [FK_NonSystemSignatures_UserModifedBy]
GO
ALTER TABLE [ESignature].[NonSystemSignatures]  WITH CHECK ADD CONSTRAINT [FK_NonSystemSignatures_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [ESignature].[NonSystemSignatures]  CHECK CONSTRAINT [FK_NonSystemSignatures_UserCreatedBy]
GO
