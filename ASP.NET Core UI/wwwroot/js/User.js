window.addEventListener("load", () => {
    let event = (() => {
        var source = document.getElementById("user-template").innerHTML;
        var template = Handlebars.compile(source);
        let noUsers = 0;
        return () => {
            $.ajax({
                type: 'GET',
                url: "/Users/GetUsers",
                data: {
                    toSkip: noUsers
                },
                success: (result) => {

                    for (let i = 0; i < result.length; i++) {
                        let user = result[i];
                        var html = template(user);
                        $("#userBody").append(html);
                    }
                    noUsers += result.length;
                }
            })

        }
    })();
    event();
    let copieFunctie = event;
    event = () => { };
    setTimeout(() => {
        event = copieFunctie;
    }, 1000);
    $(window).scroll(() => {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
            event();
            event = () => { }
            this.setTimeout(() => {
                event = copieFunctie;
            }, 1000)
        }

    })

})