TalentUtil = {};

TalentUtil.notification = {};

TalentUtil.notification.show = function (message, type, onCloseCallback, onOpenCallBack) {
    var notification = new NotificationFx({
        wrapper: document.getElementById('main-content') || document.body,
        message: message,
        //layout: 'bar',
        //effect: 'slidetop',
        layout: 'growl',
        effect: 'jelly',
        type: type,
        ttl: 2000,
        //closeTime: 5000,
        onClose: function () {
            if (onCloseCallback) onCloseCallback();
            else return false;
        },
        onOpen: function () {
            if (onOpenCallBack) onOpenCallBack();
            else return false;
        }
    });

    notification.show();
}

TalentUtil.formatHelpers = {}

// Builds date string in YYYY-MM-DD
TalentUtil.formatHelpers.formatDateISO = function (dateInput) {
    if (dateInput != undefined) {
        let dateVal = new Date(dateInput)

        // using UTC as otherwise results will be inconsistent based on locale
        // Raw database info should be UTC anyway
        const y = dateVal.getUTCFullYear()
        const m = dateVal.getUTCMonth() + 1 // months are zero-indexed
        const d = dateVal.getUTCDate()

        const result = '' + y + '-'
            + (m < 10 ? '0' : '') + m + '-'
            + (d < 10 ? '0' : '') + d
        return result
    }
    return ''
}

// Builds a date string in a format like '1st Mar, 2018'
TalentUtil.formatHelpers.formatDateWritten = function (dateInput) {

    if (dateInput != undefined) {
        // run through Date datatype - should help handle different input formats?
        let dateVal = new Date(dateInput)
        const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']

        const y = dateVal.getUTCFullYear()
        const m = months[dateVal.getUTCMonth()]
        const d = dateVal.getUTCDate()

        let postfix = ''

        if (d < 11 || d > 13) {
            switch (d % 10) {
                case 1:
                    postfix = 'st'
                    break
                case 2:
                    postfix = 'nd'
                    break
                case 3:
                    postfix = 'rd'
                    break
                default:
                    postfix = 'th'
            }
        } else {
            postfix = 'th'
        }

        const result = '' + d + postfix + ' '
            + m + ', '
            + y

        return result
    } else {
        return ""
    }
}

TalentUtil.deepCopy = function (object) {
    return JSON.parse(JSON.stringify(object))
}