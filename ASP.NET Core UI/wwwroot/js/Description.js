$(document).ready(() => {

    $(".changeDescription").click((e) => {
        let nod = e.currentTarget;
        let text = e.currentTarget.parentNode.querySelector(".description").value;
        let photoId = e.currentTarget.dataset.photoid;
        $.ajax({
            type:"GET",
            url:"/Profile/ChangeDescription",
            data:{
                photoId: parseInt(photoId),
                description: text
            },
            success: (response) => {
                if (response) {
                    nod.parentNode.querySelector("p").innerText = text;
                    nod.parentNode.querySelector(".description").value = "";
                }
            }

        })


    })

})