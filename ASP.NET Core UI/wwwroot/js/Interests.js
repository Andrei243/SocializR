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
                    already: noInterests
                },
                success: (result) => {

                    for (let i = 0; i < result.length; i++) {
                        let interest = result[i];
                        var html = template(interest);
                        $("#interestBody").append(html);
                    }
                    noInterests += result.length;
                }
            })

        }
    })();
    event();
    $("#interestGetter").click(event);

})