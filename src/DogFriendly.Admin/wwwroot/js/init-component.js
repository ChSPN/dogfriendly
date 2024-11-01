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

        $('#places').on('change', async function () {
            var places = $('#places').val();
            await objRef.invokeMethodAsync('OnPlacesChange', places.map(a => Number.parseInt(a)));
        });

        $('#placetypeid').on('change', async function () {
            var placetypeid = $('#placetypeid').val();
            await objRef.invokeMethodAsync('OnPlaceTypeChange', Number.parseInt(placetypeid));
        });

        $('#placeid').on('change', async function () {
            var placeid = $('#placeid').val();
            await objRef.invokeMethodAsync('OnPlaceChange', Number.parseInt(placeid));
        });

        $('#userid').on('change', async function () {
            var userid = $('#userid').val();
            await objRef.invokeMethodAsync('OnUserChange', Number.parseInt(userid));
        });
    }, 500);
};
