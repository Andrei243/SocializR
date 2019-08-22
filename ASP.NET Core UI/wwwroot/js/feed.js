$(document).ready(function () {

    $(".like").click(function (e) {
        console.log(e.currentTarget)
        console.log(e.currentTarget.dataset.post)
        $.ajax({
            type: 'GET',
            url: '/Feed/Reaction',
            data: {
                postId: e.currentTarget.dataset.post
            },
            success: function (response) {
                console.log(response);
                if (response) {
                    e.currentTarget.querySelector("img").src = "images/Liked.png";
                    e.currentTarget.parentNode.querySelector(".reactionCounter").innerText = parseInt(e.currentTarget.parentNode.querySelector(".reactionCounter").innerText) + 1;
                }
                else {
                    e.currentTarget.querySelector("img").src = "images/notLiked.png";
                    e.currentTarget.parentNode.querySelector(".reactionCounter").innerText = parseInt(e.currentTarget.parentNode.querySelector(".reactionCounter").innerText) - 1;
                }
            },
            error: function (error) {
                console.log(error)
            }
        }

        )

    });

    $(".buttonComment").click(function (e) {

        $.ajax({
            type: "GET",
            url: '/Feed/Comment',
            data: {
                postId: e.currentTarget.dataset.post,
                comentariu: e.currentTarget.parentNode.querySelector("input").value
            },
            success: function (response) {
                location.reload(true);
            }


        })

    })
});


