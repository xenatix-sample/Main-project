function RiskFactorsReportModel(userData) {
    var defaultData = {
        childName: '',
        screeningDate: '',
        10: { 1: false },
        11: { 7: false, 8: false, 9: false, 11: false, 12: false }, //In utero check boxes
        12: { 1: false },
        13: { 1: false },
        14: { 1: false },
        15: { 1: false },
        16: { 1: false },
        17: { 1: false },
        18: { 1: false },
        19: { 1: false },
        20: 0, //IVH Grade boxes
        21: { 1: false },
        22: { 1: false },
        23: { 1: false },
        24: { 1: false },
        25: { 1: false },
        26: { 1: false },
        28: { 1: false },
        32: { 1: false },
        33: { 1: false },
        34: { 1: false },
        35: { 1: false },
        36: { 1: false },
        37: { 1: false },
        38: { 1: false },
        39: { 1: false },
        40: { 1: false },
        41: { 1: false },
        42: { 1: false },
        43: { 1: false },
        45: { 1: false },
        46: { 1: false },
        47: { 1: false },
        48: { 1: false },
        49: { 1: false },
        50: { 1: false },
        51: { 1: false },
        52: { 1: false },
        54: { 1: false },
        55: { 1: false },
        56: { 1: false },
        57: { 1: false },
        58: { 1: false },
        59: { 1: false },
        60: { 1: false },
        61: { 1: false },
        62: { 1: false },
        63: { 1: false },
        64: { 1: false },
        65: { 1: false },
        66: { 1: false },
        67: { 1: false },
        68: { 1: false }
    };

    var reportData = $.extend(true, {}, defaultData, userData);

    //IMAGE WIDTH DEFAULTS
    var checkWidth = 7;
    var starWidth = 7;
    var checkboxWidth = 7;

    return {
        content: [
            {
                table: {
                    widths: [80, 250, 30, 150],
                    body: [
                        ['Child\'s Name:  ', reportData.childName, 'Date:  ', reportData.screeningDate]
                    ]
                },
                fontSize: 10,
                layout: 'noBorders'
            },
            {
                //HEADER TABLE
                table: {
                    widths: '*',
                    body: [
                        [{ style: 'header', alignment: 'center', text: 'Risk Factors Checklist for Vision, Hearing, and Nutrition' }],
                        [
                            {
                                ol: [
                                    'Review all available medical records to identify any of the risk factors listed below.  Check any conditions documented in records.\n OR ',
                                    'If medical records do not contain complete information, or in the absence of medical records, interview parent about the child’s history.  Check any conditions reported by parent and request applicable medical records.',
                                    'If no risk factors are present, draw a line through the entire column.'
                                ]

                            }
                        ],
                        [
                            {
                                table: {
                                    widths: ['auto', '*'],
                                    body: [
                                        [{ image: 'check', width: 8 }, { bold: true, text: 'A checked item indicates a need for referral for further evaluation in the area specified to the right – no further screening is needed in that area.' }]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ],
                        [
                            {
                                table: {
                                    widths: ['auto', '*'],
                                    body: [
                                        [{ image: 'star', width: 8 }, { bold: true, text: 'Indicates the item is a late onset risk factor:  Review hearing &/or vision status at the 6-month review & annually. ' }]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ],
                    ]

                },
                fontSize: 8.5,
                layout: {
                    hLineColor: function(i, node) {
                        return (i === 0 || i === node.table.body.length) ? 'black' : 'white'; //OUTSIDE, INSIDE VERT LINES
                    },
                    vLineColor: function(i, node) {
                        return (i === 0 || i === node.table.widths.length) ? 'black' : 'white'; //OUTSIDE, INSIDE HORIZ LINES
                    },
                    paddingLeft: function(i, node) { return 10; },
                    paddingRight: function(i, node) { return 10; },
                    paddingTop: function(i, node) { return 0; },
                    paddingBottom: function(i, node) { return 0; }
                }


            }, //END HEADER TABLE
            //Risk Factors Table
            {
                table: {
                    widths: [320, '*', '*', '*', '*'],
                    body: [
                        [{ margin: [0, 7, 0, 0], style: 'header', text: 'Risk Factor' }, { style: 'smallHeader', text: 'Risk\nFactor\nPresent' }, { style: 'smallHeader', text: 'Refer for\nHearing\nEvaluation' }, { style: 'smallHeader', text: 'Refer for\nVision\nEvaluation' }, { style: 'smallHeader', text: 'Refer for\nNutrition\nEvaluation' }],
                        [{ style: 'blackHeader', text: 'AT OR NEAR BIRTH', colSpan: 5 }],
                        [
                            [ //SUB TABLE
                                {
                                    table: {
                                        body: [
                                            ['In utero infections (TORCH):', { bold: true, italics: true, margin: [5, 0, 0, 0], text: 'Check all that apply:' }, { margin: [10, 0, 0, 0], image: reportData[11][7] ? 'checkedbox' : 'uncheckedbox', width: checkboxWidth }, 'Toxoplasmosis', { margin: [10, 0, 0, 0], image: reportData[11][8] ? 'checkedbox' : 'uncheckedbox', width: checkboxWidth }, 'Herpes']
                                        ]
                                    },
                                    fontSize: 7.5,
                                    layout: {
                                        hLineColor: function(i, node) {
                                            return 'white'; //OUTSIDE, INSIDE VERT LINES
                                        },
                                        vLineColor: function(i, node) {
                                            return 'white'; //OUTSIDE, INSIDE HORIZ LINES
                                        },
                                        paddingLeft: function(i, node) { return 0; },
                                        paddingRight: function(i, node) { return 0; },
                                        paddingTop: function(i, node) { return 0; },
                                        paddingBottom: function(i, node) { return 0; }
                                    }
                                }, //END SUB TABLE
                                //SUB TABLE
                                {
                                    table: {
                                        body: [
                                            [{ image: reportData[11][9] ? 'checkedbox' : 'uncheckedbox', width: checkboxWidth }, '(Maternal) Rubella', { margin: [10, 0, 0, 0], image: reportData[11][11] ? 'checkedbox' : 'uncheckedbox', width: checkboxWidth }, 'Cytomegalovirus (CMV)', { image: 'star', width: starWidth }, { margin: [10, 0, 0, 0], image: reportData[11][12] ? 'checkedbox' : 'uncheckedbox', width: checkboxWidth }, 'Other (e.g., Syphilis)']
                                        ]
                                    },
                                    fontSize: 7.5,
                                    layout: {
                                        hLineColor: function(i, node) {
                                            return 'white'; //OUTSIDE, INSIDE VERT LINES
                                        },
                                        vLineColor: function(i, node) {
                                            return 'white'; //OUTSIDE, INSIDE VERT LINES
                                        },
                                        paddingLeft: function(i, node) { return 0; },
                                        paddingRight: function(i, node) { return 0; },
                                        paddingTop: function(i, node) { return 0; },
                                        paddingBottom: function(i, node) { return 0; }
                                    }
                                }
                            ], //END SUB TABLE
                            { style: 'referCols', image: reportData[10][1] ? 'check' : 'blank', width: checkWidth, margin: [0, 5, 0, 0] }, { margin: [0, 5, 0, 0], style: 'referCols', image: 'check', width: checkWidth }, { margin: [0, 5, 0, 0], style: 'referCols', image: 'check', width: checkWidth }, { margin: [0, 5, 0, 0], style: 'referCols', text: '' }
                        ],
                        [{ text: 'Albinism, glaucoma, congenital cataracts, or retinoblastoma (or family history of one of these hereditary vision impairments)' }, { margin: [0, 5, 0, 0], style: 'referCols', image: reportData[12][1] ? 'check' : 'blank', width: checkWidth }, { margin: [0, 5, 0, 0], style: 'referCols', text: '' }, { margin: [0, 5, 0, 0], style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [{ text: 'Anomalies of the face, head and neck including the outer ear, ear canal, and Eustachian tube' }, { style: 'referCols', image: reportData[13][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'Anoxia (at birth or any time in history)' }, { style: 'referCols', image: reportData[14][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'APGAR score of 0-4 at one minute, or 0-6 at five minutes' }, { style: 'referCols', image: reportData[15][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [{ text: 'Birth trauma' }, { style: 'referCols', image: reportData[16][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [{ text: 'CHARGE Syndrome' }, { style: 'referCols', image: reportData[17][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Hypoxia (at birth or any time in history)' }, { style: 'referCols', image: reportData[18][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [
                            [
                                {
                                    table: {
                                        body: [
                                            [{ text: 'Intraventricular hemorrhage (IVH); stroke – Grade (Check one):' }, { image: reportData[20] == 13 ? 'checkedbox' : 'uncheckedbox', width: checkboxWidth }, 'I', { margin: [5, 0, 0, 0], image: reportData[20] == 14 ? 'checkedbox' : 'uncheckedbox', width: checkboxWidth }, 'II', { margin: [5, 0, 0, 0], image: reportData[20] == 15 ? 'checkedbox' : 'uncheckedbox', width: checkboxWidth }, 'III', { margin: [5, 0, 0, 0], image: reportData[20] == 16 ? 'checkedbox' : 'uncheckedbox', width: checkboxWidth }, 'IV']
                                        ]
                                    },
                                    fontSize: 7.5,
                                    layout: {
                                        hLineColor: function(i, node) {
                                            return 'white'; //OUTSIDE, INSIDE VERT LINES
                                        },
                                        vLineColor: function(i, node) {
                                            return 'white'; //OUTSIDE, INSIDE VERT LINES
                                        },
                                        paddingLeft: function(i, node) { return 0; },
                                        paddingRight: function(i, node) { return 0; },
                                        paddingTop: function(i, node) { return 0; },
                                        paddingBottom: function(i, node) { return 0; }
                                    }
                                }
                            ], //END SUB TABLE
                            { style: 'referCols', image: reportData[19][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }
                        ],
                        [{ text: 'Jaundice (hyperbilirubinemia) requiring transfusion' }, { style: 'referCols', image: reportData[21][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [{ text: 'Low birth weight (<3.3 pounds or 1500 grams)' }, { style: 'referCols', image: reportData[22][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Mechanical ventilation for 5 days or more' }, { style: 'referCols', image: reportData[23][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'Ototoxic medications (these would be used for serious illness that was present at birth, e.g., kidney malfunction or severely premature infant)' }, { image: reportData[24][1] ? 'check' : 'blank', width: checkWidth, margin: [0, 5, 0, 0], style: 'referCols' }, { margin: [0, 5, 0, 0], style: 'referCols', image: 'check', width: checkWidth }, { margin: [0, 5, 0, 0], style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'Pre-maturity <32 weeks, with oxygen' }, { style: 'referCols', image: reportData[25][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [{ text: 'Spina Bifida' }, { style: 'referCols', image: reportData[26][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ style: 'blackHeader', text: 'OTHER HISTORY AND PHYSICAL SYNDROMES OR CONDITIONS', colSpan: 5 }],
                        [{ columns: [{ alignment: 'left', text: 'Bacterial Meningitis', width: 70 }, { image: 'star', width: starWidth - 1 }] }, { style: 'referCols', image: reportData[28][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'Bronchopulmonary dysplasia' }, { style: 'referCols', image: reportData[32][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Cancer' }, { style: 'referCols', image: reportData[33][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Cardiovascular disease' }, { style: 'referCols', image: reportData[34][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Cerebral Palsy' }, { style: 'referCols', image: reportData[35][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Cleft lip/palate' }, { style: 'referCols', image: reportData[36][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Cystic Fibrosis' }, { style: 'referCols', image: reportData[37][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Decubitus Ulcer' }, { style: 'referCols', image: reportData[38][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Diabetes' }, { style: 'referCols', image: reportData[39][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Difficulty sucking, swallowing, or chewing; gagging' }, { style: 'referCols', image: reportData[40][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Down Syndrome' }, { style: 'referCols', image: reportData[41][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Failure to Thrive' }, { style: 'referCols', image: reportData[42][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ columns: [{ alignment: 'left', text: 'Family history of permanent childhood sensori-neural hearing loss', width: 225 }, { image: 'star', width: starWidth - 1 }] }, { style: 'referCols', image: reportData[43][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'Fetal Alcohol Syndrome' }, { style: 'referCols', image: reportData[45][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [{ text: 'Goldenhar' }, { style: 'referCols', image: reportData[46][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'Head trauma associated with loss of consciousness or skull fracture' }, { style: 'referCols', image: reportData[47][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [{ text: 'Hydrocephalus' }, { style: 'referCols', image: reportData[48][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [{ text: 'Hypertension' }, { style: 'referCols', image: reportData[49][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Inborn error of metabolism' }, { style: 'referCols', image: reportData[50][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Intrauterine drug and/or alcohol exposure' }, { style: 'referCols', image: reportData[51][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [{ text: 'Juvenile Rheumatoid Arthritis' }, { style: 'referCols', image: reportData[52][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [{ text: 'Liver Disease' }, { style: 'referCols', image: reportData[54][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Malabsorption syndromes' }, { style: 'referCols', image: reportData[55][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Meningitis' }, { style: 'referCols', image: reportData[56][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [{ columns: [{ alignment: 'left', text: 'Neurodegenerative disorder', width: 96 }, { image: 'star', width: starWidth - 1 }] }, { style: 'referCols', image: reportData[57][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'Prader Willi' }, { style: 'referCols', image: reportData[58][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Recurrent persistent otitis media with effusion for at least three months' }, { style: 'referCols', image: reportData[59][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'Renal disease (e.g., Wilms tumor, nephritic Syndrome)' }, { style: 'referCols', image: reportData[60][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Rickets' }, { style: 'referCols', image: reportData[61][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Seizure disorder' }, { style: 'referCols', image: reportData[62][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }],
                        [{ text: 'Shaken Baby Syndrome' }, { style: 'referCols', image: reportData[63][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }],
                        [{ text: 'Treacher Collins' }, { style: 'referCols', image: reportData[64][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'Trisomy 18' }, { style: 'referCols', image: reportData[65][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'Usher\'s' }, { style: 'referCols', image: reportData[66][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'Waardenburg' }, { style: 'referCols', image: reportData[67][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', image: 'check', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }],
                        [{ text: 'Other conditions with known risk factor – refer as appropriate (see TA document)' }, { style: 'referCols', image: reportData[68][1] ? 'check' : 'blank', width: checkWidth }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }, { style: 'referCols', text: '' }]
                    ]
                },
                style: 'body',
                layout: {
                    paddingLeft: function(i, node) { return 5; },
                    paddingRight: function(i, node) { return 5; },
                    paddingTop: function(i, node) { return 1; },
                    paddingBottom: function(i, node) { return 0; }
                }
            } //END RISK FACTORS TABLE
        ],
        styles: {
            header: {
                fontSize: 12,
                bold: true,
                alignment: 'center'
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
            }
        },
        pageMargins: [25, 20, 25, 20],

        images: {
            star: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAwAAAANCAYAAACdKY9CAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAFpSURBVChTdZG9qsJAEIVHTOwsRNIEiVhYaJEyYGEVCwshT6GVINhq4ePYiIJiIRYiiJWWNoqNaIqAiGghJB6zk3i5P96vmZ3ZnbNndgn/0Ov1UK1Wee26LkfBx4bxeAxFUVAoFDh/Pp8cBR8bYrEYzuczotEobrdbWA2QttstzedzWi6XpKoqdbtdajQalEgkqFgsUiQSoR/IsoxarQb/IFuZTCahFpDL5TAajcIsgI7HI/sVQ755e348HiiVStB1HY7jcI1nED79y3C5XLj4m+FwiEwmg36/HzR4ngfDMHjzO4PBAJVKBZZlIZ/Psyg3dDodNJtNPnS9XrHZbHidSqVYdb1ew7ZtrnGD8LlarVgxHo/DNE3ezGazX82z2Yyj5P8i7XY7arfbdDgcyL+BNE2j/X5PkiSRPzi/ZjKZ5EjCf7lcxnQ6ZQXBYrGAfwD1eh2n0ymsBvz5aSEgaLVaSKfTuN/vnAcAL7rq038epq0jAAAAAElFTkSuQmCC',
            check: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACLSURBVDhPxdDRDYAgDEVR5mIg52EalmEYLOVhCoItMdH7pYnHFlx+0ac4Hs6HhJcdTFBIyooLdEfEG7LgFDzJbmZNxVXehnIK5m1nQ7knXOV8KLfEWHc1lAMeT2ahwPi0xNxGxdrtBz5EPK3P2hJnxv0gnfYXJrZXN+Ykvobb6IhJW2FpwHv9hXM+AcFlMWf0Jh7VAAAAAElFTkSuQmCC',
            checkedbox: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABMAAAAUCAIAAADgN5EjAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAADPSURBVDhPnZHBEYMgEEUhtZAmtAJyShV4JJfcLAKPyS1NaAVJE9rLZl0WBieQQd9F2eEB+1cCgDjEib/7OW4KfC3A7Bpe1nP4TpOYjZvpAf8YDW/ed+fUyctTmNHr1WbQ4KF9oc70GnpBQyrMZWhXDWNIvF8Tt0mkHRYuTN359lm9l1VcYigymicemsyVkg5rbC+BEjJbM/7Tdpf1ymbiErEc8WYuIWX7OPBMf0w+W331atkrTkXfncPu3kWvPE9lbTq8HNTzNpMqsgnVIMQXsjm8fOdmR6IAAAAASUVORK5CYII=',
            uncheckedbox: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAABPSURBVDhPY/z//z8DuYAJSpMFKNLMAHT2//+3J1hBucQCqwm3//+nlrPTtoFdQRhsS4PqGLgAG9VMIhjVTCJA0jzLi5E44DULqoMCmxkYAJWVOGY1wYUUAAAAAElFTkSuQmCC',
            blank: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQMAAAAl21bKAAAABlBMVEX///////9VfPVsAAAACklEQVQImWNgAAAAAgAB9HFkpgAAAABJRU5ErkJggg=='
        }
    };
}
