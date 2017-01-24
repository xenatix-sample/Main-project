
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactDemographics]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Get Contact Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu	Modification.
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-- 08/04/2015   Sumana Sangapu	844 -  Retrieve IsPregnant column
-- 08/05/2015	Saurabh Sahu		1) Added SchoolDistrict 2) Modify DriversLicense to AlternateID  .
-- 08/14/2015   Sumana Sangapu	1227 - Refactor ContactMethods Schema
-- 08/19/2015	Sumana Sangapu		1514	- SuffixID and TitleID - corrected
-- 08/27/2015	Arun Choudhary		Added a new column for Driver License and removed [SmokingStatusID], [FullCodeDNR] and [SchoolDistrictID] 
-- 09/03/2015	Rajiv Ranjan	Removed marital Status & age
-- 09/08/2015	Rajiv Ranjan		Adedd DriverLicenseStateID & ClientIdentifierTypeID
-- 10/28/2015	Sumana Sangapu		Added GestationalAge
-- 11/25/2015   Satish Singh		Added MRN for IFSP report
-- 12/23/2015	Rajiv Ranjan		Added PreferredGenderID into result set
-- 01/08/2016   Satish Singh		Added ReportingUnit, ServiceCoordinator and ServiceCoordinatorPhone for eci header
-- 12/23/2015	Scott Martin		Removed AlternateID and ClientIndentifierTypeID
-- 01/18/2016   Lokesh Singhal      Added DispositionStatus
-- 01/19/2016   Lokesh Singhal      Added join to users phone as per saving structure
-- 01/20/2016   Lokesh Singhal      Removed join for compatability with offline mode
-- 03/08/2016	Kyle Campbell	TFS #7099	Renamed "Alias" to "PreferredName"
-- 10/06/2016   Vishal Joshi    Added IsDeceased and CauseOfDeath to result set
-- 12/30/2016	Scott Martin	Added fields for client merge
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactDemographics]
@ContactID BIGINT,
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

		SELECT 
			c.ContactID, c.ContactTypeID, c.ClientTypeID, c.FirstName, c.Middle, c.LastName, c.SuffixID, c.GenderID,c.PreferredGenderID, c.TitleID, c.DOB, c.DOBStatusID, c.SSN, 
			c.IsPregnant, c.SSNStatusID, c.DriverLicense,c.DriverLicenseStateID,c.ReferralSourceID, c.PreferredName,c.DeceasedDate, cm.ContactMethodID AS ContactMethodID,
			c.GestationalAge,c.MRN, c.IsDeceased, c.CauseOfDeath, CAST(CASE WHEN MRN.MRN IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsMerged, MRN.MRN AS MergedMRN
			
		FROM 
			Registration.Contact c
			LEFT OUTER JOIN Reference.ContactMethod cm ON cm.ContactMethodID = c.PreferredContactMethodID
			LEFT OUTER JOIN Registration.AdditionalDemographics rad ON rad.ContactID=c.ContactID
			LEFT OUTER JOIN Core.MergedContactsMapping MCM
				ON C.ContactID = MCM.ContactID
			LEFT OUTER JOIN Registration.ContactMRN MRN
				ON MCM.ChildID = MRN.ContactID
			
		WHERE 
			c.ContactID = @ContactID AND c.IsActive = 1

	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
