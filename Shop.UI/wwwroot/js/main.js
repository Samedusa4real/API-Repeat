﻿$(document).on("click", ".delete-btn", function (e) {
    e.preventDefault();

    let url = $(this).attr("href");
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url).then(response => {
                if (response.ok) {
                    Swal.fire(
                        "Deleted!",
                        "Your file has been deleted.",
                        "success"
                    ).then(() => window.location.reload());
                }
                else {
                    Swal.fire(
                        "Cancelled!",
                        "There is a problem",
                        "error"
                    );
                }
            })
        }
    });
})