-----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[RoleStage]
-- Author:		Chad Roberts
-- Date:		
--
-- Purpose:		Staging table to hold the userroles from active directory 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/29/2016	Chad Roberts	Initial creation. 
-----------------------------------------------------------------------------------------------------------------------



CREATE TABLE [dbo].[UserRoleStage] (
    [UserRoleStageID]  INT              IDENTITY (1, 1) NOT NULL,
	[BatchID]		   BIGINT	NOT NULL,
    [UserGUID]         VARBINARY (5000) NOT NULL,
    [RoleGUID]         VARBINARY (5000) NOT NULL,
    [IsActive]         BIT              CONSTRAINT [DF_UserRoleStage_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedBy]       INT              NOT NULL,
    [ModifiedOn]       DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT              DEFAULT ((1)) NOT NULL,
    [CreatedOn]        DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SystemCreatedOn]  DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SystemModifiedOn] DATETIME         DEFAULT (getutcdate()) NOT NULL,
	[UserGUIDConverted] [nvarchar](500) NULL,
	[RoleGUIDConverted] [nvarchar](500) NULL,
    CONSTRAINT [PK_UserRoleStage_UserRoleStageID] PRIMARY KEY CLUSTERED ([UserRoleStageID] ASC)
);


