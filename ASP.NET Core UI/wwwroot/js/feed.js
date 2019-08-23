$(document).ready(function () {

    $(".like").click(function (e) {
       
        $.ajax({
            type: 'GET',
            url: '/Feed/Reaction',
            data: {
                postId: e.currentTarget.dataset.post
            },
            success: function (response) {
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
            },
            error: function (error) {
                console.log(error);
            }


        })

    })

    $(".buttonDeleteComment").click(function (e) {
        $.ajax({
            type: "GET",
            url: '/Feed/RemoveComment',
            data: {
                commentId: e.currentTarget.dataset.comment,
                
            },
            success: function (response) {
                location.reload(true);
            },
            error: function (error) {
                console.log(error);
            }


        })

    })
    $(".buttonDeletePost").click(function (e) {
        $.ajax({
            type: "GET",
            url: '/Feed/RemovePost',
            data: {
                postId: e.currentTarget.dataset.post,

            },
            success: function (response) {
                location.reload(true);
            },
            error: function (error) {
                console.log(error);
            }


        })

    })
});


