window.toggleDisplayById = function (id) {
    var element = document.getElementById(id);
    if (element) {
        var display = window.getComputedStyle(element).display;
        if (display === "none") {
            element.style.display = "block";
        } else {
            element.style.display = "none";
        }
    }
}
