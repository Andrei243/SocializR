$(function () {
    html = "";

    $('#selection').keyup(function () {
        var name = $('#selection').val();
        $.ajax({
            type: 'GET',
            url: '/Users/GetUsersByName',
            data: {
                name: name
            },
            success: function (response) {
                $("#users").empty();
                html = '';
                html = html.concat("<br/>")
                for (let i = 0; i < response.length; i++) {

                    var redirect = '/Profile/Profile' + "?userId=" + response[i].id;
                    var photoURL = '/Photos/Download' + "/" + response[i].profilePhotoId;
                    if (response[i].profilePhotoId !== null) {

                        html = html.concat(`<a href=${redirect}><div class="dropdownItem"><p>${response[i].name}</p><img src=${photoURL} height="100em" />   </div></a>`)
                    }
                    else {
                        html = html.concat(`<a href=${redirect}><div class="dropdownItem"><p>${response[i].name}</p><img src="/images/DefaultProfile.png" height="100em" />   </div></a>`)
                    }

                }
                html = html.concat("<br/><br/>")
                if (response.length === 0) {
                    html = '';
                }
                $("#users").append(html);
            },
            error: function (error) {

            }

        })
    })
    $("#selection").blur(() => {        
            $("#users").empty();

    })

    $('#users').on('mousedown', function (event) {
        event.preventDefault();
    });

    $("#selection").focus(() => {
        $("#users").empty();
        $("#users").append(html);
    })


});