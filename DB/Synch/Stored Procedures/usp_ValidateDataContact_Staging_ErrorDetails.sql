

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_ValidateDataContact_Staging_ErrorDetails]
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
-- 05/19/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Synch].[usp_ValidateDataContact_Staging_ErrorDetails]
AS 
BEGIN

			/*********************************************** Synch.Contact_Staging  ***********************************/
			--ContactTypeID
			INSERT INTO Synch.Contact_Staging_ErrorDetails
			SELECT *,'ContactTypeID' FROM  Synch.Contact_Staging cs
			WHERE cs.ContactTypeID NOT IN ( SELECT  ct.contacttype FROM  Reference.ContactType ct ) 
			AND cs.ContactTypeID <> '' 

			--ClientTypeID
			INSERT INTO Synch.Contact_Staging_ErrorDetails
			SELECT *,'ClientTypeID' FROM  Synch.Contact_Staging cs
			WHERE cs.ClientTypeID NOT IN ( SELECT  ct.ClientType FROM  Reference.ClientType ct ) 
			AND cs.ClientTypeID <> '' 

			--SuffixID
			INSERT INTO Synch.Contact_Staging_ErrorDetails
			SELECT *,'SuffixID' FROM  Synch.Contact_Staging cs
			WHERE cs.SuffixID NOT IN ( SELECT  s.Suffix FROM  Reference.Suffix s ) 
			AND cs.SuffixID <> '' 

			--GenderID
			INSERT INTO Synch.Contact_Staging_ErrorDetails
			SELECT *,'GenderID' FROM  Synch.Contact_Staging cs
			WHERE cs.GenderID NOT IN ( SELECT  Gender FROM  Reference.Gender  ) 
			AND cs.GenderID <> '' 
 
			--TitleID
			INSERT INTO Synch.Contact_Staging_ErrorDetails
			SELECT *,'TitleID' FROM  Synch.Contact_Staging cs
			WHERE cs.TitleID NOT IN ( SELECT  Title FROM  Reference.Title  ) 
			AND cs.TitleID <> '' 

			--DOBStatusID
			INSERT INTO Synch.Contact_Staging_ErrorDetails
			SELECT *,'DOBStatusID' FROM  Synch.Contact_Staging cs
			WHERE cs.DOBStatusID NOT IN ( SELECT  DOBStatus FROM  Reference.DOBStatus  ) 
			AND cs.DOBStatusID <> '' 

			--SSNStatusID
			INSERT INTO Synch.Contact_Staging_ErrorDetails
			SELECT *,'SSNStatusID' FROM  Synch.Contact_Staging cs
			WHERE cs.SSNStatusID NOT IN ( SELECT  [SSNStatus] FROM  Reference.SSNStatus  ) 
			AND cs.SSNStatusID <> '' 

			--PreferredContactMethodID
			INSERT INTO Synch.Contact_Staging_ErrorDetails
			SELECT *,'PreferredContactMethodID' FROM  Synch.Contact_Staging cs
			WHERE cs.PreferredContactMethodID NOT IN ( SELECT  ContactMethod FROM  Reference.ContactMethod  ) 
			AND cs.PreferredContactMethodID <> '' 


			--ReferralSourceID
			INSERT INTO Synch.Contact_Staging_ErrorDetails
			SELECT *,'ReferralSourceID' FROM  Synch.Contact_Staging cs
			WHERE cs.ReferralSourceID NOT IN ( SELECT  ReferralSource FROM  Reference.ReferralSource  ) 
			AND cs.ReferralSourceID <> '' 





END
 