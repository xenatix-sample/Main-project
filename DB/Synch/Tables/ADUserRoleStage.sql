-----------------------------------------------------------------------------------------------------------------------
-- Table:		Synch.[UserRoleStage]
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
-- 06/09/2016	Sumana Sangapu  TFS#11411 - Refactor SSIS to move Staging tables to Synch schema
-- 09/03/2016	Rahul Vats		Review the table
-----------------------------------------------------------------------------------------------------------------------
CREATE TABLE [Synch].[ADUserRoleStage] (
    [UserRoleStageID]  INT              IDENTITY (1, 1) NOT NULL,
	[BatchID]		   BIGINT			NOT NULL,
	[UserGUID]		   nvarchar (500)	NULL,
	[RoleGUID]		   nvarchar (500)	NULL,
    [IsActive]         BIT              CONSTRAINT [DF_ADUserRoleStage_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedBy]       INT              NOT NULL,
    [ModifiedOn]       DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT              DEFAULT ((1)) NOT NULL,
    [CreatedOn]        DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SystemCreatedOn]  DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [SystemModifiedOn] DATETIME         DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_ADUserRoleStage_UserRoleStageID] PRIMARY KEY CLUSTERED ([UserRoleStageID] ASC)
)
GO