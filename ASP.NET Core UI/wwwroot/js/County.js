window.addEventListener("load", () => {
    $("a.needConfirmation").unbind("click").click((e) => {
        var x = confirm("Esti sigur de actiunea ta?");
        if (!x) {
            e.preventDefault();
            e.stopPropagation();
        }
        let val = e.currentTarget.dataset.id;
        $.ajax({
            type: "GET",
            url: "/Counties/CanDelete",
            data: {
                countyId: val
            },
            success: (result) => {
                if (!result) {
                    alert("You can't delete this county");
                    
                }
                else {
                    
                    alert("You deleted it");
                    window.location.href="/Counties/Index"
                }
            }

        })
    })
})


window.addEventListener("load", () => {
    let event = (() => {
        var source = document.getElementById("county-template").innerHTML;
        var template = Handlebars.compile(source);
        let noCounties = 0;
        return () => {
            $.ajax({
                type: 'GET',
                url: "Counties/GetCounties",
                data: {
                    already: noCounties
                },
                success: (result) => {
                    
                    for (let i = 0; i < result.length; i++) {
                        let county = result[i];
                        var html = template(county);
                        $("#countyBody").append(html);
                    }
                    noCounties += result.length;
                }
            })

        }
    })();
    event();
    $("#countyGetter").click(event);

})