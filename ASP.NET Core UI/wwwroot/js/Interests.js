window.addEventListener("load", () => {
    let event = (() => {
        var source = document.getElementById("interest-template").innerHTML;
        var template = Handlebars.compile(source);
        let noInterests = 0;
        return () => {
            $.ajax({
                type: 'GET',
                url: "/Interests/GetInterests",
                data: {
                    toSkip: noInterests
                },
                success: (result) => {

                    for (let i = 0; i < result.length; i++) {
                        let interest = result[i];
                        var html = template(interest);
                        $("#interestBody").append(html);
                        $("#interestBody tr:last-child a.needConfirmation").click(prevent);
                    }
                    noInterests += result.length;
                }
            })

        }
    })();
    event();
    let functionCopy = event;
    event = () => { };
    setTimeout(() => {
        event = functionCopy;
    }, 1000);
    $(window).scroll(() => {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
            event();
            event = () => { }
            this.setTimeout(() => {
                event = functionCopy;
            }, 1000)
        }

    })

})