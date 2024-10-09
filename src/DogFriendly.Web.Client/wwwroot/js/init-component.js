window.searchResultInit = function (objRef) {
    setTimeout(() => {
        function formatState(state) {
            if (!state.id) {
                return state.text;
            }

            var $state = $(
                '<span>' + state.element.innerHTML + '</span>'
            );

            return $state;
        };

        $('#amenities').select2({
            placeholder: "Filtres", 
            templateResult: formatState, 
            templateSelection: formatState, 
            escapeMarkup: function (markup) {
                return markup; 
            },
            width: '100%'
        });

        $('#amenities').on('change', async function () {
            var amenities = $('#amenities').val();
            await objRef.invokeMethodAsync('OnAmenitiesChange', amenities.map(a => Number.parseInt(a)));
        });

        $('#rating').on('change', async function () {
            var value = $('#rating').val();
            var rating = value ? Number.parseInt(value) : null;
            await objRef.invokeMethodAsync('OnRatingChange', rating);
        });
    }, 500);
};

window.hideSpinner = function () {
    $('#spinner').hide();
};

window.showSpinner = function () {
    $('#spinner').show();
};

window.axeptioSettings = {
    clientId: "6706df3f9f012add27a9fd33",
    cookiesVersion: "dogfriendly-test-fr-EU",
    googleConsentMode: {
        default: {
            analytics_storage: "denied",
            ad_storage: "denied",
            ad_user_data: "denied",
            ad_personalization: "denied",
            wait_for_update: 500
        }
    }
};

(function (d, s) {
    var t = d.getElementsByTagName(s)[0], e = d.createElement(s);
    e.async = true; e.src = "//static.axept.io/sdk.js";
    t.parentNode.insertBefore(e, t);
})(document, "script");
