  -----------------------------------------------------------------------------------------------------------------------
-- Script:		add_FullTextIndex_CrisisCall_17833  
-- Author:		Scott Martin
-- Date:		11/22/2016
-- TFS#:		17833
-- Release:		1.1.4

-- Purpose:		Added full text index to increase performance in search

-- Notes:		n/a (or any additional notes)

-- Depends:		n/a (or any dependencies such as other procs or functions)
-----------------------------------------------------------------------------------------------------------------------

IF EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name = N'SearchableFields'
      AND Object_ID = Object_ID(N'CallCenter.CrisisCall'))
	BEGIN
	DROP FULLTEXT INDEX ON CallCenter.CrisisCall;

	DROP FULLTEXT STOPLIST [CrisisCallEmptyStopList]; 

	ALTER TABLE CallCenter.CrisisCall DROP COLUMN [SearchableFields];
	END

ALTER TABLE CallCenter.CrisisCall ADD [SearchableFields] AS (((((coalesce(isnull(CAST([CallCenterHeaderID] AS NVARCHAR(20)),'') + ':', '') + coalesce(isnull(ReasonCalled,'')+':','')) + coalesce(isnull(Disposition,'')+':','')) + coalesce(isnull(OtherInformation,'')+':','')) + coalesce(isnull(Comments,'')+':',''))) PERSISTED;

CREATE FULLTEXT STOPLIST [CrisisCallEmptyStopList];

CREATE FULLTEXT INDEX ON CallCenter.CrisisCall
	(SearchableFields) KEY INDEX [PK_CrisisCall] WITH STOPLIST = [CrisisCallEmptyStopList], CHANGE_TRACKING = AUTO;

