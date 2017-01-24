CREATE TABLE [Synch].[CMHCAxisRecipientCodeMapping](
	[CMHCAxisRecipientCodeMappingID] [bigint] IDENTITY(1,1) NOT NULL,
	[RecipientCodeID] SMALLINT NULL,
	[CMCHRecipientCode] SMALLINT NULL,
	[CMHCDescription] [nvarchar](75) NULL,
 CONSTRAINT [PK_CMHCAxisRecipientCodeMappingID] PRIMARY KEY CLUSTERED 
(
	[CMHCAxisRecipientCodeMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Synch].[CMHCAxisRecipientCodeMapping] WITH CHECK ADD CONSTRAINT [FK_CMHCAxisRecipientCodeMapping_RecipientCodeID] FOREIGN KEY ([RecipientCodeID]) REFERENCES [Reference].[RecipientCode] ([CodeID])
GO
ALTER TABLE [Synch].[CMHCAxisRecipientCodeMapping] CHECK CONSTRAINT [FK_CMHCAxisRecipientCodeMapping_RecipientCodeID] 
GO