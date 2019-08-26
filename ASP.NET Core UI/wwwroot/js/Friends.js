window.addEventListener("load", () => {
    let event = (() => {
        var source = document.getElementById("friend-template").innerHTML;
        var template = Handlebars.compile(source);
        let noFriends = 0;
        return () => {
            $.ajax({
                type: 'GET',
                url: "/Profile/GetFriends",
                data: {
                    already: noFriends
                },
                success: (result) => {

                    for (let i = 0; i < result.length; i++) {
                        let friend = result[i];
                        var html = template(friend);
                        $("#friendBody").append(html);
                    }
                    noFriends += result.length;
                }
            })

        }
    })();
    event();
    $("#friendGetter").click(event);

})