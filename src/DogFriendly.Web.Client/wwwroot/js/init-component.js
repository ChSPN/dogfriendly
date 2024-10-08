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
}
