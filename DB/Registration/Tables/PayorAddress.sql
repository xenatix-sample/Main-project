-----------------------------------------------------------------------------------------------------------------------
-- Table:	[PayorAddress]
-- Author:		John Crossen
-- Date:		09/17/2015
--
-- Purpose:		Mapping table for payor and address 
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/24/2015	John Crossen    TFS# 2366	Initial creation
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-- 04/11/2016   John Crossen    Modified FK relationship from PayorID to PayorPlanID, added contractID
-- 04/11/2016   Atul Chauhan    Added Effective Date and Expiration date
-- 12/23/2016	Vishal Yadav	Modified ElectronicPayorID & ContactID  size to 50.
---------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Registration].[PayorAddress](
	[PayorAddressID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[PayorPlanID] [INT] NOT NULL,
	[AddressID] [BIGINT] NOT NULL,
	[ContactID] [NVARCHAR] (50) NULL,
	[ElectronicPayorID] [NVARCHAR] (50) NULL,
	[EffectiveDate] [DATE]  NULL,
	[ExpirationDate] [DATE] NULL,
	[IsActive] [BIT] NOT NULL,
	[ModifiedBy] [INT] NOT NULL,
	[ModifiedOn] [DATETIME] NOT NULL,
	[CreatedBy] [INT] NOT NULL,
	[CreatedOn] [DATETIME] NOT NULL,
	[SystemCreatedOn] [DATETIME] NOT NULL,
	[SystemModifiedOn] [DATETIME] NOT NULL,
 CONSTRAINT [PK_PayorAddress] PRIMARY KEY CLUSTERED 
(
	[PayorAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IX_PayorAddress] ON [Registration].[PayorAddress]
(
	[PayorAddressID] ASC
)
INCLUDE ( 	[PayorPlanID],
	[ContactID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE [Registration].[PayorAddress] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [Registration].[PayorAddress] ADD  DEFAULT (GETUTCDATE()) FOR [ModifiedOn]
GO
ALTER TABLE [Registration].[PayorAddress] ADD  DEFAULT ((1)) FOR [CreatedBy]
GO
ALTER TABLE [Registration].[PayorAddress] ADD  DEFAULT (GETUTCDATE()) FOR [CreatedOn]
GO
ALTER TABLE [Registration].[PayorAddress] ADD  DEFAULT (GETUTCDATE()) FOR [SystemCreatedOn]
GO
ALTER TABLE [Registration].[PayorAddress] ADD  DEFAULT (GETUTCDATE()) FOR [SystemModifiedOn]
GO
ALTER TABLE [Registration].[PayorAddress]  WITH CHECK ADD  CONSTRAINT [FK_PayorAddress_Addresses] FOREIGN KEY([AddressID]) REFERENCES [Core].[Addresses] ([AddressID])
GO
ALTER TABLE [Registration].[PayorAddress] CHECK CONSTRAINT [FK_PayorAddress_Addresses]
GO
ALTER TABLE [Registration].[PayorAddress]  WITH CHECK ADD  CONSTRAINT [FK_PayorAddress_PayorPlan] FOREIGN KEY([PayorPlanID]) REFERENCES [Reference].[PayorPlan] ([PayorPlanID])
GO
ALTER TABLE [Registration].[PayorAddress] CHECK CONSTRAINT [FK_PayorAddress_PayorPlan]
GO
ALTER TABLE [Registration].[PayorAddress]  WITH CHECK ADD  CONSTRAINT [FK_PayorAddress_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Registration].[PayorAddress] CHECK CONSTRAINT [FK_PayorAddress_UserCreatedBy]
GO
ALTER TABLE [Registration].[PayorAddress]  WITH CHECK ADD  CONSTRAINT [FK_PayorAddress_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Registration].[PayorAddress] CHECK CONSTRAINT [FK_PayorAddress_UserModifedBy]
GO


