
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Synch.usp_ValidateCMHCClientRegistration
-- Author:		Sumana Sangapu
-- Date:		09/14/2016
--
-- Purpose:		Validate CMHC ClientRegistration data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/14/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_ValidateCMHCClientRegistration]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Validate for ContactID NOT NULL
	UPDATE	ccr
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + ' ContactID cannot be NULL. '
	FROM	Synch.CMHCClientRegistration ccr
	WHERE	ContactID IS NULL


	-- Validate for MRN NOT NULL
	UPDATE	ccr
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'MRN cannot be NULL. '
	FROM	Synch.CMHCClientRegistration ccr
	WHERE	MRN IS NULL

	-- Validate for FirstName NOT NULL
	UPDATE	ccr
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'FirstName cannot be NULL. '
	FROM	Synch.CMHCClientRegistration ccr
	WHERE	FirstName IS NULL

	-- Validate for LastName NOT NULL
	UPDATE	ccr
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'LastName cannot be NULL. '
	FROM	Synch.CMHCClientRegistration ccr
	WHERE	LastName IS NULL

	-- Validate for EffectiveDate NOT NULL
	UPDATE	ccr
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'EffectiveDate cannot be NULL. '
	FROM	Synch.CMHCClientRegistration ccr
	WHERE	EffectiveDate IS NULL

	-- Validate for EffectiveDateTime NOT NULL
	UPDATE	ccr
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'EffectiveDateTime cannot be NULL. '
	FROM	Synch.CMHCClientRegistration ccr
	WHERE	EffectiveDateTime IS NULL
	
	SELECT COUNT(*) FROM Synch.CMHCClientRegistration WHERE ErrorMessage IS NOT NULL

END
