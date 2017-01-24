CREATE TABLE [Reference].[ServiceConfigServiceType]
(
	[ServiceConfigServiceTypeID] INT IDENTITY (1,1) NOT NULL,
	[ServiceConfigServiceType] NVARCHAR(50) NOT NULL,
	[IsActive] BIT NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[CreatedBy] [int] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemCreatedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
	[SystemModifiedOn] [datetime] NOT NULL DEFAULT (getutcdate()),
 CONSTRAINT [PK_ServiceConfigServiceTypeID] PRIMARY KEY CLUSTERED 
(
	[ServiceConfigServiceTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ServiceConfigServiceType]  WITH CHECK ADD  CONSTRAINT [FK_ServiceConfigServiceType_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[ServiceConfigServiceType] CHECK CONSTRAINT [FK_ServiceConfigServiceType_UserCreatedBy]
GO
ALTER TABLE [Reference].[ServiceConfigServiceType]  WITH CHECK ADD  CONSTRAINT [FK_ServiceConfigServiceType_UserModifiedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Reference].[ServiceConfigServiceType] CHECK CONSTRAINT [FK_ServiceConfigServiceType_UserModifiedBy]
GO

--------------------------Extended Properties--------------------------
EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'The type of service', 
@level0type = N'SCHEMA', @level0name = Reference, 
@level1type = N'TABLE',  @level1name = ServiceConfigServiceType,
@level2type = N'COLUMN', @level2name = ServiceConfigServiceType;
GO
