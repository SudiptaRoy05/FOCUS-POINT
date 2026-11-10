

var dataTable;

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = new DataTable('#tblData', {
        "ajax": {
            "url": "https://localhost:44358/user/MediaItem/GetAll?status"
        },
        "columns": [
            { "data": "fileName", "width": "18%" },
            { "data": "price", "width": "17%" },
            { "data": "composition.name", "width": "20%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="btn-group">
                            <a href="/User/MediaItem/Upsert?id=${data}" class="btn btn-info"><i class="bi bi-pencil-square"></i> Edit</a>
                            <a onClick="Delete('/User/MediaItem/Delete/${data}')" class="btn btn-danger"><i class="bi bi-trash"></i> Delete</a>
                        </div>`;
                },
                "width": "5%"
            }
        ]
    })
}


function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}