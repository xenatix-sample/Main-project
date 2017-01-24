
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_GetDrug]
-- Author:		Justin Spalti
-- Date:		11/25/2015
--
-- Purpose:		Get list of Drugs
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/25/2015	Justin Spalti - Initial creation
-- 11/30/2015 - Justin Spalti - Added the IsActive flag to the where clause and formatted the ProductName
-- 11/30/2015 - Justin Spalti - Grouped the results so that duplicate drugs are not displayed
-- 01/18/2016 -	Sumana Sangapu - List the Ingredients due to change of requirement
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Clinical.[usp_GetDrug]

@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'
/*
		SELECT MAX([DrugID]) AS DrugID,
			   CASE WHEN ISNULL([ProductName], '') = ISNULL([GenericMedicine], '') THEN ISNULL([ProductName], '') + ' ' + ISNULL([ProductForm], '')
				   ELSE ISNULL([ProductName], '') + ' ' + ISNULL([GenericMedicine], '') + ' ' + ISNULL([ProductForm], '')
			   END AS ProductName
		      ,[IsActive]
			  ,[ModifiedBy]
			  ,[ModifiedOn]
		FROM Clinical.[Drugs]
		WHERE [IsActive] = 1
		GROUP BY ProductName, GenericMedicine, ProductForm, IsActive, ModifiedBy, ModifiedOn
		ORDER BY ProductName
	*/
		SELECT IngredientID as  DrugID
			  ,IngredientName as  ProductName
		      ,[IsActive]
			  ,[ModifiedBy]
			  ,[ModifiedOn]
		FROM Clinical.[DrugIngredientsList]
		WHERE [IsActive] = 1
		ORDER BY ProductName


 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END