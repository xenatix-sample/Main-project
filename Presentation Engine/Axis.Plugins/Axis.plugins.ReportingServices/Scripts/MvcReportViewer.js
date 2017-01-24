(function () {
    // ASP.NET Report Viewer Web Control Enhancements

    $(document).ready(function () {
        // Detect is current browser is IE (MSIE is not used since IE 11)
        var isIE = /MSIE/i.test(navigator.userAgent) || /rv:11.0/i.test(navigator.userAgent);

        function viewerPropertyChanged(sender, e) {
            var viewer = $find("ReportViewer");

            if (e.get_propertyName() === "isLoading" && !viewer.get_isLoading()) {
                var reportViewerHeight = $('#ReportViewer').height();
                var $uiRows = $('#ReportViewer_fixedTable > tbody > tr');
                var controlsHeight = 0;
                $uiRows.each(function (i, el) {
                    if (i !== $uiRows.length - 1) {
                        controlsHeight += $(el).height();
                    }
                });

                var contentAreaHeight = reportViewerHeight - controlsHeight;
                $('#VisibleReportContentReportViewer_ctl09').height(contentAreaHeight);
            }
        }

        function printReport() {
            $find('ReportViewer').exportReport('PDF');
        }

        // 1. Fix Report Scrolling in IE 10 and IE 11
        if (window.hasUserSetHeight && isIE) {
            Sys.Application.add_load(function () {
                var reportViewer = $find("ReportViewer");
                reportViewer.add_propertyChanged(viewerPropertyChanged);
            });
        }

        // 2. Add Print button for non-IE browsers

        if (!isIE && window.showPrintButton) {
            //2016-06-13    Kishan. [Bug 11239, 11297] . add missing print button to reportviewer tool bar.
            Sys.Application.add_load(function () {
                var reportViewer = $find("ReportViewer");
                reportViewer.add_propertyChanged(addPrintButton);
            });
        }

        function addPrintButton(sender, e) {
            var viewer = $find("ReportViewer");

            if (e.get_propertyName() === "isLoading" && !viewer.get_isLoading()) {

                // see if print button is missing after view report is clicked.
                if (!$('#PrintButton').length) {

                    var buttonHtml = $('#non-ie-print-button').html();
                    $('#ReportViewer_ctl05 > div').append(buttonHtml);
                    $('#PrintButton').click(function(e) {
                        e.preventDefault();
                        printReport();
                    });

                    $('#mvcreportviewer-btn-print').hover(function () {
                        $(this).css('cursor', 'pointer').css('border', '1px solid rgb(51, 102, 153)').css('background-color', 'rgb(221, 238, 247)');
                    }, function () {
                        $(this).css('cursor', 'pointer').css('border', '1px solid transparent').css('background-color', 'transparent');
                    });
                }
            }
        }

    });
    
})();