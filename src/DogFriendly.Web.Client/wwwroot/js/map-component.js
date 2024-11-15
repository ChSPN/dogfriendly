var dotNetMapComponent;

window.mapClickEvent = function (event) {
    // If the click is outside the form control, hide the suggestions
    var isClickInside = document.querySelector('.form-control').contains(event.target);
    if (!isClickInside) {
        dotNetMapComponent.invokeMethodAsync('HideSuggestions');
    }
};

window.mapEscapeEvent = function (event) {
    // If the escape key is pressed, hide the suggestions
    if (event.key === 'Escape') {
        dotNetMapComponent.invokeMethodAsync('HideSuggestions');
    }
};

window.addMapEventListener = function (dotNetHelper) {
    // Add event listener to the document
    dotNetMapComponent = dotNetHelper;
    document.addEventListener('click', window.mapClickEvent);
    document.addEventListener('keydown', window.mapEscapeEvent);
};

window.removeMapEventListener = function () {
    // Remove event listener from the
    dotNetMapComponent = null;
    document.removeEventListener('click', window.mapClickEvent);
    document.removeEventListener('keydown', window.mapEscapeEvent);
};

setInterval(() => {
    // Set the height of the map to the height of the map section
    var height = $('.map-section').height();
    $('#map').height(height);
}, 1000);