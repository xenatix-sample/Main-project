

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Synch.usp_ValidateCMHCClientRecordedServices
-- Author:		Sumana Sangapu
-- Date:		09/13/2016
--
-- Purpose:		Validate CMHC Client Recorded Services data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/13/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_ValidateCMHCClientRecordedServices]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Validate for ServiceRecordingId  NOT NULL
	UPDATE	crs
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + ' ServiceRecordingId cannot be NULL. '
	FROM	Synch.CMHCClientRecordedServices crs
	WHERE	ServiceRecordingId  IS NULL


	-- Validate for UserId NOT NULL
	UPDATE	crs
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'UserId cannot be NULL. '
	FROM	Synch.CMHCClientRecordedServices crs
	WHERE	UserId IS NULL


	-- Validate for ServiceStartDate NOT NULL
	UPDATE	crs
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'ServiceStartDate cannot be NULL. '
	FROM	Synch.CMHCClientRecordedServices crs
	WHERE	ServiceStartDate IS NULL

	
	-- Validate for ServiceStartTime NOT NULL
	UPDATE	crs
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'ServiceStartTime  cannot be NULL. '
	FROM	Synch.CMHCClientRecordedServices crs
	WHERE	ServiceStartTime  IS NULL

	
	-- Validate for MRN NOT NULL
	UPDATE	crs
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'MRN  cannot be NULL. '
	FROM	Synch.CMHCClientRecordedServices crs
	WHERE	MRN  IS NULL

		
	-- Validate for CMHCCode NOT NULL
	UPDATE	crs
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'CMHCCode cannot be NULL. '
	FROM	Synch.CMHCClientRecordedServices crs
	WHERE	CMHCCode IS NULL

	-- Validate for CMHCRU  NOT NULL
	UPDATE	crs
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'CMHCRU cannot be NULL. '
	FROM	Synch.CMHCClientRecordedServices crs
	WHERE	CMHCRU IS NULL
	

	-- Validate for Duration  NOT NULL
	UPDATE	crs
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'Duration cannot be NULL. '
	FROM	Synch.CMHCClientRecordedServices crs
	WHERE	Duration IS NULL
	
	-- Validate for ClientDuration  NOT NULL
	UPDATE	crs
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'ClientDuration cannot be NULL. '
	FROM	Synch.CMHCClientRecordedServices crs
	WHERE	ClientDuration IS NULL

	
	-- Validate for AttendanceCode  NOT NULL
	UPDATE	crs
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'AttendanceCode cannot be NULL. '
	FROM	Synch.CMHCClientRecordedServices crs
	WHERE	AttendanceCode IS NULL

	
	-- Validate for CMHCRecipientCode  NOT NULL
	UPDATE	crs
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'CMHCRecipientCode cannot be NULL. '
	FROM	Synch.CMHCClientRecordedServices crs
	WHERE	CMHCRecipientCode IS NULL

	


-- Validate for LocationCode  NOT NULL
	UPDATE	crs
	SET		ErrorMessage = ISNULL(ErrorMessage,'') + 'LocationCode  cannot be NULL. '
	FROM	Synch.CMHCClientRecordedServices crs
	WHERE	LocationCode  IS NULL	
	
	
	SELECT COUNT(*) FROM Synch.CMHCClientRecordedServices  WHERE ErrorMessage IS NOT NULL

END

