
function getParamByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function showSuccess(msg) {
    alert(msg);
}

function showError(msg) {
    alert(msg);
}

function setButtonLoading(btn) {
    btn.addClass("loading");
}

function unsetButtonLoading(btn) {
    btn.removeClass("loading");
}