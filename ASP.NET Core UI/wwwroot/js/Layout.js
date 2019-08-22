$(function () {
    let sizeLogo = document.getElementById("logo").clientWidth;
    let sizeLinks = document.getElementById("login-register-links").clientWidth;
    let resizing = () => {
        let input = document.getElementById("divSelection");
        
        let sizeHeader = window.innerWidth;
        let offsetLeft = sizeHeader/2 - sizeLogo - input.clientWidth / 2;
        input.style.marginLeft = offsetLeft.toString()+"px";

        let offsetRight = sizeHeader/2 - sizeLinks - input.clientWidth / 2;
        input.style.marginRight = offsetRight.toString()+"px";
    }

    resizing();
    $(window).resize(resizing);

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
                for (let i = 0; i < response.length; i++) {

                    var redirect = '/Profile/Profile' + "?userId=" + response[i].id;
                    var photoURL = '/Photos/Download' + "/" + response[i].profilePhotoId;
                    if (response[i].profilePhotoId !== null) {
                        
                        $('#users').append(`<a href=${redirect}><div class="item"><img src=${photoURL} height="100em" /> <p>${response[i].name}</p>  </div></a>`)
                    }
                    else {
                        $('#users').append(`<a href=${redirect}><div class="item"><img src="/images/DefaultProfile.png" height="100em" /> <p>${response[i].name}</p>  </div></a>`)
                    }

                }
            },
            error: function (error) {

            }

        })
    })

});