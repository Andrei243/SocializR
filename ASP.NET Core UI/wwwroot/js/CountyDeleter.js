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