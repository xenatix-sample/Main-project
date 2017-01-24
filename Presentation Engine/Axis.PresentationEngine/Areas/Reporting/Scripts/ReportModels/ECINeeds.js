function ECINeedsReportModel(userData) {
    var defaultData = {
        childName: '',
        screeningDate: '',
        evaluatorName: '',
        childAge: '',
        //PAGE 2 DATA
        //RED FLAGS
        71: { 17: false },
        262: { 17: false },
        263: { 17: false },
        264: { 17: false },
        265: { 17: false },
        266: { 17: false },
        267: { 17: false },
        268: { 17: false },
        269: { 17: false },
        271: { 17: false },
        272: { 17: false },
        273: { 17: false },
        274: { 17: false },
        276: { 17: false },
        277: { 17: false },
        278: { 17: false },
        279: { 17: false },
        //CROSSWALK
        308: { 17: false },
        310: { 17: false },
        313: { 17: false },
        314: { 17: false },
        315: { 17: false },
        316: { 17: false },
        317: { 17: false },
        321: { 17: false },
        323: { 17: false },
        361: { 17: false },
        325: { 17: false },
        326: { 17: false },
        329: { 17: false },
        330: { 17: false },
        332: { 17: false },
        334: { 17: false },
        335: { 17: false },
        336: { 17: false },
        338: { 17: false },
        339: { 17: false },
        340: { 17: false },
        341: { 17: false },
        342: { 17: false },
        345: { 17: false },
        347: { 17: false },
        348: { 17: false },
        349: { 17: false },
        350: { 17: false },
        351: { 17: false },
        352: { 17: false },
        353: { 17: false },
        356: { 17: false },
        357: { 17: false },
        358: { 17: false },

        //VISION NEED IDENTIFIED
        363: 0,
        364: 0,
        //HEARING
        74: { 17: false },
        280: { 17: false },
        281: { 17: false },
        282: { 17: false },
        76: 0,
        77: 0,

        //VCFS CHECKLIST
        283: { 17: false },
        284: { 17: false },
        285: { 17: false },
        286: { 17: false },
        287: { 17: false },
        288: { 17: false },
        289: { 17: false },
        88: 0,
        89: { 17: ''},
        90: { 17: '' },

        //ASSISTIVE TECHNOLOGY
        79: 0,
        80: 0,
        81: 0,
        82: 0,
        83: 0,
        84: 0,
        85: 0,

        //NUTRITION REVIEW
        93: { 17: false },
        291: { 17: false },
        292: { 17: false },
        293: { 17: false },
        294: { 17: false },
        295: { 17: false },
        296: { 17: false },
        95: { 17: '' },
        96: 0,
        98: 0,
        99: 0,

        //AUSTISM 
        101: { 17: false },
        102: { 17: false },
        103: { 17: false },
        104: { 17: false },
        105: { 17: false },
        106: { 17: false },
        297: { 17: false },
        298: { 17: false },
        299: { 17: false },
        107: { 17: '' },
        108: { 17: '' },
        109: { 17: false }

        //END PAGE 2 DATA
    };

    var reportData = $.extend(true, {}, defaultData, userData);

////IMAGE WIDTH DEFAULTS
    var uncheckedboxWidth = 7;
    var blackstarWidth = 6;

    return {
        content: [
            { text: 'ECI Needs Assessment, Identification & Referral', alignment: 'center', bold: true },
            {
                table: {
                    widths: [80, 230, 50, 150],
                    body: [
                        ['Child\'s Name:  ', reportData.childName, 'Date:  ', reportData.screeningDate], //<---INSERT CLIENT SIGNATURE AND DATE OF CLIENT SIG
                        ['Evaluator: ', reportData.evaluatorName, 'Child Age: ', reportData.childAge]
                    ]
                },
                fontSize: 10,
                margin: [0, -3, 0, 0],
                layout: {
                    hLineColor: function(i, node) {
                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                    },
                    vLineColor: function(i, node) {
                        return 'white'; //OUTSIDE, INSIDE VERT LINES
                    },
                    paddingLeft: function(i, node) { return 2; },
                    paddingRight: function(i, node) { return 0; },
                    paddingTop: function(i, node) { return 0; },
                    paddingBottom: function(i, node) { return 0; }
                }

            },

            //MAIN CONTAINER
            {
                table: {
                    widths: [185, 'auto'],
                    body: [
                        [{ colSpan: 2, style: 'blackHeader', text: 'Vision' }, {}],

                        //RED FLAGS TABLE
                        [

                            //RED FLAGS HEADER TABLE
                            {
                                table: {
                                    widths: ['*'],
                                    body: [
                                        [{ style: 'header', text: 'Red Flags', margin: [0, 10, 0, 0] }],
                                        [{ text: 'Referral needed if one or more items are checked', italics: true, bold: true, alignment: 'center', fontSize: 8 }]
                                    ]
                                },
                                layout: 'noBorders'
                            },
                            {
                                table: {
                                    body: [
                                        [{ style: 'header', text: 'Crosswalk: Battelle' }],
                                        [{ margin: [5, 0, 0, 0], style: 'body', ul: ['If any * is failed, refer for vision evaluation.', 'If 25% or more of the other items tested are failed, team should discuss the possibility of a vision deficit and the need for referral.'] }],
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ],
                        [
                            //RED FLAGS CHECKLIST TABLE
                            {
                                table: {
                                    widths: ['*'],
                                    body: [
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[71][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'Risk factors ', bold: true }, { text: 'present ' }, { text: '(see checklist)', italics: true }] }] }],
                                        [{ text: 'Observations', bold: true, margin: [0, 2, 0, 0] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[262][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Pupil sizes are unequal, or pupils react unequally to light' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[263][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Pupil is not black' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[264][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'One or both eyelids droop significantly' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[265][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Eyes roll upward' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[266][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Eyes do not close completely when child is sleeping' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[267][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Eyes appear to be crossed or turned' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[268][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Eye movements are unsteady, shaky or jerky (nystagmus)' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[269][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Eyes appear cloudy' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[271][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Size of either or both eyes appear to be abnormally small or large' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[272][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Tilts head to see object' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[273][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Holds object close to eyes' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[274][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Over reaches for objects' }] }],
                                        [{ text: 'For children who are walking', bold: true }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[276][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Doesn\'t avoid obstacles when walking' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[277][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Cannot negotiate doorways safely' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[278][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Does not navigate drop offs & surface changes ' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[279][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'None of the above were observed' }], margin: [0, 5, 0, 0] }]
                                    ]
                                },
                                style: 'body',
                                layout: {
                                    hLineColor: function(i, node) {
                                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    vLineColor: function(i, node) {
                                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    paddingLeft: function(i, node) { return 2; },
                                    paddingRight: function(i, node) { return 0; },
                                    paddingTop: function(i, node) { return 0; },
                                    paddingBottom: function(i, node) { return 0; }
                                }
                            }, //END RED FLAGS CHECKLIST TABLE
                            //CROSSWALK TABLE
                            {
                                table: {
                                    widths: [60, 60, 60, 70, '*'],
                                    body: [
                                        //[{colSpan: 5, style: 'header', text: 'Crosswalk: Battelle'},{},{},{},{}],
                                        // [{colSpan: 5, style: 'body', ul: ['If any * is failed, refer for vision evaluation.','If 25% or more of the other items tested are failed, team should discuss the possibility of a vision deficit and the need for referral.']},{},{},{},{}],
                                        [{ colSpan: 2, text: 'Number of items tested', alignment: 'center', fontSize: 7 }, {}, { colSpan: 2, text: 'Number of items failed', alignment: 'center', fontSize: 7 }, {}, { text: '% failed', alignment: 'center', fontSize: 7 }],
                                        [{ text: 'Adaptive', alignment: 'center', fontSize: 6 }, { text: 'Personal-Social\n(AI, PI, SR)', alignment: 'center', fontSize: 6 }, { text: 'Communication\n(RC, EC)', alignment: 'center', fontSize: 6 }, { text: 'Motor\n(GM, FM, PM)', alignment: 'center', fontSize: 6 }, { text: 'Cognitive\n(AM, RA, PC)', alignment: 'center', fontSize: 6 }],
                                        //BIRTH
                                        [{ rowSpan: 8, image: 'naNoVision', width: 7, alignment: 'center', margin: [0, 35, 0, 0] }, { colSpan: 4, text: 'Birth', bold: 'true', alignment: 'center', fillColor: 'lightgray', fontSize: 8 }],
                                        [
                                            {},
                                            //AI 1
                                            {
                                                table: {
                                                    widths: [20, 20],
                                                    body: [
                                                        [{ image: 'blackstar', width: blackstarWidth, alignment: 'right' }, { text: 'AI 1', margin: [-7, 0, 0, 0] }]
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            },
                                            '',
                                            //GM 8
                                            {
                                                table: {
                                                    widths: [20, 20],
                                                    body: [
                                                        [{ image: 'blackstar', width: blackstarWidth, alignment: 'right' }, { text: 'GM 8', margin: [-6, 0, 0, 0], alignment: 'left' }]
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            },
                                            //AM 1 and AM 5
                                            {
                                                table: {
                                                    widths: [5, 20, 5, 20],
                                                    body: [
                                                        [{ image: 'blackstar', width: blackstarWidth }, { text: 'AM 1', margin: [-6, 0, 0, 0] }, { image: 'blackstar', width: blackstarWidth }, { text: 'AM 5', margin: [-6, 0, 0, 0] }],
                                                        [{ image: 'blackstar', width: blackstarWidth }, { text: 'AM 2', margin: [-6, 0, 0, 0] }, { image: 'blackstar', width: blackstarWidth }, { text: 'PC 2', margin: [-6, 0, 0, 0] }],
                                                        [{ image: 'blackstar', width: blackstarWidth }, { text: 'AM 3', margin: [-6, 0, 0, 0] }, '', '']
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            }
                                        ],
                                        //12 Months
                                        [{}, { colSpan: 4, text: '12 Months', bold: 'true', alignment: 'center', fillColor: 'lightgray', fontSize: 8 }],
                                        [
                                            {},
                                            //BLANK
                                            '',
                                            //RC 9
                                            {
                                                table: {
                                                    width: [20],
                                                    body: [
                                                        [{ text: 'RC 9', bold: true, margin: [18, 0, 0, 0] }]
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            },
                                            //FM 6
                                            {
                                                table: {
                                                    width: [20],
                                                    body: [
                                                        [{ text: 'FM 6', margin: [25, 0, 0, 0] }],
                                                        [{ text: 'FM 9', margin: [25, 0, 0, 0] }]
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            },
                                            //AM 7 and AM 11
                                            {
                                                table: {
                                                    widths: [5, 20],
                                                    body: [
                                                        [{ image: 'blackstar', width: blackstarWidth }, { text: 'AM 7', margin: [-6, 0, 0, 0] }],
                                                        [{ image: 'blank', width: blackstarWidth }, { text: 'AM 11', margin: [-6, 0, 0, 0] }]
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            }
                                        ],


                                        //24 Months
                                        [{}, { colSpan: 4, text: '24 Months', bold: 'true', alignment: 'center', fillColor: 'lightgray', fontSize: 8 }],
                                        [
                                            {},
                                            //SR 8 and SR 12
                                            {
                                                table: {
                                                    widths: [20, 20],
                                                    body: [
                                                        [{ image: 'blackstar', width: blackstarWidth, alignment: 'right' }, { text: 'SR 8', bold: false, margin: [-6, 0, 0, 0] }],
                                                        [{ image: 'blank', width: blackstarWidth }, { text: 'SR 12', bold: false, margin: [-6, 0, 0, 0] }]
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            },
                                            //RC 12
                                            {
                                                table: {
                                                    widths: [20, 20],
                                                    body: [
                                                        [{ image: 'blackstar', width: blackstarWidth, alignment: 'right' }, { text: 'RC 12', margin: [-6, 0, 0, 0] }]
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            },
                                            //PM 1
                                            {
                                                table: {
                                                    widths: [20, 20],
                                                    body: [
                                                        [{ image: 'blackstar', width: blackstarWidth, alignment: 'right' }, { text: 'PM 1', margin: [-6, 0, 0, 0] }],
                                                        [{ image: 'blank', width: blackstarWidth }, { text: 'PM 2', margin: [-6, 0, 0, 0] }],
                                                        [{ image: 'blank', width: blackstarWidth }, { text: 'PM 3', margin: [-6, 0, 0, 0] }]
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            },
                                            //AM 13 and PC 5
                                            {
                                                table: {
                                                    widths: [5, 20, 5, 20],
                                                    body: [
                                                        [{ image: 'blank', width: blackstarWidth }, { text: 'AM 13', margin: [-6, 0, 0, 0] }, { image: 'blank', width: blackstarWidth }, { text: 'PC 5', margin: [-6, 0, 0, 0] }],
                                                        [{ image: 'blackstar', width: blackstarWidth }, { text: 'AM 14', margin: [-6, 0, 0, 0] }, { image: 'blank', width: blackstarWidth }, { text: 'PC 6', margin: [-6, 0, 0, 0] }],
                                                        [{ image: 'blank', width: blackstarWidth }, { text: 'RA 3', margin: [-6, 0, 0, 0] }, '', '']
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            }
                                        ],


                                        //36 Months
                                        [{}, { colSpan: 4, text: '36 Months', bold: 'true', alignment: 'center', fillColor: 'lightgray', fontSize: 8 }],
                                        [
                                            {},
                                            //AI 17
                                            {
                                                table: {
                                                    widths: [40],
                                                    body: [
                                                        [{ text: 'AI 17', margin: [20, 0, 0, 0] }]
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            },
                                            //BLANK
                                            '',
                                            //GM 33 AND FM 13
                                            {
                                                table: {
                                                    widths: [25, 20],
                                                    body: [
                                                        [{ text: 'GM 33' }, { text: 'FM 13' }],
                                                        [{ text: 'GM 35' }, { text: 'FM 21' }],
                                                        [{ text: '' }, { text: 'PM 6' }],
                                                        [{ text: '' }, { text: 'PM 7' }],
                                                        [{ text: '' }, { text: 'PM 8\n' }]
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            },
                                            //RA 4
                                            {
                                                table: {
                                                    width: [20],
                                                    body: [
                                                        [{ text: 'RA 4' }],
                                                        [{ text: 'PC 16' }],
                                                        [{ text: 'PC 18' }]
                                                    ]
                                                },
                                                layout: 'noBorders'

                                            },
                                        ],
                                    ]
                                },
                                margin: [-1, 0, -1, -1],
                                style: 'crosswalkBody',
                                layout: {
                                    hLineColor: function(i, node) {
                                        return (i === 0 || i === node.table.body.length) ? 'white' : 'black'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    vLineColor: function(i, node) {
                                        return (i === 0 || i === node.table.widths.length) ? 'white' : 'black'; //OUTSIDE, INSIDE VERT LINES
                                    },
                                    paddingLeft: function(i, node) { return 5; },
                                    paddingRight: function(i, node) { return 0; },
                                    paddingTop: function(i, node) { return 0; },
                                    paddingBottom: function(i, node) { return 0; }
                                }
                            }
                            //END Crosswalk Battelle Table
                        ]
                    ]
                },
                layout: {
                    paddingLeft: function(i, node) { return 0; },
                    paddingRight: function(i, node) { return 0; },
                    paddingTop: function(i, node) { return 0; },
                    paddingBottom: function(i, node) { return 0; }
                },


            },
            {
                table: {
                    widths: ['*'],
                    body: [
                        [
                            {
                                table: {
                                    widths: [105, 8, 10, 8, 80, '*', 8, 10, 8, 15],
                                    body: [
                                        ['Vision need identified?', { image: reportData[363] == 43 ? 'checkedbox' : 'uncheckedbox', width: 10 }, { text: 'Yes', alignment: 'left', margin: [-5, 0, 0, 0] }, { image: reportData[363] == 44 ? 'checkedbox' : 'uncheckedbox', width: 10 }, { text: 'No', margin: [-5, 0, 0, 0] }, 'Referral for vision evaluation indicated?', { image: reportData[364] == 43 ? 'checkedbox' : 'uncheckedbox', width: 10 }, { text: 'Yes', alignment: 'left', margin: [-5, 0, 0, 0] }, { image: reportData[364] == 44 ? 'checkedbox' : 'uncheckedbox', width: 10 }, { text: 'No*', margin: [-5, 0, 0, 0] }]
                                    ]
                                },
                                style: 'h2',
                                layout: 'noBorders'
                            }
                        ]
                    ]
                },
                layout: {
                    paddingLeft: function(i, node) { return 5; },
                    paddingRight: function(i, node) { return 0; },
                    paddingTop: function(i, node) { return 0; },
                    paddingBottom: function(i, node) { return 0; }
                },
            },

            //END FIRST HALF
            //SECOND HALF
            {
                table: {
                    widths: ['*', '*'],
                    body: [
                        //HEADER ROW
                        [{ text: 'Hearing', style: 'blackHeader' }, { text: 'VCFS Checklist', style: 'blackHeader' }],
                        //HEARING RISK FACTORS CHECKLIST 
                        [
                            {
                                table: {
                                    widths: ['*'],
                                    body: [
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[74][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'Risk factors ', bold: true }, { text: 'present ' }, { text: '(see checklist)', italics: true }] }] }],
                                        [{ text: 'Developmental indicators for possible hearing loss', bold: true, margin: [0, 5, 0, 0] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[280][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Birth - 11 months: Child has at least 1 month delay in communication' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[281][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: '12 months and older: Eligibile for ECI services and has delay in communication' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[282][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'None of the above were observed' }] }],
                                        [{ columns: [{ width: 100, text: 'Hearing need identified?' }, { image: reportData[76] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, { text: '\bYes', width: 20 }, { image: reportData[76] == 44 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, '\bNo'], fontSize: 9, bold: true, alignment: 'left' }],
                                        [{ columns: [{ width: 170, text: 'Referral for hearing evaluation indicated?' }, { image: reportData[77] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, { text: '\bYes', width: 20 }, { image: reportData[77] == 44 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, '\bNo*'], fontSize: 9, bold: true, alignment: 'left' },],
                                        //[{text: 'Assistive Technology', style:'blackHeader', width: 500}]
                                    ]
                                },
                                style: 'body',
                                layout: {
                                    hLineColor: function(i, node) {
                                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    vLineColor: function(i, node) {
                                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    paddingLeft: function(i, node) { return 2; },
                                    paddingRight: function(i, node) { return 0; },
                                    paddingTop: function(i, node) { return 0; },
                                    paddingBottom: function(i, node) { return 0; }
                                }
                            },
                            //VCFS CHECKLIST TABLE
                            {
//rowSpan: 3,
                                table: {
                                    widths: ['*'],
                                    body: [
                                        [{ text: 'If child has 2 or more characteristics listed below and is not under the care of a physician or VCFS, provide family with DARS ECI VCFS brochure and recommend a follow-up', bold: true, italics: true, fontSize: 8 }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[283][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Hypotonicity' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[284][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Articulation disorder' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[285][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Resonance disorder' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[286][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Nasal regurgitation during feeding with no history of cleft palate' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[287][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Recurrent ear infections combined with cardiac anomal, feeding disorder, cleft palate, or sub-mucosal cleft palate' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[288][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Eligible with fine motor or gross motor delay' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[289][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Eligible with communication delay' }] }],
                                        [{ columns: [{ width: 170, margin: [0, 5, 0, 0], text: 'Are two or more of these risk factors present?' }, { image: reportData[88] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left', margin: [0, 5, 0, 0] }, { text: '\bYes', width: 20, margin: [0, 5, 0, 0] }, { image: reportData[88] == 44 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left', margin: [0, 5, 0, 0] }, { text: '\bNo', margin: [0, 5, 0, 0] }], fontSize: 8, bold: true, alignment: 'left' }],
                                        [{ columns: [{ width: 150, text: 'Date VCFS brochure was given to family:', fontSize: 8, bold: true }, { text: '' + reportData[89][17], bold: false, alignment: 'left' }] }]
                                    ]
                                },
                                style: 'body',
                                layout: {
                                    hLineColor: function(i, node) {
                                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    vLineColor: function(i, node) {
                                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    paddingLeft: function(i, node) { return 2; },
                                    paddingRight: function(i, node) { return 0; },
                                    paddingTop: function(i, node) { return 0; },
                                    paddingBottom: function(i, node) { return 0; }
                                }
                            }
                        ],
                        [
                            //ASSISTIVE TECHNOLOGY AND NOTES
                            { text: 'Assistive Technology', style: 'blackHeader' }, { rowSpan: 2, text: [{ text: 'Notes: ', bold: true }, { text: '' + reportData[90][17] }], fontSize: 9 }
                        ],
                        [
                            {
                                table: {
                                    widths: ['*'],
                                    body: [
                                        [{ columns: [{ width: 155, text: 'Child has functional communication system' }, { image: reportData[79] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, { text: '\bYes', width: 20 }, { image: reportData[79] == 44 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, '\bNo'], style: 'body', alignment: 'left', }],
                                        [{ columns: [{ width: 155, text: 'Child\'s positioning does not limit interactions' }, { image: reportData[80] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, { text: '\bYes', width: 20 }, { image: reportData[80] == 44 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, '\bNo'], style: 'body', alignment: 'left', }],
                                        [{ columns: [{ width: 190, text: 'Child has age appropriate physcial abilities to explore' }, { image: reportData[81] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, { text: '\bYes', width: 20 }, { image: reportData[81] == 44 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, '\bNo'], style: 'body', alignment: 'left', }],
                                        [{ columns: [{ width: 190, text: 'Childs\' sensory skills (auditory, visual, or tactile) don\'t interfere with ability to interact effectively' }, { image: reportData[82] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, { text: '\bYes', width: 20 }, { image: reportData[82] == 44 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, '\bNo'], style: 'body', alignment: 'left', }],
                                        [{ columns: [{ width: 190, text: 'Child does not have medical diagnosis that usually includes physical limitation' }, { image: reportData[83] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, { text: '\bYes', width: 20 }, { image: reportData[83] == 44 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, '\bNo'], style: 'body', alignment: 'left', }],
                                        [{ columns: [{ width: 125, text: 'Referral for AT evaluation indicated?', bold: true }, { image: reportData[84] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, { text: '\bYes', width: 20 }, { image: reportData[84] == 44 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, '\bNo', { text: 'OR', bold: true, italics: true, margin: [-30, 0, 0, 0] }], style: 'body', alignment: 'left', }],
                                        [{ columns: [{ width: 160, text: 'Team reviewed AT needs as part of evaluation', bold: true }, { image: reportData[85] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, { text: '\bYes', width: 20 }], style: 'body', alignment: 'left', }],
                                    ]
                                },
                                style: 'body',
                                layout: {
                                    hLineColor: function(i, node) {
                                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    vLineColor: function(i, node) {
                                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    paddingLeft: function(i, node) { return 2; },
                                    paddingRight: function(i, node) { return 0; },
                                    paddingTop: function(i, node) { return 0; },
                                    paddingBottom: function(i, node) { return 0; }
                                }
                            }, {}
                        ],
                        [
                            //POST ELIGIBILITY HEADER
                            { colSpan: 2, text: 'Post Eligibilty Determination', fillColor: 'lightgray', fontSize: 10, bold: true, alignment: 'center' }
                        ],
                        [{ text: 'Nutrition Review', style: 'blackHeader' }, { text: 'Autism Spectrum Disorder Risk Identification', style: 'blackHeader' }],
                        [
                            //NUTRITION REVIEW TABLE
                            {
                                table: {
                                    widths: ['*'],
                                    body: [
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[93][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'Risk factors ', bold: true }, { text: 'present ' }, { text: '(see checklist)', italics: true }] }] }],
                                        [{ text: 'Review complete by (check one or more):', bold: false, margin: [0, 2, 0, 0] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[291][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Review of medical records' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[292][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'DARS ECI Nutrition Screen' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[293][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Nutrition Evaluation' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[294][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Nursing Evaluation' }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[295][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Physician\'s physical exam' }, { text: 'Document review in child\'s record', italics: true }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[296][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: 'Discussion of family routines' }] }],
                                        [{ columns: [{ width: 55, text: 'Date of review: ' }, { text: '' + reportData[95][17], alignment: 'left' }] }],
                                        ['Are there any concerns about the child\'s nutrition/meal time/eating habits?'],
                                        [{ columns: [{ image: reportData[96] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, { text: '\bYes', width: 20 }, { image: reportData[96] == 44 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, '\bNo'], style: 'body', alignment: 'left', }],
                                        [{ columns: [{ width: 100, text: 'Nutrition need identified?' }, { image: reportData[98] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, { text: '\bYes', width: 20 }, { image: reportData[98] == 44 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, '\bNo'], fontSize: 9, bold: true, alignment: 'left' }],
                                        [{ columns: [{ width: 130, text: 'Nutrition evaluation indicated?' }, { image: reportData[99] == 43 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, { text: '\bYes', width: 20 }, { image: reportData[99] == 44 ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth, alignment: 'left' }, '\bNo*'], fontSize: 9, bold: true, alignment: 'left' }, ],
                                    ]
                                },
                                style: 'body',
                                layout: {
                                    hLineColor: function(i, node) {
                                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    vLineColor: function(i, node) {
                                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    paddingLeft: function(i, node) { return 2; },
                                    paddingRight: function(i, node) { return 0; },
                                    paddingTop: function(i, node) { return 0; },
                                    paddingBottom: function(i, node) { return 0; }
                                }
                            },
                            //AUTISM RISK TABLE
                            {
                                table: {
                                    widths: ['*'],
                                    body: [
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[101][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'Child is over 18 months ' }, { text: 'AND ', bold: true }] }] }],
                                        [{ columns: [{ width: 25, alignment: 'right', stack: [{ image: reportData[102][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: '\bHas not been screened for autism by another entity ' }, { text: 'AND ', bold: true }] }] }],
                                        [{ columns: [{ width: 25, alignment: 'right', stack: [{ image: reportData[103][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: '\bHas social, emotional, or behavior concern ' }, { text: 'with ', bold: true }] }] }],
                                        [{ columns: [{ width: 40, alignment: 'right', stack: [{ image: reportData[104][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: '\bLanguage delay or atypical speech and/or ' }] }] }],
                                        [{ columns: [{ width: 40, alignment: 'right', stack: [{ image: reportData[105][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: '\bCognitive delay ' }] }] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[106][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'No red flags are indicated at this time' }] }], margin: [0, 3, 0, 0] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[297][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'All of the above apply - red flags are present: ', bold: true }, { text: 'Discuss early screening and recommend autism screening by physician', italics: true }] }] }],
                                        [{ columns: [{ width: 25, alignment: 'right', stack: [{ image: reportData[298][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: '\bRefer to physician for Autism Screening ' }, { text: 'OR ', bold: true }] }] }],
                                        [{ columns: [{ width: 25, alignment: 'right', stack: [{ image: reportData[299][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: '\bECI provider will complete M-CHAT ' }] }] }],
                                        [{ columns: [{ width: 87, text: 'Date M-CHAT completed:' }, { text: '' + reportData[107][17], alignment: 'left' }, { text: 'Results:\b' + reportData[108][17], margin: [-40, 0, 0, 0] }], margin: [0, 3, 0, 0] }],
                                        [{ columns: [{ width: 10, stack: [{ image: reportData[109][17] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth }] }, { text: [{ text: 'Referral for autism ', bold: true }, { text: 'not ', italics: true }, { text: 'indicated at this time.', bold: true }] }], fontSize: 9 }],
                                    ]
                                },
                                style: 'body',
                                layout: {
                                    hLineColor: function(i, node) {
                                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    vLineColor: function(i, node) {
                                        return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                    },
                                    paddingLeft: function(i, node) { return 2; },
                                    paddingRight: function(i, node) { return 0; },
                                    paddingTop: function(i, node) { return 0; },
                                    paddingBottom: function(i, node) { return 0; }
                                }
                            },
                        ]
                    ]
                },
                layout: {
                    paddingLeft: function(i, node) { return 2; },
                    paddingRight: function(i, node) { return 0; },
                    paddingTop: function(i, node) { return 1; },
                    paddingBottom: function(i, node) { return 0; }
                }
            },


            //ELLIPSES
            //BIRTH
            { canvas: [{ type: reportData[308][17] ? 'ellipse' : 'blank', x: 282, y: -629, r1: 15, r2: 6, }] }, //*AI 1
            { canvas: [{ type: reportData[310][17] ? 'ellipse' : 'blank', x: 417, y: -629, r1: 15, r2: 6, }] }, //*GM 8
            { canvas: [{ type: reportData[313][17] ? 'ellipse' : 'blank', x: 479, y: -629, r1: 15, r2: 6, }] }, //*AM 1
            { canvas: [{ type: reportData[316][17] ? 'ellipse' : 'blank', x: 519, y: -629, r1: 15, r2: 6, }] }, //*AM 5
            { canvas: [{ type: reportData[314][17] ? 'ellipse' : 'blank', x: 479, y: -617, r1: 15, r2: 6, }] }, //*AM 2
            { canvas: [{ type: reportData[315][17] ? 'ellipse' : 'blank', x: 479, y: -605, r1: 15, r2: 6, }] }, //*AM 3
            { canvas: [{ type: reportData[317][17] ? 'ellipse' : 'blank', x: 519, y: -617, r1: 15, r2: 6, }] }, //*PC 2

            //12 MONTHS
            { canvas: [{ type: reportData[321][17] ? 'ellipse' : 'blank', x: 350, y: -581, r1: 15, r2: 6, }] }, //RC 9
            { canvas: [{ type: reportData[323][17] ? 'ellipse' : 'blank', x: 422, y: -581, r1: 15, r2: 6, }] }, //FM 6
            { canvas: [{ type: reportData[325][17] ? 'ellipse' : 'blank', x: 479, y: -581, r1: 15, r2: 6, }] }, //*AM 7
            { canvas: [{ type: reportData[361][17] ? 'ellipse' : 'blank', x: 422, y: -569, r1: 15, r2: 6, }] }, //FM 9
            { canvas: [{ type: reportData[326][17] ? 'ellipse' : 'blank', x: 483, y: -569, r1: 15, r2: 6, }] }, //AM 11

            //24 MONTHS
            { canvas: [{ type: reportData[329][17] ? 'ellipse' : 'blank', x: 284, y: -545, r1: 15, r2: 6, }] }, //*SR 8
            { canvas: [{ type: reportData[332][17] ? 'ellipse' : 'blank', x: 352, y: -545, r1: 16, r2: 6, }] }, //*RC 12
            { canvas: [{ type: reportData[334][17] ? 'ellipse' : 'blank', x: 417, y: -545, r1: 15, r2: 6, }] }, //*PM 1
            { canvas: [{ type: reportData[338][17] ? 'ellipse' : 'blank', x: 483, y: -545, r1: 15, r2: 6, }] }, //AM 13
            { canvas: [{ type: reportData[341][17] ? 'ellipse' : 'blank', x: 521, y: -545, r1: 15, r2: 6, }] }, //PC 5
            { canvas: [{ type: reportData[330][17] ? 'ellipse' : 'blank', x: 289, y: -533, r1: 15, r2: 6, }] }, //SR 12
            { canvas: [{ type: reportData[335][17] ? 'ellipse' : 'blank', x: 420, y: -533, r1: 15, r2: 6, }] }, //PM 2
            { canvas: [{ type: reportData[339][17] ? 'ellipse' : 'blank', x: 481, y: -533, r1: 16, r2: 6, }] }, //*AM 14
            { canvas: [{ type: reportData[342][17] ? 'ellipse' : 'blank', x: 521, y: -533, r1: 15, r2: 6, }] }, //PC 6
            { canvas: [{ type: reportData[336][17] ? 'ellipse' : 'blank', x: 420, y: -520, r1: 15, r2: 6, }] }, //PM 3
            { canvas: [{ type: reportData[340][17] ? 'ellipse' : 'blank', x: 481, y: -520, r1: 15, r2: 6, }] }, //RA 3

            //36 MONTHS
            { canvas: [{ type: reportData[345][17] ? 'ellipse' : 'blank', x: 287, y: -497, r1: 15, r2: 6, }] }, //AI 17
            { canvas: [{ type: reportData[347][17] ? 'ellipse' : 'blank', x: 401, y: -497, r1: 15, r2: 6, }] }, //GM 33
            { canvas: [{ type: reportData[349][17] ? 'ellipse' : 'blank', x: 433, y: -497, r1: 15, r2: 6, }] }, //FM 13
            { canvas: [{ type: reportData[356][17] ? 'ellipse' : 'blank', x: 476, y: -497, r1: 15, r2: 6, }] }, //RA 4
            { canvas: [{ type: reportData[348][17] ? 'ellipse' : 'blank', x: 401, y: -485, r1: 15, r2: 6, }] }, //GM 35
            { canvas: [{ type: reportData[350][17] ? 'ellipse' : 'blank', x: 433, y: -485, r1: 15, r2: 6, }] }, //FM 21
            { canvas: [{ type: reportData[357][17] ? 'ellipse' : 'blank', x: 476, y: -485, r1: 15, r2: 6, }] }, //PC 16
            { canvas: [{ type: reportData[351][17] ? 'ellipse' : 'blank', x: 433, y: -472, r1: 15, r2: 6, }] }, //PM 6
            { canvas: [{ type: reportData[352][17] ? 'ellipse' : 'blank', x: 433, y: -460, r1: 15, r2: 6, }] }, //PM 7
            { canvas: [{ type: reportData[353][17] ? 'ellipse' : 'blank', x: 433, y: -448, r1: 15, r2: 6, }] }, //PM 8
            { canvas: [{ type: reportData[358][17] ? 'ellipse' : 'blank', x: 476, y: -472, r1: 15, r2: 6, }] }, //PC 18
        ],
        styles: {
            header: {
                fontSize: 12,
                bold: true,
                alignment: 'center',
            },
            h1: {
                fontSize: 11,
                bold: true,
            },
            h2: {
                fontSize: 10,
                bold: true,
            },
            body: {
                fontSize: 7.5
            },

            smallHeader: {
                fontSize: 8,
                bold: true,
                alignment: 'center'
            },
            blackHeader: {
                fontSize: 9,
                bold: true,
                alignment: 'center',
                fillColor: 'black',
                color: 'white'
            },
            referCols: {
                margin: [0, 0, 0, 0],
                alignment: 'center'
            },
            crosswalkBody: {
                fontSize: 7,
                bold: true
            },
        },
        pageMargins: [25, 15, 25, 20],

        images: {
            star: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAwAAAANCAYAAACdKY9CAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAFpSURBVChTdZG9qsJAEIVHTOwsRNIEiVhYaJEyYGEVCwshT6GVINhq4ePYiIJiIRYiiJWWNoqNaIqAiGghJB6zk3i5P96vmZ3ZnbNndgn/0Ov1UK1Wee26LkfBx4bxeAxFUVAoFDh/Pp8cBR8bYrEYzuczotEobrdbWA2QttstzedzWi6XpKoqdbtdajQalEgkqFgsUiQSoR/IsoxarQb/IFuZTCahFpDL5TAajcIsgI7HI/sVQ755e348HiiVStB1HY7jcI1nED79y3C5XLj4m+FwiEwmg36/HzR4ngfDMHjzO4PBAJVKBZZlIZ/Psyg3dDodNJtNPnS9XrHZbHidSqVYdb1ew7ZtrnGD8LlarVgxHo/DNE3ezGazX82z2Yyj5P8i7XY7arfbdDgcyL+BNE2j/X5PkiSRPzi/ZjKZ5EjCf7lcxnQ6ZQXBYrGAfwD1eh2n0ymsBvz5aSEgaLVaSKfTuN/vnAcAL7rq038epq0jAAAAAElFTkSuQmCC',
            check: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACLSURBVDhPxdDRDYAgDEVR5mIg52EalmEYLOVhCoItMdH7pYnHFlx+0ac4Hs6HhJcdTFBIyooLdEfEG7LgFDzJbmZNxVXehnIK5m1nQ7knXOV8KLfEWHc1lAMeT2ahwPi0xNxGxdrtBz5EPK3P2hJnxv0gnfYXJrZXN+Ykvobb6IhJW2FpwHv9hXM+AcFlMWf0Jh7VAAAAAElFTkSuQmCC',
            checkedbox: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABMAAAAUCAIAAADgN5EjAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAADPSURBVDhPnZHBEYMgEEUhtZAmtAJyShV4JJfcLAKPyS1NaAVJE9rLZl0WBieQQd9F2eEB+1cCgDjEib/7OW4KfC3A7Bpe1nP4TpOYjZvpAf8YDW/ed+fUyctTmNHr1WbQ4KF9oc70GnpBQyrMZWhXDWNIvF8Tt0mkHRYuTN359lm9l1VcYigymicemsyVkg5rbC+BEjJbM/7Tdpf1ymbiErEc8WYuIWX7OPBMf0w+W331atkrTkXfncPu3kWvPE9lbTq8HNTzNpMqsgnVIMQXsjm8fOdmR6IAAAAASUVORK5CYII=',
            uncheckedbox: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAABPSURBVDhPY/z//z8DuYAJSpMFKNLMAHT2//+3J1hBucQCqwm3//+nlrPTtoFdQRhsS4PqGLgAG9VMIhjVTCJA0jzLi5E44DULqoMCmxkYAJWVOGY1wYUUAAAAAElFTkSuQmCC',
            blank: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQMAAAAl21bKAAAABlBMVEX///////9VfPVsAAAACklEQVQImWNgAAAAAgAB9HFkpgAAAABJRU5ErkJggg==',
            blackstar: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABcAAAAXCAIAAABvSEP3AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACkSURBVDhPvdHREYAgDANQ53Ig52EalmEYFESgNKboeeZPhHclLPGL/KT4nPJxF0vx23JkMxhDORGT4cqFWAxVGmIwTOkRzhBFIpRJyrj9WRJeZwluLcvTWV0oh8WNnkjifqqXGUk1BNtlRcGOocLmmVb4pRADFKOZ9jItWpFIPgOWZJTSnZDbW+WaGZULQXNXSf0clOBS0DPUJGlkQLsv8oUS4w76jnRuPfNlHwAAAABJRU5ErkJggg==',
            naNoVision: 'data:image/png;base64,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'

        }

    };
}