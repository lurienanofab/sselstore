(function () {
    // called when the document completly loaded
    var INTERNAL = 1;
    var EXTERNAL = 2;
    var MIXED = 3;
    var internalExternalOrMixed = -1;
    var UNIVERSITY_OF_MICHIGAN = "University of Michigan";
    var STAFF = "LNF Staff";
    var HEADING3 = "HEADING3";
    var globalLabelXml;
    var globalLabelMiniXml;

    var jsObjLabelMain = new Object();
    var jsObjLabelMini = new Object();

    function onload() {
        var printersSelect = document.getElementById('printersSelect');
        var printMainButton = document.getElementById('btnMainPrint');
        var printMiniButton = document.getElementById('btnMiniPrint');

        if ($(".lblStaff").text() == STAFF) {
            $(".chkLNFStaff").attr("checked", true);
        }

        // Generates label preview and updates corresponend <img> element
        // Note: this does not work in IE 6 & 7 because they don't support data urls
        // if you want previews in IE 6 & 7 you have to do it on the server side
        function displayRows(mgr, org, intern) {
            //$(".trmngprof").css("display", "none");//prof);
            /*$(".trmanagers").css("display", mgr);
            $(".trorg").css("display", org);
            $(".trintern").css("display", intern);*/
            //console.log("mgr:"+mgr +" ,    profc: "+ prof +" ,  org:"+  org + " , intern: " +  intern);
        }

        function updatePreview(callfrom) {
            //alert('update preview:-->callfrom:' + callfrom + '   label:  ' + label + '   labelmin:  '+labelmini);
            if (!globalLabelXml || !globalLabelMiniXml)
                return;

            var label = dymo.label.framework.openLabelXml(globalLabelXml);
            var labelmini = dymo.label.framework.openLabelXml(globalLabelMiniXml);
            // reset
            displayRows("", "", "");

            var userType = $(".lblExternalInternal").text();
            if ("Internal" == userType) {
                internalExternalOrMixed = INTERNAL;
            } else if ("External" == userType) {
                internalExternalOrMixed = EXTERNAL;
            } else if ("Mixed" == userType) {
                internalExternalOrMixed = MIXED;
            }

            if ($(".ddlOrgs option:selected").text() == UNIVERSITY_OF_MICHIGAN) {
                internalExternalOrMixed = INTERNAL;
            } else {
                internalExternalOrMixed = EXTERNAL;
            }

            // update label with latest data
            //label.setObjectText('FirstName', $(".lblUserFirstName").text());
            //label.setObjectText('LastName', $(".lblUserLastName").text());

            storeMainLabel(label, 'FirstName', $(".lblUserFirstName").text());
            storeMainLabel(label, 'LastName', $(".lblUserLastName").text());

            //if ($('.chkLNFIntern:checked')) {
            if ($(".chkLNFIntern").prop("checked")) {
                //label.setObjectText(HEADING3, "LNF INTERN");
                //label.setObjectText('StartDate', "");
                storeMainLabel(label, HEADING3, "LNF INTERN");
                storeMainLabel(label, 'StartDate', "");
                displayRows("none", "none", "");
            } else {

                //if ($(".lblStaff").text() == STAFF) {
                if ($(".chkLNFStaff").prop("checked")) {
                    //label.setObjectText(HEADING3, STAFF);
                    //label.setObjectText('StartDate', "");
                    storeMainLabel(label, HEADING3, STAFF);
                    storeMainLabel(label, 'StartDate', "");

                    /*$(".trmanagers").css("display", "none");
                    $(".trmngprof").css("display", "none");
                    $(".trorg").css("display", "none"); */
                    displayRows("none", "none", "");
                } else {
                    var mang = "";
                    if (INTERNAL == internalExternalOrMixed) {
                        mang = "Prof. " + $(".ddlAllManagers option:selected").text();
                        displayRows("", "none", "");
                    } else if (EXTERNAL == internalExternalOrMixed) {
                        mang = $(".ddlOrgs option:selected").text();
                        displayRows("", "", "none");
                    } else if (MIXED == internalExternalOrMixed) {
                        if ($(".ddlOrgs option:selected").text() == UNIVERSITY_OF_MICHIGAN) {
                            mang = "Prof. " + $(".ddlAllManagers option:selected").text();
                        }
                    } else {
                        mang = $(".ddlAllManagers option:selected").text();
                    }

                    //label.setObjectText(HEADING3, mang);
                    //label.setObjectText('StartDate', $(".startDate").text());
                    storeMainLabel(label, HEADING3, mang);
                    storeMainLabel(label, 'StartDate', $(".startDate").text());
                }
            }

            //var pngData = label.render();
            var labelImage = document.getElementById('labelImage');
            labelImage.src = "data:image/png;base64," + label.render(); //pngData;

            // ----- mini label -----
            //labelmini.setObjectText('FirstName', $(".lblUserFirstName").text());
            //labelmini.setObjectText('LastName', $(".lblUserLastName").text());
            storeMiniLabel(labelmini, 'FirstName', $(".lblUserFirstName").text());
            storeMiniLabel(labelmini, 'LastName', $(".lblUserLastName").text());

            var labelMiniImage = document.getElementById('labelMiniImage');
            labelMiniImage.src = "data:image/png;base64," + labelmini.render();
        }

        function storeMainLabel(lab, name, val) {
            val = val.trim();
            lab.setObjectText(name, val);
            jsObjLabelMain[name] = val;
        }

        function storeMiniLabel(lab, name, val) {
            lab.setObjectText(name, val);
            jsObjLabelMini[name] = val;
        }

        function createLabel(labtype) {
            var label = null;
            var jsobj = null;

            if (labtype == 'MAIN') {
                jsobj = jsObjLabelMain;
                label = dymo.label.framework.openLabelXml(globalLabelXml);
            } else if (labtype == 'MINI') {
                jsobj = jsObjLabelMini;
                label = dymo.label.framework.openLabelXml(globalLabelMiniXml);
            }

            for (var prop in jsobj) {
                label.setObjectText(prop, jsobj[prop]);
            }
            return label;
        }

        function updateCustomLabel(textTop1, textTop2, textBottom) {
            if (!globalLabelXml || !globalLabelMiniXml)

                return;
            var label = dymo.label.framework.openLabelXml(globalLabelXml);
            var labelmini = dymo.label.framework.openLabelXml(globalLabelMiniXml);

            storeMainLabel(label, 'FirstName', textTop1);
            storeMainLabel(label, 'LastName', textTop2);
            storeMainLabel(label, 'StartDate', textBottom);
            storeMainLabel(label, 'HEADING3', '');

            //var pngData = label.render();
            var labelImage = document.getElementById('labelImage');
            labelImage.src = "data:image/png;base64," + label.render(); //pngData;
            // ----- mini label -----
            //labelmini.setObjectText('FirstName', textTop1);
            //labelmini.setObjectText('LastName', textBottom);

            storeMiniLabel(labelmini, 'FirstName', textTop1);
            storeMiniLabel(labelmini, 'LastName', textBottom);

            var labelMiniImage = document.getElementById('labelMiniImage');
            labelMiniImage.src = "data:image/png;base64," + labelmini.render();;
        }

        $('.ddlAllManagers').change(function () {
            updatePreview('ddlAllManagers');
        });

        $('.ddlOrgs').change(function () {
            updatePreview('ddlOrgs');
        });

        $('.chkLNFIntern').click(function () {
            updatePreview('chkLNFIntern');
        });

        $('.chkLNFStaff').click(function () {
            updatePreview('chkLNFStaff');
        });

        $('.chkCustomLabel').click(function () {
        	if ($(".chkCustomLabel").prop("checked")) {
                updateCustomLabel('Visitor', '', 'Medium');
                $('.txtTextTop').val('Visitor');
                $('.txtTextBottom').val('Medium');

                $('.tblDetails').hide();
                //$('.tblCustomLabel').show();
                $('.tblCustomLabel').fadeIn("slow");
        	} else {
                $('.txtTextTop').val('');
                $('.txtTextBottom').val('');

                $('.tblCustomLabel').hide();
                $('.tblDetails').fadeIn(1000);
                updateCustomLabel('', '', '');

                $(".ddlUsers").val("--Select--");
            }
        });

        $('.txtTextTop').keyup(function () {
            updateCustomLabel($(".txtTextTop").val(), '', $(".txtTextBottom").val());
        });

        $('.txtTextBottom').keyup(function () {
            updateCustomLabel($(".txtTextTop").val(), '', $(".txtTextBottom").val());
        });

        // loads all supported printers into a combo box --------------------------
        var printers;
        function loadPrinters() {
            printers = dymo.label.framework.getPrinters();
            if (printers.length == 0) {
                alert("No DYMO printers are installed. Install DYMO printers.");
                return;
            }

            for (var i = 0; i < printers.length; i++) {
                var printer = printers[i];
                if (printer.printerType == "LabelWriterPrinter") {
                    var printerName = printer.name;

                    var option = document.createElement('option');
                    option.value = printerName;
                    option.appendChild(document.createTextNode(printerName));
                    printersSelect.appendChild(option);
                }
            }
        }

        function isTwinTurboOROtherPrinter() {
            if (!printers) return -1;
            if (typeof printers.isTwinTurbo == "undefined") {
                return -1;
            }
            else {
                if (printers.isTwinTurbo)
                { return 2; }
            }
            return 0;
        }

        // prints the label
        printMainButton.onclick = function () {
            var label = createLabel('MAIN');
            var printParams = {};
            //if (isTwinTurboOROtherPrinter() == 2) {
            printParams.twinTurboRoll = dymo.label.framework.TwinTurboRoll.Left; //Auto or Left or Right
            try {
                if (!label) {
                    alert("Load main label before printing");
                    return;
                }
                //label.print(printersSelect.value);
                label.print(printersSelect.value, dymo.label.framework.createLabelWriterPrintParamsXml(printParams));
            }
            catch (e) {
                alert(e.message || e);
            }
            //}
        }

        printMiniButton.onclick = function () {
            var labelmini = createLabel('MINI');
            var printParams = {};
            //if (isTwinTurboOROtherPrinter() == 2) {
            printParams.twinTurboRoll = dymo.label.framework.TwinTurboRoll.Right; //Auto or Left or Right
            try {
                if (!labelmini) {
                    alert("Load mini label before printing");
                    return;
                }
                //labelmini.print(printersSelect.value);
                labelmini.print(printersSelect.value, dymo.label.framework.createLabelWriterPrintParamsXml(printParams));
            }
            catch (e) {
                alert(e.message || e);
            }
            //}
        }

        function makeUmichNonDefault() {
            if ($(".ddlOrgs").size() > 1) {
                if ($(".ddlOrgs option:selected").text() == "University of Michigan") {
                    //$(".ddlOrgs option:selected").prop('selectedIndex', 1);
                    //alert($('.ddlOrgs option:eq(1)').text());
                    //$('.ddlOrgs option:selected').selectedIndex = 1;
                    $('.ddlOrgs').get(0).selectedIndex = 1;
                }
            }
            updatePreview('makeUmichNonDefault');
        }

        function loadLabelsFromWeb() {
            // use jQuery API to load label
            $.get("../labeltemplates/userlabel.xml", function (labelXml) {
                globalLabelXml = labelXml;
                //label = dymo.label.framework.openLabelXml(labelXml);

                updatePreview('loadLabelsFromWeb:1:label');

                printMainButton.disabled = false;

                makeUmichNonDefault();

            }, "text");

            // mini
            $.get("../labeltemplates/mini.xml", function (labelMiniXml) {
                globalLabelMiniXml = labelMiniXml;
                //labelmini = dymo.label.framework.openLabelXml(labelXml);
                if (!labelMiniXml) {
                    alert('labelmini is null');
                }
                updatePreview('loadLabelsFromWeb:2:labelmini');

                printMiniButton.disabled = false;
            }, "text");
        }

        // load printers list on startup
        loadPrinters();

        loadLabelsFromWeb();
    };

    // register onload event
    if (window.addEventListener)
        window.addEventListener("load", onload, false);
    else if (window.attachEvent)
        window.attachEvent("onload", onload);
    else
        window.onload = onload;

}());