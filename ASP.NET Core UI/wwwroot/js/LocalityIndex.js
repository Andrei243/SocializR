window.addEventListener("load", () => {
    let event = (() => {
        var source = document.getElementById("locality-template").innerHTML;
        var template = Handlebars.compile(source);
        let noLocalities = 0;
        return () => {
            $.ajax({
                type: 'GET',
                url: "/Localities/GetLocalities",
                data: {
                    already: noLocalities
                },
                success: (result) => {

                    for (let i = 0; i < result.length; i++) {
                        let locality = result[i];
                        var html = template(locality);
                        $("#localityBody").append(html);
                    }
                    noLocalities += result.length;
                }
            })

        }
    })();
    event();
    $("#localityGetter").click(event);

})