window.addEventListener("load", function () {
    let eventLike = function (e) {

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

    };

    let eventAddComment = function (e) {
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

    };

    let eventDeleteComment = function (e) {
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

    };

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

    });

    var sourceComment = document.getElementById("comment-template").innerHTML;
    var templateComment = Handlebars.compile(sourceComment);
    let eventComment = (idPost) => {
        let noComments = 0;
        return () => {
            $.ajax({
                type: 'GET',
                url: 'Feed/GetComments',
                data: {
                    already: noComments,
                    postId: idPost
                },
                success: (result) => {
                    for (let i = 0; i < result.length; i++) {
                        let comment = result[i];
                        let html = templateComment(comment);
                        $("#commentBody" + idPost).append(html);
                        $("#deleteComment" + comment.id).click(eventDeleteComment);
                    }
                    noComments += result.length;
                }
            })
        }

    };

    let eventPost = ((userId) => {
        var sourcePost = document.getElementById("post-template").innerHTML;
        var templatePost = Handlebars.compile(sourcePost);
        let noPosts = 0;
        return () => {
            $.ajax({
                type: 'GET',
                url: 'Feed/GetPersonPosts',
                data: {
                    already: noPosts,
                    userId: userId
                },
                success: (result) => {
                    for (let i = 0; i < result.length; i++) {
                        let post = result[i];
                        let html = templatePost(post);
                        $("#postBody").append(html);
                        let eventComentariu = eventComment(post.id);
                        eventComentariu();
                        $("#commentGetter" + post.id).click(eventComentariu);
                        $("#like" + post.id).click(eventLike);
                        $("#commentAdd" + post.id).click(eventAddComment);
                    }
                    noPosts += result.length;
                }


            })

        }

    })(document.getElementById("postGetter").dataset.user);
    eventPost();
    $("#postGetter").click(eventPost);

});