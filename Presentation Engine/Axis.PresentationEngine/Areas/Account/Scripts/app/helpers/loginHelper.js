
function capLock(e) {
    kc = e.keyCode ? e.keyCode : e.which;
    sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
    if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
        document.getElementById('divCapsCheck').style.visibility = 'visible';
    else
        document.getElementById('divCapsCheck').style.visibility = 'hidden';
}

function validateUserName(e) {
    if ($('#UserName').val()) {
        kc = e.keyCode ? e.keyCode : e.which;
        if ((kc >= 65 && kc <= 90) || (kc >= 97 && kc <= 122))
            $('span[data-valmsg-for="UserName"]').hide();
    }
}
function validatePassword(e) {
    if ($('#Password').val()) {
        kc = e.keyCode ? e.keyCode : e.which;
        if ((kc >= 65 && kc <= 90) || (kc >= 97 && kc <= 122))
            $('span[data-valmsg-for="Password"]').hide();
    }
}

(function (global) {

    if (typeof (global) === "undefined") {
        throw new Error("window is undefined");
    }

    var _hash = "login";
    var noBackPlease = function () {
        global.location.href += "#";

        global.setTimeout(function () {
            global.location.href += _hash;
        });
    };

    global.onhashchange = function () {
        if (global.location.hash !== _hash) {
            global.location.hash = _hash;
        }
    };

    global.onload = function () {
        noBackPlease();

        // disables backspace on page except on input fields and textarea..
        document.body.onkeydown = function (e) {
            var elm = e.target.nodeName.toLowerCase();
            if (e.which === 8 && (elm !== 'input' && elm !== 'textarea')) {
                e.preventDefault();
            }
            // stopping event bubbling up the DOM tree..
            e.stopPropagation();
        };
    };
})(window);
