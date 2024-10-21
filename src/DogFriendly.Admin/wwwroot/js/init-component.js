window.initSelect = function (objRef) {
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

        $('select').select2({
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
    }, 500);
};
