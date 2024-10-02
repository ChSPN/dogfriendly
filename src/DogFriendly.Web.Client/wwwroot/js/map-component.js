var dotNetMapComponent;

window.mapClickEvent = function (event) {
    var isClickInside = document.querySelector('.form-control').contains(event.target);
    if (!isClickInside) {
        dotNetMapComponent.invokeMethodAsync('HideSuggestions');
    }
};

window.mapEscapeEvent = function (event) {
    if (event.key === 'Escape') {
        dotNetMapComponent.invokeMethodAsync('HideSuggestions');
    }
};

window.addMapEventListener = function (dotNetHelper) {
    dotNetMapComponent = dotNetHelper;
    document.addEventListener('click', window.mapClickEvent);
    document.addEventListener('keydown', window.mapEscapeEvent);
};

window.removeMapEventListener = function () {
    dotNetMapComponent = null;
    document.removeEventListener('click', window.mapClickEvent);
    document.removeEventListener('keydown', window.mapEscapeEvent);
};

setInterval(() => {
    var height = $('.map-section').height();
    $('#map').height(height);
}, 1000);