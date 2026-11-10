

var dataTable;

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = new DataTable('#tblData', {
        "ajax": {
            "url": "https://localhost:44358/user/packege/GetAll?status"
        },
        "columns": [
            { "data": "name", "width": "18%" },
            { "data": "price", "width": "17%" },
            { "data": "createdDateTime", "width": "20%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="btn-group">
                            <a href="/User/packege/Upsert?id=${data}" class="btn btn-info"><i class="bi bi-pencil-square"></i> Edit</a>
                            <a onClick="Delete('/User/packege/Delete/${data}')" class="btn btn-danger"><i class="bi bi-trash"></i> Delete</a>
                        </div>`;
                },
                "width": "5%"
            }
        ]
    })
}

