-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ServiceItemsLegacyMapping]
-- Author:		Sumana Sangapu
-- Date:		07/06/2016
--
-- Purpose:		Holds CMHC ServiceItem mappings
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/06/2016	Sumana Sangapu	Initial Creation .
-- 07/06/2016	Sumana Sangapu	Added LegacyCode for CMHC
-- 08/19/2016	Sumana Sangapu	Added ProjectCode and LOF 
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[ServiceItemsLegacyMapping](
	[ServiceItemLegacyMappingID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceID] [int] NULL,
	[ServiceStatusID] [smallint] NULL,
	[TrackingFieldID] [int] NULL,
	[LegacyCode] [nvarchar](10) NULL,
	[LegacyCodeDescription] [nvarchar](100) NULL,
	[ProjectCode] INT NULL,
	[LOF] int null,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[SystemCreatedOn] [datetime] NOT NULL,
	[SystemModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ServiceItemLegacyMappingID] PRIMARY KEY CLUSTERED 
(
	[ServiceItemLegacyMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping] ADD  DEFAULT (getutcdate()) FOR [ModifiedOn]
GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping] ADD  DEFAULT ((1)) FOR [ModifiedBy]
GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping] ADD  DEFAULT ((1)) FOR [CreatedBy]
GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping] ADD  DEFAULT (getutcdate()) FOR [SystemCreatedOn]
GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping] ADD  DEFAULT (getutcdate()) FOR [SystemModifiedOn]
GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItemsLegacyMapping_ServiceID] FOREIGN KEY([ServiceID])
REFERENCES [Reference].[Services] ([ServicesID])
GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping] CHECK CONSTRAINT [FK_ServiceItemsLegacyMapping_ServiceID]
GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItemsLegacyMapping_ServiceStatusID] FOREIGN KEY([ServiceStatusID])
REFERENCES [Reference].[ServiceStatus] ([ServiceStatusID])
GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping] CHECK CONSTRAINT [FK_ServiceItemsLegacyMapping_ServiceStatusID]
GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItemsLegacyMapping_TrackingFieldID] FOREIGN KEY([TrackingFieldID])
REFERENCES [Reference].[TrackingField] ([TrackingFieldID])
GO

ALTER TABLE [Reference].[ServiceItemsLegacyMapping] CHECK CONSTRAINT [FK_ServiceItemsLegacyMapping_TrackingFieldID]
GO

