var dataTable;
$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable('all');
                }
            }   
        }
    }
});


function loadDataTable(status) {
    dataTable = new DataTable('#tblData', {
        "ajax": {
            "url": "https://localhost:44358/User/Booking/GetAll?status=" + status
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "18%" },
            
            { "data": "phoneNumber", "width": "17%" },
            { "data": "applicationUser.email", "width": "20%" },
            { "data": "bookingStatus", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="w-75 btn-group">
                                <a href="/User/Booking/Details?bookingId=${data}" class="btn btn-info mx-2"><i class="bi bi-sliders2-vertical"></i></a>
                                
                            </div>`
                },
                "width": "5%"
            }
        ]
    })
}
