function IFSPReportModel(userData) {
    var AION = {
        0: '',
        68: 'A',
        69: 'O',
        70: 'I',
        71: 'N'
    };
    var defaultData = {
        childName: '',
        childDOB: '',
        clientID: '',
        ifspDate: '',
        parentGuardian: '',
        //PAGE 1//
        119: { 17: false },
        120: { 17: false },
        121: { 17: false },
        122: { 44: false, 43: false },
        123: { 17: false },
        125: { 17: false },
        127: { 17: false },

        //PAGE 2
        137: { 72: false, 73: false, 75: false },
        138: 0,
        139: { 17: '' },
        140: { 72: false, 73: false, 75: false },
        141: 0,
        142: { 17: '' },
        143: { 72: false, 73: false, 75: false },
        144: 0,
        145: { 17: '' },
        148: { 72: false, 73: false, 75: false },
        149: 0,
        150: { 17: '' },

        //PAGE 3
        151: { 72: false, 73: false, 75: false },
        152: 0,
        153: { 17: '' },
        154: { 72: false, 73: false, 75: false },
        155: 0,
        156: { 17: '' },
        159: { 72: false, 73: false, 75: false },
        160: 0,
        161: { 17: ''},
        162: { 72: false, 73: false, 75: false },
        163: 0,
        164: { 17: '' },

        //PAGE 4
        165: { 72: false, 73: false, 75: false },
        166: 0,
        167: { 17: '' },
        170: { 72: false, 73: false, 75: false },
        171: 0,
        172: { 17: '' },
        173: { 72: false, 73: false, 75: false },
        174: 0,
        175: { 17: '' },
        176: { 72: false, 73: false, 75: false },
        177: 0,
        178: { 17: '' },

        //PAGE 5
        179: { 72: false, 73: false, 75: false },
        180: 0,
        181: { 17: '' },
        182: { 72: false, 73: false, 75: false },
        183: 0,
        184: { 17: '' },
        187: { 72: false, 73: false, 75: false },
        188: 0,
        189: { 17: '' },
        190: { 72: false, 73: false, 75: false },
        191: 0,
        192: { 17: '' },
        193: { 17: '' },

        //PAGE 6
        200: { 76: false, 77: false, 78: false, 79: false },
        201: { 17: '' },
        202: { 76: false, 77: false, 78: false, 79: false },
        203: { 17: '' },
        204: { 76: false, 77: false, 78: false, 79: false },
        205: { 17: '' },
        206: { 76: false, 77: false, 78: false, 79: false },
        207: { 17: '' },
        208: { 76: false, 77: false, 78: false, 79: false },
        209: { 17: '' },
        210: { 76: false, 77: false, 78: false, 79: false },
        211: { 17: '' },
        212: { 76: false, 77: false, 78: false, 79: false },
        213: { 17: '' },
        214: { 76: false, 77: false, 78: false, 79: false },
        215: { 17: '' },
        216: { 76: false, 77: false, 78: false, 79: false },
        217: { 17: '' },
        220: { 76: false, 77: false, 78: false, 79: false },
        221: { 17: '' },
        222: { 76: false, 77: false, 78: false, 79: false },
        223: { 17: '' },
        224: { 76: false, 77: false, 78: false, 79: false },
        225: { 17: '' },
        226: { 76: false, 77: false, 78: false, 79: false },
        227: { 17: '' },
        230: { 76: false, 77: false, 78: false, 79: false },
        231: { 17: '' },
        232: { 76: false, 77: false, 78: false, 79: false },
        233: { 17: '' },
        234: { 76: false, 77: false, 78: false, 79: false },
        235: { 17: '' },
        236: { 76: false, 77: false, 78: false, 79: false },
        237: { 17: '' },
        238: { 76: false, 77: false, 78: false, 79: false },
        239: { 17: '' },
        240: { 76: false, 77: false, 78: false, 79: false },
        242: { 17: '' },
        243: { 76: false, 77: false, 78: false, 79: false },
        244: { 17: '' },
        245: { 76: false, 77: false, 78: false, 79: false },
        246: { 17: '' },
        247: { 76: false, 77: false, 78: false, 79: false },
        248: { 17: '' },
        249: { 76: false, 77: false, 78: false, 79: false },
        250: { 17: '' },

        //PAGE 7
        359: { 17: false},
        253: { 17: '' },
        255: { 17: '' },
        256: { 17: '' },
        258: { 80: false, 81: false, 82: false, 83: false, 86: false },
        360: { 17: '' },
        261: { 17: '' },
    };

    var reportData = $.extend(true, {}, defaultData, userData);

    var checkedboxWidth = 10; //SETS SIZE OF CHECKED BOX ICONS 
    var uncheckedboxWidth = 10; //SETS SIZE OF UNCHECKED BOX ICONS 
    var checkWidth = 15;
    var smallcheckWidth = 10;

//Column widths for Evaluation Questions 
    var leftImgColWidth = 36;
    var leftColWidth = 35;
    var rightColWidth = 15;

//Column widths for Child Resources and Cse Management Needs Evaluations 
    var leftImgColWidthSmall = 25;

//Row Heights for text areas 
    var txtAreaRowHeight = 100;
    var txtAreaRowHeightSmall = 80;

    return {
        header: function(page, pages) {
            if (page !== 1) {
                return [
                    {
                        columns: [
                            { stack: [{ text: 'Child\s Name:' }, { text: '' + 'Client ID: ' }], width: 80 },
                            { stack: [{ alignment: 'left', text: ' ' + reportData.childName }, { text: ' ' + reportData.clientID }], width: 330 },
                            { stack: [{ text: '\n' }, { text: 'IFSP Date: ', alignment: 'right' }] },
                            { stack: [{ text: 'Page ' + page + ' of ' + pages, alignment: 'right' }, { text: reportData.ifspDate, alignment: 'right' }] }
                        ],
                        margin: [25, 20, 25, 0],
                    }
                ];
            }
        },


/*
    { 
        table:  { 
                widths: ['auto','*','auto','auto'], 
                body:   [ 
                            [{alignment: 'left', text:'Child\s Name: '},childName, {alignment: 'right', text: 'Page 2 of'}, {alignment: 'left',text: ''}], 
                            [{alignment: 'left', text:'Client ID: '},clientID, {alignment: 'right', text: 'IFSP Date'}, ifspDate] 
                        ] 
                     
                }, layout: 'noBorders' 
    }, 
    */
        content: [

            //BEGIN PAGE 1 
            //HEADER TABLE 
            {
                table: {
                    widths: ['*', 'auto', '*'],
                    body: [
                        [{ image: 'ecilogo', width: 100 }, { margin: [0, 33, 0, 0], style: 'h1', text: 'Individualized Family Service Plan (IFSP)' }, '']
                    ]

                },
                layout: 'noBorders'
            },
            //END HEADER TABLE 

            //MAIN CONTAINER TABLE 
            {
                table: {
                    widths: '*',
                    body: [
                        //CHILD AND FAMILY SECTION 
                        [{ style: 'h1gray', text: 'Child and Family Information' }],
                        [
                            {
                                table: {
                                    widths: ['auto', 230, 'auto', '*'],
                                    body: [
                                        ['Child\'s Name: ', reportData.childName, { alignment: 'right', text: 'Client ID:' }, reportData.clientID],
                                        ['Date of Birth: ', reportData.childDOB, { alignment: 'right', text: 'IFSP Date:' }, reportData.ifspDate],
                                        ['Parent/Guardian: ', reportData.parentGuardian, '', '']
                                    ]
                                },
                                layout: 'noBorders'


                            }
                        ],
                        //END CHILD AND FAMILY SECTION 

                        //TRANSITION INFORMATION SECTION 
                        [{ style: 'h2', text: 'Transition Information' }],
                        [
                            {
                                table: {
                                    widths: ['*'],
                                    body: [
                                        [
                                            {
                                                ul: [
                                                    'Planning for transition will help you and your child move smoothly from ECI to whatever comes next for your child.  Options after early intervention might include:  Head Start, childcare, pre-kindergarten or early childhood special education through the public schools (PPCD).',
                                                    'Transition occurs at different times depending on the needs and circumstances of the family.  It can occur when you move to another service area within Texas or out of state, when your child no longer meets eligibility requirements for ECI, or when your child turns three.  Your service coordinator will help you plan for any of these transitions and develop outcomes and procedures to address them.',
                                                    'After your child\'s second birthday but no later than 30 months of age, you and your IFSP team will develop more specific steps, procedures regarding your child\'s future transition needs.    '
                                                ],
                                                style: 'body'
                                            }
                                        ]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ],
                        //END TRANSITION INFORMATION SECTION 

                        //FUNCTIONAL ABILITIES SECTION 
                        [{ style: 'h1gray', text: ['Functional Abilities, Strengths and Needs\n', { italics: true, text: 'Present Levels of Development' }] }],
                        [{ style: 'h1', text: 'Physical Development' }],

                        //CHILD CURRENT HEALTH SUB SECTION 
                        [
                            {
                                table: {
                                    widths: ['*'],
                                    body: [
                                        [{ style: 'h2', alignment: 'left', text: 'Describe child\'s current health status and pertinent medical history:' }],
                                        [{ style: 'body', italics: true, text: 'Include any medical diagnoses, concerns about child\'s health and any relevant nutrition information.' }],
                                        [{ style: 'smallbody', text: reportData[119][17] }],
                                        [{ style: 'body', italics: true, text: 'Medications:' }],
                                        [{ style: 'smallbody', text: reportData[120][17] }],
                                        //SUB TABLE 
                                        [
                                            {
                                                table: {
                                                    widths: ['auto', 75, 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto'],
                                                    body: [
                                                        [
                                                            { style: 'body', italics: true, text: 'Date of last physical:' },
                                                            { style: 'body', text: reportData[121][17] },
                                                            { style: 'body', italics: true, text: 'Premature?' },
                                                            { image: reportData[122][44] ? 'checkedbox' : 'uncheckedbox', width: checkedboxWidth },
                                                            { style: 'body', text: 'No', margin: [0, 0, 5, 0] },
                                                            { image: reportData[122][43] ? 'checkedbox' : 'uncheckedbox', width: uncheckedboxWidth },
                                                            { style: 'body', text: 'Yes', margin: [0, 0, 0, 0] },
                                                            { style: 'body', italics: true, text: '> if yes, gestational age in weeks:' },
                                                            { style: 'body', text: reportData[123][17] }
                                                        ]
                                                    ]
                                                },
                                                layout: 'noBorders'
                                            }
                                        ]
                                        //END SUB TABLE 
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ],

                        //HEARING SUB SECTION 
                        [
                            {
                                table: {
                                    widths: [50, 'auto'],
                                    body: [
                                        [{ alignment: 'left', style: 'h2', text: 'Hearing:' }, { style: 'body', italics: true, text: 'Describe in functional terms and include any concerns about child\'s hearing' }],
                                        [{ colSpan: 2, style: 'body', text: reportData[125][17] }]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ],
                        //VISION SUB SECTION 
                        [
                            {
                                table: {
                                    widths: [50, '*'],
                                    body: [
                                        [{ alignment: 'left', style: 'h2', text: 'Vision:' }, { style: 'body', italics: true, text: 'Describe in functional terms and include any concerns about child\'s vision' }],
                                        [{ colSpan: 2, style: 'body', text: reportData[127][17] }]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ],
                        //END FUNCTIONAL ABILITIES SECTION 

                        //INSTRUCTIONS  SECTION 
                        [{ style: 'body', text: 'On the following pages describe the child\'s functional abilities within familiar activities in terms of positive social-emotional development, acquiring and using knowledge and skills, and ability to take appropriate actions to get his/her needs met.\nSummarizing how the child uses skills to function in his/her daily life provides information that assists the team (including the parents) in developing functional IFSP outcomes, and procedures to meet these outcomes, so that progress can be monitored over time.' }],
                        [{ margin: [15, 0, 0, 0], style: 'body', text: 'a.  Check the appropriate box to note whether the skill/ability is strength, a concern or a priority.\nb.  Identify the child\'s functional abilities with the following codes:*' }],

                        //AOIN LEGEND SECTION 
                        [
                            {
                                table: {
                                    widths: 'auto',
                                    body: [
                                        [{ bold: true, text: 'A', alignment: 'center' }, '=', 'age-appropriate skills'],
                                        [{ bold: true, text: 'O', alignment: 'center' }, '=', 'occasionally age appropriate skills'],
                                        [{ bold: true, text: 'I', alignment: 'center' }, '=', 'immediate foundational skills'],
                                        [{ bold: true, text: 'N', alignment: 'center' }, '=', 'not age-appropriate or immediate foundational skills']
                                    ]
                                },
                                style: 'body',
                                layout: 'noBorders'
                            }
                        ]
                    ]
                },
                pageBreak: 'after'

            },
            //END MAIN CONTAINER TABLE 
            //END PAGE 1 


            //BEGIN PAGE 2 

            /*
    { 
        table:  { 
                widths: ['auto','*','auto','auto'], 
                body:   [ 
                            [{alignment: 'left', text:'Child\s Name: '},childName, {alignment: 'right', text: 'Page 2 of'}, {alignment: 'left',text: ''}], 
                            [{alignment: 'left', text:'Client ID: '},clientID, {alignment: 'right', text: 'IFSP Date'}, ifspDate] 
                        ] 
                     
                }, layout: 'noBorders' 
    }, 
    
    */
            //MAIN CONTAINER TABLE 
            {
                table: {
                    widths: [leftColWidth, leftColWidth, leftColWidth, '*', rightColWidth, rightColWidth, rightColWidth, rightColWidth],
                    body: [
                        //HOW YOUR DAY STARTS 
                        [{ image: 'colPositive', width: leftImgColWidth, margin: [0, 6, 0, 0] }, { image: 'colAcquiring', width: leftImgColWidth, margin: [0, 4, 0, 0] }, { image: 'colTaking', width: leftImgColWidth }, { style: 'routines', text: 'Routines' }, { image: 'colStrength', width: 13, margin: [0, 25, 0, 0] }, { image: 'colNeed', width: 12, margin: [0, 9, 0, 0] }, { image: 'colPriority', width: 13, margin: [0, 28, 0, 0] }, { image: 'colCode', width: 11, margin: [0, 34, 0, 0] }],
                        [{ colSpan: 8, style: 'h1gray', text: 'How your day starts' }],
                        //HOW DOES YOUR CHILD LET YOU KNOW HE/SHE IS AWAKE 
                        [{ style: 'xstyle', text: 'X' }, { style: 'xstyle', text: 'X' }, { style: 'xstyle', text: 'X' }, { style: 'question', text: ['How does your child let you know he/she is awake?\n', { style: 'parenthesis', text: '(cognitive, communication and social-emotional)' }] }, { image: reportData[137][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { image: reportData[137][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { image: reportData[137][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { text: AION[reportData[138]], style: 'xstyle' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[139][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeight }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],
                        //HOW DOES YOUR CHILD GET OUT OF BED 
                        [{ style: 'xstyle', text: '' }, { style: 'xstyle', text: 'X' }, { style: 'xstyle', text: 'X' }, { style: 'question', text: ['How does your child get out of bed?\n', { style: 'parenthesis', text: '(adaptive/self-help and motor)' }] }, { image: reportData[140][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { image: reportData[140][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { image: reportData[140][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { text: AION[reportData[141]], style: 'xstyle' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[142][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeight }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],
                        //IS YOUR CHILD HAPPY OR SAD WHEN HE/SHE WAKES UP 
                        [{ style: 'xstyle', text: 'X' }, { style: 'xstyle', text: 'X' }, { style: 'xstyle', text: '' }, { style: 'question', text: ['Is your child happy or sad when he/she wakes up?\n', { style: 'parenthesis', text: '(social-emotional and communication)' }] }, { image: reportData[143][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { image: reportData[143][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { image: reportData[143][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { text: AION[reportData[144]], style: 'xstyle' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[145][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeight }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],

                        //BATHING, DRESSING, DIAPERING CONTD 
                        [{ colSpan: 8, style: 'h1gray', text: 'Bathing, dressing, diapering and toileting' }],
                        [{ style: 'xstyle', text: '' }, { style: 'xstyle', text: 'X' }, { style: 'xstyle', text: 'X' }, { style: 'question', text: ['How does your child help with dressing?\n', { style: 'parenthesis', text: '(communication, adaptive/self-help and motor)' }] }, { image: reportData[148][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { image: reportData[148][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { image: reportData[148][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { text: AION[reportData[149]], style: 'xstyle' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[150][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeight }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],
                    ]
                },
                pageBreak: 'after'


            },
            //END MAIN CONTAINER TABLE 
            //END PAGE 2 

            //BEGIN PAGE 3 
            //MAIN CONTAINER TABLE 
            {
                table: {
                    widths: [leftColWidth, leftColWidth, leftColWidth, '*', rightColWidth, rightColWidth, rightColWidth, rightColWidth],
                    body: [
                        //BATHING, DRESSING, DIAPERING CONTD 
                        [{ image: 'colPositive', width: leftImgColWidth, margin: [0, 6, 0, 0] }, { image: 'colAcquiring', width: leftImgColWidth, margin: [0, 4, 0, 0] }, { image: 'colTaking', width: leftImgColWidth }, { style: 'routines', text: 'Routines' }, { image: 'colStrength', width: 13, margin: [0, 25, 0, 0] }, { image: 'colNeed', width: 12, margin: [0, 9, 0, 0] }, { image: 'colPriority', width: 13, margin: [0, 28, 0, 0] }, { image: 'colCode', width: 11, margin: [0, 34, 0, 0] }],
                        [{ colSpan: 8, style: 'h1gray', text: 'Bathing, dressing, diapering and toileting (cont.)' }],
                        //WHAT DOES BATH TIME LOOK LIKE FOR YOU AND YOUR CHILD 
                        [{ style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: '' }, { style: 'xstyle2', text: 'X' }, { style: 'question', text: ['What does bath time look like for you and your child?\nIs bath time a fun or stressful time of day?\n', { fontSize: 9.5, text: '(adaptive/self-help, cognitive, communication, motor and social-emotional)' }] }, { image: reportData[151][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[151][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[151][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { text: AION[reportData[152]], style: 'xstyle2' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[153][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeightSmall }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],
                        //HOW DOES YOUR CHILD LET YOU KNOW THAT HE/SHE NEEDS A DIAPER CHANGE 
                        [{ style: 'xstyle2', text: '' }, { style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: 'X' }, { style: 'question', text: ['How does your child let you know that he/she needs a diaper change or needs to use the toilet?\n', { style: 'parenthesis', text: '(adaptive/self-help and communication)' }] }, { image: reportData[154][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[154][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[154][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { text: AION[reportData[155]], style: 'xstyle2' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[156][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeightSmall }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],

                        //MEAL TIMES 
                        [{ colSpan: 8, style: 'h1gray', text: 'Meal times' }],
                        //WHAT DO MEAL TIMES LOOK LIKE FOR YOUR CHILD 
                        [{ style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: 'X' }, { style: 'question', text: ['What do meal times look like for your child?\nIs there anything difficult or special about meal times?\n', { style: 'parenthesis', text: '(adaptive/self-help, motor, social-emotional and communication)' }] }, { image: reportData[159][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[159][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[159][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { text: AION[reportData[160]], style: 'xstyle2' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[161][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeightSmall }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],
                        //HOW DOES YOUR CHILD LET YOU KNOW WHEN HE/SHE IS HUNGRY OR THIRSTY 
                        [{ style: 'xstyle2', text: '' }, { style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: 'X' }, { style: 'question', text: ['How does your child let you know when he/she is hungry or thirsty, what he wants and when he is finished?\n', { style: 'parenthesis', text: '(communication, adaptive/self-help and cognitive)' }] }, { image: reportData[162][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[162][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[162][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { text: AION[reportData[163]], style: 'xstyle2' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[164][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeightSmall }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],
                    ]
                },
                pageBreak: 'after'


            },
            //END MAIN CONTAINER TABLE 
            //END PAGE 3 

            //BEGIN PAGE 4 
            //MAIN CONTAINER TABLE 
            {
                table: {
                    widths: [leftColWidth, leftColWidth, leftColWidth, '*', rightColWidth, rightColWidth, rightColWidth, rightColWidth],
                    body: [
                        //MEAL TIMES CONTD 
                        [{ image: 'colPositive', width: leftImgColWidth, margin: [0, 6, 0, 0] }, { image: 'colAcquiring', width: leftImgColWidth, margin: [0, 4, 0, 0] }, { image: 'colTaking', width: leftImgColWidth }, { style: 'routines', text: 'Routines' }, { image: 'colStrength', width: 13, margin: [0, 25, 0, 0] }, { image: 'colNeed', width: 12, margin: [0, 9, 0, 0] }, { image: 'colPriority', width: 13, margin: [0, 28, 0, 0] }, { image: 'colCode', width: 11, margin: [0, 34, 0, 0] }],
                        [{ colSpan: 8, style: 'h1gray', text: 'Meal times (cont.)' }],
                        //WHAT ARE YOU CHILDS LIKES AND DISLIKES 
                        [{ style: 'xstyle', text: '' }, { style: 'xstyle', text: 'X' }, { style: 'xstyle', text: '' }, { style: 'question', text: ['What are your child\'s likes or dislikes? How do you know?\n', { style: 'parenthesis', text: '(communication and nutrition)' }] }, { image: reportData[165][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { image: reportData[165][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { image: reportData[165][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { text: AION[reportData[166]], style: 'xstyle' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[167][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeightSmall }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],


                        //PLAYTIME AND OTHER DAILY ACTIVITIES 
                        [{ colSpan: 8, style: 'h1gray', text: 'Playtime and other daily activities' }],
                        //HOW DOES YOUR CHILD PLAY 
                        [{ style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: 'X' }, { style: 'question', fontSize: 11.5, text: ['How does your child play? What does he/she like to play with? Are there times that are easier or more frustrating than others?\n', { style: 'parenthesis', text: '(cognitive, communication, motor and social-emotional)' }] }, { image: reportData[170][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[170][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[170][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { text: AION[reportData[171]], style: 'xstyle2' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[172][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeightSmall }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],
                        //DOES YOUR CHILD HAVE THE OPPORTUNITY TO BE AROUND OTHER CHILDREN? 
                        [{ style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: '' }, { style: 'question', fontSize: 10.5, text: ['Does your child have the opportunity to be around other children and adults? If yes, how and where does your child interact with them?\n', { style: 'parenthesis', text: '(cognitive, social-emotional)' }] }, { image: reportData[173][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[173][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[173][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { text: AION[reportData[174]], style: 'xstyle2' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[175][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeightSmall }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],
                        //HOW DOES YOUR CHILD ACT WHEN YOU TAKE THEM OUT IN PUBLIC 
                        [{ style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: '' }, { style: 'xstyle2', text: 'X' }, { style: 'question', fontSize: 11.5, text: ['How does your child act when you take them out in public? How does your child respond to separations and transitions?\n', { style: 'parenthesis', text: '(motor, social-emotional and communication)' }] }, { image: reportData[176][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[176][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[176][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { text: AION[reportData[177]], style: 'xstyle2' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[178][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeightSmall }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ]
                    ]
                },
                pageBreak: 'after'


            },
            //END MAIN CONTAINER TABLE 
            //END PAGE 4 

            //BEGIN PAGE 5 
            //MAIN CONTAINER TABLE 
            {
                table: {
                    widths: [leftColWidth, leftColWidth, leftColWidth, '*', rightColWidth, rightColWidth, rightColWidth, rightColWidth],
                    body: [
                        //PLAYTIME AND OTHER DAILY ACTIVITES (CONT) 
                        [{ image: 'colPositive', width: leftImgColWidth, margin: [0, 6, 0, 0] }, { image: 'colAcquiring', width: leftImgColWidth, margin: [0, 4, 0, 0] }, { image: 'colTaking', width: leftImgColWidth }, { style: 'routines', text: 'Routines' }, { image: 'colStrength', width: 13, margin: [0, 25, 0, 0] }, { image: 'colNeed', width: 12, margin: [0, 9, 0, 0] }, { image: 'colPriority', width: 13, margin: [0, 28, 0, 0] }, { image: 'colCode', width: 11, margin: [0, 34, 0, 0] }],
                        [{ colSpan: 8, style: 'h1gray', text: 'Playtime and other daily activities (cont.)' }],
                        //HOW DOES YOUR CHILD FOLLOW DIRECTIONS 
                        [{ style: 'xstyle', text: 'X' }, { style: 'xstyle', text: 'X' }, { style: 'xstyle', text: '' }, { style: 'question', text: ['How does your child follow directions? Respond to limits?', { style: 'parenthesis', text: '(cognitive, communication and social-emotional)' }] }, { image: reportData[179][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { image: reportData[179][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { image: reportData[179][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle' }, { text: AION[reportData[180]], style: 'xstyle' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[181][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeightSmall }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],
                        //ARE THERE CERTAIN DAYS THAT LOOK DIFFERENT 
                        [{ style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: '' }, { style: 'xstyle2', text: '' }, { style: 'question', text: ['Are there certain days that look different? If yes, how does your child respond to the changes?\n', { style: 'parenthesis', text: '(social-emotional)' }] }, { image: reportData[182][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[182][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[182][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { text: AION[reportData[183]], style: 'xstyle2' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[184][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeightSmall }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],


                        //BED TIME AND NAP TIME 
                        [{ colSpan: 8, style: 'h1gray', text: 'Bed time and Nap time' }],
                        //HOW DO YOU PREPARE YOUR CHILD FOR BED TIME 
                        [{ style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: 'X' }, { style: 'xstyle2', text: 'X' }, { style: 'question', fontSize: 11.5, text: ['How do you prepare your child for bed time and nap time? How does your child let you know he/she is sleepy?\n', { style: 'parenthesis', text: '(adaptive/self-help, cognitive, communication and social-emotional)' }] }, { image: reportData[187][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[187][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[187][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { text: AION[reportData[188]], style: 'xstyle2' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[189][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeightSmall }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],
                        //HOW DOES YOUR CHILD FALL ASLEEP 
                        [{ style: 'xstyle', text: 'X' }, { style: 'xstyle', text: '' }, { style: 'xstyle', text: 'X' }, { style: 'question', fontSize: 11.5, text: ['How does your child fall asleep? How long does he/she sleep?\n', { style: 'parenthesis', text: '(adaptive/self-help and social-emotional)' }] }, { image: reportData[190][72] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[190][73] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { image: reportData[190][75] ? 'check' : 'blank', width: checkWidth, style: 'xstyle2' }, { text: AION[reportData[191]], style: 'xstyle2' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[192][17], style: 'textarea' }, { image: 'blank', height: txtAreaRowHeightSmall }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ],
                        //DESCRIBE THE PARENTS RESOURCES AVAILABLE 
                        [{ colSpan: 8, style: 'h2gray', text: 'Describe the parent\'s resources available to meet all developmental concerns and prioities identified.' }],
                        [
                            {
                                colSpan: 8,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ text: reportData[193][17], style: 'textarea' }, { image: 'blank', height: 30 }]
                                    ]

                                },
                                layout: 'noBorders'
                            }
                        ]
                    ]
                },
                pageBreak: 'after'


            },
            //END MAIN CONTAINER TABLE 
            //END PAGE 5 

            //BEGIN PAGE 6 
            //MAIN CONTAINER TABLE 
            {
                table: {
                    widths: [25, 25, 25, 25, 175, '*'],
                    body: [
                        //CHILD AND FAMILY RESOURCES 
                        [{ colSpan: 6, style: 'h1gray', text: 'Child and Family Resources and Case Management Needs' }, {}, {}, {}, {}, {}],
                        [
                            {
                                colSpan: 6,
                                table: {
                                    widths: '*',
                                    body: [
                                        [{ style: 'body', text: 'Your service coordinator must monitor the implementation of the IFSP and follow up with you to ensure that your child\'s needs are being adequately addressed.  Your assigned service coordinator must:\n' }],
                                        [
                                            {
                                                style: 'body',
                                                ul: [
                                                    'Talk with you on a regular basis to determine if services are being provided in accordance with the IFSP and if your child\'s goals/outcomes are being met.\n',
                                                    { ul: ['This includes contacting your child\'s service providers, or other entities or individuals who can provide information related to your child\'s needs and related services if needed.'] },
                                                    'Determine if there are changes in your child\'s needs or status'
                                                ]
                                            },
                                        ],
                                        [{ style: 'body', text: 'Your family may have additional concerns related to your child\'s medical, social, educational or other needs that have not already been identified.  We will identify resources and supports to assist you in addressing these concerns.  You may choose to identify and address these needs now, at the initial IFSP or at another time.  As new needs are identified your service coordinator will add them to this plan.' }]
                                    ]
                                },
                                layout: 'noBorders'
                            }, {}, {}, {}, {}
                        ],
                        //AREAS OF NEED AND RESOURCES RELATED 
                        [
                            { image: 'colNeedIdentified', width: 25, alignment: 'center' },
                            { image: 'colOutcomeDeclined', width: 22, alignment: 'center', margin: [0, 4, 0, 0] },
                            { image: 'colNoNeeds', width: 22, alignment: 'center', margin: [0, 7, 0, 0] },
                            { image: 'colResourceIdentified', width: 24, alignment: 'center', margin: [0, 4, 0, 0] },
                            { text: 'Areas of Need\nand\nResources Related to the\nFamily\'s Ability to\nEnhance the Child\'s\nDevelopment', style: 'h1' },
                            { text: '' }
                        ],
                        //Medical 
                        [{ colSpan: 4, alignment: 'center', fillColor: 'lightgray', style: 'body', text: 'Check appropriate boxes\nfor each' }, {}, {}, {}, { fillColor: 'lightgray', fontSize: 13, bold: true, text: 'Medical' }, { fillColor: 'lightgray', fontSize: 14, bold: true, text: 'Notes' }],
                        [{ image: reportData[200][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[200][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[200][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[200][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Medical insurance (CHIP, Medicaid etc.)', style: 'notes' }, { text: reportData[201][17], style: 'notes' }],
                        [{ image: reportData[202][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[202][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[202][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[202][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Well Child Check', style: 'notes' }, { text: reportData[203][17], style: 'notes' }],
                        [{ image: reportData[204][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[204][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[204][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[204][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Other medical/dental providers', style: 'notes' }, { text: reportData[205][17], style: 'notes' }],
                        [{ image: reportData[206][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[206][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[206][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[206][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Primary care physician', style: 'notes' }, { text: reportData[207][17], style: 'notes' }],
                        [{ image: reportData[208][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[208][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[208][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[208][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Medical equipment and supplies', style: 'notes' }, { text: reportData[209][17], style: 'notes' }],
                        [{ image: reportData[210][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[210][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[210][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[210][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Prescriptions', style: 'notes' }, { text: reportData[211][17], style: 'notes' }],
                        [{ image: reportData[212][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[212][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[212][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[212][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Immunizations', style: 'notes' }, { text: reportData[213][17], style: 'notes' }],
                        [{ image: reportData[214][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[214][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[214][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[214][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Hearing and/or Vision Evaluation', style: 'notes' }, { text: reportData[215][17], style: 'notes' }],
                        [{ image: reportData[216][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[216][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[216][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[216][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Other (specify)', style: 'notes' }, { text: reportData[217][17], style: 'notes' }],

                        //Educational 
                        [{ image: reportData[220][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[220][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[220][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[220][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Child care or Head Start', style: 'notes' }, { text: reportData[221][17], style: 'notes' }],
                        [{ image: reportData[222][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[222][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[222][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[222][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Private Therapy', style: 'notes' }, { text: reportData[223][17], style: 'notes' }],
                        [{ image: reportData[224][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[224][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[224][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[224][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Transition', style: 'notes' }, { text: reportData[225][17], style: 'notes' }],
                        [{ image: reportData[226][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[226][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[226][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[226][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Other (specify)', style: 'notes' }, { text: reportData[227][17], style: 'notes' }],


                        //Social 
                        [{ colSpan: 4, fillColor: 'lightgray', style: 'body', text: '' }, {}, {}, {}, { colSpan: 2, fillColor: 'lightgray', fontSize: 13, bold: true, text: 'Social' }, {}],
                        [{ image: reportData[230][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[230][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[230][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[230][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: '* Translation', style: 'notes' }, { text: reportData[231][17], style: 'notes' }],
                        [{ image: reportData[232][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[232][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[232][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[232][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: '* Transportation', style: 'notes' }, { text: reportData[233][17], style: 'notes' }],
                        [{ image: reportData[234][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[234][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[234][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[234][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Diapers for ECI child', style: 'notes' }, { text: reportData[235][17], style: 'notes' }],
                        [{ image: reportData[236][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[236][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[236][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[236][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'WIC', style: 'notes' }, { text: reportData[237][17], style: 'notes' }],
                        [{ image: reportData[238][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[238][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[238][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[238][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'SNAP (food stamps)', style: 'notes' }, { text: reportData[239][17], style: 'notes' }],
                        [{ image: reportData[240][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[240][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[240][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[240][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'TANF', style: 'notes' }, { text: reportData[242][17], style: 'notes' }],
                        [{ image: reportData[243][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[243][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[243][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[243][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Clothing for ECI child', style: 'notes' }, { text: reportData[244][17], style: 'notes' }],
                        [{ image: reportData[245][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[245][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[245][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[245][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Food Pantry', style: 'notes' }, { text: reportData[246][17], style: 'notes' }],
                        [{ image: reportData[247][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[247][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[247][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[247][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: 'Other (specify)', style: 'notes' }, { text: reportData[248][17], style: 'notes' }],
                        [{ text: '' }, { text: '' }, { text: '' }, { text: '' }, { colSpan: 2, italics: true, text: '* Helping families access this service for the ECI child is TCM, providing the service is not', style: 'notes' }, {}],

                        //Other 
                        [{ colSpan: 4, fillColor: 'lightgray', style: 'body', text: '' }, {}, {}, {}, { colSpan: 2, fillColor: 'lightgray', fontSize: 13, bold: true, text: 'Other' }, {}],
                        [{ image: reportData[249][76] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[249][77] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[249][78] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { image: reportData[249][79] ? 'check' : 'blank', width: smallcheckWidth, alignment: 'center' }, { text: reportData[250][17], style: 'notes' }, { text: '', style: 'notes' }]
                    ],
                },
                layout: {
                    paddingLeft: function(i, node) { return 3; },
                    paddingRight: function(i, node) { return 2; },
                    paddingTop: function(i, node) { return 1; },
                    paddingBottom: function(i, node) { return 1; }
                },
                pageBreak: 'after'
            },
            //END MAIN CONTAINER TABLE 
            //END PAGE 6 

            //BEGIN PAGE 7 
            {
                columns: [{ margin: [20, 0, 0, 0], image: reportData[359][17] ? 'checkedbox' : 'uncheckedbox', width: 15 }, { margin: [25, 2, 0, 0], text: 'Parent does not want this outcome to be sent to other agencies\n' }],
            },


            //MAIN CONTAINER
            { text: '\n' },
            {
                table: {
                    widths: [100, '*'],
                    body: [
                        [{ colSpan: 2, style: 'h1gray', text: 'Child and Family Outcomes', fontSize: 13 }, {}],
                        [{ colSpan: 2, margins: [0, 20, 0, 20], columns: [{ text: 'Outcome #: ' + reportData[253][17] }, { text: 'Date Added: ' + reportData[255][17] }, { text: 'Target Date: ' + reportData[256][17] }] }, {}],
                        [
                            {
                                table: {
                                    widths: ['auto', '*'],
                                    body: [
                                        [{ image: reportData[258][80] ? 'checkedbox' : 'uncheckedbox', width: 15, margin: [0, 30, 0, 0] }, { text: 'Developmental', fontSize: 11, margin: [0, 30, 0, 0] }],
                                        [{ image: reportData[258][81] ? 'checkedbox' : 'uncheckedbox', width: 15 }, { text: 'Educational', fontSize: 11 }],
                                        [{ image: reportData[258][82] ? 'checkedbox' : 'uncheckedbox', width: 15 }, { text: 'Medical', fontSize: 11 }],
                                        [{ image: reportData[258][83] ? 'checkedbox' : 'uncheckedbox', width: 15 }, { text: 'Social', fontSize: 11 }],
                                        [{ image: reportData[258][86] ? 'checkedbox' : 'uncheckedbox', width: 15, margin: [0, 0, 0, 30] }, { text: 'Other', fontSize: 11, margin: [0, 0, 0, 30] }],
                                    ]
                                },
                                layout: 'noBorders'


                            },
                            {
                                table: {
                                    widths: ['*'],
                                    body: [
                                        [{ style: 'h1', text: 'Measurable Outcome and Criteria', fontSize: 13 }],
                                        [{ text: 'What do we want to happen within which routines or activities, and how we will measure success?', fontSize: 10.5, italics: true }],
                                        [{ text: reportData[360][17], style: 'textarea' }]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ],
                        [{ colSpan: 2, style: 'h1gray', text: 'Procedures/Activities to Achieve this Outcome', fontSize: 13 }, {}],
                        [
                            {
                                colSpan: 2,
                                table: {
                                    widths: ['*', 1],
                                    body: [
                                        [{ colSpan: 2, fontSize: 10.5, text: 'In what ways will your family and team work toward achieving this outcome? Who will help, and what will they do?', italics: true, margin: [15, 0, 0, 0] }, {}],
                                        [{ text: reportData[261][17], style: 'textarea' }, { image: 'blank', width: 1, height: 400 }]
                                    ]
                                },
                                layout: 'noBorders'
                            }, {}
                        ]
                    ]

                }
            },
            //END MAIN CONTAINER
            //END PAGE 7 
        ],

        styles: {
            h1: {
                fontSize: 14,
                bold: true,
                alignment: 'center'
            },
            h1gray: {
                fontSize: 14,
                bold: true,
                alignment: 'center',
                fillColor: 'lightgray'
            },
            h2: {
                fontSize: 11,
                bold: true,
                alignment: 'center'
            },
            h2gray: {
                fontSize: 11,
                bold: true,
                alignment: 'center',
                fillColor: 'lightgray'
            },
            body: {
                fontSize: 10
            },
            smallbody: {
                fontSize: 9
            },
            routines: {
                fontSize: 16,
                margin: [0, 38, 0, 0],
                alignment: 'center',
                bold: true
            },
            xstyle: {
                fontSize: 14,
                alignment: 'center',
                bold: true,
                margin: [0, 6, 0, 0]
            },
            xstyle2: {
                fontSize: 14,
                alignment: 'center',
                bold: true,
                margin: [0, 13, 0, 0]
            },
            parenthesis: {
                fontSize: 10,
                alignment: 'center'

            },
            question: {
                fontSize: 12,
                alignment: 'center',
                italics: true
            },
            textarea: {
                fontSize: 10,
                alignment: 'left'
            },
            notes: {
                fontSize: 9.5,
                alignment: 'left'
            }
        },

        images: {
            star: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAwAAAANCAYAAACdKY9CAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAFpSURBVChTdZG9qsJAEIVHTOwsRNIEiVhYaJEyYGEVCwshT6GVINhq4ePYiIJiIRYiiJWWNoqNaIqAiGghJB6zk3i5P96vmZ3ZnbNndgn/0Ov1UK1Wee26LkfBx4bxeAxFUVAoFDh/Pp8cBR8bYrEYzuczotEobrdbWA2QttstzedzWi6XpKoqdbtdajQalEgkqFgsUiQSoR/IsoxarQb/IFuZTCahFpDL5TAajcIsgI7HI/sVQ755e348HiiVStB1HY7jcI1nED79y3C5XLj4m+FwiEwmg36/HzR4ngfDMHjzO4PBAJVKBZZlIZ/Psyg3dDodNJtNPnS9XrHZbHidSqVYdb1ew7ZtrnGD8LlarVgxHo/DNE3ezGazX82z2Yyj5P8i7XY7arfbdDgcyL+BNE2j/X5PkiSRPzi/ZjKZ5EjCf7lcxnQ6ZQXBYrGAfwD1eh2n0ymsBvz5aSEgaLVaSKfTuN/vnAcAL7rq038epq0jAAAAAElFTkSuQmCC',
            ecilogo: 'data:image/png;base64,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',
            check: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACLSURBVDhPxdDRDYAgDEVR5mIg52EalmEYLOVhCoItMdH7pYnHFlx+0ac4Hs6HhJcdTFBIyooLdEfEG7LgFDzJbmZNxVXehnIK5m1nQ7knXOV8KLfEWHc1lAMeT2ahwPi0xNxGxdrtBz5EPK3P2hJnxv0gnfYXJrZXN+Ykvobb6IhJW2FpwHv9hXM+AcFlMWf0Jh7VAAAAAElFTkSuQmCC',
            checkedbox: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABMAAAAUCAIAAADgN5EjAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAADPSURBVDhPnZHBEYMgEEUhtZAmtAJyShV4JJfcLAKPyS1NaAVJE9rLZl0WBieQQd9F2eEB+1cCgDjEib/7OW4KfC3A7Bpe1nP4TpOYjZvpAf8YDW/ed+fUyctTmNHr1WbQ4KF9oc70GnpBQyrMZWhXDWNIvF8Tt0mkHRYuTN359lm9l1VcYigymicemsyVkg5rbC+BEjJbM/7Tdpf1ymbiErEc8WYuIWX7OPBMf0w+W331atkrTkXfncPu3kWvPE9lbTq8HNTzNpMqsgnVIMQXsjm8fOdmR6IAAAAASUVORK5CYII=',
            uncheckedbox: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAABPSURBVDhPY/z//z8DuYAJSpMFKNLMAHT2//+3J1hBucQCqwm3//+nlrPTtoFdQRhsS4PqGLgAG9VMIhjVTCJA0jzLi5E44DULqoMCmxkYAJWVOGY1wYUUAAAAAElFTkSuQmCC',
            colPositive: 'data:image/png;base64,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',
            colAcquiring: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADUAAAB/CAIAAADARx0tAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAABNZSURBVHhezZx5nE31G8entFBR8itDskSLZcYyGiUl0oZGMYTBq2JQzCRMMr0qiZo2wzQtM1EKNbQwKI2SJduPLFleFaUiS1kqSSn95vd2nueeznzPuXfunHtVnz+u7/M933vOc57vs3yec884obi4OOZfjBP13yhh3759W7Zs+fPPP48ePapTEQL7RQXbtm1r27btySeffO6556Jfr169Bg8erMciQHTsd+DAgSuuuOKkk05KS0s7dOgQ523Xrl1ubm5eXp6u8A1RM0JkZWVVrlyZwQcffIAJf/rpJ8bYr27dutZx/4iO/ZYuXYr9GOzfv/+EE06QydatW+/evfv3338X0R+io1/VqlVRhUG5cuX4PPHEY6fduHHjmWeeiTmPrfANtWNkwH6c6v333583b16FChX++OOPjz76iJmMjAxd4RdRi9+xY8eiUKVKlfisXr06n23atPnll1/0sF9EMz+vWbOmsLDw66+/xoTEb9euXfVABPi314/o6Ddp0qSJEycSHD///DPJmfg4/fTTSYc4Iinm9ttvx5y6tIyITvxSM7744otly5adeuqp559//llnnbVy5UqChrhet27dtddeO23aNF1aVhxzwoixefNm1CJ+VS4u3rp1a82aNWfNmsWYRM0NYEs5VCbo/nL3eLdoTIJlp/5nAVFSGp8dOnQ45ZRTrCUmevTo8d1333344YcqWxg6dOj8+fM3bdpEIoyPj//qq69q166tx8KHqDl+/HiVg4DyBTeRxW6kpKTExcWpEEBycnJCQgIDufNvvvlG5ssE1e/HH39kRwCGxE6NGzeeO3cu4ueff15QUFCjRo0xY8bISk8sX74cDQYMGIC1du7cybceeeQRZt544409e/bAay644ALiRleXBab/rVixonz58t9//73KFtgmKtXevXtV9sIrr7wiyVlAWXvyySeZz8zMJJAXLlwoy8oKU7/s7OwqVaqoEMD27du55Pr161UOgsOHD6MHueadd96BKMgkm7Br1y4Z+4Cpn/gK6UDl4mL2BffCflxep/5GmPqBIUOGoGKzZs1uvfXWzp07n3feeYiYRA87cOed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',
            colTaking: 'data:image/png;base64,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',
            colStrength: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABMAAAA+CAIAAABGALdzAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAVCSURBVEhLnZdLSFVdFICvr0Iz/x5qWb7KXtigVBqYgRU5KEpwpGA0SYUoaKI1MCtIRFBEZ5LZQHxE9JhEgv0iZZFiiKIpPshMSwqfqb3//L971rrHe656/8c3OHevddbae5+911p7X4+FhQXb/8JTf/87rmP+/v374cOHbW1t8/PzqjLIyckJDw9XQcDTmfT0dJR+fn5/WGlvb1cLBxbPvr4+3EpKSr59+/aXFbVwwuL57NkzPOfm5lR2i+tsd+7c+ejRIxXcYl+hiYmJ58+fM5qXl9fjx4/r6uquX78eGRmJxiQxMXHdunUqCHg2NTWpsDItLS3GSIvYx/zy5cvbt2957enpuWHDhpmZmZ8/f4qDsHHjxqCgIN6qLEgHJrdu3Vq6Qjdv3mTZVXCgnh8/fuRrOzo66Ov+/fu0TQYHB1HSo1iaqGd2drYxg+Xx9vbu6ekRSxONPsasrq6enJwsKCjIyMjYs2eP+ICvr29CQsK+fftUNpEOTPLz83/8+KGCWywRT5SNjo5OTU2xtqL38PCQZ2BgYEREhIiK4a+8e/eOYNAXSyBXGhoa1NRcIYGhLl++TKIUFhb+aVBUVLRmzZqsrKz6+voTJ06sWrWKSYmxxZMV2rp1671791Q2IF2Dg4PJW9rr168vLS0VvcXz6dOnzMolEthSlK2trbRjYmLOnTsnektARUVF8ayoqBBRqKys5EkO8SRId+zYYaiX7MrVq1dRJiUlXblyJTc399ixY4jFxcW/fv2Kjo4mJFhFsXT1hNra2oMHD/r7+7NUxMCDBw9QDg8PHzlypLGxUWzAtYL9e5bxfPLkSWdnJ9Nj33mKwdmzZ0NCQsRAsQ/sRGZmpuhxc4Y6qhYOLJ4DAwP4sDbj4+Ofrcjgzlg8m5ub8cROZbdYPKkj27ZtIx5Udotrrly7do1duXHjBpmhWoP9+/ezTyoI0oHgJldevnypRg4sY5LTr169osFiEi6sCoFOm+fSMVeMBGbuJldhmfOTGrlr1y4KLPlFSJw8eVLSxRVjzouQ0ygvXrxI4bp9+zZfzqFw6NAhyU9nLJ5kNoWctKJN4ZEkliLM9xsmi1hm+/r1a/pOSUmhTUOOA+plQEAA0zZMFrF4rl27lifHjIjC7OwsJz+frbKJjm1ABQsLCztz5gxtag/fyQojrl69mg8RGxPXFeK0I6G3b99OpxyhmzdvpnHnzh197cQy+/nhw4eysrLe3l7CeO/evRwWsbGx+s4Z6UB4//49RZXCobJbLJ5Mlb7MWuwey2y/f/9++vRp1oNIcFnMLVu2UOBVEKQDYWxsjLXVF1aWvyfoS5vt69evnDmMLAeham027mCHDx8mHlQWDH+FtCKMVHCCAKIyqeDA4snJTV/cjVQ2YAooiQqVHXgbA9vr3d27dxmTdlpaGokiepBgdD7/BfUkyru7u4nPoaGh0NDQTZs2iR4IqePHj8fHx6tsomMbEKVHjx598+aNym5ZsZow8xcvXvj4+HA6qcoF6YAL7YULF/gYvgqRMDSvMXFxcYhi5ox6EjoYpaamyk6eOnUKsby8vKamhhOes1DMnLF79vf3Y0eBFtXIyAgiVw0RmTMi6yeiid2zqqqKKBEZEDHt6uoSkQ+m0qIU0cReTaanp1kJGgIXGALN3ECMeLpeUYEX3HtofPr0yehrgQsQwSBt4FzgLRVQZQd2T9aTrT9w4ABhRNXBzgxAHKgpu3fvZqtFY2L3BEwpWfY52Gx5eXmiTE5ORmTmS/+sgHoCFygu9GauUG/Pnz9/6dKllUrEijH0D9hsfwMcSnhyZd/E8gAAAABJRU5ErkJggg==',
            colNeed: 'data:image/png;base64,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',
            colPriority: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABIAAAA0CAIAAAAIWf8rAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAQVSURBVEhLpZZZKHVfFMDvNc/8EVJSlKGQBw94EUUeiCciUgplKM+UUuRVvBKhJElKGV4NDyQUkQzxN2ceM3+/c/a+93/Oca+v7/v/Hu49a+2z9l57rbXXPuavry/Tn+Mg/1U+Pz/l0+/Qmc3Pz6ekpPT29l5fX0uVPXDSyt7eXmpqKkonJ6eCgoLJycm3tzc5pkdnJtja2mpubo6IiMA+NDS0sbFxeXlZjlmwYWZlY2OjtLRU9ckUGxvb2dn5+voqhmybvby8TE9PV1ZWhoeHe3t75+XlFRYWOjs7Jycn397e8oLOjJ3Mzc3V1NT4+/uzQnR0dFtbGxsWo7u7uyj7+/t51pnNzs4yEBgYWF9fv7S0JLUaamtrV1ZWeNCZbW9vDw4O4qGUVd7f38/OzviVsorOjBhGRkZ+fHxIWeXi4sLPz08sYsUJr5iptbX16Ojo+Ph4Z2eH6Hl6eqIX7O/v39zcuLu7S1lFMSO5RGl0dBT3ECcmJhwdHdVRBV9f35aWlqioKCkL5KoqFFdwcPD5+bmU7aOYUcFiP/yyE8PeGDXEAxSzrq4ucvr09LS6ukr0fXx8ELVQYgYXlL3FxMSUl5ezn6CgIHJ9d3dnOEGurq6GkOj2xnmhSqTwIzqznp4eJjKk2ya6YxoXFxcWFra5uSll++h6CRnPycmhIDjjpEtqTSYPD4/u7m5qRcqGpkC4XFxcsHl8fPxXw+HhoSFIf9m5bJiRwKmpKXKIb4mJiZmZmWazWY5ZUeKiYXFxMSQkBD3eUqs8JCQkUOJy2ILO7P7+nppkb2tra8/Pz4gzMzPMkp6eLt+woDMbGxvDn6urKymriCNvbQ0CXSRx5h8VKauII0OQhSjQmSUlJbEUjklZZWhoiF9OvRAlclULubm5KJuamji1w8PD1dXViA0NDXLYgtGMSFRVVbm5ualzmgICAugXckyD3XSzGRIgGuZ3FLODg4P19fWsrCxqioNDMDEgbwzRvTnaiGlpaWikETDW3t7Ow8PDw8LCglB+5+TkhDetKKtdXl5SrPHx8UzMIcDeULgODg5cHaJoJMJaQB1WVFRYr5Uf0JnRiwwT2UMXSaKXn5+fnZ1dXFxMw5Ja4mY209FwVcogrAXcmobKEhDD09NT+ZKKbjXCNTIyQnsT/VRqTSZafUlJibbn2U33z/znLk2GjRUVFY2Pj0vVDyiefn2VlZXxTBsXntisQy2KGWeZVzs6Omis1Bc3LSI1IN6wiWLW19dHDxQykAYizqmTsi2UvXE0tfcgcaOOWE3KtlDMyCMTCBnkfNrkfkMZ4yVtJxT1ob2+vyPvblzKyMgQKj5qSHddXR3NT2joswMDA9o7QDFjmPbESRUqVub7h48kLmSh8fLyYiLxLPjfVfIHmEy/ALYEG/d8PJ9OAAAAAElFTkSuQmCC',
            colCode: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAApCAIAAACz5D5TAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAANeSURBVEhLlZS5L7xdFMeNXZDXlog9iEKEwpLQkFgjagpRKPwHokA0OoVWpRG1ECIiQSgQjTWW2ELEFvsaYpnf57nnzIzHTPH6FjPP99yz3LNdh9Pp9PsL/PX/f8N3hN3d3YODg+Li4sDAwODg4JCQED0AGPzE5eVlWVmZHG1sbPT396enpx8fH+ux02m70ufnZ01NzePj4/j4eEBAwNPTU3V1tcPhaGxsVA2ghgazs7NIbm5uvr6+0JuamkK4sLCA8OjoSHRsETY3N2NjY2NiYq6urqD+/tZpRkYGxufn50bFXiWue3t7+/HxERkZiTNuhXB5eZnvlJQU0bFd6fX1NSkpqb6+fn9/PygoiCstLi5GRUVVVlaqBiXVfxfW19c9zgwKCgouLi702On00Yf39/eJiYmdnR1SLyoqolB6YPDn0bAMRkZGuru7hYeGhoaFhZEMcSgOPYZ+f38PDQ1RPRSsKtH8/wyo6f39/czMzMnJCZRakT0UM6mYBSuRHygsLOzq6uL2QonT3NxMp4UCm8Hg4CBXUuLC2dkZfpkrobbGEZdxwqtyA4aCX9oi1BaBNkdHR5eXl9Pd6+trJpf6klhpaalqeDeOUUtNTVVnBhUVFYyjHvtsHFhbW6NxVC8vLy8zM1OlBrYcBKTx8PBAfZ+fn7mkSt2QQG6srq4mJycjp2uiwCyRjB7/ygGXiYmJaJA0e3d3dzc9PR0XF1dVVaUavwzGxsZwyfYoN5ibm0PoXmtbDkwEA4NL5QbZ2dn8+t64/Px8slxaWlJuMDw8zK+nVhJIwFRyXd6inp6eyclJ3o62tjZ0WltbVcO7cTwtTU1Nsv4gPDy8s7NTzwx8N476kCXbTNfdxgqxE3Al/TI4PDwkJSUueAw6OjpImlEVinFWVhbTMTAwIBKBGrS0tBCttrb25eVFJBjQFuYUOfspQmAZSB37+vpE9At1dXXx8fG8bkItg/b29rS0NOHeYHJxt729LdSqALOQkJDAh0/QeArF/Aq1DHJzcymIcG/gm3xsb+vp6SkfdFeC/sTb2xs7RPWUu6vU29uLTUNDw+joKLu2t7fHhFMG3nMmZWVlRdSAGgCOIyIiTFQPSkpK5Ll3wzYaLND8/PzW1hbbg282KScnR89c+ONj7Of3D2tYopuSybVfAAAAAElFTkSuQmCC',
            colNeedIdentified: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACcAAACbCAIAAABuyRVsAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAABClSURBVHhe1Zt3dBXVFsbzFBtK0SAsiIDECIo0AUE06FKKio8uEDSgiNKVpaBCIkWjIvAsYAEEExFBLCgGCwgiKK4QKbEigtjArtgVG7wfs/eMd2bOnHuD43L5/ZF1zsye+82cs8/e3z4zSdvzT+Bfyzpjxox+Vlx//fVq6iIG1jPPPDPNitq1a6upixhYCwsLhzoYOXJknTp1oOnSpQtdnvLwww+vWrXq7bffrqYu4pzXrVu3Hnrooffee6/29+x5++23q1SpknhEECfrpZdeyoN+8cUX2nfQoEGDGjVq/P7779p3ECfriBEjYL3kkks+++yz33777fvvvy8qKjrggAOY17+R9eWXX97rPGlplStXPvroo3lE6d50001q4SJOVrBw4cLMzEwhA3hTfn6+nktAzKyAscWJVq9eXVJS8s033+hRP+JnZQrLysoef/zxnTt3/vzzz3rUj5hZn376aW+EV6xYMWDAgB49evz666962kWcrO+8885BBx0EX/369ffff/9nn322Xbt2dAsKCtTCRXLWdevWvfnmm9qx4rrrroNj2rRpDDL0S5Yseeutt4gSWVlZu3fvViMHSVixPuyww7Kzs7Vvxfnnnw8ry5R2hQoVmFoajRs35hd++eUXx0SRnDU9Pf2MM87QvhUTJ06EldX5ww8/HHzwwcwxK/jAAw9s3ry5WrhIzlqtWjWyivatePfddytWrAhxRkYGz0pIog1mzpypFi7iZAX4La4kZIDZHTt2rJ5LQMysArzvqaeeeumll77++ms95Ef8rIzzo48+escdd8yfP/+VV17Ro37EzIpYIcno+Dro3bv3Tz/9pKddJGflV1q1aqV9K5588klhOvHEE88555zTTz8dn6I7atQotXCRnJWbHTNmjPatIP7BMWnSJO07EYasV716dVKCHnKQhLVcwF1h/eqrr7TvAC3BQuLute8gTtZPP/20Xr16V111laccCE+HHHIIziVdD3GyktIJuTzucccdRxBt2rQpbVjbt29PF6AdxTJOVp4SGjvEMk7W0tLS/zm45ZZbbr31Vv4COSKgShBLH+vSpUv/a0WvXr2i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',
            colOutcomeDeclined: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACIAAACPCAIAAAAQp56lAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAABBiSURBVGhDvZp5mJbTG8enxZYloey0oLIk5ZcQSVKEq5QrImtC9iRL6GKqsSQy4yoKKROFEiGypEiSUJRQZBsVErKF+X2e53vPM897nmXemXf4/vHOOee533M/55x7+d7nnRqlpaV5/z5q2t9/Gf+Rmpw2rbCwcO7cudaJQ9OmTfPz870WaqqMo48+2p8tEXvssYckc1rNI488smjRIhqbbrrpY4899uWXX/bo0WO33Xb75ZdfnnrqqY0bNw4bNqx///6eqLTliI8++qhOnToTJkywfmnpypUrt9122zFjxqhbPWrOO+883njNmjXW98HB1K9f/6+//qKdZmnvvPPOsmXLrJOKLbfcks+BAwd+/fXXv//++7p16+67774VK1awRAkkruaff/7Zeuut27VrZ/1U8EKabYsttthll13q1aun7u233y6BNDXbbbddhw4drF8Rpk6d2qxZM80OGjRocMstt9izFEtjHNEWLVq8/PLLNpQFVq1axQnVrl2bgynfseqNApxKSUkJR7L77ruzGTYq+GuKAXI77LADDmj9ioCj4DGa86WXXurTp0/Xrl1RrKfVs5qPP/74lFNO+eqrr/bff392jFf89ttvn3322YKCAglUj5ri4mJ8fvTo0UuWLKlVq9Yff/xBG/ecOHGidi9Nzfr16wkb1knFp59+yucZZ5zB599//41LNmnShBPCHGgzmKame/fuGLR1UtG8eXM+R4wYwZuxaZtsssnrr7+Oa2PihDtPQkeUI7BjfJnZ8AE2baeddvKmzst74IEHJFA9asCcOXM4f80Ottpqq5tvvtme5ZgIoli8ePFnn31GsMGvMQEbzTF7OsDA/vzzTw4GK8BjAAdDkPae+WvKFSi48MILCZqbbbYZUwdo3769BGw1+C2h21ObgM0337yoqKhu3brWz8TYsWP79etnnRBat269cOFCryVtd9xxhz+eBpxcwlHIYy6++OK33nrr3RCIDhKorSnIK1dddRUNrH7+/PmvvfZap06dDjjgAHZ59uzZ77///mWXXZa0FLDffvvxOXTo0PCxZ0DawmjcuDFJ1zqlpUSRNm3aELKs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',
            colNoNeeds: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACMAAACICAIAAADiYMUjAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAA3kSURBVGhD7Zp3kBXVEsaJShIJgiQlPjIoGVcFCglSSxKKUGQBJQcpCgUkWJJBFBCEImdZclhEySDsgoqSc1TJeQUFgX2/ne57dnbu3PvYuUjVq/e+P4bT5/ROz5zT4evhJo2NjU3yVJBM//3n4bR05MiR8ePH//rrryo/QbB7dqxfv57JlClT1q1bd/78+deuXdOFkOG0dObMmfDw8DRp0shzpE+fvmXLlpGRkXfv3lUNr3BaEly4cGHBggWtWrXKmzevmGTQo0eP6Oho1Ug83C0ZLFmypFChQmJM0KVLl0ePHulyYuBi6cqVK8uWLWvTpk22bNnk7tmzZ+/cuXPPnj1Tp06NGBUVpaqJgdPS3r17n3/+eTGAX7zzzjsRERE3btyQ1aFDhzI/ffp0ERMFp6UNGzZwr8qVK0+aNAlf11kfNm7c2KRJk/3796ucGDhzBD526dIl4whPEGrp+vXrv//+OwN27Nlnn8Uex24pKJIlS4ZrpEiRQmUPsN4s9quvvlI5AHgCXF+UvUGzEQ8bHGnTpk2aNKkoe4Pu3h9//IGDyZQrMIOvJ0+eXOXE4+lVDbX03XffTZw4UaZckSpVqilTpmTIkEFlD4g7rNjY0aNHqxwYOKcoe4O+0w8//LB8+XK5oyueeeaZ3r17p0uXTuXE46mfkwN37ty5evWqCd6//vprx44d9evXf+GFF2TGC+K2MCFGjhyZI0cOHJow4kp2EM3ffvtNNTzBaenYsWNyXwc6dOhAzKmSJzgZyy+//MK1ffv2+AjR2rBhQwa84osvvkiaEB2PUIs+zJw5k8nVq1czJqW++uqrDPA6JkP0cuc7FSxYkOuXX35J5a1Wrdq+ffs2bdrElUlJ9t6hFn24f/9+1apVmV+xYsXu3btFB1CITeX1Bhff4+QHDRp06NAhxt26dYOIvfzyyzALWfWMpx65e/bsWbNmDQdTuHBhDknWAO5HHiKEGfB+Ibmf9WaaYfv163fw4EGZ90eIvqe8oFixYjVr1ixVqlSmTJmIIfwCyBKA5uERhkJ7g/Oc/vzzz4sXL2bNmtW+UeQhSETZsmUdBX7AgAHr1q1TwQ0lS5aEHKogr2Zw4MABJvE9lS107NiRydOnT6vsQ40aNax7BESBAgVU1ezegwcPhg0bRg/DsyOSI27evClLQOjm5cuX8+TJIzOCPn36kOAZkIuHDx9OwEGqc+XKRZzMmzfv3Llz3FM046AWY2MHDhyoU26gXgSJ3J07d2IMiqCyFZQcuX1v4s+J1+revfvhw4e3bNkChy1fvrzMAzIsJLlChQoq+6F169Zz5syh98qdO7dOWYmNIkdWU0YlBg3Onj1bpUoV/kzlx8NHH33Erd566y3akFOnTkHc2VhmiM6HDx+Kjks28oDjx48/99xzcQ9uBbsMwOzZs1XDWCKfvvbaazS29BdhYWGMHSDtBu95KWNEpHREULYyZcrgFLpmQS2NGTMGjf79+4eeI3BRoIIN6hGEEb3Rm2++mS9fPl5Zbm0H2Y9jD54mOKGtW7dyfffddxFpKRPoi8HQMW7cOMPaib86deqUKFEC39PlQFXjxx9/xAnxe5Wt2AwPD+cAVE6In376iVxF51OrVq1vvvmGsjB16lS68a5du06YMEGVxKABdyd0dC0hgpyTEI2VK1cyptFbtWoVKQaSkzNnTm4oOk4esX379kWLFjFA6V82vPLKKxyV6PhDchjHzJUAwtFxQnIEmSK+txSDBrNmzWKSt4bGomSHarjh888/56/atm0LJ2CH8e+5c+cyU6lSJdXwj1w8B9IKF1P58cDJ2/MQhyoD2U+B09Lff//dpUsXKArp8sSJEyd94AnMjruCNIG/GZchDzlSmtMSTN/kFTvoZ6iQqhQYMTExR44cOX/+vMo2OL0cS2+88YYKNuBR+D3upLIF3oN9o7By8rLtqOHrOMW9e/e48nzFixcXZZfqTm1VwQbuQgE1ByBo0KAB/R3Bx/P16NFDZ23AY4Xox8F6M4/g7jgCXHrGjBm0wHBQdt4OaJ2qmt2j+qEttl3BUcPUzMcrASSALX3vvfe4CRuoszawB/FNuGUvdtSoUSoHhn+nJqHKgGCnKxGCHQi671SgTp06ydgVHDW7oYIPRDdPvXjxYrgY5xEREeFgALxQxYoVVVCLnvD+++/rXQKgSJEiqhoolz8myLnsBKwBWsL7ZcyY0ZHsSfBkWxXEYCggdIYOHYoNboo9O2j6VSnEdzIg/5LGOEuV3RCSpaVLl7J1cGkSFU5B04BJXbPw0ksvtWvXTgXrzYKBM9CRH6RPjY6OHjFihNzNASypqvFyO3i0sWPHNm3alBOuV68e7ounUkeyZ8+uGj5Au9ixLFmyQBmo6zprA76nI6AWfSAtCqmPjIwcMmSI6AAqgmp4hdMSbSj3hUBB6osWLUqdxiRxzcC/atATUCDwda6usHMjpyWpyhSxu3fvMoBBMAnjZLx7927RMWjTpg27J6BvhN1xZYzHyxiKr6r+jEVAD0MhYCAdB0/NVf6rwQ6yKsEkIHR4OIqOY6yqQC36AHNjkn2Tjy3Lli2DC2AD7+C+quQDlQx9gPvRCJFkye6I27Zta9myJUlv8+bNquq/e3jE22+/bT1DEpgzD0WNYfzZZ5+phhsgQ+hQ11W2QNfVuHFjFfwtAd4aL6dbpn1EhGGRbGQpEJo3b44lx/9OEUzUM3KHiC6WPEBaFRjVF198AUkm+OjamHn99dcNUdRsBB2wR48d+Dc6+BIHBi3V2YSAG1euXFk+mRmkTZv222+/xZjKlr1YjkHlwAjePxFb9OSQ1vz585crV45kSKekaxb0nWikcRgGcCipoR06dChVqhRsEopJORg8ePCHH37oKD+JgjOXEwqcJCTE/imhRo0auAldmPn66w8U8Ai6QXTiWLyV1InL6tWri4LTIyRH0AeobIGGgMkg/5XGxhYrVsy6XwK4fGMxyJo1K9dBgwYRqtBPdo9dlX40SOs5ZcoUaZAJQfZfJgH7ryOgFn1g9+LZjA2QE9VwAxkBHeLXRI8/XOKJBNy+fXupRrSuJNlPPvmE3KHLbpDmLrhzBqvuRAlVUfYzOHDx2rVrE0Bkk8yZM+usxXzJSSqIQQeuXbvGY+LZhB6vCCXWhQCgurt+OA0LC1MN16qBC1Ct6auxFBUV9fXXX5Okd+3apctuwHHgXCrYwJ7ryD+ebt++TTtHhwwv+PnnnyGOxARdInVk7969jq7GgD2A2DDA8QAVQNId5Sa+K7XeLB5r165lknaFMUWdEOZ5w8PDmQxO8A0uXbrk6j7O3UOPK/vLFZclveJ+8nMC1/0xYBugdjlz5syVKxfZZOTIkfav4XFQiz5QMZksXbo0W1emTBka/++//x73w4uuX7+uSn5gz9leuSHYuHGj0AIqiGr4xxP727BhQ/kDeSEZ9+nTRzXcMHnyZHSaNWvGg/JMHMHChQv5W5oc1XCNXMhGt27dTFgQwh9//LGccCC0atUKTeFceAQsmgHJjI4a77BUgtZcqvvRo0eDJBgDngxL1BrGsDDOidRO2SQNmkd0t0TLP2HChM6dO1NnOWoIsy4EgNQ2gpcNxFLdunWJSGZ69eqlGq6WuLsJ+E8//RT2QhiRLHQ5AMiN8icG1HW7EzkjlzXKMxkPfooL4XuEHlfoHCQryOcwAMlds2bN8ePH2TfMkGWMQ8VBDBrwQkxCjxkTuSR1Bo0aNWLS249KDJzZhazMlYTElewg6Uc+4tj/90ZAYt20aZMKbiDI4GUqqEUfeHAmocobNmwgsbZo0WLFihXy8YTwVCUfhNQFQZ48eVTVtT6R66ZNm6aCDxCYvn37quADDyH/t0MMEby0PQQWhAcGT9G5desW3sR5i7KL75EfcTlKLedPwBOA48eP17UAIM3j3Pb/tSR/wkTsbN49ngRsl39/4Qr5QE5rprIFjoBEw2GLqB5x5syZ+M9jvozHO6FBjhDXgLnx4KqREDw+V8oYm0zbS4JYsGDBiRMniA1uJTr6TvEeEhiOR7aD0+KoRA33MWE3ceJE1TC7xyMQoXbg6PioPfSCfx3FV6kU5g0If8fpqiW8gHARSMXk3Qly+UviCW80Ox4EJ0+epG2FZZoUbuDuEXBS88mVVBRk3x4fzniCA9FTkPYZlyxZctSoUTVr1pQlf7CfMTExKriBICG8VBCDAJ/u3bu3dBNUP1iALgQGNUJuEggQb1U1Xk5Vbtq0KWfDGJfFX+kdIiMjcXFRCO7ljwPdPbz8gw8+kKlA4LQcn44StXtqCYLRr18/mZIZB6iNHF4ovwJSS1R7O7N1hYlNb3DJ5f8QAvatTxz/txQK3C3Bp6jKZKNy5coRZ5BZXQgF+J4Dy5cvjy9fFsi2pGdd9gqnJTilhCe8Zf78+TNnzqxXrx6i/auWNzgt2XtCA2Fb1B6VPcF5TtITxn8qs0DLxpWmRUSPUIs+REdHM1m1alXDI8+ePQvxgBrYP0l7gNMSJVwaR6hEgwYN6E+E99DDqoZXuPgeVUq2y6BatWrwdV32CvcMS16nDBJVqVOnJqT+4y+YHgdPL5erpf+B3y1DXWmY6CAWL14spAVH37NnzxN4J3/ALOQjDqAHpYPXBa9wsRQVFWV+TkBbuH79el0IDQkskV579uwpNtKkSTNkyBBouq6FDLVEAEVEROTLlw8bHBLt7RPh4naoR2DG/PiHBgguDmm1f2Z7Yl5O66tyYPwjXu4Pcvl/za/znxY3SpLk33hJ339olZdVAAAAAElFTkSuQmCC',
            colResourceIdentified: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACcAAACXCAIAAAAZC9UXAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAwsSURBVHhe7ZtpbFXVFscLpVCEAlKQGaEFK1BKBVoQhESjFlsTBygSCAljosgHIv1QE0iAMBioSipNIFqNCgZUAiHiAMWqUJla0DKVAkojUKQMIlAok+/XsxY3l2J71z6CLy95vw+XPazD/+x99l5r7XNv6/31119h/zr19d9/F9NYz5w5k5WVVbflmDFjevfurZWQ8H+FZP/+/WpdO++//75aGzCNtby8/NVXXxXLa9eurV+/vmXLlgMGDGjUqNGRI0f27Nnz6KOPLl68ODk5WexD42k78Nprr7Vv376srEyqN2/eHD9+fGJiYmVlpbRYcFOtqKjgRtPT07XusWPHDhpzc3O1bsBtDV+/fj08PHzz5s1FRUXS8scff7z99tsUzp07Jy0mVN3Mc889x1X16tVjVpOSktq0aUO1YcOGhw4dUgsDzqq///57SkqKd8NKTEzMF198od02fPqmffv2sXovXbrUrl27QYMGMVbtsOFTlf2DKp8OniEYb8RuzJw58/777+fahISEkpKSvn37btmyRftsOKsuWLBAbhfhoUOH5ufnU46Ojj516pRaGHBTvXz5MgKtW7dmiPPnz+/Spcv58+cnTpyI8EcffaRGBtz268mTJ4kEjz/+eFxcHF6pqqqqWbNmU6ZMoQtvJTYW3FRxv40bN8YZ4RMo4IdpZJR8Mm7PxIaO2czUqVO5qm3btrGxsU2bNu3Tpw/V5s2bs4/VwoCzKnt05MiR3g0rHTp02LBhg3bb8Llfd+/eXVhY+Oeff/KAecxNmjTRDhsmVWxwCPjeiIiIq1ev1q9fv0GDBtrnQSMttGs9FCbVo0ePsjXxBmvXriWAHz9+XDuCWLp0aWpqqlZCgmpIDh48iGWPHj0oR0ZGyoU1+Pjjj8XYgmmsPL93330XVUaza9cuFtSd0bR///7kGFoJiYjXTWlpaatWrRCmPHv27JycHGn3jUmVjcH94YNwTBSI5GfvgAWl1gZMqnv37vXmJaxFixYsVJIYXH8NWGhqbcCkissVF18HK1asUGsDDl6ClXzs2LEnn3wSb7xmzRptvUXPnj159loJhbNvysvLi4qKIgXXui9MqleuXMH/IYavJ3EhypKQat8tHn74YftYHbxEr169KBNe5MIafPLJJ2JsweQ58bGIkQ5SJo7yXKkGYAE/8MADRD0xtmCa4Rs3bpCpoE3mgFdiSRMJtM9z/cw591Sbs/wbZMh28P4jRozQigepRceOHZ0S8dsCVh1s376dLJBzzrZt21g1aGhHWBhpIjuKiJuWlqZNIVH1UCxfvlwvqIV75SVGjRpFti3BlSmVRuBhc+jIzs4mf9OmULh5CUIe+5KIVlBQoE1hYZIpOuHsm06fPs1WIQBo3RfOqrBp0ybWLbFd6x7Mf3x8vFZCgqoTGRkZeuXt3P13MAGI6p07d66srOSgHrygIDMzc/DgwVoJiYgbkQU8ZMgQrfvF7ZzD6h02bBgOUut+CZ81a5YWDXCbfJK2caohL//1118P3wLv7xAAZMhG8PJsG73ydnBeamTAbYZJ1WJiYphnIMgEQ8hTIwPO+5W8gksaNmwojoIqt8KTDrRY8OMlgM1TUlLCWZ3Ap01OVE+zI6T/kiIlJCSQ3CQlJW3dulX7bDirLly4UG6XBxl4B8NNVFRUqIUB5zWMAOzbt2/evHnkUCSL48ePR9jpTOe2htmmxJwnnniCnJuLea6MWN5UcMYVGwtuqmxWQje5McnbfffdJ5FV0owHH3zQM7EhQ7Yjb5c6dOjQvXt3nFG/fv2okqCfPHlSLQw4q168ePHFF1/0bljBRXz11VfabcPnft25c6fMMwkNj5nUSTtsmFSxCY4zOCPQioe8mQ9OzevGpFpWVsaASPkpB//X1XN1q5qTk/PMM89IOTRcGZIDBw6ode3c/XcwrKCNGzdWW9erV1xcTEgePXr0s88+y87Zv3//ggULUlJSli5dyhlLLwiJp+0A7p4js1Y83nvvPfZuaWmp1g24eYnffvsNR18jsLNzCEF3vjOoC1W3Qe7P4ZV5fv3113/88Uf2D8ebrl278v84PVfnGV60aJHcbjDJyckk5WphwFkVcnNzebrR0dFNmjQhDEyfPp2jtPbZ8OmbgO3LKd3hfB6ESZXk6KeffgpY8lzxTQ0aNKCAV8Jt0fXQQw8xejEIDReERN7B1M29egdDOKsD+5EZTDPMHIb8ehVh+/HZ/2r6J7j5prvF/1WNkJlqyRE/qniMSZMmJSYmpqenl5eXv/HGG7gR7TPi7VoHvv/++8C36IMHD/7hhx8opKam4qTUwoCbKr73kUceQWb+/PkTJkyIj4/HbcXFxdGSl5enRgbcVE+cOIHA008/TTkrK6tt27YU0KORPMYzMeH2XCUjlEVEKipTzeGHT5IYPq2oug1mWI7Jr7zyCkupTZs2LCUJNbt27VIjA86rqaio6M4XItOmTdNuG3788JEjR8i5d+zYceHCBZbS8OHDX3rpJe2z8b/j/ZcsWTJ37lwpkyyOGDGCY6RUrXjz7MDixYu5imxUqoMGDaJ6b3/Lhedj3ZI2BL6C3bx582OPPYbw3f9OMoC8fODUrHUP0nEa582bp3UDbs+V03FERMShQ4eC3T0HLz5b2b+k87GGWTurV68mD+WMRRbHLiIAyK04vBCRIdshtCUlJenFHvhCp7QUfO7XdevWFRQU4CU4caSlpckBywERN8LjZCm9/PLLZ86c0SZfuKlKpAOO6NrkC7c1TEDNzMxk7Xz++eenT58mOycKCfxfamRBxI1wiBs2bJiEVSJPbGxst1s4vehyU718+XIgaaqB0+/W3NYwM/nll1+irfUgiPY1vleqg/9OpHNTZfl88MEHFy9e1HoQ7Nru3btrJSTV02zmb+dW4G7UyIDbWK9du0ayIkmhwP4pLS0lSSbuDh06VFtDIuK+4T7GjRtHiL2Huf/fsnPnTu6eQKR1A26+iQt+/vlnZAIQ5uT3riUlJWJjQsSN4P1btmypV94Opy41MuCsSjwnoAaIiorq0aPHm2++qRY23NYwxsQ4di1ZRGRkpLzlQljOP3b8+ybJnmJiYlx/7llN9YAd+eyzz0gh5HIe8/Tp09k/2mfDWXX9+vWi16hRI+ZWymPHjtVuG26qxBz5wfBbb7117NgxHNN3333H+qKFw5YaGXBTlQzmqaee0roHmRuNixYt0roBP6crLtOSh1RrfK8UAk/bCjOckJDAVVlZWTLD+fn593yGQeYTglfTCy+8oN02nFXh008/7dWrl+i1aNFi8uTJxHnts+HfS+zdu5cJ7927t6tjAv+qAgkpp7kaJ5/QeCN2gLPNqFGj2K94YNm7MHv2bO224azKU0Rm4sSJHM4p4BGJPBQOHDigFgbcVFk1rFumtLCwcMyYMYjl5eWtXLmSgvx+24iblzh16hQznJKS0q9fP8I4LQMHDiS+UnB6DeOmKkHt7NmzRUVFeIm4uDha5OchHLw8Exs6ZhtslcTERK6SL1Hmzp377bffysmHvEmNDDivpk2bNsl7RAZ6/vz5Dz/8kPKcOXO024azKpCFy9d2cPjw4W+++UbKdnx6iS1btrCa8EoZGRncRKdOnbTDiIjbYRnLyzTAS7Cs0HYKruCsKtt0wIABycnJfG7dulXuwOlNhZvquXPnIiIi4uPjKXOcat++PYV33nkH1ezsbM/EhNt+ZdGSDsqXGpWVlRJt5FuPGj92rRs31Xbt2jG+rz3wjuHh4cS7GTNm0BWIBCZ0zGbwt3IhA5WxAg6SjFwtDPjZr8uWLevWrZvoEQwIfGwe7bPhc78SXH/55ReeZceOHVu3bk1LVVWV/btmt7EWFxcT4xDQukd5efnIkSNXrVqldQNW1bKyssBrB1JRDsvSzmzLHxk7veWyqgb+gkCCXdeuXSsqKtLT06WREMtiVlMDJlUiedOmTdErKCggej///PMoSTpOyp+Zmen0IxgwqcqXDH379pUqj7B6gN7fUbv+la9g8hJXr17ls8ZJpkuXLkg6/AI9CJOqeIOAqvwMZPjw4bgqaXHF6hERxgkzONi9ezctx48fl6rASUssTehM10lpaala187d//UNXj46OpqDVHMPCqROgaogubgRk0e84f0FlFZqga1V22vyO/mnpyt/WFfT3SQs7D/aiL5hoCO2rAAAAABJRU5ErkJggg==',
            blank: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQMAAAAl21bKAAAABlBMVEX///////9VfPVsAAAACklEQVQImWNgAAAAAgAB9HFkpgAAAABJRU5ErkJggg=='
        },

        pageMargins: [25, 50, 25, 0]

    };
}