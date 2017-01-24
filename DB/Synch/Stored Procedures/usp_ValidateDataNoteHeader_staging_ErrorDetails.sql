
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataNoteHeader_staging_ErrorDetails]
-- Author:		Sumana Sangapu
-- Date:		05/19/2016
--
-- Purpose:		Validate lookup data in the staging files used for Data Conversion
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/24/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_ValidateDataNoteHeader_staging_ErrorDetails]
AS 
BEGIN

		/******************************************** [Synch].[NoteHeader_Staging_ErrorDetails] ***********************************/
		-- NoteTypeID 
		INSERT INTO  [Synch].[NoteHeader_Staging_Error_Details]
		SELECT *,'NoteTypeID ' FROM [Synch].[NoteHeader_Staging] c
		WHERE c.NoteTypeID  NOT IN ( SELECT NoteType FROM  Reference.NoteType  ) 
		AND c.NoteTypeID  <> ''

END