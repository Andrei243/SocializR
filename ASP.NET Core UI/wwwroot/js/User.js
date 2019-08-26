window.addEventListener("load", () => {
    let event = (() => {
        var source = document.getElementById("user-template").innerHTML;
        console.log(document.getElementById("user-template").innerHTML)
        var template = Handlebars.compile(source);
        let noUsers = 0;
        return () => {
            $.ajax({
                type: 'GET',
                url: "Users/GetUsers",
                data: {
                    already: noUsers
                },
                success: (result) => {

                    for (let i = 0; i < result.length; i++) {
                        debugger;
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
    $("#userGetter").click(event);

})