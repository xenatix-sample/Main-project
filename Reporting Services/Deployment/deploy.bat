set varServerPath=http://localhost/reportserver
set varReportFolder=Axis Reports
Set varDatasetFolder=Datasets
set varDataSourceFolder=Data Sources
Set varDataSourcePath=Data Sources
set varReportName=
set varReportFilePath=.\Reports



rs.exe -i Commonscript.rss -s %varServerPath% -v ReportFolder="%varReportFolder%" -v DataSetFolder="%varDatasetFolder%" -v DataSourceFolder="%varDataSourceFolder%" -v DataSourcePath="%varDataSourcePath%" -v ReportName="%varReportName%"  -v filePath="%varReportFilePath%" -e Mgmt2010