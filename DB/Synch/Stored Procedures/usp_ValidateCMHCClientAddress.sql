
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Synch.usp_ValidateCMHCClientAddress
-- Author:		Sumana Sangapu
-- Date:		09/13/2016
--
-- Purpose:		Validate CMHC ClientAddress data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/13/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_ValidateCMHCClientAddress]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Validate for ContactID NOT NULL
	UPDATE	cca
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + ' ContactID cannot be NULL. '
	FROM	Synch.CMHCClientAddress cca
	WHERE	ContactID IS NULL


	-- Validate for MRN NOT NULL
	UPDATE	cca
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'MRN cannot be NULL. '
	FROM	Synch.CMHCClientAddress cca
	WHERE	MRN IS NULL

	-- Validate for ContactAddressID NOT NULL
	UPDATE	cca
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'ContactAddressID cannot be NULL. '
	FROM	Synch.CMHCClientAddress cca
	WHERE	ContactAddressID IS NULL

	SELECT COUNT(*) FROM Synch.CMHCClientAddress WHERE ErrorMessage IS NOT NULL

END
