

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Synch.usp_ValidateCMHCSelfPay
-- Author:		Sumana Sangapu
-- Date:		09/13/2016
--
-- Purpose:		Validate CMHC SelfPay data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/13/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_ValidateCMHCSelfPay]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Validate for SelfPayID NOT NULL
	UPDATE	csp
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + ' SelfPayID cannot be NULL. '
	FROM	Synch.CMHCSelfPay csp
	WHERE	SelfPayID IS NULL


	-- Validate for ContactID NOT NULL
	UPDATE	csp
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + ' ContactID cannot be NULL. '
	FROM	Synch.CMHCSelfPay csp
	WHERE	ContactID IS NULL


	-- Validate for MRN NOT NULL
	UPDATE csp
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + ' MRN cannot be NULL. '
	FROM	Synch.CMHCSelfPay csp
	WHERE	MRN IS NULL

	-- Validate for EffectiveDate NOT NULL
	UPDATE	csp
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + ' EffectiveDate cannot be NULL. '
	FROM	Synch.CMHCSelfPay csp
	WHERE	EffectiveDate IS NULL

	
	-- Validate for SelfPayAmount NOT NULL
	UPDATE	csp
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'SelfPayAmount cannot be NULL. '
	FROM	Synch.CMHCSelfPay csp
	WHERE	SelfPayAmount IS NULL

		
	-- Validate for IsPercent NOT NULL
	UPDATE	csp
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'IsPercent cannot be NULL. '
	FROM	Synch.CMHCSelfPay csp
	WHERE	IsPercent IS NULL

	-- Validate for Division NOT NULL
	UPDATE	csp
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'Division cannot be NULL. '
	FROM	Synch.CMHCSelfPay csp
	WHERE	Division IS NULL


	SELECT COUNT(*) FROM Synch.CMHCSelfPay WHERE ErrorMessage IS NOT NULL

END

