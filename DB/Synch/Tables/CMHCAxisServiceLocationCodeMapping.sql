CREATE TABLE [Synch].[CMHCAxisServiceLocationCodeMapping](
	[CMHCAxisServiceLocationCodeMappingID] [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceLocationID] SMALLINT NULL,
	[CMCHLocationCode] [nvarchar](10) NULL,
	[CMHCDescription] [nvarchar](75) NULL,
 CONSTRAINT [PK_CMHCAxisServiceLocationCodeMappingID] PRIMARY KEY CLUSTERED 
(
	[CMHCAxisServiceLocationCodeMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Synch].[CMHCAxisServiceLocationCodeMapping] WITH CHECK ADD CONSTRAINT [FK_CMHCAxisServiceLocationCodeMapping_ServiceLocationID] FOREIGN KEY ([ServiceLocationID]) REFERENCES [Reference].[ServiceLocation] ([ServiceLocationID])
GO
ALTER TABLE [Synch].[CMHCAxisServiceLocationCodeMapping] CHECK CONSTRAINT [FK_CMHCAxisServiceLocationCodeMapping_ServiceLocationID] 
GO