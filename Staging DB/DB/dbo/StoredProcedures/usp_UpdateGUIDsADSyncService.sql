
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	dbo.usp_UpdateGUIDsADSyncService
-- Author:		Sumana Sangapu
-- Date:		03/02/2016
--
-- Purpose:		Convert the GUIDs from varbinary to varchar Used in ADUserSync SSIS package.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
--03/01/2016	Sumana Sangapu	Initial creation.
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE dbo.usp_UpdateGUIDsADSyncService
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	UPDATE u
	SET 	UserGUIDConverted = Substring(( MASTER.dbo.Fn_varbintohexstr(UserGUID) ), 1, 2) + Substring(Upper(MASTER.dbo.Fn_varbintohexstr(UserGUID)), 3, 8000),
			ManagerGUIDConverted = Substring(( MASTER.dbo.Fn_varbintohexstr(ManagerGUID) ), 1, 2) + Substring(Upper(MASTER.dbo.Fn_varbintohexstr(ManagerGUID)), 3, 8000)
	FROM	dbo.UserStage  u

	UPDATE	r
	SET		RoleGUIDConverted = Substring(( MASTER.dbo.Fn_varbintohexstr(RoleGUID) ), 1, 2) + Substring(Upper(MASTER.dbo.Fn_varbintohexstr(RoleGUID)), 3, 8000)
	FROM	dbo.RoleStage  r

	UPDATE	ur
	SET 	UserGUIDConverted = Substring(( MASTER.dbo.Fn_varbintohexstr(UserGUID) ), 1, 2) + Substring(Upper(MASTER.dbo.Fn_varbintohexstr(UserGUID)), 3, 8000),
			RoleGUIDConverted = Substring(( MASTER.dbo.Fn_varbintohexstr(RoleGUID) ), 1, 2) + Substring(Upper(MASTER.dbo.Fn_varbintohexstr(RoleGUID)), 3, 8000)
	FROM	dbo.UserRoleStage  ur

END
GO
