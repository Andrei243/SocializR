var url = '/Account/GetLocalities';
debugger;
$(function () {
    $('#CountyId').on('change', function () {
        $.ajax({
            type: 'GET',
            url: url,
            data: {
                countyId: $('#CountyId').val()
            },
            success: function (response) {
                var elements = $.map(response, function (item) {
                    return `<option value="${item.value}">${item.text}</option>`
                });
                $('#LocalityId').empty();
                $('#LocalityId').append(elements);
            },
            error: function (error) {

            }

        })
    })


});