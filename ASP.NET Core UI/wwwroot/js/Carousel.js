//$(".carouselBegin").load((e) => {
//    let target = $(this);
//    $.ajax({
//        type:'GET',
//        url: '/Profile/GetPhotos',
//        data: {
//            albumId: e.currentTarget.dataset.id
//        },
//        success: function (response) {
//            if(response.length>0){
//                let divWithPhotos = e.currentTarget.parentElement.getElementsByClassName('poze')[0];
//                divWithPhotos.innerHTML = "";
//                let photosHtml = "";
//                for (let i = 0; i < response.length; i++) {
//                    let beginAnchor = `<a href="/Photos/Download?id=${response[i]}"`;
//                    if (i === 0) {
//                        beginAnchor = beginAnchor + ">";
//                    }
//                    else {
//                        beginAnchor = beginAnchor + "hidden>";
//                    }
//                    let photo = `<img src="/Photos/Download/${response[i]}" height="40em"/>`
//                    let finishAnchor = "</a>";
//                    photosHtml = photosHtml + '\n' + beginAnchor + '\n' + photo + '\n' + finishAnchor;

//                }
//                divWithPhotos.innerHTML = photosHtml;
//                e.currentTarget.parentElement.removeChild(e.currentTarget);
//            }
//        }

//    })
//    $(".poze").lightGallery({

//        thumbnail: false
//    })
//})
window.addEventListener("load", () => {
    $(".poze").each((index, div) => {
        
        $.ajax({
        type:'GET',
        url: '/Profile/GetPhotos',
        data: {
            albumId: div.dataset.id
        },
        success: function (response) {
            if(response.length>0){
                let photosHtml = "";
                for (let i = 0; i < response.length; i++) {
                    let beginAnchor = `<a href="/Photos/Download/${response[i].id}" data-lightbox="album${div.dataset.id}" data-title="${response[i].description ? response[i].description : ""}" data-alt="${response[i].description ? response[i].description : ""}" `;
                    if (i === 0) {
                        beginAnchor = beginAnchor + ">";
                    }
                    else {
                        beginAnchor = beginAnchor + " hidden>";
                    }
                    let photo = `<img src="/Photos/Download/${response[i].id}" height="40em"/>`
                    let finishAnchor = "</a>";
                    photosHtml = photosHtml + '\n' + beginAnchor + '\n' + photo + '\n' + finishAnchor;
                }
                div.innerHTML = photosHtml;
            }
        }

    })

    })
    lightbox.option({
        'disableScrolling': true,
        'maxHeight': Math.floor(window.innerHeight * 0.7),
        'maxWidth': Math.floor(window.innerWidth * 0.7),
        'wrapAround': true
    })
})