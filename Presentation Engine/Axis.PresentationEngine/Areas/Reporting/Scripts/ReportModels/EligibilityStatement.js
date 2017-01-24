function EligibilityStatementReportModel(eligibilityData) {
    
    var defaultData = {
        FullName: '',
        ClientID: '',
        DOB: '',
        EligibilityDate: '',
        Duration: '',
        RedetermineDate: '',
        ChronologicalAge: '',
        AdjustedAge: '',
        AAE: '',
        AMD: '',
        APD: '',
        PersonalSocialAE: '',
        PersonalSocialMD: '',
        PersonalSocialPD: '',
        CommAE: '',
        CommMD: '',
        CommPD: '',
        ECAE: '',
        ECMD: '',
        ECPD: '',
        GMAE: '',
        GMD: '',
        GMPD: '',
        FMAE: '',
        FPMD: '',
        FPMPD: '',
        CognitiveAE: '',
        CD: '',
        CPD: '',
        Notes: '',
        TeamMembers: '',
        EligibilityCategory: 'Initial'
};

    var reportData = $.extend(true, { }, defaultData, eligibilityData);

////DYNAMIC DATA - SETTINGS
    var checkWidth = 7; //SETS SIZE OF CHECK MARK ICONS
    var starWidth = 7; //SETS SIZE OF STAR ICONS
    var checkedboxWidth = 7; //SETS SIZE OF CHECKED BOX ICONS
    var uncheckedboxWidth = 13; //SETS SIZE OF UNCHECKED BOX ICONS
    var chkboxColWidth = 18;
//////////////////////////////////

/////DYNAMIC DATA - HEADER////
    var childName = reportData.FullName;
    var clientID = reportData.ClientID;
    var childDOB = reportData.DOB;
    var eligibilityDate = reportData.EligibilityDate;
    var duration = reportData.Duration;
    var redetermineDate = reportData.RedetermineDate;

    /////DYNAMIC DATA - PAGE 1//////
    var chkEntry = false;//temporary
    var chkAnnual = false;
    var chkRedetermine = false;
    var chkOther = false;
    if (reportData.EligibilityCategory.indexOf("Initial") > -1) {
        chkEntry = true;
    }
    else if (reportData.EligibilityCategory.indexOf("Annual") > -1) {
        chkAnnual = true;
    }
    else if (reportData.EligibilityCategory.indexOf("Qual") > -1) {
        chkRedetermine = true;
    }
    else if (reportData.EligibilityCategory.indexOf("Other") > - 1) {
        chkOther = true;
    }

    var chronAge = reportData.ChronologicalAge.toString();
    var adjustedAge = reportData.AdjustedAge;
    //what causes this to check?
    var chkNotEligible = false;
    var chkEligible = true;

    //populate this list
    var teamMemberNames = reportData.TeamMembers;
    
    var chkMedicallyHeader = false;
    var diagnosis = '';
    var icdCode = '';
    var chkChartContains = false;

    var chkHearingHeader = false;
    var chkHearing = false;
    var chkVision = false;

    //update via variable based on elig type
    var chkBDI2 = true;
    var chkQualitative = false;

    var chkAdaptive = false;
    if (reportData.APD !== undefined && reportData.APD !== '' && reportData.APD >= 25) {
        chkAdaptive = true; }
    var adaptiveAE = reportData.AAE.toString();
    var adaptiveMonths = reportData.AMD.toString();
    var adaptivePctDelay = reportData.APD.toString();

    var chkPersonalSocial = false;
    if (reportData.PersonalSocialPD !== undefined && reportData.PersonalSocialPD !== '' && reportData.PersonalSocialPD >= 25) {
        chkPersonalSocial = true; }
    var personalAE = reportData.PersonalSocialAE.toString();
    var personalMonths = reportData.PersonalSocialMD.toString();
    var personalPctDelay = reportData.PersonalSocialPD.toString();
    var chkQualPersonalSocial = false;

    var chkCommunicationRCEC = false;
    if (reportData.CommPD !== undefined && reportData.CommPD !== '' && reportData.CommPD >= 25) {
        chkCommunicationRCEC = true;
    }
    var communicationRCECAE = reportData.CommAE.toString();
    var communicationRCECMonths = reportData.CommMD.toString();
    var communicationRCECPctDelay = reportData.CommPD.toString();

    var chkCommunicationEC = false;
    if (reportData.ECPD !== undefined && reportData.ECPD !== '' && reportData.ECPD >= 25) {
        chkCommunicationEC = true;
    }
    var communicationECAE = reportData.ECAE.toString();
    var communicationECMonths = reportData.ECMD.toString();
    var communicationECPctDelay = reportData.ECPD.toString();

    var chkGrossMotor = false;
    if (reportData.GMPD !== undefined && reportData.GMPD !== '' && reportData.GMPD >= 25) {
        chkGrossMotor = true;
    }
    var grossMotorAE = reportData.GMAE.toString();
    var grossMotorMonths = reportData.GMD.toString();
    var grossMotorPctDelay = reportData.GMPD.toString();
    var chkQualGrossMotor = false;

    var chkFineMotor = false;
    if (reportData.FPMPD !== undefined && reportData.FPMPD !== '' && reportData.FPMPD >= 25) {
        chkFineMotor = true;
    }
    var fineMotorAE = reportData.FMAE.toString();
    var fineMotorMonths = reportData.FPMD.toString();
    var fineMotorPctDelay = reportData.FPMPD.toString();

    var chkCognitive = false;
    if (reportData.CPD !== undefined && reportData.CPD !== '' && reportData.CPD >= 25) {
        chkCognitive = true;
    }
    var cognitiveAE = reportData.CognitiveAE.toString();
    var cognitiveMonths = reportData.CD.toString();
    var cognitivePctDelay = reportData.CPD.toString();

    var chkQualFineMotor = false;
    var chkQualCommunicationSpeech = false;
    var chkQualCommunicationOral = false;

    var chkCommunicationOralMotor = false;
    var chkMotor = false;
/////END DATA PAGE 1

////DYNAMIC DATA - PAGE 4
    var notes = reportData.Notes;
////END DATA PAGE 4

/*
defaultData =   {
                10:'<data>'   
};
var userData = {};
var reportData = defaultData; //$.extend(true, {}, defaultData, userData);
*/

    return {
        content: [
//PAGE 1
            {
                table: {
                    widths: [10, '*', 150],
                    body: [
                        [{ image: chkEntry ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }, 'Entry', { image: 'ecitextlogo', rowSpan: 4, width: 160, margin: [-15, 20, 0, 0] }],
                        [{ image: chkAnnual ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }, 'Annual', {}],
                        [{ image: chkRedetermine ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }, 'Re-Determine Qualitative', {}],
                        [{ image: chkOther ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }, 'Other', {}],
                    ]
                },
                layout: 'noBorders'
            },
            { text: 'Eligibility Statement', alignment: 'center', fontSize: 14, bold: true, margin: [0, -18, 0, 0] },

//CHILD NAME CLIENTID DOB
            {
                table: {
                    widths: [65, 180, 42, '*', 20, '*'],
                    body: [
                        [{ text: 'Child\'s Name', style: 'h1' }, { text: childName, fontSize: 11 }, { text: 'Client ID', style: 'h1' }, { text: clientID, fontSize: 11 }, { text: 'DOB', style: 'h1' }, { text: childDOB, fontSize: 11 }]
                    ]
                },
                layout: 'noBorders',
                margin: [0, 5, 0, 0]
            },


//CONTAINER
            {
                table: {
                    widths: ['*'],
                    body: [
                        [
                            {
                                //NOT ELIGIBLE / ELIGIBLE TABLE    
                                table: {
                                    widths: [10, 230, 200, 60],
                                    body: [
                                        [{ image: chkNotEligible ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }, { text: 'Not Eligible', style: 'h1' }, { text: 'Chronological Age', alignment: 'right', style: 'h1' }, { text: chronAge, rowSpan: 2 }],
                                        [{ image: chkEligible ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }, { text: [{ text: 'Eligible', style: 'h1' }, { text: ' - Select one (1-3 below) & enter\nrequired information' }] }, { text: '(According to BDI-2 instructions)', alignment: 'right', italics: true }, {}],
                                        ['', { text: [{ text: 'Eligibility Date ', style: 'h1' }, { text: eligibilityDate }, { text: '\n(if testing conducted on more than one date, enter the first date)', fontSize: 8 }] }, { text: 'Adjusted Age\nDuration', alignment: 'right', style: 'h1' }, { text: adjustedAge + '\n' + duration }],
                                        [{ colSpan: 2, text: [{ text: 'Interdisciplinary Team Member Names:\n ', style: 'h1' }, { text: '(and Discipline)' }] }, {}, { colSpan: 2, text: teamMemberNames, margin: [-50, 1, 0, 0] }, {}]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ]
                    ]
                }
            },

//SPACER
            { image: 'blank', height: 2 },


//CONTAINER
            {
                table: {
                    widths: ['*'],
                    body: [
                        [
                            {
                                //MEDICALLY DIAGNOSED CONDITION   
                                table: {
                                    widths: [50, 340, 50, 71],
                                    body: [
                                        [{ colSpan: 4, margin: [175, 0, 0, 0], columns: [{ width: chkboxColWidth, stack: [{ image: chkMedicallyHeader ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: '1 - Medically Diagnosed Condition', style: 'h1' }], fillColor: 'lightgray' }, {}, {}, {}],
                                        [{ text: 'Diagnosis', style: 'h1' }, { text: diagnosis }, { text: 'ICD Code', style: 'h1' }, { text: icdCode }],
                                        [{ image: chkChartContains ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }, { colSpan: 3, text: 'Child\'s chart conatins medical records confirming diagnosis', margin: [-40, 2, 0, 0], alignment: 'left' }, {}, {}]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ]
                    ]
                }
            },

//SPACER
            { image: 'blank', height: 2 },


//CONTAINER
            {
                table: {
                    widths: ['*'],
                    body: [
                        [
                            {
                                //HEARING VISION IMPAIRMENT  
                                table: {
                                    widths: [10, 517],
                                    body: [
                                        [{ colSpan: 2, margin: [150, 0, 0, 0], columns: [{ width: chkboxColWidth, stack: [{ image: chkHearingHeader ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: '2 - Hearing/Vision Impairment', style: 'h1' }, { text: ' (check one or both)', italics: true }] }], fillColor: 'lightgray' }, {}],
                                        [{ image: chkHearing ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }, { text: [{ text: 'Hearing Impairment: ', bold: true }, { text: 'As defined by the Texas Education rule at 19 TAC Section 89.1040 (Qualifies for AI services)' }], margin: [0, 2, 0, 0] }],
                                        [{ image: chkVision ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }, { text: [{ text: 'Vision Impairment: ', bold: true }, { text: 'As defined by the Texas Education rule at 19 TAC Section 89.1040 (Qualifies for AI services)' }], margin: [0, 2, 0, 0] }]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ]
                    ]
                }
            },

//SPACER
            { image: 'blank', height: 2 },

//DEVELOPMENTAL DELAY TABLE
            {
                table: {
                    widths: [178, 60, 45, 70, '*'],
                    body: [
                        [
                            { colSpan: 5, text: '3 - Developmental Delay', style: 'h1', fillColor: 'lightgray', alignment: 'center' }, {}, {}, {}, {}
                        ],
                        [
                            { rowSpan: 2, margin: [0, 40, 0, 0], text: [{ text: 'Area(s) of Delay\n', bold: true, alignment: 'center' }, { text: '- Enter months & percent delay in every area\n in which a delay is identified\n- Check all areas in which a qualifying delay is identified', italics: true, fontSize: 9 }] },
                            { colSpan: 3, margin: [70, 0, 0, 0], columns: [{ width: chkboxColWidth, stack: [{ image: chkBDI2 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { alignment: ' left', text: 'BDI-2', bold: true }] },
                            {},
                            {},
                            { margin: [25, 0, 0, 0], columns: [{ width: chkboxColWidth, stack: [{ image: chkQualitative ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { alignment: 'left', text: 'Qualitative\nDetermination of Delay', bold: true }] },
                        ],
                        [
                            {},
                            { text: [{ text: 'Domain Age\nEquivalent\n', bold: true, alignment: 'center' }, { text: 'Use Raw Score to AE Tables to calculate AE', fontSize: 9, alignment: 'center' }] },
                            { text: 'Months of Delay', bold: true, alignment: 'center' },
                            { text: [{ text: 'Percent\nDelay\n', bold: true, alignment: 'center' }, { text: 'Delay >= 25% for\nInitial; >= 15% for\nContinuing', fontSize: 9, alignment: 'center' }] },
                            { text: '- Check box and enter percent delay below\n- Team must complete Pg.2 of this form to support Qualitative Determination of Delay', fontSize: 9 }
                        ],
                        [{ columns: [{ width: chkboxColWidth, stack: [{ image: chkAdaptive ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'Adaptive', bold: true }, { text: ' (SC, PR)' }] }] }, { text: adaptiveAE, alignment: 'center' }, { text: adaptiveMonths, alignment: 'center' }, { text: adaptivePctDelay, alignment: 'center' }, { text: '', fillColor: 'lightgray' }],
                        [{ columns: [{ width: chkboxColWidth, stack: [{ image: chkPersonalSocial ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'Personal-Social', bold: true }, { text: ' (AI, PI, SR)' }] }] }, { text: personalAE, alignment: 'center' }, { text: personalMonths, alignment: 'center' }, { text: personalPctDelay, alignment: 'center' }, { image: chkQualPersonalSocial ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }],
                        [{ columns: [{ width: chkboxColWidth, stack: [{ image: chkCommunicationRCEC ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'Communication', bold: true }, { text: ' (RC, EC)' }] }] }, { text: communicationRCECAE, alignment: 'center' }, { text: communicationRCECMonths, alignment: 'center' }, { text: communicationRCECPctDelay, alignment: 'center' }, { text: '', fillColor: 'lightgray' }],
                        [{ columns: [{ width: chkboxColWidth, stack: [{ image: chkCommunicationEC ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'Communication', bold: true }, { text: ' (EC) ' }, { text: 'Expressive only\n33% delay for Initial eligibility;\n15% delay for Continuing eligibility', italics: true, fontSize: 9 }] }] }, { text: communicationECAE, alignment: 'center' }, { text: communicationECMonths, alignment: 'center' }, { text: communicationECPctDelay, alignment: 'center' }, { text: '', fillColor: 'lightgray' }],
                        [{ columns: [{ width: chkboxColWidth, stack: [{ image: chkGrossMotor ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'Gross Motor', bold: true }, { text: ' (GM)' }] }] }, { text: grossMotorAE, alignment: 'center' }, { text: grossMotorAE, alignment: 'center' }, { text: grossMotorPctDelay, alignment: 'center' }, { image: chkQualGrossMotor ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }],
                        [{ columns: [{ width: chkboxColWidth, stack: [{ image: chkFineMotor ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'Fine Motor', bold: true }, { text: ' (FM, PM)' }] }] }, { text: fineMotorAE, alignment: 'center' }, { text: fineMotorMonths, alignment: 'center' }, { text: fineMotorPctDelay, alignment: 'center' }, { image: chkQualFineMotor ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }],
                        [{ columns: [{ width: chkboxColWidth, stack: [{ image: chkCognitive ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'Cognitive', bold: true }, { text: ' (AM, RA, PC)' }] }] }, { text: cognitiveAE, alignment: 'center' }, { text: cognitiveMonths, alignment: 'center' }, { text: cognitivePctDelay, alignment: 'center' }, { text: '', fillColor: 'lightgray' }],
                        [{ columns: [{ width: chkboxColWidth, stack: [{ image: 'blank', width: uncheckedboxWidth }] }, { text: [{ text: 'Communication', bold: true }, { text: ' (Intelligibility of Speech)\n-considered ', fontSize: 9 }, { text: 'Expressive only:\n', fontSize: 9, italics: true }, { text: '33% delay for Initial', fontSize: 9 }] }] }, { text: '', fillColor: 'lightgray' }, { text: '', fillColor: 'lightgray' }, { text: '', fillColor: 'lightgray' }, { image: chkQualCommunicationSpeech ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }],
                        [{ columns: [{ width: chkboxColWidth, stack: [{ image: 'blank', width: uncheckedboxWidth }] }, { text: [{ text: 'Communication', bold: true }, { text: ' (Oral Motor/Feeding)', fontSize: 9 }] }] }, { text: '', fillColor: 'lightgray' }, { text: '', fillColor: 'lightgray' }, { text: '', fillColor: 'lightgray' }, { image: chkQualCommunicationOral ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }],
                    ]

                }
            },
//END DEVELOPMENTAL DELAY TABLE

//SPACER
            { image: 'blank', height: 2 },

//CONTAINER
            {
                table: {
                    widths: ['*'],
                    body: [
                        [
                            {
                                //HEARING VISION IMPAIRMENT  
                                table: {
                                    widths: [10, 517],
                                    body: [
                                        [{ colSpan: 2, text: [{ text: 'Children younger than 3 months without a qualifying percent of delay\n', bold: true }, { text: 'Select box for domain that applies below, if applicable\n', bold: true, fontSize: 9 }, { text: 'Team must complete Pg.3 of this form to support Qualitative Determination of Delay', italics: true, fontSize: 8 }], fillColor: 'lightgray', alignment: 'center', margin: [65, 0, 0, 0] }, {}],
                                        [{ image: chkCommunicationOralMotor ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }, { text: [{ text: 'Communication - ', bold: true }, { text: 'Oral Motor / Feeding - Qualitative Determination of Delay' }], margin: [0, 2, 0, 0] }],
                                        [{ image: chkMotor ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }, { text: [{ text: 'Motor - ', bold: true }, { text: 'Qualitative Determination of Delay' }], margin: [0, 2, 0, 0] }]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ]
                    ]
                },
                pageBreak: 'after'
            },
//END PAGE 1
//PAGE 4
//HEADER TABLE
            { text: 'Eligibility Statement', style: 'header', alignment: 'center' },
            { text: 'Notes', style: 'header', alignment: 'center' },
            {
                table: {
                    widths: [70, 290, '*', '*'],
                    body: [
                        [{ text: 'Child\'s Name', style: 'h1' }, { text: childName }, { text: 'Eligibility Date', style: 'h1', alignment: 'right' }, { text: eligibilityDate }],
                        [{ text: 'Client ID', style: 'h1' }, { text: clientID }, '', ''],
                    ]
                },
                layout: 'noBorders'
            },
//END HEADER TABLE
            { text: '\n\nNOTES', bold: true },


//NOTES TABLE 
            {
                table: {
                    widths: ['*', 1],
                    body: [
                        [
                            {
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: notes }, { image: 'blank', height: 600 }]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ]
                    ]
                }
            }
//END PAGE 4
        ],

        defaultStyle: {
            fontSize: 10
        },

        styles: {
            header: {
                fontSize: 12,
                bold: true
            },
            h1: {
                fontSize: 11,
                bold: true
            },
            h2: {
                fontSize: 10,
                bold: true
            },
        },


        pageMargins: [25, 20, 25, 20],

        images: {
            star: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAwAAAANCAYAAACdKY9CAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAFpSURBVChTdZG9qsJAEIVHTOwsRNIEiVhYaJEyYGEVCwshT6GVINhq4ePYiIJiIRYiiJWWNoqNaIqAiGghJB6zk3i5P96vmZ3ZnbNndgn/0Ov1UK1Wee26LkfBx4bxeAxFUVAoFDh/Pp8cBR8bYrEYzuczotEobrdbWA2QttstzedzWi6XpKoqdbtdajQalEgkqFgsUiQSoR/IsoxarQb/IFuZTCahFpDL5TAajcIsgI7HI/sVQ755e348HiiVStB1HY7jcI1nED79y3C5XLj4m+FwiEwmg36/HzR4ngfDMHjzO4PBAJVKBZZlIZ/Psyg3dDodNJtNPnS9XrHZbHidSqVYdb1ew7ZtrnGD8LlarVgxHo/DNE3ezGazX82z2Yyj5P8i7XY7arfbdDgcyL+BNE2j/X5PkiSRPzi/ZjKZ5EjCf7lcxnQ6ZQXBYrGAfwD1eh2n0ymsBvz5aSEgaLVaSKfTuN/vnAcAL7rq038epq0jAAAAAElFTkSuQmCC',
            check: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACLSURBVDhPxdDRDYAgDEVR5mIg52EalmEYLOVhCoItMdH7pYnHFlx+0ac4Hs6HhJcdTFBIyooLdEfEG7LgFDzJbmZNxVXehnIK5m1nQ7knXOV8KLfEWHc1lAMeT2ahwPi0xNxGxdrtBz5EPK3P2hJnxv0gnfYXJrZXN+Ykvobb6IhJW2FpwHv9hXM+AcFlMWf0Jh7VAAAAAElFTkSuQmCC',
            checkedbox: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABMAAAAUCAIAAADgN5EjAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAADPSURBVDhPnZHBEYMgEEUhtZAmtAJyShV4JJfcLAKPyS1NaAVJE9rLZl0WBieQQd9F2eEB+1cCgDjEib/7OW4KfC3A7Bpe1nP4TpOYjZvpAf8YDW/ed+fUyctTmNHr1WbQ4KF9oc70GnpBQyrMZWhXDWNIvF8Tt0mkHRYuTN359lm9l1VcYigymicemsyVkg5rbC+BEjJbM/7Tdpf1ymbiErEc8WYuIWX7OPBMf0w+W331atkrTkXfncPu3kWvPE9lbTq8HNTzNpMqsgnVIMQXsjm8fOdmR6IAAAAASUVORK5CYII=',
            uncheckedbox: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAABPSURBVDhPY/z//z8DuYAJSpMFKNLMAHT2//+3J1hBucQCqwm3//+nlrPTtoFdQRhsS4PqGLgAG9VMIhjVTCJA0jzLi5E44DULqoMCmxkYAJWVOGY1wYUUAAAAAElFTkSuQmCC',
            blank: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQMAAAAl21bKAAAABlBMVEX///////9VfPVsAAAACklEQVQImWNgAAAAAgAB9HFkpgAAAABJRU5ErkJggg==',
            ecitextlogo: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAPkAAAA4CAYAAAA/+16BAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAACLfSURBVHhe7d0J9HbV9AfwW8gsMjbwGjMkKYSKMkXITMr4JpkpRYkMpWTKlLFkaTCPaTCEIiplnlLmRJkzlCHR5/B91n6P5/m9v7f1/9eT7nets+69Z9hnn3P23mefc6eV/nkBhksB0syVVlqpHWehdsfy8o4YcUnAyv85/s+Dwgr/+Mc/llHkivPPP3/RxmDEiEsKLlUzOcWlyLkOqmJf5jKXGRV9xP8ULjVKDppKcT/3uc8NH/rQhyZKnC549KMfPdz+9rdv5+KElVe+1Dg7I/5Hcalck++///7Di170ojZru45Cv+51rxse+tCHDn/84x+HK17xisNlL3vZVmbEiEsyFj1NZb2aMA2Jr3lyXuOgj58War6F0OdfDCjw5S53uXZMMGufe+65w0knnTR8/etfH/72t79NpZf6EmrciBHzhkUreWY8x5zbxOoRQa8zJKQsY5HNL9dJq6h15Nq5kDV1j5r/woCCK//9739/+NGPfjR87WtfG04//fRleExInGPOg1n8jRhxcWGFF5wRdIhiCHVDqxd011UJoxy5hpqW8lGgmm+hNXLyXVhF++1vfzv89Kc/ndD5yU9+sgxf045Q21fjR4yYByxKyQmvEAUjyBHqCHaE29FaN+jL5DployCJj4JW2o5BPQ8qHVjIEMyCslz1v/71r+1c+P3vf9/opk5H4Ve/+tVw6qmntlmfS596U27EiHnCorSB4FIcAu7cjHfaaadNFeqzzz57+M53vtPO//73v7e1LYg/55xzhq985SsTRU554ayzzhrOO++8Vk9Pc3lKi6/wl+sVhTLXuMY1hqtd7WoTWle4whWWqdu5th944IHDG97whuGNb3zjcPzxxzd+q5EZMWKesOJT3gX47ne/O/ziF7+YCDWFcC5Q4lNOOWX49a9/PXz1q19t61vpX/rSl1oa42CmpzC1/De/+c0WT4l+97vfTdIYBzRAnLw9xP/5z39uxsU6etpeQQVj8pe//GWZwADZUb/+9a8/MUKrrrpqox1e4De/+U2bxc3yDNOnP/3pVrYagxEj5gnLlUy3k3qIW7JkSTuvCkB5CP/tbne75tL++Mc/HtZee+3mBlMEa1xptQzloKDinCsvL4j71Kc+NRx11FFN+SlflImbLA7EnXDCCcMHPvCBZhSmGYIKvN/jHveYhLvf/e7DPe95z2H11VcfrnKVq7SddjTQ1dbwA9x56eFXffgPllf3iBEXNabeJ08U5fziF784rLXWWk2gCbjZzix9i1vcYpKPm3vGGWcMf/rTn9qDJo94xCOGVVZZpSnoXe5yl+HEE09seX/2s58NW2211WQmRHO11VYbfvjDHzZ6t7rVrZpCUSL1uv7lL3/Z0s3Oa6yxxnClK12pzbBoWA6sueaaw9WvfvWm8IzI/e9//+EPf/hDuw121atetRkehkW58FuNTA/LCx6IvG6x8VjQeeQjH9nq+cxnPjMcdNBBLR1thuEFL3hBSxO3EO0RIy4O/JeSuxQooJ3mww47rCnada973WGdddZpMzQlo0g///nPh/XXX3/YaKONhuOOO64pHYW+3vWu14T9yle+cjMKX/7yl5uSccevfe1rtxn5W9/61nCTm9ykGQflGIMtt9yyzdoMBPde3mte85otDzryc93NtspYP2c2v/zlL9/irKPxf+aZZ7YZlit+hzvcYbjf/e7X8kH1CCBdoL14ZYAYGsble9/7XmvL0572tGHjjTcejjzyyOFd73pXS6fkG2644fD0pz+90UNnVPIR84aZM7nAJaa0YKblfh9zzDFN0ay7KZZZnoKa8SgBBbzhDW/Y8lBqSsI9zi70LW95yxZn5rO+pcBceobBrEzRuMjqN0syBp/85CfbPoAZVZnM5pT31re+9XDTm960KTXX+TrXuU7zKCgyI2M2xhNaaRfUo8B42USj4PikxOpkyBiPxzzmMcMWW2wxHHHEEcN73/veVlYdeN95552bEVIOrVHRR8wTps7kUAWW0h199NHDvvvu21ziKAElN1NypSkeV3zrrbcebn7zmzcai0U/s4KlAtffLrZNucqPo/zK8TCe8pSnDA972MOaEQjkSb5cKwuUmeHQhsSjI2/yO1Lw3CnwuCuj8ta3vrUZMOlmckbl+c9/fvM4oNYzYsQ8YOpMTnkIqkDB3CrijpshowRmLjN1Zk3xjhT8CU94QluXV1Thz7mjoCxD4SjebPrKV76yucZmbu472lBpgHKMjY0z9Vp/Jy3o633zm988vOQlL5nUazb2cgpX3zVIY2jcEWDI7LozImZ37QZ5GQdKzjOp/TZixLxg2emzgKB+4xvfGHbZZZe2Lo+CR4FswmXXOcopUAIvf5j1zXSgTA1AIZwr4xgaNr523HHH4ZBDDmnrbbMtZZIWxKgAhbMU+OhHPzo8+9nPbsao5gX0Ux/ECxEYEArMtVdP+Eg+7r46LCu0LX0QoxMaI0bMK5qmRAESCDK3/BWveEVbExPkKBXIQyEi7JQigVJQ/je96U3NtQ1NqPnQqwrlaD3+8pe/vC0NpGXGBDTkSX0VrimajbK99tpr8qAOpO6+rtATqtvumDLqRzdpoZF0fWB259XgS/qIEfOGlSPAEV5HG1j77LPP8PnPf36iaNIi3PKA68y0Zm15CTtFF/fa1762udxVwXrUOimoOtHsFSrnjkJm3RqnXkbJLa08UCPIk3w9xPFKgponnog2gbTeqNUwYsQ8YmXCSQEyE3F9X/WqVw0f//jHJwpeZ9TMVhHqKJtgF9oxcda01tbuaU+DegP3ns3gFDw8Cakv1wlACaNg8mmDmdc99gMOOGCSD0Kzhzh0HNMmR/3Am3GrMPHaFUTx3eaDWfRHjLi40WbyKBLYeX7/+9//XwJLiCP8uab88hF4oTcKZtYf/OAHzXWPolSEpttv73nPexptvCQ+tFNPlCwzrPjQlRYDgca73/3uNqsnfRaSv9bnPLf30E17ar7U7d79iBHzjDaTE2wC7OizSGYvgiwNxBN2wX1pu8kMgQ05m2zuj9u44vbKKyiPpvCJT3xicr+9B5qMAEXP2jg0cm1324zp/rtNMk++mbGt/eUD9VB+gQK673344YdP0haCOlIvaDcvJJ5C0gNxvBZP63lAaMSIecYy0m833b1pCmQWBgpCEcVts802w8EHH9xuVW2wwQbtwZalS5cOhx56aHPLPZQShUigqO6zv/Od75woUYVbVB/72MeaQkUZKZFzRsPGlvrUy53HH+Oyxx57DPe6172WUULKnVlXPLqMx/IQ70Sdoceo5N550kBajp72y/3xQPkRI+YJTXIjwB4Q8Vw6RaHYBNmRoj7+8Y8fXvziFw/Xuta1Wt4e973vfYeXvexlTTGUq0rBYBx77LHtSbfE5Wjtb5MsyhmolwLZjHNP2/337HR7AMWDNzb2GB51iQ/PrvHsjTS31HpQxAT56wso4hgjj8wyWniQRxCfdPfU89HHESPmGW1NDmZbL3UAIY6SOl933XWHZz7zme1WWgS+h7g73/nOzRjkOkeK4TlyG2IVZlDPqFNOeSB1Oz7nOc8ZHvCAB7T4IIopcJd33XXX9gAMl56hUC5uNrqeRQ9SB8iXurSde556c7QEYVgC8aHBw/AYbkWlP2LEvKCtycE9Zk+3EWpxlIigmxGf+tSnNqFO3DRhTpyZ9ba3vW1TMEg8xfPuNWVKnPvigjogCiaP58S9+TUL4cVz6c973vOah2GdjHaUHS1K7kGelAnCg2Nei3UeuspqA+OUckkDhkF9I0bMOyYzuRc8KFeUMwJt3W2GDjLL9RCnrBdHvLDSKznF+/a3v92WA8FnP/vZyQshmSXVy2N4+MMf3uJ6hF9pSWdULBcoM9fbUVt4CoyXW2E9Up+Adzvp4rJsEMc7yLvuED7lYbTSxhEj5hmTmfwLX/jCxM0FRzPV5ptv3mbxxEXQe1QF8U43V7oqgTRrb+tkoIBx35WlVGgrw7B4fXUawkMQpX/sYx/bdvq574LHcYVnPetZbX3dI+VydC/fJl1oOzIUufefOAGPjtVgjRgxr2gvqBBmiknQbZJF6Qjz9ttvP6y33noT4QbpUY5AeuLsittt5x1wxRNPYTxJ96hHPao9l+7NLnXmHXA0zMRPfvKTm8JG8YUgtHJMWs0zC6nj7W9/e9udZ3hSDm9ectlss80m9XLh87kqqHXpC3zOMkYjRswLmpJ7dZLCeQCEAkeYwRqdkFOQKAnUPCCtKj9Fz4yXvNxfirH77ru3Gd2mWr4QA5RL8Py69XitryJ1SJO/1lvz9+VzHSV3nrKOXozxfrx+YJy4+dx9+UI/UK9XXOtSZsSIeUTTLp9lsvYk6BHoKAe32kxPQSmuoyCuBnFm4VxHcUIn8KANcHUZAflqMKu7DQd92UB80tQBNS6YVT51pSxQWvVy7ZPus1Y8mxgrcRX9PfIRI+YRTcq9HMJdnSb89XxFEFo1AGMAjEeUJ5DHG103uMENJtcXJSi098Kj0Hbu88VW0BfpD3mzVzFixDyjSSyBjmA7mtUoYNIurLKhVQOEljp6uvLYWZ/1wM3/J9J2ih1eKbSlCl5BHpuTjmZ8BmnEiHnHZJrOznqCNSkhJ+AXdjbvQTkWMh65jpJdlKiKnXZD7uGH32zCmckvDj5HjFhRNO2NQhPaKBplpPjceOt16+0VDcrVwFVHE7KLX6F+6/ncZruolShtr0cbj+ETP5Rf0D/JN2LEPKPtrntJxH3mfOIJCLCPM/r2mQ0ogi0uitcL+DSF7PNQcN9T8zSbnWu76/YDap1mSt+Uu/e97/1/6kVA+Peuud31fpb2f3JvlXkJRr0ea/VEnu+w40s/2HxksDw//8IXvnDqPfgRI+YJTcnNnG6h+b464Y4yWB97k8uLGotByi0GblP5Fro6laHQjgzBnnvuOTzucY9r9CBHvCVf6knaYuoNf/U+eeLBCy9RcnFebfVdN39nAW/Deb7fb41vfOMbt8dpGYLQrRAnVAMmT+Kn8att4Qnky7Eau/RBTyfnqSOoZWt8z0NPbyE6FbVcDHPKVZ6cV3rOe95q/Wkn1PiFUOvp64OefngVcl7HoZZ3nvI5r3VBaNbzPh1quaRBX0e9hpTLOXo1vYe0xo1vkttVtuOdBMGMlZ8XQIjnWBHmw1jOZ4Eb7HVOdVZGlfG4q2VC4sJP7TTnlb5nyT0263VZH4P0HL5zH47QjsUCzdC3CeeWnnMGzz308OHakgPEhZeet9BLHkEbap7QdM2b8rZe/SJNLZsgLsKU63ouLUe0PHSkjyrkZVT9ndV/3SxBPDPhyT/8BKEjjgfGuzE+jh4HDtBLPghPuZZe6Sav+OzV5NpReiAu7zpUGmBpmP/vyZcgHx7Chzj96wGneJA1j3TIeepJuhDkPHUlpGw9Tz5QZ98u42IMTHygj0284S0IvcTps5wnPgEcxbdn17mtd73rXZeZRVQgUJJcyytMA8ZDtFbSIx1nKWA2VKd8yevagKXBkDpDP3XV4F1z33z3zLsHaXwS2rfYLUMI7mKRdoJ68hYaZbebTrnlcWtNvyWvuJQNf+AortKsR5AmH3q+864d9RXZlA1SNuVcp44+AE9k6dKlbdkRvqQ5pyAeULKEsWfikWCfrE56zS94UIkXREksVzzZWCeHlIOUyTVU3vu+znXiajq8+tWvbt/vs2SqYMA8RemDJ6Ef3lPeUdrJJ5/c+iLeGagHwmelEcQIhb/QDn2o57Ws8+TvoS5vYuLJjzvk0b8vfelLW3rKpnylEz5z3fdf8v871wXw44BkmCReQITrajPMubhZqGV91GG//fYb3vKWt7T1ta+2+kmCL8SEBmX2PjbGKnPSPZzjA5AgTt0ZCEjegJB5w43X4Zyw5uEcZfNwzUIITTObuvAxLWRXvT7R56g96hICaaGVa1CPc3nrOZghtV+fQy0fQXOdeMf0Reg4JgBe/SgiHk3ygfJm7hhV+fIUYqWbMvrYW3v6ycypXOoJXKddfQiSXo08+pX//lpd8TgqtM/MF08l+RO0ER1QVhvJBkifdqxtd95PRkF4hKSnTPKqH2pZ54knq/mtl3hLWG0ND9PKg2shfRQ+5E8aTF5Q8VklT3CFYAr6bJOXV4LKfA/xLOTee+89vOY1r2mW1/fXfRjSDFD/hAKbbLLJ5J544nSmGe0d73hHs9D4kCYekk98BtvMT8mTR1oazHgt5r47voQsEwSzdvrBABgMcfgj4BEU9eKLchgcCqpM4JwgSpMndyzyZCAXktKg74g+yCMvGBe8KEOY0VTWMYOqP8ID3ioPEVJlvSikHuBR+diHV4TVr2700s9g1k5707foCfLjEc20u5bVBoZBeviUnrFCG78R8GrI5JOuvP5Tl3IZ28CeEe/DHo/y2ointFW9gjbjUXmTQfoWtE890gP5LXWUNUbopQ8Aj66lOYfa/wGeMpbpD/Vrjzq0TVsd5fH/AC9W1XqUEVKX/PhKW9Wnrc71kbSMw+RVU5tMt7nNbRpDiMikEkQpqbVXGiKthzQzEAXPBxA1QF7CYxc6Px1MnZ5s8804+ZRXX9K42FyW+maYtDQcNIbVw5/GZpZVt7zq9gJJyi8PyqT9gF4ULjRzXQVEmp9BeN3VDxC5v9zutIkA+S6el18ESvXEJz6xGT5p3HP9tsMOOzTPiSEh3N7j91WcDJwZ1Ec5uMjpM5BGeHhO6DOeftDI+IF8aPojqw1Wv2vef//9m6ARLJ/vYsjTfxkH6TYp3emwnHvuc5/b+JInY0EJdtttt1anNuW3UtLsjTAed7zjHdsLUDy69K/jhz/84faTS8//W175iKj+labut73tbe2lIWPovQLKo2wvf2Z3ee3BWG/jwxeFbN5uuummrW+103/8TDqWYPrKprJ2WKp429LY6Tc08C+//lZmu+22a+Pk55ZmXbB2XnqBm03+jId/5JFxE4t/59mkxa/NW3S44ejoS0sduqWPfOMQT369/ZGPfKR9Lcn3CbVTX5jwfOrMtxTVx73HHx2xNPV/BC+Spa2UXbugjRMmgEVHhMAkDiEDb0OLwulkcdIVDiEggCqwho/FhdDzG2Jr8AppGEtejZLXtUb7ZJSXWWxKgLoh/Olkb7XJR4gBDeV1uo09Hb5YaI9OzXnqQ5ORyjpc/ZRKHWBpoaP1gTw2Dik1YVBWmgFkbSmJc6/ZMoaE2Uabr9XaV6DQ6NsDUL/fPzMEYE1tgHlcxqX2v2/guQVI6RhVxkLfxfKjS6gJMINIsAihOowvd1e/pd3OP/jBDzaDon5KgheGw9jIJw+ltjxTJ0PhDgXoC3dJKJ5+Yqwt4eTFDwUi9PoAbxSBEeG96WN9hJb+kq5v1CUt4xJoo3oou5lYf/par7sg+PJ9QUqjHdKUJ1MmCArF69Qnxpgh9IqyfkRX3zCIvjpsc5qnmn0qNLTZh0UZKH2l3V6z1rfepCQDGWMfP+UZ40n7jDl+Y1QsI/STl6LUp60+yYY/iquc8aeL2qJfjIe/7JIntMgBQ2Fs9LPQeivC4mf81q91lgCV6QxWjkKlcPLoTBs2WXNjGE3nURqzXBSxDhKrRhnVGeWMoIHBfcYzntFmPffzDTrrZzY08xm81EOhUrdzH1p0Xz7tC/rrQBmhts21/J5TF6IIlFzHOx500EGtw91SO/DAA1tb8Ur5CaaBsmTIu+5oU1K0wJHi6F/7FGgxFj5rZTBj5AiIfGbGAC0eVH6n7JNZlONGN7pREwjjpYx26GeCeJ/73Ke5fgRDvPrT32k7g05gjIcNzNe//vXtPf++j/Bq08v3/xgfskCwyYtNLm0wU5ppGAoeD+Uy61EssyMlZOztnFMELq98qZuR1J7w14+fuL4N7ozYpENffgZKHQwJPsiU2ZF3oRyDaOzEUVBjpt/AMxG8FZ4BxWeIgByqR7/of2lcbd6P50sYCApnrAE9HsmTnvSkFocn3y70aTXyy8t64AMf2NptLNHjoTASvBPGxkRs2ctgoyGYEHgG6tRWBjRyKzR3PYFCcI0MXEv8j8JkQFkxLiRGdApFc+/YAGIYIrigLIbN4BowDTe72c2ayyavhsZS48G5ellOMw9Xhatpx5y75dNOOqAf4BzVmfjFIB0TGrl25OnoTJ0qHV2KQqDxd6c73akpt7Zy1SipmdesLh83zUAZYC4Wq4y2oK2+fmsdxsiqD7zHb0ffLCWfwfWL5/w1NnyKx4f6vf5qfNAyw8RA6CcKI+hHCiktdVXodzS9T88TIkDcUAaEUFe+3X6lMNpMAM0myvEkGEWKZswIP9ki+IwPRbFUo3QMPXlyi9JdBekUnjFjPIwjz8gzCfhNu2cBX5YP2267bTOcZlYzLEPL6ICvCVF2fKjfA1rGjlHw0JNxA/LLlX/wgx/c+PVdP3Innj5Y5hprM7nlrjEQp5/RYUgZLfkZcO6+MWJwGVITH56k0z9lyRaZNvub1X1pSd9buvAyeByMNyNgXOPG003GkIern9JXTQNqp1lDYU5HqbhXWjMXC2b2omg2vFhm+SipPFVwXFtv6YhZYCQouzqVRYcyRZico28WN7NxpVwnXh4dk3opkHUe4VpRpE7QL/rAMfwT3BgO9XERHc2yBIv3wAjpJ26YdDwaiIBQRVnCN+XNuXh9QGjUR8m5b1wzZY0PpL3WZvrubne7W7sGhpPgUXb9QZhiHBgSdJWfpjD44DpSWELLwAFhNKvhDeSj5Hnqj5JqK8HDL4HLg1RmMcIqTTt4HwxbyhJu/BljQmqWN6tmNmVEKWkdn1nQJv/TB4aBsTOOgn6Sru3ZO6LQ1uPGjhGOnEnTRpvSwDDikVE3JsozxBkvhpE3zICQeXLK+JIFdCixuvGkvYyMsXGUjj+0MnGhbwz0E8ML2oUfRovx0I54OQxHNpnVgwY0aUU4g81yswiEQhzmpWNCIUG868y6AoQoONcA1jsuk9BDHEFh3TQe7fAC6IQ/9SS90nKOT3l1kMaaXQjntDpnQXmhKhprqb58mdVAa7c08Y4GxyaitlJmyi74uwo6aDJIgXNtST2QPhQH4hkWg2pmtJYlDAQI5FM/xBBFIUIjSL4c9XP6elr/4AVNYIwCfIf39I8+1j4gYOJAnuQNeELSkyeCixZeco1/5WJcILz3bZuF9LejflS+b6826lMzOONp/EwOjBHlTR+nTv0rnoxZ85uJjbd0cYyJsoLxR4/x0G550u6co137J0h/Jk/fTzkKYK8gSN5A3rbxpiIE0yguOBcpwiOkcxwxoUxfaSCNgrP6O++882TmqXmglucOqVNnKZ+OdXQtX/iDxIWGgF/x3DQdLX/P20JQNvsGoE8oORoRGjOgwZbXrGBWdG4WsBa2SWJDyW6qWZTg23CrD7eYBcxUGUx8pr09r2YwrqtNMLTUA2k7sOx4yAMezrmJPAprW0Imb/JLhwhRD/3Is1COWxiYYc0gs/q0CiWBNxPx8kAZa3T9a3ZniLnumVnRNfszPtLkS3uAoTPDwrS6eySPY8YuEIcfbrw2UkTjZvzczbC08lBV5CfjBIwBGbVXRK59rty4yGNtbs8DLUtZXoEZXVvQSb+rv29DHX/5XKNL1rLRJ43XxstiuHhB0dsgtNUnNHoSZEJYhHMNJ6DcBJYuBYTkF0JMA1PW0cChYZNJB6ZcD3lBOsGw7tCJXBQIPQ0RamcrIy7ngvzWVgYpSB3Lg/Ly4sPRNWHnckGOSVcXd4kXYqnB5cvto4MPPrj1H6HmGRkMmzqeaLIh5Zzgpc70PeRc3WAdKC+3j6vINUt7U4Zbz8Pwj/ajjjqqCYL7xgSRgOG11lHrQqeHuj3DT8hs4jAUdt9tflK0jHeAfoCefQSzGCW3o841t9Fm55p3Y21K0fWPjSU7yh6askegvWZTxtM+jxlT3RTHGBD6aTz3iGzgTf7Ko3OKmjW2ehgwbbL3YyPMbr9+F1fr0/940w/OGTPKrj0MaxSSDNgP8JdeHkn4CNCttI1FxkNe/BtX/Nnf8DQfHm2A2uPRh7wsNGrboiuhL60peQRAcA6Yd/8PMYqegmEqBKA2wKyVDRduOsg7C7VOncWauqdK0GoHpO5ptPCGhwc96EHNgpoNIOV6oBPagXyMUx5iIExmW4oszboKGC9QnqBoq40urhsPyM61+5qUzsYRJSe0lJRXY3fVIOEXXUdtrX0ZBQfuOqESh5Y2Vd7lt77LjrkNLobOepxRsC+hXbUOcJ5Z1Lk+RDfxxt9mG8Vyy8amDyWVLshbBSpQj7hsZBFOPNhA04filyxZ0voMjLe9BLvTZlYzKMPp3jwvSjkbXxQR7do3AV7EO4YvxyBtT1zokBOyZma0M23/4X3ve18bO5MTOikb4NHszVO1R8GIUraswS078c54MXb6rfYZhEf9DKkn45E0s7VNWnRNmOiaJHjI3uBMudpW55Vf+Ld2FSSTwDp5HFUH6BANkx6iGMo1pgS3KTw84HYDpMIqCAuBq8bl2Wmnndpaw6yuMalLowK0pTMOjApXy3n4qwifgB6BUzZB2ygi91I+7qrbYDrYdbwIHc+6UnB1gV82uY1EAZUnpAbFrEUI7Axzu/GuHKGmgGiha8Mq6yr5XeerMwaUkrvmWQF+0p/qFLiGlCF1UDD9QSjNSHgKTaibNOjLh4760i5Ljhhchp5wx2uQFw3n4cWMxThoE0V2S1Ff6VtxBJYsGTd0KQQjqf95O7wfHoD22S22hJOX4VWvpQqee1lKn5FRnpa2ogvy4lO7nDPeeMpeAzllWLTR+Jud3S4z/ll2hFb6nXLrX7IePOQhD2ntUcZmKz7JsHFWBn+p07X+zmarfnNOHtSBV/2q7Zaw7lzIp5/QIU9kCx1tyeYlKIdndJSXp71q+p/0CRLliLhB4oZwB1kS66cIvTwaZnAMig0HFYFKlA89FS4P8srn6FaF+5XWs1xhdYoX0DXwLKU6bYjApGFT6pKmrEHgQvZ5dDQ3zYBrj91SrhLFJZxmUnDbhUDYBDNweDHruN2hrygJFz5tAXVyX8VRAF4CgSMM1psGirIRaPkIqjh0KYcBNisSBEg7c65veB14cI2uGV596tIGwkNgpXOBKUfq1w7p1v+8FX2BPrfUWlkZysJgMJLSudnqxas+8NwAj4aQoSHOHYa4+CaN1C9NP7tNpG0UVDr6Ab658OrTN2gwYDY5lQ+U1x5yx5DhyxgI6jJW6rJ3oX/N3Noaw6qPtRFtNCiwfmPgjRs6+imQnw6gh9+0hyEkG2RAf1qjo+OavKkPff2KB5NieFIPxVeXMdBWCiyvdHck1GP8I1v4NXZoKisv2cEHGUMDpip5mJYkpEN1tg41aATKdQTF4BFaqOV6GsuDvAGmXeskdRpIQkTABJ0W9xnP8kOOFeFD2rT0gDCglbWfjjQYhEcb+rKh27cvdYWvWXWmryFlKqxbeQlcZx4SyBfIP4sH6GmmbOJqeuXF2E5rb9DT7dHXEyTecRq/4aHysiLAd4R7eTTwMI2PaW2fRkucPGlTnz6Nvuvl9VvS0QdxaRMkvqcLKVvpTFVyqIVqgYUgX827ImUr0gjoO24aFsNr8iR9oTzSMoDJF55cy4evmkdcykubRivIeV9+2tFaldtNwSl66IJ0NBKXehPvKATJ18e5zjFwjkaFuOQNalnHnNfrjGGfHtT+yZGxrYKdclDLBtID6cmf+JSZdp284Jh2V74rLch5lYPEOe/zQ58H6nWtNzQd9UPyOaZsRfJGBiB5Zyo5JFM9QipxLVTCSUuF0KcthNCE5K/1OU96UNODaXXVTlgovaKvK0je0HHtXOj5yHU9d6x1pTzUfNw/7ps1fdZ00NOt5SHpNV/qnFUX1HKQtMQnraYnbRpq/uRxnNXP8uEv50GlMw19//d1pHw9dwwS18e7rv1V89UZP2WSr0fK5TgN0haTD6RD8uKjL//vuH8O/wICvwAEqeQStgAAAABJRU5ErkJggg==',
        }

    };
}