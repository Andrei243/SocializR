window.addEventListener("load", () => {
    let changeDescription = (e) => {
        let nod = e.currentTarget;
        let text = e.currentTarget.parentNode.querySelector(".description").value;
        let photoId = e.currentTarget.dataset.photoid;
        $.ajax({
            type: "GET",
            url: "/Profile/ChangeDescription",
            data: {
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


    }
    let albumIdc = document.getElementById("imageGetter").dataset.album;
    let event = ((albumId) => {
        var source = document.getElementById("image-template").innerHTML;
        var template = Handlebars.compile(source);
        let noImages = 0;
        return () => {
            $.ajax({
                type: 'GET',
                url: "/Profile/GetPhotosJson",
                data: {
                    toSkip: noImages,
                    albumId: albumId
                },
                success: (result) => {
                    for (let i = 0; i < result.length; i++) {
                        
                        let image = result[i];
                        var html = template(image);
                        $("#imageBody").append(html);
                        $("#changeDescription" + image.id).click(changeDescription);
                    }
                    noImages += result.length;
                }
            })

        }
    })(albumIdc);
    
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